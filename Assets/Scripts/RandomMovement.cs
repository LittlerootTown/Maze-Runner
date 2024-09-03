using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public float range; // radius of sphere
    public Transform centrePoint; // centre of the area the agent wants to move around in
    public Transform player; // reference to the player
    public float detectionRadius = 15f; // radius within which the enemy can detect the player
    public float chaseSpeed = 5f; // speed while chasing
    public float patrolSpeed = 3.5f; // speed while patrolling
        
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

        // Add a SphereCollider for detection
        SphereCollider detectionCollider = gameObject.AddComponent<SphereCollider>();
        detectionCollider.isTrigger = true;
        detectionCollider.radius = detectionRadius;

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

    public void TeleportEnemy()
    {
        transform.position = centrePoint.position;
        transform.rotation = centrePoint.rotation;
        Debug.Log("Enemy Teleported to Starting Point!");

        // Stop chasing the player after teleporting
        isChasing = false;
        agent.speed = patrolSpeed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            agent.speed = chaseSpeed;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            agent.speed = patrolSpeed;
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
}
