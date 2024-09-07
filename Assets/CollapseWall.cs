using UnityEngine;
using System.Collections;

public class CollapseWall : MonoBehaviour
{
    public GameObject intactWall;    // Reference to the intact wall
    public GameObject brokenWall;    // Reference to the broken wall pieces
    public float collapseDelay = 2.0f; // Time delay before the wall collapses
    public float explosionForce = 500f; // Explosion force applied to the broken pieces
    public float explosionRadius = 10f; // Radius of the explosion effect

    private bool isCollapsing = false;

    void Start()
    {
        // Make sure the broken wall is initially disabled
        brokenWall.SetActive(false);
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

        // Disable the intact wall
        intactWall.SetActive(false);

        // Enable the broken wall pieces
        brokenWall.SetActive(true);

        // Add explosion force to the broken pieces
        foreach (Transform piece in brokenWall.transform)
        {
            Rigidbody rb = piece.gameObject.GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = piece.gameObject.AddComponent<Rigidbody>();
            }
            rb.isKinematic = false; // Ensure Rigidbody is not kinematic
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);

            // Optionally, remove or adjust colliders if needed
            Collider collider = piece.gameObject.GetComponent<Collider>();
            if (collider != null && collider is MeshCollider)
            {
                Destroy(collider);
                // Optionally add primitive colliders if needed
                piece.gameObject.AddComponent<BoxCollider>(); // Replace with appropriate collider
            }
        }

        // Optionally, destroy the broken wall pieces after some time
        Destroy(brokenWall, 5f);  // Adjust the time as needed
    }
}
