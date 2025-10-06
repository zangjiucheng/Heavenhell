
using UnityEngine;

public class CharacterDisposer : MonoBehaviour
{
    [SerializeField] public Vector2 targetPosition = new Vector2(0, 1);
    [SerializeField] public float moveDuration = 1.0f;
    [SerializeField] public float speed = 5f;
    [SerializeField] private float opacityFadeSpeed = 2f;
    [SerializeField] private float finalDisposeTimer = 1.0f;
    
    [Header("Fancy Effects")]
    [SerializeField] private float rotationSpeed = 360f; // Degrees per second
    [SerializeField] private float scaleMultiplier = 0.5f; // Final scale (0.5 = shrink to 50%)
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 1, 1, 0.5f);
    
    // Editor Invisible
    private float lerpTimer;
    private Vector3 startPosition;
    private Vector3 startScale;
    private float opacity = 1f;
    private float totalRotation = 0f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        startScale = transform.localScale;
        
        // Clear all dialogs
        // Get Dialogs game object
        GameObject dialogs = GameObject.Find("Dialogs");
        if (dialogs != null)
        {
            // Clear all children
            foreach (Transform child in dialogs.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpTimer < moveDuration)
        {
            lerpTimer += Time.deltaTime;
            float progress = lerpTimer / moveDuration;
            
            // Smooth lerp from current position to target (creates deceleration effect)
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
            
            // Add rotation effect
            totalRotation += rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, totalRotation);
            
            // Add scale effect using animation curve
            float scaleValue = scaleCurve.Evaluate(progress);
            transform.localScale = startScale * scaleValue;
        }
        else
        {
            opacity -= opacityFadeSpeed * Time.deltaTime;
            opacity = Mathf.Clamp(opacity, 0f, 1f);
            
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Color currentColor = spriteRenderer.color;
                currentColor.a = opacity;
                spriteRenderer.color = currentColor;
            }
            
            if (opacity <= 0f)
            {
                finalDisposeTimer -= Time.deltaTime;
                if (finalDisposeTimer <= 0f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
    
}
