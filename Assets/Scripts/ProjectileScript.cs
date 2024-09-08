using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        // Access the PlayerCheckpoint script on the player
        PlayerCheckpoint playerCheckpoint = collision.gameObject.GetComponent<PlayerCheckpoint>();
        if (playerCheckpoint != null)
        {
            // Send the player back to the last checkpoint
            playerCheckpoint.Respawn();
        }
    }

    // Destroy the projectile on any collision
    Destroy(gameObject);
}

}
