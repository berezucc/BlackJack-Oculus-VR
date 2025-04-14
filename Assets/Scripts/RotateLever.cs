using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.Newtonsoft.Json.Serialization;
using Valve.VR;
using UnityEngine.UIElements.Experimental;

public class RotateLever : MonoBehaviour, ISelectable
{
    public GameObject lever, cylinder, handle, knob, Base, Base1;
    public Material baseMaterial;
    public Material base1Material;
    public Material redMaterial;

    // limit angle range to 45-135 degrees
    public float minAngle = 45.0f;
    public float maxAngle = 135.0f;

    // reference to hand position
    public SteamVR_Behaviour_Pose controllerPose;
    public Transform facing;

    public void OnPress()
    {
        // Log the current rotation angle
        transform.LookAt(new Vector3(facing.position.x, facing.position.y, transform.position.z));

    }

    // highlight the mesh/body of the control
    public void OnSelect(Material highlightMaterial)
    {
        handle.GetComponent<Renderer>().material = highlightMaterial;
        knob.GetComponent<Renderer>().material = highlightMaterial;

    }

    // return colours to original
    public void OnDeSelect()
    {
        handle.GetComponent<Renderer>().material = baseMaterial;
        knob.GetComponent<Renderer>().material = redMaterial;

    }

    // have lever update cylinders scale
    void UpdateCylinderScale(float angle)
    {
        // angle is between 45 - 135 ==> 0 - 90 ==> 0.0 - 1.0
        float newAngle = angle - 45.0f;
        float scale = newAngle / 90.0f;
        cylinder.transform.localScale = new Vector3(scale, scale, scale);
    }
}
