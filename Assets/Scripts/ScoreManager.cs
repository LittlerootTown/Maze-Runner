using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance; // Singleton instance
    public TextMeshProUGUI scoreText; // UI text for score
    private int score;

    void Awake()
    {
        // Implement Singleton pattern
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Keep it across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    void Start()
    {
        // Initialize the score if needed
        score = 0;
        UpdateScoreUI();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    public void DeductScore()
    {
        if (score >= 50)
            score -= 50;
        else
            score = 0;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public int GetScore()
    {
        return score;
    }
}
