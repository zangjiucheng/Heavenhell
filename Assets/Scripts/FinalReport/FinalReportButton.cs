using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
public class FinalReportButton : MonoBehaviour
{
    [Header("Hover Scale Settings")]
    [Range(1f, 1.5f)] public float hoverScale = 1.05f;
    [Range(0.1f, 1f)] public float transitionSpeed = 0.2f;

    [Header("Report to Spawn")]
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
        if (reportPrefab != null)
        {
            // Get a GameObject named "Character"
            GameObject characterObj = GameObject.Find("Character");
            if (characterObj != null)
            {
                
                var obj =  Instantiate(reportPrefab);
                var filler = obj.GetComponent<ReportFiller>();
                filler.character = characterObj.GetComponent<Character>();
                Debug.Log("Report prefab spawned");
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
                obj.GetComponent<ReportFiller>().parentButton = gameObject;
            }
            else
            {
                Debug.Log("Character object not found in scene");
            }
        }
        else
        {
            Debug.LogWarning("Report prefab not assigned on " + gameObject.name);
        }
    }
}
