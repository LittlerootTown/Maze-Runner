using UnityEngine;

public class CharacterControllerDoorPusher : MonoBehaviour
{
    public float pushForce = 5f;  // Adjust the force applied to the door

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if the object hit is the door
        if (hit.gameObject.CompareTag("Door"))
        {
            Debug.Log("Player collided with the door.");

            // Get the door's Rigidbody component
            Rigidbody doorRigidbody = hit.gameObject.GetComponent<Rigidbody>();
            if (doorRigidbody != null)
            {
                // Calculate the push direction based on the player's movement direction
                Vector3 pushDirection = hit.moveDirection.normalized;

                // Apply force to the door's Rigidbody in the direction of the player's movement
                doorRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
                Debug.Log("Force applied to the door.");
            }
            else
            {
                Debug.LogError("No Rigidbody found on the door.");
            }
        }
    }
}
