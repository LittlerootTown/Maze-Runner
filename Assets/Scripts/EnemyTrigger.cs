using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{   
    public AudioClip deathClip;
    public Transform playerSpawn;
    private AudioSource audioSource;
    private RandomMovement enemy;
    private FPSCharacterController playerController;

    void Start()
    {

        audioSource = gameObject.AddComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource component found on this GameObject.");
        }

        audioSource.clip = deathClip;

        enemy = GetComponentInParent<RandomMovement>();
        if (enemy == null)
        {
            Debug.LogError("No RandomMovement script found on parent GameObject.");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<FPSCharacterController>();
            if (playerController == null)
            {
                Debug.LogError("No FPSCharacterController script found on the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player GameObject not found.");
        }

        if (playerSpawn == null)
        {
            Debug.LogError("PlayerSpawn transform not assigned.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected in trigger.");

            if (audioSource != null && deathClip != null)
            {
                audioSource.Play();
            }
            else
            {
                Debug.LogError("AudioSource or deathClip is not assigned.");
            }

            if (playerController != null && playerSpawn != null)
            {
                playerController.TeleportToSpawn();
            }
            else
            {
                Debug.LogError("PlayerController or PlayerSpawn transform is not assigned.");
            }

            if (enemy != null)
            {
                enemy.TeleportEnemy();
            }
            else
            {
                Debug.LogError("RandomMovement script on parent GameObject not found.");
            }
        }
    }
}
