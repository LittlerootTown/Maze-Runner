using UnityEngine;

public class PickUpCoin : MonoBehaviour
{
    public AudioSource pickSnd;
    
    public int coinValue = 100; // The value of each coin

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Assuming player has the "Player" tag
        {
            // Check if ScoreManager instance is not null
            if (ScoreManager.instance != null)
            {
                // Add coin value to the score using ScoreManager
                ScoreManager.instance.AddScore(coinValue);
            }
            else
            {
                Debug.LogWarning("ScoreManager instance not found!");
            }

            pickSnd.Play();

            // Destroy the coin after it's picked up
            Destroy(gameObject);
        }
    }
}
