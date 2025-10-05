using UnityEngine;
using TMPro;




public class ReportFiller : MonoBehaviour
{
    [Header("Character Reference")]
    [SerializeField]
    [Tooltip("Character component to read profile data from")]
    public Character character;
    
    [Header("Fields Container")]
    [SerializeField]
    [Tooltip("Container holding all the field objects (optional - will auto-find 'Fields' if not set)")]
    private Transform fieldsContainer;
    
    [Header("Collider Reference")]
    [SerializeField]
    [Tooltip("BoxCollider2D to detect clicks outside (optional - will auto-find if not set)")]
    private BoxCollider2D boxCollider;
    
    private TMP_Text nameField;
    private TMP_Text dobField;
    private TMP_Text dodField;
    private TMP_Text jobField;
    private TMP_Text evilTop1Field;
    private TMP_Text evilTop2Field;
    private TMP_Text evilTop3Field;

    public GameObject parentButton; // Reference to the button that spawned this report
    
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
        
        // Auto-find the Fields container if not manually assigned
        if (fieldsContainer == null)
        {
            fieldsContainer = transform.Find("Fields");
            if (fieldsContainer == null)
            {
                Debug.LogError("Could not find 'Fields' container in Character Report prefab!");
                return;
            }
        }
        
        // Find all the field objects and their TMP_Text components
        FindFieldComponents();
        
        // Fill the report with character data
        FillReport();
    }

    // Update is called once per frame
    void Update()
    {
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
        Transform nameTransform = fieldsContainer.Find("Name");
        if (nameTransform != null)
            nameField = nameTransform.GetComponent<TMP_Text>();
        
        Transform dobTransform = fieldsContainer.Find("DOB");
        if (dobTransform != null)
            dobField = dobTransform.GetComponent<TMP_Text>();
        
        Transform dodTransform = fieldsContainer.Find("DOD");
        if (dodTransform != null)
            dodField = dodTransform.GetComponent<TMP_Text>();
        
        Transform jobTransform = fieldsContainer.Find("Job");
        if (jobTransform != null)
            jobField = jobTransform.GetComponent<TMP_Text>();
        
        Transform evil1Transform = fieldsContainer.Find("Evil Top 1");
        if (evil1Transform != null)
            evilTop1Field = evil1Transform.GetComponent<TMP_Text>();
        
        Transform evil2Transform = fieldsContainer.Find("Evil Top 2");
        if (evil2Transform != null)
            evilTop2Field = evil2Transform.GetComponent<TMP_Text>();
        
        Transform evil3Transform = fieldsContainer.Find("Evil Top 3");
        if (evil3Transform != null)
            evilTop3Field = evil3Transform.GetComponent<TMP_Text>();
        
        
        
        // Log warnings for any missing fields
        if (nameField == null) Debug.LogWarning("Name field or its TMP_Text component not found!");
        if (dobField == null) Debug.LogWarning("DOB field or its TMP_Text component not found!");
        if (dodField == null) Debug.LogWarning("DOD field or its TMP_Text component not found!");
        if (jobField == null) Debug.LogWarning("Job field or its TMP_Text component not found!");
        if (evilTop1Field == null) Debug.LogWarning("Evil Top 1 field or its TMP_Text component not found!");
        if (evilTop2Field == null) Debug.LogWarning("Evil Top 2 field or its TMP_Text component not found!");
        if (evilTop3Field == null) Debug.LogWarning("Evil Top 3 field or its TMP_Text component not found!");
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
        
        // Fill in the basic information
        if (nameField != null)
            nameField.text = profile.Name ?? "Unknown";
        
        if (dobField != null)
            dobField.text = profile.DOB ?? "Unknown";
        
        if (dodField != null)
            dodField.text = profile.DOD ?? "Unknown";
        
        if (jobField != null)
            jobField.text = profile.Work ?? "Unknown";
        
        // Fill in the evil deeds (top 3)
        if (profile.EvilList != null && profile.EvilList.Length > 0)
        {
            if (evilTop1Field != null && profile.EvilList.Length > 0)
                evilTop1Field.text = profile.EvilList[0]?.title ?? "None";
            
            if (evilTop2Field != null && profile.EvilList.Length > 1)
                evilTop2Field.text = profile.EvilList[1]?.title ?? "None";
            
            if (evilTop3Field != null && profile.EvilList.Length > 2)
                evilTop3Field.text = profile.EvilList[2]?.title ?? "None";
        }
        else
        {
            if (evilTop1Field != null) evilTop1Field.text = "None";
            if (evilTop2Field != null) evilTop2Field.text = "None";
            if (evilTop3Field != null) evilTop3Field.text = "None";
        }
        
        Debug.Log($"Report filled for character: {profile.Name}");
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
}
