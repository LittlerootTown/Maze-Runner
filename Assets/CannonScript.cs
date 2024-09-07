using System.Collections;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    public GameObject projectilePrefab;  // The projectile prefab
    public Transform firePoint;  // The position where the projectile will be fired from
    public float fireInterval = 2f;  // Time between shots
    public float projectileSpeed = 10f;  // Speed of the projectile

    private void Start()
    {
        // Start the automatic firing of the cannon
        StartCoroutine(FireProjectiles());
    }

    IEnumerator FireProjectiles()
    {
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
    }
}
