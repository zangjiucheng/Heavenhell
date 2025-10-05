using UnityEngine;

public class CharacterAppear : MonoBehaviour
{
    [Header("Pop-up Animation Settings")]
    [SerializeField]
    [Tooltip("Vertical offset to move the character down initially")]
    private float offset = 2f;

    [SerializeField]
    [Tooltip("Duration of the pop-up animation in seconds")]
    private float duration = 0.5f;

    [SerializeField]
    [Tooltip("Sprite Renderer component for transparency animation")]
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    [Tooltip("Audio source for playing spawn sound effect")]
    private AudioSource audioSource;
    
    [SerializeField]
    [Tooltip("Character component for expression control")]
    private Character character;

    private Vector3 originalPosition;
    private Vector3 startPosition;
    private float timer = 0f;
    private bool isAnimating = false;

    void Start()
    {
        // Get SpriteRenderer if not assigned
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        // Get Character component if not assigned
        if (character == null)
        {
            character = GetComponent<Character>();
        }

        // Store the original position
        originalPosition = transform.position;
        
        // Move character down by offset
        startPosition = originalPosition - new Vector3(0, offset, 0);
        transform.position = startPosition;

        // Set initial transparency to 0 if Sprite Renderer is assigned
        if (spriteRenderer != null)
        {
            Color color = spriteRenderer.color;
            color.a = 0f;
            spriteRenderer.color = color;
        }

        // Play spawn sound effect
        if (audioSource != null)
        {
            audioSource.Play();
        }
        
        // Start expression timing (happy for 150% of duration, then normal)
        if (character != null)
        {
            character.StartHappyThenNormal(duration);
        }

        // Start the pop-up animation
        isAnimating = true;
    }

    void Update()
    {
        if (isAnimating)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // Lerp position from start position to original position
            transform.position = Vector3.Lerp(startPosition, originalPosition, t);

            // Lerp transparency from 0 to 1
            if (spriteRenderer != null)
            {
                Color color = spriteRenderer.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                spriteRenderer.color = color;
            }

            // Stop animating when duration is reached
            if (timer >= duration)
            {
                isAnimating = false;
                
                // Ensure final position and transparency are exact
                transform.position = originalPosition;
                if (spriteRenderer != null)
                {
                    Color color = spriteRenderer.color;
                    color.a = 1f;
                    spriteRenderer.color = color;
                }
            }
        }
    }
}
