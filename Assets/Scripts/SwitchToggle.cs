using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchToggle : MonoBehaviour, ISelectable
{
    public GameObject switchObjectOn, switchObjectOff, Base, Base1;
    public Material baseMaterial;
    public Material switchMaterial;

    public Light controlledLight;
    public AudioSource switchAudioSource;
    bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
    }

    // Update is called once per frame
    public void OnPress()
    {
        isActive = !isActive;

        // Turn switch on
        if (isActive)
        {
            switchObjectOn.SetActive(true);
            switchObjectOff.SetActive(false);
        }
        // Turn Switch Off
        else
        {
            switchObjectOn.SetActive(false);
            switchObjectOff.SetActive(true);
        }

        // Turn on/off directional light on light switch actions
        controlledLight.enabled = isActive;

        // Play the switch audio clip  each press
        if (switchAudioSource != null)
        {
            switchAudioSource.Play();
        }
    }

    // highlight the mesh/body of the control
    public void OnSelect (Material highlightMaterial)
    {
        switchObjectOn.GetComponent<Renderer>().material = highlightMaterial;
        switchObjectOff.GetComponent<Renderer>().material = highlightMaterial;
        Base.GetComponent<Renderer>().material = highlightMaterial;
        Base1.GetComponent<Renderer>().material = highlightMaterial;
    }

    // return colours to original
    public void OnDeSelect()
    {
        switchObjectOn.GetComponent<Renderer>().material = switchMaterial;
        switchObjectOff.GetComponent<Renderer>().material = switchMaterial;
        Base.GetComponent<Renderer>().material = baseMaterial;
        Base1.GetComponent<Renderer>().material = switchMaterial;
    }

}
