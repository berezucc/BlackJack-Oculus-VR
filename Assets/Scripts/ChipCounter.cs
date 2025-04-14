using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipCounterArea : MonoBehaviour
{
    public int chipCount = 0;
    public PlayerBalanceManager balanceManager;

    private int playerBalance;

    void Start()
    {
        // storing user's balance to see if more bets can be made
        playerBalance = balanceManager.Balance;
    }

    // Check if a chip was added to the bet
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chip") && playerBalance >= 50)
        {
            chipCount++;
            Debug.Log("Chip entered. Total chips: " + chipCount);
        }
    }

    // Check for chips that were removed from bet
    private void OnTriggerExit(Collider other)
    {
        // Check if the object exiting the trigger has the "Chip" tag
        if (other.CompareTag("Chip"))
        {
            chipCount--;
            balanceManager.AddToBalance(50);
            Debug.Log("Chip exited. Total chips: " + chipCount);
        }
    }
}

