using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class DocumentButton : MonoBehaviour
{
    [Header("Hover Scale Settings")]
    [Range(1f, 1.5f)] public float hoverScale = 1.05f;
    [Range(0.1f, 1f)] public float transitionSpeed = 0.2f;

    [Header("Document to Spawn")]
    [SerializeField] private GameObject documentPrefab;
    [SerializeField] private GameObject reportPrefab;

    private Vector3 _normalScale;
    private Vector3 _targetScale;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _normalScale = transform.localScale;
        _targetScale = _normalScale;
    }

    // Update is called once per frame
    void Update()
    {
        // Smoothly lerp to target scale
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, transitionSpeed);
    }

    private void OnMouseEnter()
    {
        _targetScale = _normalScale * hoverScale;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        // Note: Unity doesn't support custom cursor modes like CSS pointer,
        // but we can use SetCursor with null to reset to system cursor
    }

    private void OnMouseExit()
    {
        _targetScale = _normalScale;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    private void OnMouseDown()
    {
        // Try documentPrefab first, then reportPrefab
        GameObject prefabToSpawn = documentPrefab != null ? documentPrefab : reportPrefab;
        
        if (prefabToSpawn != null)
        {
            // Get a GameObject named "Character"
            GameObject characterObj = GameObject.Find("Character");
            if (characterObj != null)
            {
                var obj = Instantiate(prefabToSpawn);
                obj.transform.position = new Vector3(5, 0, 0);
                var filler = obj.GetComponent<DocumentFiller>();
                if (filler != null)
                {
                    filler.character = characterObj.GetComponent<Character>();
                    filler.parentButton = gameObject;
                    
                    // Make the spawned document/report non-clickable by disabling its collider
                    var objCollider = obj.GetComponent<Collider2D>();
                    if (objCollider != null)
                    {
                        objCollider.enabled = false;
                        Debug.Log("Document collider disabled - won't block clicks");
                    }
                    
                    Debug.Log("Document prefab spawned");
                }
                else
                {
                    Debug.LogWarning("DocumentFiller component not found on spawned prefab!");
                }
                
                // Set this button to invisible
                var sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.enabled = false;
                }
                // Disable this collider
                var col = GetComponent<Collider2D>();
                if (col != null)
                {
                    col.enabled = false;
                }
                
                // Set DocumentFiller sorting order to front
                var fillerRenderer = obj.GetComponent<SpriteRenderer>();
                if (fillerRenderer != null)
                {
                    fillerRenderer.sortingOrder = 1000; // High value to bring to front
                }
            }
            else
            {
                Debug.LogError("Character object not found in scene");
            }
        }
        else
        {
            Debug.LogWarning("Neither documentPrefab nor reportPrefab assigned on " + gameObject.name);
        }
    }
}
