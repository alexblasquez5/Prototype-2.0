using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CoinCounterOne : MonoBehaviour
{
    public TextMeshProUGUI coinText; // Drag your TextMeshPro element here
    // public Text coinText; // Use this for regular Text instead of TextMeshPro
    private int coinCount = 0;

    void Start()
    {
        UpdateCoinText();
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        UpdateCoinText();
    }

    private void UpdateCoinText()
    {
        coinText.text = "Coins: " + coinCount.ToString();
    }
}
