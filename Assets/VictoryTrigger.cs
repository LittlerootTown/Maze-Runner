using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryTrigger : MonoBehaviour
{
    public Canvas canvas; // Reference to the Victory Canvas

    private void Start()
    {
        canvas.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowVictoryWindow();
        }
    }

    private void ShowVictoryWindow()
    {
        if (canvas != null)
        {
            canvas.enabled = true;
            Time.timeScale = 0f; // Pause the game
            Cursor.lockState = CursorLockMode.None; // Unlock the mouse cursor
            Cursor.visible = true; // Make the cursor visible
        }
    }

    public void ResumeGame()
    {
        canvas.enabled = true;
        Time.timeScale = 1f; // Resume the game
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor
        Cursor.visible = false; // Hide the cursor
    }

    // This method should be called by the "Play Again" button in the UI
    public void PlayAgain()
    {
        Time.timeScale = 1f; // Resume the game time
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor
        Cursor.visible = false; // Hide the cursor

        // Reload the current scene to reset the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
