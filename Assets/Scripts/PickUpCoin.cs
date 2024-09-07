using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpCoin : MonoBehaviour
{
    public AudioSource pickSnd;
    public static int score;
    public TMP_Text textCoin;
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Only Player can pick coins
        {
            score+=100;
            pickSnd.Play();
            this.gameObject.SetActive(false); // Turn THIS off
            UpdateCoinText();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        UpdateCoinText();
    }

    // Update the coin text in the desired format
    void UpdateCoinText()
    {
        textCoin.text = "Score: " + score;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

