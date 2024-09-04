using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class FPSCharacterController : MonoBehaviour
{
    public float walkSpeed = 3.0f;
    public float runSpeed = 6.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 2.0f; // Height of the jump
    public float mouseSensitivity = 100f;
    public Transform playerCamera; // First-person camera
    public Transform thirdPersonCamera; // Third-person camera
    public float thirdPersonDistance = 4.0f; // Distance from player in third-person view
    public float thirdPersonHeight = 1.5f; // Height of the third-person camera from player position
    public Transform flashlight;
    public Transform playerStartingPoint;
    private float xRotation = 0f;
    private float yRotation = 0f; // Y rotation for third-person camera

    public List<AudioClip> footStepSounds = new List<AudioClip>();
    public List<AudioClip> footStepRunSounds = new List<AudioClip>();
    public AudioClip flashlightButtonClip;
    public AudioClip jumpSoundClip; // Sound for jumping
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

    private bool isFirstPerson = true; // Flag to determine camera mode

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

        // Initialize cameras: enable first-person camera, disable third-person camera
        playerCamera.gameObject.SetActive(true);
        thirdPersonCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }

        if (Input.GetKeyDown(KeyCode.F5))
        {
            ToggleCameraView();
        }

        HandleMouseLook();

        isGrounded = characterController.isGrounded;

        if (isGrounded)
        {
            lastGroundedPosition = transform.position;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset the downward velocity when grounded
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
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
            if (isFirstPerson)
            {
                playerCamera.localPosition = originalCameraPosition + new Vector3(0, Mathf.Sin(bobTimer) * bobHeight, 0);
            }
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

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 45f); // Clamp to prevent excessive looking up/down for third person

        if (isFirstPerson)
        {
            // First-person camera rotation
            playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            flashlight.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
        else
        {
            // Third-person camera rotation around the player
            yRotation += mouseX;

            Vector3 offset = new Vector3(0, thirdPersonHeight, -thirdPersonDistance); // Camera offset behind the player
            Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
            thirdPersonCamera.position = transform.position + rotation * offset; // Camera position behind and above the player
            thirdPersonCamera.LookAt(transform.position + Vector3.up * thirdPersonHeight); // Look at the player's upper body
        }

        // Apply horizontal rotation to the player
        transform.Rotate(Vector3.up * mouseX);
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

    void Jump()
    {
        // Calculate the upward velocity required for the desired jump height
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        // Play jump sound (if available)
        if (jumpSoundClip != null)
        {
            footStepAudioSource.PlayOneShot(jumpSoundClip);
        }
    }

    void ToggleFlashlight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashlight.gameObject.SetActive(isFlashlightOn);
        PlaySound(flashlightAudioSource, flashlightButtonClip);
    }

    void ToggleCameraView()
    {
        isFirstPerson = !isFirstPerson;
        playerCamera.gameObject.SetActive(isFirstPerson);
        thirdPersonCamera.gameObject.SetActive(!isFirstPerson);

        if (!isFirstPerson)
        {
            // Reset third-person camera position and rotation when toggling
            yRotation = transform.eulerAngles.y;
            thirdPersonCamera.position = transform.position - transform.forward * thirdPersonDistance + Vector3.up * thirdPersonHeight;
            thirdPersonCamera.LookAt(transform.position + Vector3.up * thirdPersonHeight);
        }
    }

    public void TeleportToSpawn()
    {
        characterController.enabled = false; // Disable to teleport without physics constraints
        transform.position = playerStartingPoint.position;
        transform.rotation = playerStartingPoint.rotation;
        characterController.enabled = true; // Re-enable to resume normal physics
    }
}
