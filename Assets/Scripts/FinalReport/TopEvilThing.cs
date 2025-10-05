using UnityEngine;
using TMPro;

public class TopEvilThing : MonoBehaviour
{
    [SerializeField] private GameObject dialogPrefab;
    
    private BoxCollider2D boxCollider;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private bool isHovering = false;
    private float lerpSpeed = 10f;
    
    [HideInInspector]
    public string explanation = "null";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Auto get BoxCollider2D component
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogWarning("BoxCollider2D not found on " + gameObject.name);
        }
        
        // Store original scale
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Lerp towards target scale
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * lerpSpeed);
    }

    void OnMouseEnter()
    {
        isHovering = true;
        // Set target scale to 105%
        targetScale = originalScale * 1.025f;
    }

    void OnMouseExit()
    {
        isHovering = false;
        // Reset to original scale
        targetScale = originalScale;
    }

    void OnMouseDown()
    {
        if (isHovering)
        {
            OnClicked();
        }
    }

    private void OnClicked()
    {
        Debug.Log("OnClicked called! Explanation: " + explanation);
        
        if (dialogPrefab != null)
        {
            // Get Dialogs object
            Transform dialogsTransform = GameObject.Find("Dialogs")?.transform;
            
            // Spawn the dialog prefab
            GameObject dialog = Instantiate(dialogPrefab, dialogsTransform);
            Debug.Log("Dialog instantiated: " + dialog.name);
            
            // Find the "Text" child object
            Transform textTransform = dialog.transform.Find("Text");
            Debug.Log("Text transform found: " + (textTransform != null));
            
            if (textTransform != null)
            {
                // Try TMP_Text (base class for both TextMeshProUGUI and TextMeshPro)
                TMP_Text textComponent = textTransform.GetComponent<TMP_Text>();
                Debug.Log("TMP_Text component found: " + (textComponent != null));
                
                if (textComponent != null)
                {
                    textComponent.text = explanation;
                    Debug.Log("Text set to: " + textComponent.text);
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
                    Debug.Log("Destroyed old dialog: " + child.gameObject.name);
                }
            }
        }
        else
        {
            Debug.LogWarning("dialogPrefab is not assigned!");
        }
    }
}
