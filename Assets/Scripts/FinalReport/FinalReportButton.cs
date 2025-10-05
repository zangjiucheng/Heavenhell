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
    private bool _wasHovering;
    private Camera _mainCamera;
    private Collider2D _collider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _normalScale = transform.localScale;
        _targetScale = _normalScale;
        _mainCamera = Camera.main;
        _collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if mouse is over this object using raycast
        bool isHovering = IsMouseOver();

        // Handle hover enter
        if (isHovering && !_wasHovering)
        {
            _targetScale = _normalScale * hoverScale;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        // Handle hover exit
        if (!isHovering && _wasHovering)
        {
            _targetScale = _normalScale;
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        // Handle click
        if (isHovering && Input.GetMouseButtonDown(0))
        {
            OnClicked();
        }

        _wasHovering = isHovering;

        // Smoothly lerp to target scale
        transform.localScale = Vector3.Lerp(transform.localScale, _targetScale, transitionSpeed);
    }

    private bool IsMouseOver()
    {
        if (_mainCamera == null || _collider == null || !_collider.enabled) return false;

        Vector2 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 0f);

        foreach (var hit in hits)
        {
            if (hit.collider == _collider)
                return true;
        }

        return false;
    }

    private void OnClicked()
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
