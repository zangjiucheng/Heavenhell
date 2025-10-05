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
    // Add field for a json file for the character profile with proper label
    [SerializeField]
    [Tooltip("JSON file containing the character profile")]
    private TextAsset characterProfileJson;
    
    private CharacterProfileData profileData;
    
    // Public property to access profile data
    public CharacterProfileData ProfileData => profileData;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadCharacterProfile();
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
        
    }
}
