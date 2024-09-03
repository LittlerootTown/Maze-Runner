using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FPSCharacterController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float gravity = -9.81f;
    public float mouseSensitivity = 100f;
    public Transform playerCamera;
    public Transform flashlight;
    public Transform playerStartingPoint;
    private float xRotation = 0f;
    public List<AudioClip> footStepSounds = new List<AudioClip>();
    public List<AudioClip> footStepRunSounds = new List<AudioClip>();

    public AudioClip flashlightButtonClip;
    public float fallThreshold = 1.0f;

    private CharacterController characterController;
    private AudioSource footStepAudioSource;
    private AudioSource flashlightAudioSource;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool wasGrounded;
    private Vector3 lastGroundedPosition;

    private bool isFlashlightOn = true;
    public float walkBobFrequency = 2.0f;
    public float runBobFrequency = 4.0f;
    public float bobHeight = 0.1f;
    private float bobTimer = 0.0f;
    private Vector3 originalCameraPosition;

    public float walkStepInterval = 0.5f;  // Delay between walking footsteps
    public float runStepInterval = 0.3f;   // Delay between running footsteps
    private float stepTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController component is missing on the player.");
        }

        footStepAudioSource = GetComponent<AudioSource>();
        if (footStepAudioSource == null)
        {
            footStepAudioSource = gameObject.AddComponent<AudioSource>();
        }

        flashlightAudioSource = gameObject.GetComponent<AudioSource>();
        if (flashlightAudioSource == null)
        {
            flashlightAudioSource = gameObject.AddComponent<AudioSource>();
        }

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component is missing on the player.");
        }

        Cursor.lockState = CursorLockMode.Locked;
        originalCameraPosition = playerCamera.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        flashlight.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.Rotate(Vector3.up * mouseX);

        isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            lastGroundedPosition = transform.position;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        float currentBobFrequency = isRunning ? runBobFrequency : walkBobFrequency;
        float currentStepInterval = isRunning ? runStepInterval : walkStepInterval;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        characterController.Move(move * currentSpeed * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootsStepAudio(isRunning);
                stepTimer = currentStepInterval;
            }
        }

        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (characterController.velocity.magnitude > 0.1f && isGrounded)
        {
            bobTimer += Time.deltaTime * currentBobFrequency;
            playerCamera.localPosition = originalCameraPosition + new Vector3(0, Mathf.Sin(bobTimer) * bobHeight, 0);
        }
        else
        {
            bobTimer = 0;
            playerCamera.localPosition = originalCameraPosition;
        }

        float inputMagnitude = new Vector3(moveX, 0, moveZ).magnitude;

        animator.SetBool("isRunning", isRunning);
        animator.SetBool("isWalking", (inputMagnitude > 0.0f));
    }


    void PlaySound(AudioSource audioSource, AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    void PlayFootsStepAudio(bool isRunning)
    {

        List<AudioClip> stepClips = isRunning ? footStepRunSounds : footStepSounds;
        int index = Random.Range(0, stepClips.Count);
        footStepAudioSource.clip = stepClips[index];
        footStepAudioSource.PlayOneShot(footStepAudioSource.clip);
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashlight.gameObject.SetActive(isFlashlightOn);
        PlaySound(flashlightAudioSource, flashlightButtonClip);
    }

    public void TeleportToSpawn()
    {
        characterController.enabled = false; // Disable to teleport without physics constraints
        transform.position = playerStartingPoint.position;
        transform.rotation = playerStartingPoint.rotation;
        characterController.enabled = true; // Re-enable to resume normal physics
    }


}
