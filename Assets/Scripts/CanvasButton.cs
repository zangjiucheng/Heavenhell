using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class SpriteButton : MonoBehaviour
{
    [Header("Canvas to Show/Hide")]
    [SerializeField]
    private Canvas reportCanvas;

    [Header("Optional: Hover Highlight")]
    public Color normalColor = Color.white;
    public Color hoverColor = new Color(0.9f, 0.9f, 0.9f, 1f);

    private SpriteRenderer _sr;
    private Collider2D _collider;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _collider = GetComponent<Collider2D>();
        _sr.color = normalColor;
        
        // Debug checks
        if (_collider == null)
        {
            Debug.LogError("No Collider2D found on " + gameObject.name);
        }
        else if (!_collider.enabled)
        {
            Debug.LogWarning("Collider2D is disabled on " + gameObject.name);
        }
        else
        {
            Debug.Log("SpriteButton initialized on " + gameObject.name + " with collider: " + _collider.GetType().Name);
        }
        
        // Check for Physics2DRaycaster on camera
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Physics2DRaycaster raycaster = mainCam.GetComponent<Physics2DRaycaster>();
            if (raycaster == null)
            {
                Debug.LogWarning("Main Camera is missing Physics2DRaycaster component! Adding it now...");
                mainCam.gameObject.AddComponent<Physics2DRaycaster>();
            }
        }
        
        // Hide canvas on start
        if (reportCanvas != null)
        {
            reportCanvas.enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse entered: " + gameObject.name);
        _sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse exited: " + gameObject.name);
        _sr.color = normalColor;
    }

    private void OnMouseDown()
    {
        Debug.Log("Mouse clicked on: " + gameObject.name);
        
        if (reportCanvas == null)
        {
            Debug.LogWarning("reportCanvas is not assigned!");
            return;
        }
        
        // Toggle report canvas visibility
        reportCanvas.enabled = !reportCanvas.enabled;
        Debug.Log("Canvas enabled: " + reportCanvas.enabled);
    }

    private void Update()
    {
        // Close report when ESC is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (reportCanvas != null && reportCanvas.enabled)
            {
                reportCanvas.enabled = false;
                Debug.Log("Canvas closed with ESC");
            }
        }
    }
}
