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
    public float fieldOfViewAngle = 90f; // increased FOV angle for detection
    public float peripheralRadius = 5f; // radius for peripheral detection
    public float losePlayerDelay = 2f; // delay before stopping chase after losing sight
    public LayerMask playerLayer; // Layer mask for detecting player
    public LayerMask obstacleLayer; // Layer mask for obstacles

    private Animator animator;
    private bool isChasing = false;
    private AudioSource audioSource;
    private Coroutine stopChasingCoroutine;

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

            // Check if the player is within detection range
            if (distanceToPlayer <= detectionRadius)
            {
                float angle = Vector3.Angle(transform.forward, directionToPlayer);

                if (angle <= fieldOfViewAngle / 2)
                {
                    // Main FOV detection
                    PerformRaycast(directionToPlayer);
                }
                else if (distanceToPlayer <= peripheralRadius)
                {
                    // Player is outside FOV but close enough (peripheral detection)
                    Debug.Log("Player detected in peripheral range.");
                    isChasing = true;
                    agent.speed = chaseSpeed;
                }
                else
                {
                    // Player is not within FOV or peripheral detection
                    if (stopChasingCoroutine == null)
                    {
                        stopChasingCoroutine = StartCoroutine(StopChasingAfterDelay());
                    }
                }
            }
            else
            {
                // Player is out of detection radius
                if (stopChasingCoroutine == null)
                {
                    stopChasingCoroutine = StartCoroutine(StopChasingAfterDelay());
                }
            }

            yield return new WaitForSeconds(raycastInterval);
        }
    }

    void PerformRaycast(Vector3 directionToPlayer)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 1.0f, directionToPlayer.normalized);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * detectionRadius, Color.red);

        if (Physics.Raycast(ray, out hit, detectionRadius, playerLayer) && hit.transform.CompareTag("Player"))
        {
            // Check for obstacles
            if (!Physics.Raycast(ray.origin, ray.direction, hit.distance, obstacleLayer))
            {
                isChasing = true;
                agent.speed = chaseSpeed;

                if (stopChasingCoroutine != null)
                {
                    StopCoroutine(stopChasingCoroutine);
                    stopChasingCoroutine = null;
                }

                Debug.Log("Raycast hit the player");
            }
            else if (stopChasingCoroutine == null)
            {
                stopChasingCoroutine = StartCoroutine(StopChasingAfterDelay());
            }
        }
        else if (stopChasingCoroutine == null)
        {
            stopChasingCoroutine = StartCoroutine(StopChasingAfterDelay());
        }
    }

    IEnumerator StopChasingAfterDelay()
    {
        yield return new WaitForSeconds(losePlayerDelay);
        isChasing = false;
        agent.speed = patrolSpeed;
        stopChasingCoroutine = null;
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
        if (centrePoint != null)
        {
            transform.position = centrePoint.position;
            transform.rotation = centrePoint.rotation;
            Debug.Log("Enemy Teleported to Centre Point!");

            // Stop chasing the player after teleporting
            isChasing = false;
            agent.speed = patrolSpeed;
        }
        else
        {
            Debug.LogError("Centre Point is not assigned!");
        }
    }
}
