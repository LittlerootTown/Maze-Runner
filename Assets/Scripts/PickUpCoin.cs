using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpCoin : MonoBehaviour
{
    public AudioSource pickSnd;
    public static int coinCount;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Only Player can pick coins
        {
            // Play the coin pickup sound
            if (pickSnd != null)
            {
                pickSnd.Play();
            }

            coinCount++;
            this.gameObject.SetActive(false); // Turn THIS off
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        coinCount = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
