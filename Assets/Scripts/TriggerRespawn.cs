using UnityEngine;

public class TriggerRespawn : MonoBehaviour
{
    private PlayerCheckpoint playerCheckpoint;  // Reference to the PlayerCheckpoint script

    void Start()
    {
        // Find the player and the PlayerCheckpoint script
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerCheckpoint = player.GetComponent<PlayerCheckpoint>();
            if (playerCheckpoint == null)
            {
                Debug.LogError("No PlayerCheckpoint script found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered respawn trigger.");

            // Call the Respawn method from the PlayerCheckpoint script
            if (playerCheckpoint != null)
            {
                playerCheckpoint.Respawn();
            }
            else
            {
                Debug.LogError("PlayerCheckpoint script not assigned.");
            }
        }
    }
}
