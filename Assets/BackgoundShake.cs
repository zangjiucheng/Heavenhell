using UnityEngine;

public class BackgoundShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField]
    [Tooltip("Minimum intensity of the shake")]
    private float minShakeIntensity = 0.05f;
    
    [SerializeField]
    [Tooltip("Maximum intensity of the shake")]
    private float maxShakeIntensity = 0.15f;
    
    [SerializeField]
    [Tooltip("Duration of each shake")]
    private float shakeDuration = 0.3f;
    
    [SerializeField]
    [Tooltip("Minimum time between shakes")]
    private float minTimeBetweenShakes = 1f;
    
    [SerializeField]
    [Tooltip("Maximum time between shakes")]
    private float maxTimeBetweenShakes = 2f;
    
    private Vector3 originalPosition;
    private float nextShakeTime;
    private bool isShaking = false;
    private float shakeTimer;
    private float currentShakeIntensity;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Store the original position
        originalPosition = transform.localPosition;
        
        // Schedule the first shake
        ScheduleNextShake();
    }

    // Update is called once per frame
    void Update()
    {
        if (isShaking)
        {
            // Perform shake
            shakeTimer += Time.deltaTime;
            
            if (shakeTimer < shakeDuration)
            {
                // Generate random offset based on intensity
                float offsetX = Random.Range(-currentShakeIntensity, currentShakeIntensity);
                float offsetY = Random.Range(-currentShakeIntensity, currentShakeIntensity);
                
                transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);
            }
            else
            {
                // Shake finished, return to original position
                transform.localPosition = originalPosition;
                isShaking = false;
                
                // Schedule next shake
                ScheduleNextShake();
            }
        }
        else
        {
            // Check if it's time to shake
            if (Time.time >= nextShakeTime)
            {
                StartShake();
            }
        }
    }
    
    private void ScheduleNextShake()
    {
        // Randomly determine when the next shake will occur
        float waitTime = Random.Range(minTimeBetweenShakes, maxTimeBetweenShakes);
        nextShakeTime = Time.time + waitTime;
    }
    
    private void StartShake()
    {
        isShaking = true;
        shakeTimer = 0f;
        
        // Randomize shake intensity for variety
        currentShakeIntensity = Random.Range(minShakeIntensity, maxShakeIntensity);
        
        Debug.Log($"Background shake started with intensity: {currentShakeIntensity}");
    }
    
    // Public method to trigger a shake manually (can be called from other scripts)
    public void TriggerShake(float intensity = -1f)
    {
        if (intensity > 0)
        {
            currentShakeIntensity = intensity;
        }
        else
        {
            currentShakeIntensity = Random.Range(minShakeIntensity, maxShakeIntensity);
        }
        
        StartShake();
    }
}
