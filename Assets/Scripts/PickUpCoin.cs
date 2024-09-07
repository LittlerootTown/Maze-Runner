using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpCoin : MonoBehaviour
{
    public AudioSource pickSnd;
    public static int coinCount;
    public TMP_Text textCoin;
    public int totalCoins = 10; // Total number of coins in the game

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Only Player can pick coins
        {
            coinCount++;
            pickSnd.Play();
            this.gameObject.SetActive(false); // Turn THIS off
            UpdateCoinText();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        coinCount = 0;
        UpdateCoinText();
    }

    // Update the coin text in the desired format
    void UpdateCoinText()
    {
        textCoin.text = "Coins: " + coinCount + "/" + totalCoins;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

