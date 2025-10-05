
using UnityEngine;

public class CharacterDisposer : MonoBehaviour
{
    [SerializeField] public Vector2 targetPosition = new Vector2(0, 1);
    [SerializeField] public float moveDuration = 1.0f;
    [SerializeField] public float speed = 5f;
    [SerializeField] private float opacityFadeSpeed = 2f;
    [SerializeField] private float finalDisposeTimer = 1.0f;
    
    // Editor Invisible
    private float lerpTimer;
    private Vector3 startPosition;
    private float opacity = 1f;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosition = transform.position;
        // Clear all dialogs
        // Get Dialogs game object
        GameObject dialogs = GameObject.Find("Dialogs");
        // Clear all children
        foreach (Transform child in dialogs.transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (lerpTimer < moveDuration)
        {
            lerpTimer += Time.deltaTime;
            
            // Smooth lerp from current position to target (creates deceleration effect)
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
        }
        else
        {
            opacity -= opacityFadeSpeed * Time.deltaTime;
            opacity = Mathf.Clamp(opacity, 0f, 1f);
            Color currentColor = GetComponent<SpriteRenderer>().color;
            currentColor.a = opacity;
            GetComponent<SpriteRenderer>().color = currentColor;
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
