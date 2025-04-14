using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Events;


public class ButtonPress : MonoBehaviour, ISelectable
{
    public GameObject button, Base;
    //public UnityEvent onPress;
    public GameObject presser;
    public bool isPressed;
    private float pressDuration = 0.5f;
    public Material greenMaterial;
    public Material redMaterial;
    public Material baseMaterial;

    // track bet and player balance
    public PlayerBalanceManager balanceManager;
    public ChipCounterArea chipCounter;

    // audio source to play when pressed button
    public AudioSource buttonAudioSource;
    public AudioSource errorAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        isPressed = false;
        button.GetComponent<Renderer>().material = redMaterial;
    }

    // Update is called once per frame
    public void OnPress()
    {
        // check that button has not been pressed yet and player has enough funds
        if (!isPressed && chipCounter.chipCount * 50 <= balanceManager.Balance && chipCounter.chipCount >= 1)
        {
            // change inner button to green to indicate it was pressed
            Base.GetComponent<Renderer>().material = baseMaterial;
            button.GetComponent<Renderer>().material = greenMaterial;
            isPressed = true;
            // make the button descend to make it seem like its really pressed
            button.transform.localPosition = new Vector3(0, 0.015f, 0);

            Debug.Log("Pressed");
            buttonAudioSource.Play();
        }
        // if they do not have enough funds, error sound
        else if (chipCounter.chipCount * 50 > balanceManager.Balance || chipCounter.chipCount <= 0) 
        {
            errorAudioSource.Play();
        }

        // Check if the button is currently pressed and start the release process after a delay
        if (isPressed)
        {
            StartCoroutine(ReleaseButtonAfterDelay());
        }
    }

    // highlight the mesh/body of the control
    public void OnSelect(Material highlightMaterial)
    {
        button.GetComponent<Renderer>().material = highlightMaterial;
        Base.GetComponent<Renderer>().material = highlightMaterial;
    }

    // return colours to original
    public void OnDeSelect()
    {
        button.GetComponent<Renderer>().material = redMaterial;
        Base.GetComponent<Renderer>().material = baseMaterial;
    }

    // 0.5 second press release that brings inner button back up and makes it red again
    IEnumerator ReleaseButtonAfterDelay()
    {
        yield return new WaitForSeconds(pressDuration);
        button.transform.localPosition = new Vector3(0, 0.024f, 0); 
        isPressed = false;
        button.GetComponent<Renderer>().material = redMaterial;
    }

    // spawn a sphere object every press
    void SpawnSphere()
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        sphere.AddComponent<Rigidbody>();
    }
}
