using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range; // radius of sphere for patrol area
    public Transform centrePoint; // center point of patrol area
    public Transform player; // reference to the player
    public float detectionRadius = 15f; // maximum detection distance
    public float chaseSpeed = 5f; // speed while chasing
    public float patrolSpeed = 3.5f; // speed while patrolling
    public float raycastInterval = 0.1f; // interval for performing raycast detection
    public float fieldOfViewAngle = 60f; // angle of field of view for detection
    public LayerMask playerLayer; // Layer mask for detecting player

    private Animator animator;
    private bool isChasing = false;
    private AudioSource audioSource;

    public AudioClip[] walkingFootstepClips; // array for walking footstep clips
    public AudioClip[] runningFootstepClips; // array for running footstep clips
    public float patrolFootstepInterval = 0.75f;
    public float chaseFootstepInterval = 0.5f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        agent.speed = patrolSpeed;

        // Start raycasting for player detection
        StartCoroutine(DetectPlayerWithRaycast());
        StartCoroutine(PlayFootsteps());
    }

    void Update()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }

        animator.SetBool("isChasing", isChasing);
        animator.SetBool("isMoving", agent.velocity.magnitude > 0.0f);
    }

    void Patrol()
    {
        if (agent.remainingDistance <= agent.stoppingDistance) // done with path
        {
            Vector3 point;
            if (RandomPoint(centrePoint.position, range, out point)) // pass in our centre point and radius of area
            {
                Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); // so you can see with gizmos
                agent.SetDestination(point);
            }
        }
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector3 randomPoint = center + Random.insideUnitSphere * range; // random point in a sphere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    IEnumerator DetectPlayerWithRaycast()
    {
        while (true)
        {
            Vector3 directionToPlayer = player.position - transform.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (distanceToPlayer <= detectionRadius)
            {
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (angle <= fieldOfViewAngle / 2)
                {
                    // Perform raycasts in multiple directions (center, left, right)
                    Vector3[] rayDirections = {
                    directionToPlayer.normalized, // center
                    Quaternion.Euler(0, -15, 0) * directionToPlayer.normalized, // left
                    Quaternion.Euler(0, 15, 0) * directionToPlayer.normalized // right
                };

                    bool playerDetected = false;
                    foreach (Vector3 dir in rayDirections)
                    {
                        Ray ray = new Ray(transform.position + Vector3.up * 1.0f, dir);
                        RaycastHit hit;
                        Debug.DrawRay(ray.origin, ray.direction * detectionRadius, Color.red);

                        if (Physics.Raycast(ray, out hit, detectionRadius, playerLayer) && hit.transform.CompareTag("Player"))
                        {
                            playerDetected = true;
                            break;
                        }
                    }

                    if (playerDetected)
                    {
                        isChasing = true;
                        agent.speed = chaseSpeed;
                        Debug.Log("Raycast hit the player");
                    }
                    else
                    {
                        isChasing = false;
                        agent.speed = patrolSpeed;
                        Debug.Log("Raycast did not hit the player");
                    }
                }
                else
                {
                    isChasing = false;
                    agent.speed = patrolSpeed;
                }
            }
            else
            {
                isChasing = false;
                agent.speed = patrolSpeed;
            }

            yield return new WaitForSeconds(raycastInterval);
        }
    }





    IEnumerator PlayFootsteps()
    {
        while (true)
        {
            if (agent.velocity.magnitude > 0.1f) // Check if the enemy is moving
            {
                PlayFootstepSound();
            }

            float interval = isChasing ? chaseFootstepInterval : patrolFootstepInterval;
            yield return new WaitForSeconds(interval);
        }
    }

    void PlayFootstepSound()
    {
        AudioClip clip = null;
        if (isChasing)
        {
            if (runningFootstepClips.Length > 0)
            {
                clip = runningFootstepClips[Random.Range(0, runningFootstepClips.Length)];
            }
            else
            {
                Debug.LogWarning("No running footstep clips assigned!");
            }
        }
        else
        {
            if (walkingFootstepClips.Length > 0)
            {
                clip = walkingFootstepClips[Random.Range(0, walkingFootstepClips.Length)];
            }
            else
            {
                Debug.LogWarning("No walking footstep clips assigned!");
            }
        }

        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogError("Selected footstep clip is null!");
        }
    }

    public void TeleportEnemy()
    {
        transform.position = centrePoint.position;
        transform.rotation = centrePoint.rotation;
        Debug.Log("Enemy Teleported to Starting Point!");

        // Stop chasing the player after teleporting
        isChasing = false;
        agent.speed = patrolSpeed;
    }
}
