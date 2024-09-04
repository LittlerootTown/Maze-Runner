using UnityEngine;
using System.Collections; 
public class CollapsePlatform : MonoBehaviour
{
    public GameObject intactPlatform;  // Reference to the intact platform
    public GameObject brokenPlatform;  // Reference to the broken platform pieces
    public float collapseDelay = 2.0f; // Time delay before the platform collapses

    private bool isCollapsing = false;

    void Start()
    {
        // Make sure the broken platform is initially disabled
        brokenPlatform.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player or another object triggers the collapse
        if (other.CompareTag("Player") && !isCollapsing)
        {
            StartCoroutine(Collapse());
        }
    }

    IEnumerator Collapse()
    {
        isCollapsing = true;

        // Wait for the specified delay
        yield return new WaitForSeconds(collapseDelay);

        // Disable the intact platform
        intactPlatform.SetActive(false);

        // Enable the broken platform pieces
        brokenPlatform.SetActive(true);

        // Optionally, add some physics to the broken pieces
        foreach (Transform piece in brokenPlatform.transform)
        {
            Rigidbody rb = piece.gameObject.AddComponent<Rigidbody>();
            rb.AddExplosionForce(100f, transform.position, 5f); // Add an explosion effect for a more dramatic collapse
        }

        // Optional: Destroy the broken pieces after some time
        Destroy(brokenPlatform, 5f);  // Adjust the time as needed
    }
}
