using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;
    public int scoreLossOnRespawn = 50; // Score to deduct on respawn
    public AudioClip respawnSound; // Respawn sound clip
    private AudioSource audioSource; // AudioSource to play the sound

    void Start()
    {
        // Set the initial checkpoint to the player's starting position
        lastCheckpointPosition = transform.position;

        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Call this method when the player hits a checkpoint
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        Debug.Log("Checkpoint saved!");
    }

    // Call this method to respawn the player at the last checkpoint and deduct points
    public void Respawn()
    {
        Debug.Log("Respawning player to checkpoint at: " + lastCheckpointPosition);

        // Play respawn sound
        if (respawnSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(respawnSound);
        }
        else
        {
            Debug.LogWarning("Respawn sound or AudioSource is not set!");
        }

        // Respawn the player at the last checkpoint
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.DeductScore();
        }

        CharacterController cc = GetComponent<CharacterController>();

        if (cc != null)
        {
            // Reset position directly
            cc.enabled = false;
            transform.position = lastCheckpointPosition;
            cc.enabled = true;
        }
        else
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.MovePosition(lastCheckpointPosition);
            }
            else
            {
                transform.position = lastCheckpointPosition;
            }
        }

        Debug.Log("Player respawned at last checkpoint!");
    }
}
