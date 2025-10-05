using UnityEngine;

public class CharacterSpawner : MonoBehaviour
{
    [Header("Character Settings")]
    [Tooltip("Array of character prefabs to spawn")]
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private int nextCharacterIndex = 0;
    
    // Private variables
    private float checkTimer = 0f;

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
                nextCharacterIndex = nextCharacterIndex % characterPrefabs.Length;

                Instantiate(characterPrefabs[nextCharacterIndex]);
            
                nextCharacterIndex = (nextCharacterIndex + 1) % characterPrefabs.Length;
            }
        }
    }
}
