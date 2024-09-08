using UnityEngine;
using TMPro;

public class ControlsTrigger : MonoBehaviour
{
    public TextMeshProUGUI instructionText; // Reference to the TextMeshProUGUI component

    void Start()
    {
        // Ensure the instruction text is initially hidden
        if (instructionText != null)
        {
            instructionText.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player has entered the trigger zone
        if (other.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            if (instructionText != null)
            {
                instructionText.gameObject.SetActive(true); // Show the instruction text
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player has exited the trigger zone
        if (other.CompareTag("Player")) // Assuming the player has the "Player" tag
        {
            if (instructionText != null)
            {
                instructionText.gameObject.SetActive(false); // Hide the instruction text
            }
        }
    }
}
