using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    public int nextSceneIndex; // Set this in the Inspector

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the "Player" tag
        if (other.CompareTag("Player"))
        {
            // Load the next scene by index
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}
