using UnityEngine;

public class Gesture_Detector : MonoBehaviour
{
    public int enterCount = 0;
    public bool gestureTriggered = false;
    private bool outside = true;
    public Blackjack blackjackGame;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Entered collider: " + other.name);
        // incremenet everytime the right controller went from exit to enter state
        if (other.CompareTag("Right Controller") && outside && blackjackGame.gameInProgress)
        {
            enterCount++;
            outside = false;
            if (enterCount == 3 && !gestureTriggered)
            {
                gestureTriggered = true;
                // reset 
                enterCount = 0;
            }
            else
            {
                gestureTriggered = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // keep track each time controller exited collider
        //Debug.Log("Exited collider: " + other.name);
        if (other.CompareTag("Right Controller") && blackjackGame.gameInProgress)
        {
            outside = true;

            if (gestureTriggered)
            {
                // reset flag
                gestureTriggered = false;
            }
        }
    }
}
