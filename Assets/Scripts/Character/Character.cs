using UnityEngine;

[System.Serializable]
public class EvilDeed
{
    public string title;
    public string explain;
}

[System.Serializable]
public class CharacterProfileData
{
    public string Name;
    public string DOB;
    public string DOD;
    public string Work;
    public string Introduction;
    public EvilDeed[] EvilList;
    public string DeathReason;
    public bool FinalDecision;
}

public class Character : MonoBehaviour
{
    [Header("Character Profile")]
    // Add field for a json file for the character profile with proper label
    [SerializeField]
    [Tooltip("JSON file containing the character profile")]
    private TextAsset characterProfileJson;
    
    private CharacterProfileData profileData;
    
    // Public property to access profile data
    public CharacterProfileData ProfileData => profileData;
    
    [Header("Character Expressions")]
    [SerializeField]
    [Tooltip("Sprite Renderer component to display expressions")]
    private SpriteRenderer spriteRenderer;
    
    [SerializeField]
    [Tooltip("Normal or default expression")]
    private Sprite normalExpression;
    
    [SerializeField]
    [Tooltip("Happy or positive expression")]
    private Sprite happyExpression;
    
    [SerializeField]
    [Tooltip("Sad or negative expression")]
    private Sprite sadExpression;
    
    [SerializeField]
    [Tooltip("Fearful or scared expression")]
    private Sprite fearExpression;
    
    [Header("UI Elements")]
    [SerializeField]
    [Tooltip("Report button prefab to spawn when appear animation is finished")]
    private GameObject reportButtonPrefab;
    
    private GameObject reportButtonInstance;
    
    // Public properties to access expressions
    public Sprite NormalExpression => normalExpression;
    public Sprite HappyExpression => happyExpression;
    public Sprite SadExpression => sadExpression;
    public Sprite FearExpression => fearExpression;
    
    private float expressionTimer = 0f;
    private float happyDuration = 0f;
    private bool isShowingHappy = false;
    private bool hasSpawnedReportButton = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCharacterProfile();
        
        // Get SpriteRenderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    private void LoadCharacterProfile()
    {
        if (characterProfileJson == null)
        {
            Debug.LogWarning("Character profile JSON is not assigned. Using placeholder data.");
            profileData = new CharacterProfileData
            {
                Name = "Unknown",
                DOB = "Unknown",
                DOD = "Unknown",
                Work = "Unknown",
                Introduction = "No introduction available.",
                EvilList = new EvilDeed[3]
                {
                    new EvilDeed { title = "None", explain = "N/A" },
                    new EvilDeed { title = "None", explain = "N/A" },
                    new EvilDeed { title = "None", explain = "N/A" }
                },
                DeathReason = "Unknown",
                FinalDecision = false
            };
            return;
        }

        try
        {
            profileData = JsonUtility.FromJson<CharacterProfileData>(characterProfileJson.text);
            
            if (profileData == null)
            {
                throw new System.Exception("Failed to parse JSON data.");
            }
            
            // Validate that EvilList has exactly 3 items
            if (profileData.EvilList == null || profileData.EvilList.Length != 3)
            {
                Debug.LogWarning($"EvilList should contain exactly 3 items. Found: {(profileData.EvilList?.Length ?? 0)}");
            }
            
            Debug.Log($"Successfully loaded profile for {profileData.Name}. Work: {profileData.Work}, Death: {profileData.DeathReason}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load character profile: {e.Message}. Using placeholder data.");
            profileData = new CharacterProfileData
            {
                Name = "Unknown",
                DOB = "Unknown",
                DOD = "Unknown",
                Work = "Unknown",
                Introduction = "No introduction available.",
                EvilList = new EvilDeed[3]
                {
                    new EvilDeed { title = "None", explain = "N/A" },
                    new EvilDeed { title = "None", explain = "N/A" },
                    new EvilDeed { title = "None", explain = "N/A" }
                },
                DeathReason = "Unknown",
                FinalDecision = false
            };
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle expression timing
        if (isShowingHappy && happyDuration > 0f)
        {
            expressionTimer += Time.deltaTime;
            
            if (expressionTimer >= happyDuration)
            {
                // Switch to normal expression
                SetExpression(normalExpression);
                isShowingHappy = false;
                
                // Spawn the report button when the appear animation is finished
                SpawnReportButton();
            }
        }
    }
    
    /// <summary>
    /// Start showing happy expression for the first 150% of the appearing duration, then switch to normal
    /// </summary>
    /// <param name="appearDuration">The duration of the appearing animation</param>
    public void StartHappyThenNormal(float appearDuration)
    {
        happyDuration = appearDuration * 1.5f;
        expressionTimer = 0f;
        isShowingHappy = true;
        SetExpression(happyExpression);
    }
    
    /// <summary>
    /// Set the character's current expression
    /// </summary>
    /// <param name="expression">The sprite to display</param>
    public void SetExpression(Sprite expression)
    {
        if (spriteRenderer != null && expression != null)
        {
            spriteRenderer.sprite = expression;
        }
    }

    private void SpawnReportButton()
    {
        if (reportButtonPrefab != null && !hasSpawnedReportButton)
        {
            // Instantiate the report button prefab
            reportButtonInstance = Instantiate(reportButtonPrefab, transform.position, Quaternion.identity);
            reportButtonInstance.transform.rotation = Quaternion.Euler(33.6f, 2.16f, -375f);
            hasSpawnedReportButton = true;
        }
    }

    // Call this method to trigger the appear animation finished logic
    public void OnAppearAnimationFinished()
    {
        // Spawn the report button when the appear animation is finished
        SpawnReportButton();
    }
}
