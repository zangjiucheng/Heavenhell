using UnityEngine;
using UnityEngine.UI;

public class CharacterScript : MonoBehaviour
{
    [Header("Pop-up Animation Settings")]
    [SerializeField]
    [Tooltip("Vertical offset to move the character down initially")]
    private float offset = 100f;

    [SerializeField]
    [Tooltip("Duration of the pop-up animation in seconds")]
    private float duration = 0.5f;

    [SerializeField]
    [Tooltip("UI Image component for transparency animation")]
    private Image uiImage;

    [SerializeField]
    [Tooltip("Audio source for playing spawn sound effect")]
    private AudioSource audioSource;

    private Vector3 originalPosition;
    private Vector3 startPosition;
    private float timer = 0f;
    private bool isAnimating = false;

    void Start()
    {
        // Store the original position
        originalPosition = transform.position;
        
        // Move character down by offset
        startPosition = originalPosition - new Vector3(0, offset, 0);
        transform.position = startPosition;

        // Set initial transparency to 0 if UI Image is assigned
        if (uiImage != null)
        {
            Color color = uiImage.color;
            color.a = 0f;
            uiImage.color = color;
        }

        // Play spawn sound effect
        if (audioSource != null)
        {
            audioSource.Play();
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
            if (uiImage != null)
            {
                Color color = uiImage.color;
                color.a = Mathf.Lerp(0f, 1f, t);
                uiImage.color = color;
            }

            // Stop animating when duration is reached
            if (timer >= duration)
            {
                isAnimating = false;
                
                // Ensure final position and transparency are exact
                transform.position = originalPosition;
                if (uiImage != null)
                {
                    Color color = uiImage.color;
                    color.a = 1f;
                    uiImage.color = color;
                }
            }
        }
    }
}
