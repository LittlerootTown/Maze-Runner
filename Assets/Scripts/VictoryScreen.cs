using UnityEngine;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying score

    void Start()
    {
        // Display the score from ScoreManager
        if (scoreText != null)
        {
            int score = ScoreManager.instance.GetScore();
            scoreText.text = "Final Score: " + score.ToString();
        }

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
