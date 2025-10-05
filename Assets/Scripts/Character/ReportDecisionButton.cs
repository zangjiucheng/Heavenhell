using UnityEngine;

public class ReportDecisionButton : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D circleCollider;
    private Vector3 defaultScale;
    private Vector3 hoverScale;
    private Color defaultColor;
    private Color hoverColor;
    private bool isHovering = false;
    private bool wasMouseDownOnButton = false;
    private Camera mainCamera;

    [SerializeField] private GameObject report;
    [SerializeField] private float transitionSpeed = 10f;
    [SerializeField] private float defaultScaleMultiplier = 0.8f;
    [SerializeField] private float defaultAlpha = 0.15f;
    [SerializeField] private bool isHeavenMode = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        circleCollider = GetComponent<CircleCollider2D>();
        mainCamera = Camera.main;
        
        // Store the original scale as the hover scale
        hoverScale = transform.localScale;
        
        // Calculate default scale (80% of original)
        defaultScale = hoverScale * defaultScaleMultiplier;
        
        // Set up colors
        defaultColor = spriteRenderer.color;
        defaultColor.a = defaultAlpha; // 90% opacity (10% transparency)
        
        hoverColor = spriteRenderer.color;
        hoverColor.a = 1f; // 100% opacity
        
        // Apply default state
        transform.localScale = defaultScale;
        spriteRenderer.color = defaultColor;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if mouse is over this object using raycast
        isHovering = IsMouseOver();
        
        // Track if mouse was pressed while hovering
        if (isHovering && Input.GetMouseButtonDown(0))
        {
            wasMouseDownOnButton = true;
        }
        
        // Reset tracking if mouse button is released (regardless of hover state)
        if (Input.GetMouseButtonUp(0))
        {
            // Only perform action if both mouse down AND mouse up happened while hovering
            if (isHovering && wasMouseDownOnButton)
            {
                PerformAction();
            }
            wasMouseDownOnButton = false;
        }
        
        // Smoothly interpolate to target state
        Vector3 targetScale = isHovering ? hoverScale : defaultScale;
        Color targetColor = isHovering ? hoverColor : defaultColor;
        
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, targetColor, Time.deltaTime * transitionSpeed);
    }

    private bool IsMouseOver()
    {
        if (mainCamera == null || circleCollider == null || !circleCollider.enabled) 
            return false;

        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 0f);

        foreach (var hit in hits)
        {
            if (hit.collider == circleCollider)
                return true;
        }

        return false;
    }
    
    private void PerformAction()
    {
        // TODO: Implement button action based on isHeavenMode
        if (!isHeavenMode)
        {
            Debug.Log($"[{gameObject.name}] Heaven button clicked!");
            
            // Get character
            var characterObject = GameObject.Find("Character");
            var character = characterObject.GetComponent<Character>();
            if (character != null)
            {
                character.SetExpression(Expression.Happy);
                character.SendToHeaven();
            }
        }
        else
        {
            Debug.Log($"[{gameObject.name}] Hell button clicked!");
            
            // Get character
            var characterObject = GameObject.Find("Character");
            var character = characterObject.GetComponent<Character>();
            if (character != null)
            {
                character.SetExpression(Expression.Sad);
                character.SendToHell();
            }
        }

        // Close the report UI (dispose)
        if (report != null)
        {
            Destroy(report);
            var reportFiller = report.GetComponent<ReportFiller>();
            if (reportFiller != null)
            {
                reportFiller.DisposeReport();
            }
        }
        
        // Find and destroy all "Final Document" prefab instances in the scene
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        foreach (GameObject obj in allObjects)
        {
            // Check if the object name contains "Final Document" (case-insensitive)
            if (obj.name.Contains("Final Document") || obj.name.Contains("final document"))
            {
                Debug.Log($"Destroying Final Document instance: {obj.name}");
                Destroy(obj);
            }
        }
    }
}
