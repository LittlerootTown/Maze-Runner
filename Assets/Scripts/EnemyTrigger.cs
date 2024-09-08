using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    private RandomMovement enemy;
    private PlayerCheckpoint playerCheckpoint;  // Reference to PlayerCheckpoint

    void Start()
    {
        enemy = GetComponentInParent<RandomMovement>();
        if (enemy == null)
        {
            Debug.LogError("No RandomMovement script found on parent GameObject.");
        }

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
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger.");

            if (playerCheckpoint != null)
            {
                // Respawn player at the last checkpoint
                playerCheckpoint.Respawn();
            }
            else
            {
                Debug.LogError("PlayerCheckpoint script is not assigned.");
            }

            if (enemy != null)
            {
                enemy.TeleportEnemy();  // Teleport enemy back to its spawn point
            }
            else
            {
                Debug.LogError("RandomMovement script on parent GameObject not found.");
            }
        }
    }
}
