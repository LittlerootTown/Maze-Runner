using System.Collections;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile prefab
    public Transform firePoint;  // The position where the projectile will be fired from
    public float fireInterval = 2f;  // Time between shots
    public float projectileSpeed = 10f;  // Speed of the projectile
    public AudioClip fireSound;  // The sound to play when firing
    public float fireSoundVolume = 0.5f; // Volume of the firing sound
    public float fireOffset = 0f;  // Offset to start firing after

    private AudioSource audioSource;

    private void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Set the AudioSource to 3D sound
        audioSource.spatialBlend = 1.0f;  // 1.0 makes it fully 3D
        audioSource.rolloffMode = AudioRolloffMode.Linear; // Adjust how the sound fades with distance

        // Optionally set the min and max distance
        audioSource.minDistance = 5f;  // Distance at which the sound starts to fade
        audioSource.maxDistance = 20f; // Distance at which the sound is inaudible

        // Start the automatic firing of the cannon with an offset
        StartCoroutine(FireProjectilesWithOffset());
    }

    IEnumerator FireProjectilesWithOffset()
    {
        // Wait for the offset time before starting
        yield return new WaitForSeconds(fireOffset);

        while (true)
        {
            Fire();  // Fire a projectile
            yield return new WaitForSeconds(fireInterval);  // Wait for the next fire interval
        }
    }

    void Fire()
    {
        // Instantiate the projectile at the firePoint's position and rotation
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        // Set the projectile's velocity
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = firePoint.forward * projectileSpeed;
        }

        // Play the firing sound with adjusted volume
        if (fireSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fireSound, fireSoundVolume); // Adjust volume here
        }
    }
}
