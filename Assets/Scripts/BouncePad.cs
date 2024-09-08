using UnityEngine;

public class BouncePad : MonoBehaviour
{
    public float bounceForce = 10f;  // The force applied to the player when they collide with the pad
    public AudioClip bounceSound;    // Optional: sound to play when the player bounces
    private AudioSource audioSource;

    private void Start()
    {
        // Add an AudioSource if you want a sound to play when the bounce happens
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collision detected with: " + hit.collider.name);

        // Check if the object colliding is the player
        if (hit.collider.CompareTag("Player"))
        {
            Debug.Log("Player detected. Applying bounce force.");

            // Send a message or call a method on the player to handle bouncing
            hit.collider.SendMessage("ApplyBounce", bounceForce, SendMessageOptions.DontRequireReceiver);

            // Play bounce sound if available
            if (bounceSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(bounceSound);
            }
        }
    }
}
