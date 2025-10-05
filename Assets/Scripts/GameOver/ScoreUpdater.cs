using System;
using UnityEngine;
using TMPro;

public class ScoreUpdater : MonoBehaviour
{
    [Header("Score Display Settings")]
    [Tooltip("TextMeshPro component to display the score")]
    [SerializeField] private TextMeshPro scoreText;
    
    [Tooltip("Prefix text before the score (e.g., 'Your Score: ')")]
    [SerializeField] private string scorePrefix = "Your Score: ";
    
    [Tooltip("Suffix text after the score (e.g., ' points')")]
    [SerializeField] private string scoreSuffix = "";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ScoreUpdater: Start() called");
        UpdateScore();
    }

    private void UpdateScore()
    {
        // Retrieve the score from PlayerPrefs (default to 0 if not found)
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);
        
        Debug.Log($"ScoreUpdater: Retrieved score from PlayerPrefs: {finalScore}");
        Debug.Log($"ScoreUpdater: scoreText is null? {scoreText == null}");
        
        // Update the TextMeshPro text with the score
        if (scoreText != null)
        {
            string displayText = scorePrefix + Math.Round((finalScore / 8.0 * 100), 1).ToString() + scoreSuffix;
            scoreText.text = displayText;
            Debug.Log($"ScoreUpdater: Updated scoreText to: '{displayText}'");
        }
        else
        {
            Debug.LogWarning("ScoreUpdater: scoreText is not assigned in the Inspector!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
