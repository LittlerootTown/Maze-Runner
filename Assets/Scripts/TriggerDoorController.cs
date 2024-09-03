using System.Collections;
using UnityEngine;

public class TriggerDoorController : MonoBehaviour
{
    public Animator myDoor;
    public AudioClip doorOpenClip;
    public AudioClip doorCloseClip;
    private AudioSource audioSource;
    bool doorOpen;



    [Range(0, 10)] public float doorOpenDelay = 0.0f;
    [Range(0, 10)] public float doorCloseDelay = 1.1f;

    void Start()
    {
        doorOpen = false;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Load default clips if they are not assigned
        if (doorOpenClip == null)
        {
            doorOpenClip = Resources.Load<AudioClip>("doorOpenClip");
        }
        if (doorCloseClip == null)
        {
            doorCloseClip = Resources.Load<AudioClip>("doorCloseClip");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            doorOpen = true;
            doorControl("Open");
            StartCoroutine(PlaySoundWithDelay(doorOpenClip, doorOpenDelay));
        }
    }

    private void doorControl(string state)
    {
        myDoor.SetTrigger(state);
    }

    private void OnTriggerExit(Collider other)
    {
        if (doorOpen)
        {
            doorControl("Close");
            StartCoroutine(PlaySoundWithDelay(doorCloseClip, doorCloseDelay));
            doorOpen = false;
        }
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    IEnumerator PlaySoundWithDelay(AudioClip clip, float delay)
    {
        yield return new WaitForSeconds(delay);
        PlaySound(clip);
    }


}