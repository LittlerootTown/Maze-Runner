using UnityEngine;
using System.Collections;

public class CollapsePlatform : MonoBehaviour
{
    public GameObject intactPlatform;  // Reference to the intact platform
    public GameObject brokenPlatform;  // Reference to the broken platform pieces
    public float collapseDelay = 2.0f; // Time delay before the platform collapses
    public float respawnTime = 5.0f;  // Time before the platform respawns

    private bool isCollapsing = false;
    private Vector3[] initialPositions;  // To store the initial positions of broken pieces
    private Quaternion[] initialRotations; // To store the initial rotations of broken pieces

    void Start()
    {
        // Make sure the broken platform is initially disabled
        if (brokenPlatform != null)
        {
            brokenPlatform.SetActive(false);
        }

        // Store the initial positions and rotations of the broken pieces
        if (brokenPlatform != null)
        {
            initialPositions = new Vector3[brokenPlatform.transform.childCount];
            initialRotations = new Quaternion[brokenPlatform.transform.childCount];
            for (int i = 0; i < brokenPlatform.transform.childCount; i++)
            {
                initialPositions[i] = brokenPlatform.transform.GetChild(i).localPosition;
                initialRotations[i] = brokenPlatform.transform.GetChild(i).localRotation;
            }
        }
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
        if (intactPlatform != null)
        {
            intactPlatform.SetActive(false);
        }

        // Enable the broken platform pieces
        if (brokenPlatform != null)
        {
            brokenPlatform.SetActive(true);

            // Optionally, add some physics to the broken pieces
            foreach (Transform piece in brokenPlatform.transform)
            {
                Rigidbody rb = piece.gameObject.AddComponent<Rigidbody>();
                rb.AddExplosionForce(100f, transform.position, 5f); // Add an explosion effect for a more dramatic collapse
            }
        }

        // Wait for respawn time
        yield return new WaitForSeconds(respawnTime);

        // Respawn the intact platform
        if (intactPlatform != null)
        {
            intactPlatform.SetActive(true);
        }

        // Reset the broken platform pieces
        if (brokenPlatform != null)
        {
            foreach (Transform piece in brokenPlatform.transform)
            {
                // Reset position and rotation
                piece.localPosition = initialPositions[piece.GetSiblingIndex()];
                piece.localRotation = initialRotations[piece.GetSiblingIndex()];

                // Remove Rigidbody to reset physics
                Rigidbody rb = piece.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Destroy(rb);
                }
            }

            // Disable the broken platform again
            brokenPlatform.SetActive(false);
        }

        isCollapsing = false;
    }
}
