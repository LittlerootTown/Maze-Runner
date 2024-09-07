using UnityEngine;

public class PickUpCoin : MonoBehaviour
{
    public AudioClip coinPickupSound;  // Assign your coin pickup sound in the Inspector
    private AudioSource audioSource;

    private void Start()
    {
        // Get or add the AudioSource component on the coin object
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = coinPickupSound;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Play the pickup sound
            if (audioSource != null && coinPickupSound != null)
            {
                audioSource.Play();
            }

            // Disable the coin after picking it up, but allow sound to finish
            StartCoroutine(DisableCoinAfterSound());
        }
    }

    private IEnumerator DisableCoinAfterSound()
    {
        // Wait for the sound to finish before disabling the object
        yield return new WaitForSeconds(audioSource.clip.length);
        gameObject.SetActive(false);
    }
}
