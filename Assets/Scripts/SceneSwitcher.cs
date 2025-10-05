using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class SceneSwitcher : MonoBehaviour, IPointerClickHandler
{
    [Header("Scene Settings")]
    [SerializeField]
    [Tooltip("Name of the scene to load (must be added in Build Settings)")]
    private string sceneName = "";
    
    [Header("UI References")]
    [SerializeField]
    [Tooltip("TextMeshPro component to use as a button")]
    private TMP_Text buttonText;
    
    [Header("Visual Feedback")]
    [SerializeField]
    [Tooltip("Color when hovering over the button")]
    private Color hoverColor = Color.yellow;
    
    private Color normalColor;
    
    void Start()
    {
        // Auto-find TextMeshPro if not assigned
        if (buttonText == null)
        {
            buttonText = GetComponent<TMP_Text>();
        }
        
        // Store the original color
        if (buttonText != null)
        {
            normalColor = buttonText.color;
        }
    }
    
    // Call this function to load the scene set in the Inspector
    public void LoadScene()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Scene name is not set in SceneSwitcher!");
            return;
        }
        
        Debug.Log($"Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }
    
    // Call this function to load a new scene by name (overrides Inspector value)
    public void LoadSceneByName(string sceneNameOverride)
    {
        if (string.IsNullOrEmpty(sceneNameOverride))
        {
            Debug.LogWarning("Scene name parameter is empty!");
            return;
        }
        
        Debug.Log($"Loading scene: {sceneNameOverride}");
        SceneManager.LoadScene(sceneNameOverride);
    }
    
    // Handle click on the TextMeshPro text
    public void OnPointerClick(PointerEventData eventData)
    {
        LoadScene();
    }
}
