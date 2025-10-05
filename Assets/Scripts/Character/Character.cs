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

public enum Expression
{
    Normal,
    Happy,
    Sad,
    Fear
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

    [Header("Character Expressions")] [SerializeField] [Tooltip("Sprite Renderer component to display expressions")]
    private SpriteRenderer spriteRenderer;

    [SerializeField] [Tooltip("Normal or default expression")]
    private Sprite normalExpression;

    [SerializeField] [Tooltip("Happy or positive expression")]
    private Sprite happyExpression;

    [SerializeField] [Tooltip("Sad or negative expression")]
    private Sprite sadExpression;

    [SerializeField] [Tooltip("Fearful or scared expression")]
    private Sprite fearExpression;

    [Header("UI Elements")]
    [SerializeField]
    [Tooltip("Report button prefab to spawn when appear animation is finished")]
    private GameObject reportButtonPrefab;
    
    [SerializeField]
    [Tooltip("Document button prefab to spawn when appear animation is finished")]
    private GameObject documentButtonPrefab;

    [Header("Audio")] [SerializeField] [Tooltip("Audio source component for playing sounds")]
    private AudioSource audioSource;

    [SerializeField] [Tooltip("Sad talk sound effects")]
    private AudioClip[] sadTalkSounds;

    [SerializeField] [Tooltip("Normal talk sound effects")]
    private AudioClip[] normalTalkSounds;

    [SerializeField] [Tooltip("Happy talk sound effects")]
    private AudioClip[] happyTalkSounds;

    private GameObject reportButtonInstance;
    private GameObject documentButtonInstance;
    
    // Public properties to access expressions
    public Sprite NormalExpression => normalExpression;
    public Sprite HappyExpression => happyExpression;
    public Sprite SadExpression => sadExpression;
    public Sprite FearExpression => fearExpression;

    private float expressionTimer = 0f;
    private float happyDuration = 0f;
    private bool isShowingHappy = false;
    private bool hasSpawnedReportButton = false;

    // Talk animation variables
    private float talkTimer = 0f;
    private bool isTalking = false;
    private float talkBounceDuration = 0.5f;
    private float talkBounceHeight = 0.025f; // 2.5% height
    private Vector3 originalScale;
    private Vector3 originalPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCharacterProfile();

        // Get SpriteRenderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Store original scale and position for talk animation
        originalScale = transform.localScale;
        originalPosition = transform.localPosition;
        
        // Rename self to "Character"
        gameObject.name = "Character";
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
                Debug.LogWarning(
                    $"EvilList should contain exactly 3 items. Found: {(profileData.EvilList?.Length ?? 0)}");
            }

            Debug.Log(
                $"Successfully loaded profile for {profileData.Name}. Work: {profileData.Work}, Death: {profileData.DeathReason}");
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

        // Handle talk bounce animation
        if (isTalking)
        {
            talkTimer += Time.deltaTime;

            if (talkTimer <= talkBounceDuration)
            {
                // Calculate bounce using a sine wave for smooth animation
                // One complete bounce = one full sine cycle (0 to 2π)
                float progress = talkTimer / talkBounceDuration;
                float bounceAmount = Mathf.Sin(progress * Mathf.PI * 2f) * talkBounceHeight;

                // Apply scale change to y-axis only
                Vector3 newScale = originalScale;
                newScale.y = originalScale.y * (1f + bounceAmount);
                transform.localScale = newScale;

                // Apply position offset to y-axis (bounce up and down)
                Vector3 newPosition = originalPosition;
                newPosition.y = originalPosition.y + (bounceAmount * 2f); // Multiply for more visible movement
                transform.localPosition = newPosition;
            }
            else
            {
                // Animation finished, reset to original scale and position
                transform.localScale = originalScale;
                transform.localPosition = originalPosition;
                isTalking = false;
                talkTimer = 0f;
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
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer is not assigned on Character!");
            return;
        }

        if (expression == null)
        {
            Debug.LogWarning("Trying to set a null sprite expression!");
            return;
        }

        spriteRenderer.sprite = expression;
        Debug.Log($"Character expression changed to: {expression.name}");
    }

    /// <summary>
    /// Set the character's current expression using the Expression enum
    /// </summary>
    /// <param name="expression">The Expression enum value</param>
    public void SetExpression(Expression expression)
    {
        Debug.Log($"Setting expression to: {expression}");

        switch (expression)
        {
            case Expression.Normal:
                if (normalExpression == null) Debug.LogWarning("Normal expression sprite is not assigned!");
                SetExpression(normalExpression);
                break;
            case Expression.Happy:
                if (happyExpression == null) Debug.LogWarning("Happy expression sprite is not assigned!");
                SetExpression(happyExpression);
                break;
            case Expression.Sad:
                if (sadExpression == null) Debug.LogWarning("Sad expression sprite is not assigned!");
                SetExpression(sadExpression);
                break;
            case Expression.Fear:
                if (fearExpression == null) Debug.LogWarning("Fear expression sprite is not assigned!");
                SetExpression(fearExpression);
                break;
            default:
                SetExpression(normalExpression);
                break;
        }
    }

    private void SpawnReportButton()
    {
        if (!hasSpawnedReportButton)
        {
            // Instantiate the report button prefab
            if (reportButtonPrefab != null)
            {
                reportButtonInstance = Instantiate(reportButtonPrefab, transform.position, Quaternion.identity);
                reportButtonInstance.transform.rotation = Quaternion.Euler(33.6f, 2.16f, -375f);
                Debug.Log("Report button spawned");
            }
            
            // Instantiate the document button prefab
            if (documentButtonPrefab != null)
            {
                documentButtonInstance = Instantiate(documentButtonPrefab, transform.position, Quaternion.identity);
                documentButtonInstance.transform.rotation = Quaternion.Euler(33.6f, 2.16f, -375f);
                Debug.Log("Document button spawned");
            }
            
            hasSpawnedReportButton = true;
        }
    }

    // Call this method to trigger the appear animation finished logic
    public void OnAppearAnimationFinished()
    {
        // Spawn the report button when the appear animation is finished
        SpawnReportButton();
    }

    /// <summary>
    /// Trigger a talk animation that bounces the character's y-scale and y-position once
    /// </summary>
    /// <param name="bounceDuration">Duration of the bounce animation in seconds (default: 0.5s)</param>
    /// <param name="bounceHeight">Height of the bounce as a percentage (default: 0.025 = 2.5%)</param>
    public void Talk(float bounceDuration = 0.5f, float bounceHeight = -0.025f)
    {
        talkBounceDuration = bounceDuration;
        talkBounceHeight = bounceHeight;
        talkTimer = 0f;
        isTalking = true;

        // Store current position as the original position for this animation
        originalPosition = transform.localPosition;

        Debug.Log($"Character {profileData?.Name ?? "Unknown"} is talking with bounce animation!");
    }

    /// <summary>
    /// Play a random talk sound effect based on emotion
    /// </summary>
    /// <param name="emotion">Emotion type: -1 for sad, 0 for normal, 1 for happy</param>
    /// <param name="pitch">Pitch of the audio playback (default: 1.0)</param>
    public void TalkSound(int emotion, float pitch = 1f)
    {
        // Get audio source if not assigned
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource component not found on Character! Cannot play talk sound.");
                return;
            }
        }

        // Select the appropriate sound array based on emotion
        AudioClip[] soundArray;
        string emotionName;

        switch (emotion)
        {
            case -1:
                soundArray = sadTalkSounds;
                emotionName = "sad";
                break;
            case 0:
                soundArray = normalTalkSounds;
                emotionName = "normal";
                break;
            case 1:
                soundArray = happyTalkSounds;
                emotionName = "happy";
                break;
            default:
                Debug.LogWarning($"Invalid emotion value: {emotion}. Use -1 (sad), 0 (normal), or 1 (happy).");
                return;
        }

        // Check if the array has sounds
        if (soundArray == null || soundArray.Length == 0)
        {
            Debug.LogWarning($"{emotionName} talk sounds array is empty or not assigned!");
            return;
        }

        // Randomly select a sound from the array
        int randomIndex = Random.Range(0, soundArray.Length);
        AudioClip selectedClip = soundArray[randomIndex];

        if (selectedClip == null)
        {
            Debug.LogWarning($"Selected audio clip at index {randomIndex} is null!");
            return;
        }

        // Set pitch and play the sound
        audioSource.pitch = pitch;
        audioSource.PlayOneShot(selectedClip);

        Debug.Log($"Playing {emotionName} talk sound: {selectedClip.name} at pitch {pitch}");
    }

    public void SendToHell()
    {
        // Add Disposer component to character
        var disposer = gameObject.AddComponent<CharacterDisposer>();
        disposer.targetPosition = new Vector2(5.0f, 1.2f);
        disposer.moveDuration = 1.5f;
        disposer.speed = 2.5f;
    }
    
    public void SendToHeaven()
    {
        // Add Disposer component to character
        var disposer = gameObject.AddComponent<CharacterDisposer>();
        disposer.targetPosition = new Vector2(-5.0f, 1.2f);
        disposer.moveDuration = 1.5f;
        disposer.speed = 2f;
    }
}