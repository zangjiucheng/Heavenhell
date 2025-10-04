using UnityEngine;
using System;

[System.Serializable]
public class Profile
{
    [Header("Character Information")]
    [SerializeField]
    [Tooltip("Character's name")]
    private string characterName;

    [SerializeField]
    [Tooltip("Date of Birth")]
    private string dateOfBirth;

    [SerializeField]
    [Tooltip("Date of Death")]
    private string dateOfDeath;

    [SerializeField]
    [Tooltip("Work / occupation")]
    private string work;

    [Header("Visual")]
    [SerializeField]
    [Tooltip("Character portrait image")]
    private Sprite characterImage;

    [Header("Evil Deeds")]
    [SerializeField]
    [Tooltip("First evil thing they did")]
    private string evilThing1;

    [SerializeField]
    [Tooltip("Second evil thing they did")]
    private string evilThing2;

    [SerializeField]
    [Tooltip("Third evil thing they did")]
    private string evilThing3;

    // Properties for accessing the data
    public string CharacterName => characterName;
    public string DateOfBirth => dateOfBirth;
    public string DateOfDeath => dateOfDeath;
    public string Work => work;
    public Sprite CharacterImage => characterImage;
    public string EvilThing1 => evilThing1;
    public string EvilThing2 => evilThing2;
    public string EvilThing3 => evilThing3;

    // Constructor
    public Profile(string name, string dob, string dod, string work, Sprite image, string evil1, string evil2, string evil3)
    {
        characterName = name;
        dateOfBirth = dob;
        dateOfDeath = dod;
        this.work = work;
        characterImage = image;
        evilThing1 = evil1;
        evilThing2 = evil2;
        evilThing3 = evil3;
    }

    // Default constructor for Unity serialization
    public Profile()
    {
        characterName = "";
        dateOfBirth = "";
        dateOfDeath = "";
        work = "";
        characterImage = null;
        evilThing1 = "";
        evilThing2 = "";
        evilThing3 = "";
    }

    // Method to get all evil things as an array
    public string[] GetEvilThings()
    {
        return new string[] { evilThing1, evilThing2, evilThing3 };
    }

    // Method to display profile info
    public string GetProfileSummary()
    {
        return $"Name: {characterName}\n" +
               $"DOB: {dateOfBirth}\n" +
               $"DOD: {dateOfDeath}\n" +
               $"Work: {work}\n" +
               $"Evil Deeds:\n1. {evilThing1}\n2. {evilThing2}\n3. {evilThing3}";
    }

    // Load profile from JSON file
    public static Profile LoadFromJSON(string jsonFilePath)
    {
        try
        {
            if (System.IO.File.Exists(jsonFilePath))
            {
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);
                ProfileData data = JsonUtility.FromJson<ProfileData>(jsonContent);
                
                Profile profile = new Profile();
                profile.characterName = data.characterName;
                profile.dateOfBirth = data.dateOfBirth;
                profile.dateOfDeath = data.dateOfDeath;
                profile.work = data.work;
                profile.evilThing1 = data.evilThing1;
                profile.evilThing2 = data.evilThing2;
                profile.evilThing3 = data.evilThing3;
                
                // Load sprite if image path is provided
                if (!string.IsNullOrEmpty(data.imagePath))
                {
                    profile.characterImage = Resources.Load<Sprite>(data.imagePath);
                    if (profile.characterImage == null)
                    {
                        Debug.LogWarning($"Could not load sprite from path: {data.imagePath}");
                    }
                }
                
                Debug.Log($"Profile loaded successfully from {jsonFilePath}");
                return profile;
            }
            else
            {
                Debug.LogError($"JSON file not found: {jsonFilePath}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading profile from JSON: {e.Message}");
            return null;
        }
    }

    // Load profile from TextAsset (for files in Resources folder)
    public static Profile LoadFromTextAsset(TextAsset jsonFile)
    {
        try
        {
            if (jsonFile == null)
            {
                Debug.LogError("TextAsset is null!");
                return null;
            }

            ProfileData data = JsonUtility.FromJson<ProfileData>(jsonFile.text);
            
            Profile profile = new Profile();
            profile.characterName = data.characterName;
            profile.dateOfBirth = data.dateOfBirth;
            profile.dateOfDeath = data.dateOfDeath;
                profile.work = data.work;
            profile.evilThing1 = data.evilThing1;
            profile.evilThing2 = data.evilThing2;
            profile.evilThing3 = data.evilThing3;
            
            // Load sprite if image path is provided
            if (!string.IsNullOrEmpty(data.imagePath))
            {
                profile.characterImage = Resources.Load<Sprite>(data.imagePath);
                if (profile.characterImage == null)
                {
                    Debug.LogWarning($"Could not load sprite from path: {data.imagePath}");
                }
            }
            
            Debug.Log($"Profile loaded successfully from TextAsset");
            return profile;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading profile from TextAsset: {e.Message}");
            return null;
        }
    }

    // Export profile to JSON string
    public string ToJSON()
    {
        ProfileData data = new ProfileData
        {
            characterName = this.characterName,
            dateOfBirth = this.dateOfBirth,
            dateOfDeath = this.dateOfDeath,
            work = this.work,
            evilThing1 = this.evilThing1,
            evilThing2 = this.evilThing2,
            evilThing3 = this.evilThing3,
            imagePath = "" // Set manually if needed
        };
        
        return JsonUtility.ToJson(data, true);
    }
}

// Helper class for JSON serialization (without Sprite)
[System.Serializable]
public class ProfileData
{
    public string characterName;
    public string dateOfBirth;
    public string dateOfDeath;
    public string work;
    public string imagePath; // Path to sprite in Resources folder
    public string evilThing1;
    public string evilThing2;
    public string evilThing3;
}
