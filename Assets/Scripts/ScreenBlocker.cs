using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ScreenBlocker : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler
{
    [Tooltip("When enabled this panel will block all UI input behind it.")]
    public bool blocksInput = true;

    private Image _image;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError("ScreenBlocker requires an Image component.");
            enabled = false;
            return;
        }

        // Ensure the Image will block raycasts when blocking is enabled
        _image.raycastTarget = blocksInput;

        // Ensure a CanvasGroup exists so we can reliably block raycasts even when the Image is partially transparent
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _canvasGroup.interactable = false;
            _canvasGroup.ignoreParentGroups = true;
        }
        _canvasGroup.blocksRaycasts = blocksInput;

        if (EventSystem.current == null)
        {
            Debug.LogWarning("No EventSystem found in the scene. UI pointer events won't work without one.");
        }
    }

    /// <summary>
    /// Enable or disable blocking at runtime.
    /// </summary>
    public void SetBlocking(bool block)
    {
        blocksInput = block;
        if (_image != null) _image.raycastTarget = block;
        if (_canvasGroup != null) _canvasGroup.blocksRaycasts = block;
    }

    // Consume events so they don't pass through to UI behind this panel.
    public void OnPointerClick(PointerEventData eventData) { eventData.Use(); }
    public void OnPointerDown(PointerEventData eventData) { eventData.Use(); }
    public void OnPointerUp(PointerEventData eventData) { eventData.Use(); }
    public void OnBeginDrag(PointerEventData eventData) { eventData.Use(); }
    public void OnDrag(PointerEventData eventData) { eventData.Use(); }
    public void OnEndDrag(PointerEventData eventData) { eventData.Use(); }
    public void OnScroll(PointerEventData eventData) { eventData.Use(); }

    private void Update()
    {
        // Optional: toggle blocking off with ESC for quick testing
        if (Input.GetKeyDown(KeyCode.Escape) && blocksInput)
        {
            SetBlocking(false);
            Debug.Log("ScreenBlocker: blocking disabled via ESC");
        }
    }
}
