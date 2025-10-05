using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSpawner : MonoBehaviour
{
    
    [Header("Character Settings")]
    [Tooltip("Array of character prefabs to spawn")]
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private int nextCharacterIndex = 0;
    [SerializeField] private bool shouldEndGameAfterAllCharacters = false;
    
    [Header("End Game Settings")]
    [Tooltip("Scene to load when all characters are spawned")]
    [SerializeField] private string endScene = "";
    
    [Header("Status")]
    [Tooltip("Score achieved at the end of the game")]
    [SerializeField]
    public int finalScore = 0;
    
    // Private variables
    private float checkTimer = 0f;
    private bool hasEndedGame = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        checkTimer -= Time.fixedDeltaTime;
        if (checkTimer <= 0)
        {
            checkTimer = 0.25f;
            // Check if there are any characters in the scene (named "Character")
            if (GameObject.Find("Character") == null && characterPrefabs.Length > 0)
            {
                // Check if we should end the game after all characters
                if (shouldEndGameAfterAllCharacters && nextCharacterIndex >= characterPrefabs.Length - 1 && !hasEndedGame)
                {
                    hasEndedGame = true;
                    EndGame();
                    return;
                }
                
                nextCharacterIndex = nextCharacterIndex % characterPrefabs.Length;

                Instantiate(characterPrefabs[nextCharacterIndex]);
            
                nextCharacterIndex = (nextCharacterIndex + 1) % characterPrefabs.Length;
            }
        }
    }
    
    private void EndGame()
    {
        if (!string.IsNullOrEmpty(endScene))
        {
            // Store the score before switching scenes
            PlayerPrefs.SetInt("FinalScore", finalScore);
            PlayerPrefs.Save();
            
            SceneManager.LoadScene(endScene);
        }
    }
}
