using UnityEngine;
using TMPro;
using Unity.Mathematics.Geometry;
using Math = System.Math;

public class TopEvilThing : MonoBehaviour
{
    [SerializeField] private GameObject dialogPrefab;

    private BoxCollider2D _boxCollider;
    private Vector3 _originalScale;
    private Vector3 _targetScale;
    private bool _wasHovering;
    private float _lerpSpeed = 10f;
    private Camera _mainCamera;

    [HideInInspector] public string explanation = "null";
    [HideInInspector] public Character character; // Reference to the character being questioned

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Auto get BoxCollider2D component
        _boxCollider = GetComponent<BoxCollider2D>();
        if (_boxCollider == null)
        {
            Debug.LogWarning("BoxCollider2D not found on " + gameObject.name);
        }

        // Store original scale
        _originalScale = transform.localScale;
        _targetScale = _originalScale;
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if mouse is over this object using raycast
        bool isHovering = IsMouseOver();

        // Handle hover enter
        if (isHovering && !_wasHovering)
        {
            _targetScale = _originalScale * 1.025f;
        }

        // Handle hover exit
        if (!isHovering && _wasHovering)
        {
            _targetScale = _originalScale;
        }

        // Handle click
        if (isHovering && Input.GetMouseButtonDown(0))
        {
            OnClicked();
        }

        _wasHovering = isHovering;

        // Lerp towards target scale
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, Time.deltaTime * _lerpSpeed);
    }

    private bool IsMouseOver()
    {
        if (_mainCamera == null || _boxCollider == null || !_boxCollider.enabled) return false;

        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 0f);

        foreach (var hit in hits)
        {
            if (hit.collider == _boxCollider)
                return true;
        }

        return false;
    }

    private void OnClicked()
    {
        // Make the character show fear expression when questioned about evil deeds
        if (character != null)
        {
            character.SetExpression(Expression.Fear);
            Debug.Log($"Character {character.ProfileData?.Name} is now showing fear expression!");
        }
        else
        {
            Debug.LogWarning("Character reference not set in TopEvilThing!");
        }

        if (dialogPrefab != null)
        {
            // Get Dialogs object
            Transform dialogsTransform = GameObject.Find("Dialogs")?.transform;

            if (dialogsTransform == null)
            {
                Debug.LogWarning("Dialogs object not found!");
                return;
            }

            // Spawn the dialog prefab
            GameObject dialog = Instantiate(dialogPrefab, dialogsTransform);
            // Let character talk
            character.Talk(0.33f);
            character.TalkSound((int) Math.Round(-1 + Math.Pow(Random.value, 2)), Random.value * 0.4f + 0.8f);

            // Find the "Text" child object
            Transform textTransform = dialog.transform.Find("Text");

            if (textTransform != null)
            {
                // Try TMP_Text (base class for both TextMeshProUGUI and TextMeshPro)
                TMP_Text textComponent = textTransform.GetComponent<TMP_Text>();

                if (textComponent != null)
                {
                    textComponent.text = explanation;
                }
                else
                {
                    Debug.LogWarning("TMP_Text component not found on 'Text' child");
                }
            }
            else
            {
                Debug.LogWarning("'Text' child not found in dialogPrefab");
            }

            // Find all existing dialog game objects from Dialogs object
            foreach (Transform child in dialogsTransform)
            {
                if (child.gameObject != dialog)
                {
                    Destroy(child.gameObject);
                }
            }
        }
        else
        {
            Debug.LogWarning("dialogPrefab is not assigned!");
        }
    }
}