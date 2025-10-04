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
    [Tooltip("Reason for death")]
    private string deathReason;

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
    public string DeathReason => deathReason;
    public string EvilThing1 => evilThing1;
    public string EvilThing2 => evilThing2;
    public string EvilThing3 => evilThing3;

    // Constructor
    public Profile(string name, string dob, string dod, string deathReason, string evil1, string evil2, string evil3)
    {
        characterName = name;
        dateOfBirth = dob;
        dateOfDeath = dod;
        this.deathReason = deathReason;
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
        deathReason = "";
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
               $"Death Reason: {deathReason}\n" +
               $"Evil Deeds:\n1. {evilThing1}\n2. {evilThing2}\n3. {evilThing3}";
    }
}
