using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private Vector3 lastCheckpointPosition;

    void Start()
    {
        // Set the initial checkpoint to the player's starting position
        lastCheckpointPosition = transform.position;
    }

    // Call this method when the player hits a checkpoint
    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        lastCheckpointPosition = checkpointPosition;
        Debug.Log("Checkpoint saved!");
    }

    // Call this method to respawn the player at the last checkpoint
    public void Respawn()
    {
        Debug.Log("Respawning player to checkpoint at: " + lastCheckpointPosition);

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
