using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Blackjack : MonoBehaviour
{
    // Store lists for cards currently in the deck, player's hand, and dealer's hand
    private List<string> deck;
    private List<string> playerHand;
    private List<string> dealerHand;
    public bool gameInProgress = false;

    // balance & chip bet objects
    public PlayerBalanceManager balanceManager;
    public ChipCounterArea chipCounter;
    private int bet;
    // check if user starts round
    public ButtonPress startGameButton;

    // track right controllers gestures (hit or stand)
    public Gesture_Detector rightControllerHitGestures;
    public Gesture_Detector rightControllerStandGestures;

    // game objects where deck cards will be placed
    public Transform playerHandPosition;
    public Transform dealerHandPosition;

    // particle system special effect
    public ParticleSystem confettiParticleSystem;

    // List to keep track of instantiated card GameObjects for cleanup
    private List<GameObject> instantiatedCardObjects = new List<GameObject>();
    public Transform deckPosition;

    public TextMeshPro endGameFinish;

    void Start()
    {
        // start with creating a deck of cards + storing user's balance
        balanceManager = FindObjectOfType<PlayerBalanceManager>();
        InitializeDeck();
    }

    void Update()
    {
        // is user presses button than the game starts
        // I added keyboard inputs for testing
        if ((Input.GetKeyDown(KeyCode.P) || startGameButton.isPressed) && !gameInProgress)
        {
            endGameFinish.text = "";
            StartGame(chipCounter.chipCount * 50);
        }
        else if ((rightControllerHitGestures.gestureTriggered || Input.GetKeyDown(KeyCode.H)) && gameInProgress)
        {
            PlayerHit();
            rightControllerHitGestures.gestureTriggered = false;
        }
        else if ((rightControllerStandGestures.gestureTriggered || Input.GetKeyDown(KeyCode.S)) && gameInProgress)
        {
            PlayerStand();
            rightControllerStandGestures.gestureTriggered = false;
        }
        // debugging particle system
        else if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(PlayParticleSystemForDuration(confettiParticleSystem, 5f));
        }

        // keep track of the bet before the game starts
        if (!gameInProgress)
        {
             bet = chipCounter.chipCount * 50;   
        }
    }

    void InitializeDeck()
    {
        // initialize list for all cards by their value.
        deck = new List<string>
        {
            "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A",
            "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A",
            "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A",
            "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"
        };

        // shuffle the card order randomly in the deck
        for (int i = 0; i < deck.Count; i++)
        {
            string temp = deck[i];
            int randomIndex = UnityEngine.Random.Range(0, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }

    public void StartGame(int bet)
    {
        // ensure user has enough balance to play
        if (balanceManager.Balance >= bet && !gameInProgress)
        {
            // create empty lists to store users and dealers hands
            playerHand = new List<string>();
            dealerHand = new List<string>();
            gameInProgress = true;

            // dealing of 2 cards to each players hands
            playerHand.Add(DealCard("player"));
            dealerHand.Add(DealCard("dealer"));
            playerHand.Add(DealCard("player"));
            dealerHand.Add(DealCard("dealer"));

            // decrease balance by bet
            balanceManager.SubtractFromBalance(bet);

            Debug.Log("Game started. Press H to Hit or S to Stand.");
            DisplayHands();
        }
        else
        {
            Debug.Log("Not enough balance");
        }
    }

    string DealCard(string handType)
    {
        // remove the top card from the deck 
        if (deck.Count > 0)
        {
            string card = deck[0];
            deck.RemoveAt(0);

            Transform targetPosition = handType == "player" ? playerHandPosition : dealerHandPosition;

            string[] suits = new string[] { "Heart", "Diamond", "Club", "Spades" };
            // Randomly select a suit
            string suit = suits[UnityEngine.Random.Range(0, suits.Length)];

            string cardName = "PlayingCards_" + card + suit;
            InstantiateCardInScene(handType, cardName, targetPosition);

            return card;
        }
        // ran out of cards in the deck --> restart the game
        else
        {
            Debug.LogError("Restarting game.");
            StartGame(bet);
            return null;
        }
    }

    //void InstantiateCardInScene(string handType, string cardName, Transform targetPosition)
    //{
    //    string path = $"Free_Playing_Cards/{cardName}";
    //    GameObject cardPrefab = Resources.Load<GameObject>(path);
    //    int cardCount = handType == "player" ? playerHand.Count - 1 : dealerHand.Count - 1;

    //    if (cardPrefab != null)
    //    {
    //        Quaternion rotation = Quaternion.Euler(-90, 0, 0);
    //        GameObject cardObject = Instantiate(cardPrefab, deckPosition.transform.position, rotation);


    //        Vector3 offset = new Vector3();
    //        if (handType == "player")
    //        {
    //            offset = new Vector3(cardCount * 0.05f, cardCount * -0.001f, cardCount * 0.05f);
    //        }
    //        else // "dealer"
    //        {
    //            offset = new Vector3(cardCount * 0.1f, 0, 0);
    //        }


    //        // Instantiate and position the card with the offset
    //        //GameObject cardObject = Instantiate(cardPrefab, targetPosition.position + offset, rotation, targetPosition);
    //        instantiatedCardObjects.Add(cardObject); 

    //    }
    //    else
    //    {
    //        Debug.LogError($"Failed to load card prefab at path: {path}");
    //    }
    //}

    void InstantiateCardInScene(string handType, string cardName, Transform targetPosition)
    {
        string path = $"Free_Playing_Cards/{cardName}";
        GameObject cardPrefab = Resources.Load<GameObject>(path);
        int cardCount = handType == "player" ? playerHand.Count - 1 : dealerHand.Count - 1;

        if (cardPrefab != null)
        {
            Quaternion rotation = Quaternion.Euler(-90, 0, 0);

            // Instantiate the card at the deck position
            GameObject cardObject = Instantiate(cardPrefab, deckPosition.transform.position, rotation);

            // Start coroutine to animate the card to the target position
            StartCoroutine(MoveCardToPosition(cardObject, targetPosition.position + GetOffset(handType, cardCount)));

            instantiatedCardObjects.Add(cardObject);
        }
        else
        {
            Debug.LogError($"Failed to load card prefab at path: {path}");
        }
    }

    IEnumerator MoveCardToPosition(GameObject cardObject, Vector3 targetPosition)
    {
        float duration = 1.25f;
        float elapsedTime = 0f;
        Vector3 startPosition = cardObject.transform.position;

        while (elapsedTime < duration)
        {
            cardObject.transform.position = Vector3.Lerp(startPosition, targetPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cardObject.transform.position = targetPosition;
    }

    Vector3 GetOffset(string handType, int cardCount)
    {
        if (handType == "player")
        {
            return new Vector3(cardCount * 0.05f, cardCount * -0.001f, cardCount * 0.05f);
        }
        else // "dealer"
        {
            return new Vector3(cardCount * 0.1f, 0, 0);
        }
    }

    void CleanupCards()
    {
        StartCoroutine(DelayedCleanup());
    }

    IEnumerator DelayedCleanup()
    {
        // 8 second delay until cards are cleared to play again
        yield return new WaitForSeconds(8f); 

        foreach (GameObject cardObject in instantiatedCardObjects)
        {
            Destroy(cardObject);
        }

        // Clear the list after cleanup
        instantiatedCardObjects.Clear();
    }

    public void PlayerHit()
    {
        playerHand.Add(DealCard("player"));
        DisplayHands();
        if (CalculateHandValue(playerHand) > 21)
        {
            Debug.Log("Player busts! Dealer wins.");
            CleanupCards();
            CleanupChips();
            gameInProgress = false;
            DisplayMessage("You Lose :(");
        }
    }

    public void PlayerStand()
    {
        while (CalculateHandValue(dealerHand) < 17)
        {
            dealerHand.Add(DealCard("dealer"));
        }

        int playerScore = CalculateHandValue(playerHand);
        int dealerScore = CalculateHandValue(dealerHand);
        DisplayHands();

        if (dealerScore > 21 || playerScore > dealerScore)
        {
            Debug.Log("Player wins!");
            balanceManager.AddToBalance(bet * 2);

            // Play the confetti particle system
            if (confettiParticleSystem != null)
            {
                StartCoroutine(PlayParticleSystemForDuration(confettiParticleSystem, 3f));
            }

            CleanupCards();
            CleanupChips();
        }
        else if (playerScore < dealerScore)
        {
            Debug.Log("Dealer wins.");
            CleanupCards();
            CleanupChips();
            gameInProgress = false;
            DisplayMessage("You Lose :(");
        }
        else
        {
            Debug.Log("Push. It's a tie.");
            CleanupCards();
            CleanupChips();
            gameInProgress = false;
            DisplayMessage("It's a Tie");
        }

        gameInProgress = false;
    }

    void DisplayMessage(string message)
    {
        // update text to display lose/tie
        endGameFinish.text = message;
        
    }

    IEnumerator DisplayMessageForSeconds(string message, float duration)
    {
        // wait 2 seconds
        yield return new WaitForSeconds(duration);

        // Clear the message
        if (endGameFinish != null)
        {
            endGameFinish.text = "";
        }
    }

    IEnumerator PlayParticleSystemForDuration(ParticleSystem ps, float duration)
    {
        yield return new WaitForSeconds(1f);
        // Start playing the particle system
        ps.Play();

        yield return new WaitForSeconds(duration);
        // Stop the particle system
        ps.Stop();
        ps.Clear();
    }

    int CalculateHandValue(List<string> hand)
    {
        // Calculate the value of the cards in the hand

        int value = 0;
        int aceCount = 0;

        foreach (string card in hand)
        {
            if (int.TryParse(card, out int cardValue))
            {
                value += cardValue;
            }
            else if (card == "J" || card == "Q" || card == "K")
            {
                value += 10;
            }
            else if (card == "A")
            {
                aceCount++;
                value += 11;
            }
        }

        while (value > 21 && aceCount > 0)
        {
            value -= 10; // Convert an Ace from 11 to 1 if possible
            aceCount--;
        }

        return value;
    }

    // Once round finishes, remove all chips from the board
    void CleanupChips()
    {
        GameObject[] chips = GameObject.FindGameObjectsWithTag("Chip");
        foreach (GameObject chip in chips)
        {
            Destroy(chip);
        }
        // reset chip count to 0
        chipCounter.chipCount = 0;
        rightControllerHitGestures.enterCount = 0;
        rightControllerStandGestures.enterCount = 0;
    }

    // console Debug the players and dealers hands
    void DisplayHands()
    {
        Debug.Log($"Player's hand: {string.Join(", ", playerHand)} (Total: {CalculateHandValue(playerHand)})");
        Debug.Log($"Dealer's hand: {string.Join(", ", dealerHand)} (Total: {CalculateHandValue(dealerHand)})");
    }
}
