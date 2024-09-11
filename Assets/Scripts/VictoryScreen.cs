using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText; // Reference to the TextMeshProUGUI component for displaying score

    void Start()
    {
        // Display the score from ScoreManager
        if (scoreText != null)
        {
            int score = ScoreManager.instance.GetScore();
            scoreText.text = "Final Score: " + score.ToString() + "/1500";
        }

        // Unlock the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void RestartGame()
    {
        // Reset the score
        ScoreManager.instance.ResetScore();

        // Destroy the existing ScoreManager instance
        ScoreManager.instance.DestroyInstance();

        // Load the new game scene or restart the current scene
        SceneManager.LoadScene("MainMenu");
    }
}
