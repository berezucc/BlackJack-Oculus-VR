using UnityEngine;
using TMPro;

public class PlayerBalanceManager : MonoBehaviour
{
    public static PlayerBalanceManager Instance { get; private set; }

    // Initial balance <-- { get; private set; } is important to make balance readable but not modifiable outside the class
    public int Balance { get; private set; } = 1000;
    public TextMeshPro balanceText;

    private void Start()
    {
        UpdateBalanceDisplay(Balance);
        GameObject balanceTextObject = GameObject.FindWithTag("BalanceText");
        balanceText = balanceTextObject.GetComponent<TextMeshPro>();
    }

    private void Update()
    {
        UpdateBalanceDisplay(Balance);
    }

    // User won blackjack, balance increased
    public void AddToBalance(int amount)
    {
        Balance += amount;
        UpdateBalanceDisplay(Balance);
    }

    // User bet / lost, balance decreased
    public void SubtractFromBalance(int amount)
    {
        Balance -= amount;
        UpdateBalanceDisplay(Balance);
    }

    // Update Canvas text to show proper balance
    public void UpdateBalanceDisplay(int newBalance)
    {
        balanceText.text = "Balance: $" + newBalance;
    }
}
