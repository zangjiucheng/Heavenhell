using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DecisionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Canvas to Show/Hide")]
    [SerializeField]
    private Canvas reportCanvas;

    [Header("Opacity")]
    [Range(0f, 1f)] public float normalAlpha = 0.5f;
    [Range(0f, 1f)] public float hoverAlpha = 1f;

    private Image _image;
    private Color _originalColor;

    private void Awake()
    {
        _image = GetComponent<Image>();
        if (_image == null)
        {
            Debug.LogError("DecisionButton requires an Image component on " + gameObject.name);
            enabled = false;
            return;
        }

        _originalColor = _image.color;
        SetAlpha(normalAlpha);

        // Hide canvas on start
        if (reportCanvas != null)
        {
            reportCanvas.enabled = false;
        }

        // Check EventSystem
        if (EventSystem.current == null)
        {
            Debug.LogWarning("No EventSystem found in the scene. UI pointer events won't work without one.");
        }

        // Check that the canvas has a GraphicRaycaster (commonly needed for UI clicks)
        if (GetComponentInParent<Canvas>() == null)
        {
            Debug.LogWarning("DecisionButton is not under a Canvas. Pointer events may not work as expected.");
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetAlpha(hoverAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(normalAlpha);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (reportCanvas == null)
        {
            Debug.LogWarning("reportCanvas is not assigned on " + gameObject.name);
            return;
        }

        // Toggle this canvas
        bool newState = !reportCanvas.enabled;

        if (newState)
        {
            // If opening this canvas, close any other enabled canvases in the scene so only this one is visible
            Canvas[] canvases = UnityEngine.Object.FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (Canvas c in canvases)
            {
                if (c != reportCanvas && c.enabled)
                {
                    c.enabled = false;
                    Debug.Log($"Closed other canvas '{c.name}'");
                }
            }
        }

        reportCanvas.enabled = newState;
        Debug.Log($"Toggled canvas '{reportCanvas.name}' to {reportCanvas.enabled}");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (reportCanvas != null && reportCanvas.enabled)
            {
                reportCanvas.enabled = false;
                Debug.Log("Canvas closed with ESC");
            }
        }
    }

    private void SetAlpha(float a)
    {
        Color c = _image.color;
        c.a = a;
        _image.color = c;
    }
}


