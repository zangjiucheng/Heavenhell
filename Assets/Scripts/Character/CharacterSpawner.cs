using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

public class CharacterSpawner : MonoBehaviour
{
    
    [Header("Character Settings")]
    [Tooltip("Array of character prefabs to spawn")]
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private int nextCharacterIndex = 0;
    [SerializeField] private bool shouldEndGameAfterAllCharacters = false;
    
    [Header("Shuffle Settings")]
    [Tooltip("Shuffle the order of characters at the start")]
    [SerializeField] private bool shuffleCharactersAtStart = true;
    
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
    private int cyclesCount = 0;
    private List<GameObject> characterList = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Initialize the character list
        if (shuffleCharactersAtStart)
        {
            // Populate with random order
            characterList = characterPrefabs.OrderBy(x => Random.value).ToList();
        }
        else
        {
            // Populate in original order
            characterList = new List<GameObject>(characterPrefabs);
        }
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
            if (GameObject.Find("Character") == null && characterList.Count > 0)
            {
                Instantiate(characterList[nextCharacterIndex % characterList.Count]);
            
                nextCharacterIndex++;
                if (nextCharacterIndex > characterList.Count)
                {
                    nextCharacterIndex = 0;
                    cyclesCount++;
                    if (shouldEndGameAfterAllCharacters)
                    {
                        EndGame();
                        hasEndedGame = true;
                    }
                }
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
