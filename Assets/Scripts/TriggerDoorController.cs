using UnityEngine;

public class PushDoorController : MonoBehaviour
{
    private Rigidbody doorRigidbody;
    public float pushForce = 5f;  // Adjust this to set how hard the door opens

    private void Start()
    {
        // Get the Rigidbody attached to the door
        doorRigidbody = GetComponent<Rigidbody>();

        if (doorRigidbody == null)
        {
            Debug.LogError("No Rigidbody component found on the door.");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate direction of the push
            Vector3 pushDirection = collision.contacts[0].point - transform.position;
            pushDirection = -pushDirection.normalized;

            // Apply force to the door in the direction the player is pushing
            doorRigidbody.AddForce(pushDirection * pushForce, ForceMode.Impulse);
        }
    }
}
