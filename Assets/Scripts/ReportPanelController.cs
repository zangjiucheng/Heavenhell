using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Report : MonoBehaviour
{
    [Header("Profile Data")]
    [SerializeField]
    [Tooltip("Profile to display")]
    private Profile profile;

    [Header("UI Text References")]
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI dobText;
    [SerializeField]
    private TextMeshProUGUI dodText;
    [SerializeField]
    private TextMeshProUGUI workText;
    [SerializeField]
    private TextMeshProUGUI evilThing1Text;
    [SerializeField]
    private TextMeshProUGUI evilThing2Text;
    [SerializeField]
    private TextMeshProUGUI evilThing3Text;

    void Start()
    {
        // Auto-find text components if not assigned (optional fallback)
        AutoAssignTextComponents();
        
        // Update the display initially
        UpdateProfileDisplay();
    }

    private void AutoAssignTextComponents()
    {
        if (nameText == null) nameText = FindTextComponent("NameText");
        if (dobText == null) dobText = FindTextComponent("DOBText");
        if (dodText == null) dodText = FindTextComponent("DODText");
    if (workText == null) workText = FindTextComponent("WorkText");
        if (evilThing1Text == null) evilThing1Text = FindTextComponent("EvilThing1Text");
        if (evilThing2Text == null) evilThing2Text = FindTextComponent("EvilThing2Text");
        if (evilThing3Text == null) evilThing3Text = FindTextComponent("EvilThing3Text");

        // Log warnings if any text component is still missing
        if (nameText == null) Debug.LogWarning("NameText component not assigned or found!");
        if (dobText == null) Debug.LogWarning("DOBText component not assigned or found!");
        if (dodText == null) Debug.LogWarning("DODText component not assigned or found!");
    if (workText == null) Debug.LogWarning("WorkText component not assigned or found!");
        if (evilThing1Text == null) Debug.LogWarning("EvilThing1Text component not assigned or found!");
        if (evilThing2Text == null) Debug.LogWarning("EvilThing2Text component not assigned or found!");
        if (evilThing3Text == null) Debug.LogWarning("EvilThing3Text component not assigned or found!");
    }

    private TextMeshProUGUI FindTextComponent(string childName)
    {
        Transform child = transform.Find(childName);
        if (child != null)
        {
            return child.GetComponent<TextMeshProUGUI>();
        }
        return null;
    }

    private void UpdateProfileDisplay()
    {
        if (profile == null)
        {
            Debug.LogWarning("Profile is not assigned!");
            return;
        }

        // Update character information
        SetTextSafe(nameText, profile.CharacterName);
        SetTextSafe(dobText, profile.DateOfBirth);
        SetTextSafe(dodText, profile.DateOfDeath);
    SetTextSafe(workText, profile.Work);

        // Update evil deeds
        string[] evilThings = profile.GetEvilThings();
        SetTextSafe(evilThing1Text, evilThings.Length > 0 ? evilThings[0] : "N/A");
        SetTextSafe(evilThing2Text, evilThings.Length > 1 ? evilThings[1] : "N/A");
        SetTextSafe(evilThing3Text, evilThings.Length > 2 ? evilThings[2] : "N/A");
    }

    private void SetTextSafe(TextMeshProUGUI textComponent, string value)
    {
        if (textComponent != null)
        {
            textComponent.text = value;
        }
        else
        {
            Debug.LogWarning($"Text component is not assigned for value: {value}");
        }
    }

    // Public method to change the profile at runtime
    public void SetProfile(Profile newProfile)
    {
        profile = newProfile;
        UpdateProfileDisplay();
    }
}
