using UnityEngine;
using TMPro;

public class DocumentFiller : MonoBehaviour
{
    [Header("Character Reference")]
    [SerializeField]
    [Tooltip("Character component to read profile data from")]
    public Character character;

    [Header("Fields Container")]
    [SerializeField]
    [Tooltip("Container holding all the field objects (optional - will auto-find 'Fields' if not set)")]
    private Transform documentContainer;

    [Header("Collider Reference")]
    [SerializeField]
    [Tooltip("BoxCollider2D to detect clicks outside (optional - will auto-find if not set)")]
    private BoxCollider2D boxCollider;

    private TMP_Text lifeStoryField;
    private TMP_Text deathReasonField;

    public GameObject parentButton; // Reference to the button that spawned this report

    private bool isActive = false; // Prevents immediate closure on spawn

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Auto-find the BoxCollider2D if not manually assigned
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Debug.LogWarning("No BoxCollider2D found on ReportFiller. Click-outside detection will not work.");
            }
        }

        if (character == null)
        {
            character = GameObject.Find("Character")?.GetComponent<Character>();
            if (character == null)
            {
                Debug.LogError("Character GameObject not found in scene!");
            }
        }

        // Auto-find the Fields container if not manually assigned
        if (documentContainer == null)
        {
            documentContainer = transform.Find("Fields");
            if (documentContainer == null)
            {
                Debug.LogError("Could not find 'Fields' container in Character Report prefab!");
                return;
            }
        }

        // Find all the field objects and their TMP_Text components
        FindFieldComponents();

        // Fill the report with character data
        FillReport();

        // Enable click detection after one frame to prevent immediate closure
        StartCoroutine(EnableInputAfterDelay());
    }

    private System.Collections.IEnumerator EnableInputAfterDelay()
    {
        // Wait for end of frame to ensure the spawn click is finished
        yield return new WaitForEndOfFrame();
        // Wait one more frame to be safe
        yield return null;
        isActive = true;
        Debug.Log("Document input enabled");
    }

    // Update is called once per frame
    void Update()
    {
        // Don't process input until fully initialized
        if (!isActive)
            return;

        // Check for ESC key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseReport();
            return;
        }

        // Check for mouse click outside the box collider
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            if (boxCollider != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // Check if the click is outside the box collider
                if (!boxCollider.OverlapPoint(mousePosition))
                {
                    CloseReport();
                }
            }
        }
    }

    private void FindFieldComponents()
    {
        // Find each field by name and get its TMP_Text component
        Transform lifeStoryTransform = documentContainer.Find("LifeStory");
        if (lifeStoryTransform != null)
            lifeStoryField = lifeStoryTransform.GetComponent<TMP_Text>();

        Transform deathReasonTransform = documentContainer.Find("DeathReason");
        if (deathReasonTransform != null)
            deathReasonField = deathReasonTransform.GetComponent<TMP_Text>();

        // Log warnings for any missing fields
        if (lifeStoryField == null) Debug.LogWarning("LifeStory field or its TMP_Text component not found!");
        if (deathReasonField == null) Debug.LogWarning("DeathReason field or its TMP_Text component not found!");

        Debug.Log("Field components found: " +
                  $"LifeStory: {(lifeStoryField != null ? "Yes" : "No")}, " +
                  $"DeathReason: {(deathReasonField != null ? "Yes" : "No")}");
    }

    private void FillReport()
    {
        if (character == null)
        {
            Debug.LogError("Character reference is not assigned in ReportFiller!");
            return;
        }

        CharacterProfileData profile = character.ProfileData;

        if (profile == null)
        {
            Debug.LogError("Character profile data is null!");
            return;
        }

        // Fill in the LifeStory field
        if (lifeStoryField != null)
            lifeStoryField.text = profile.Introduction ?? "Unknown";

        // Fill in the DeathReason field
        if (deathReasonField != null)
            deathReasonField.text = profile.DeathReason ?? "Unknown";

        Debug.Log($"Document filled for character: {profile.Name}");
    }

    // Public method to refresh the report if character data changes
    public void RefreshReport()
    {
        FillReport();
    }

    // Close and dispose of the report
    private void CloseReport()
    {
        Debug.Log($"Closing report for character: {character?.ProfileData?.Name ?? "Unknown"}");
        Destroy(gameObject);
        if (parentButton != null)
        {
            // Re-enable the parent button's sprite renderer and collider
            var sr = parentButton.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = true;
            }
            var col = parentButton.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = true;
            }
        }
    }
    public void DisposeDocument()
    {
        if (parentButton != null)
        {
            Destroy(parentButton);
        }
    }
}
