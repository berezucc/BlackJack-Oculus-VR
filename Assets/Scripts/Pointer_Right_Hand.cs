using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Valve.VR;


public class Pointer_Right_Hand : MonoBehaviour
{
    // length of laser
    public float length;
    // line render object
    private LineRenderer laser;

    // reference to hand position
    public SteamVR_Behaviour_Pose controllerPose;
    // Flag to track if the controller is currently in an upwards movement

    // reference to left hand and button
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean trigger;
    public SteamVR_Action_Boolean selectButton;

    // Prefab for ball at laser intesrection with ground
    public RaycastHit hit;

    // highlighting/selecting controls
    public bool isSelected;
    public Material highlightMaterial;
    ISelectable control;

    // keep track of current object and fixed join
    public GameObject objectInHand;
    public FixedJoint fixedJoint;
    public bool isHolding = false;
    public GameObject chip;

    // interact with black jack game and keep track of controller
    public Blackjack blackjackGame;

    private void Start()
    {
        // checker for if user is triggering/shooting the laser
        laser = GetComponent<LineRenderer>();
        if (laser != null)
        {
            laser.enabled = false;
            isSelected = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // draw laser
        if (laser != null)
        {
            DrawLaser();
        }
    }

    // draw laser and check for collisions
    private void DrawLaser()
    {
        if (laser != null)
        {
            if (trigger.GetState(hand))
            {
                laser.enabled = true;

                hit = RayCast(length);

                Vector3 endPosition = transform.position + (transform.forward * length);

                laser.SetPosition(0, transform.position);
                laser.SetPosition(1, endPosition);

                // highlight and select objects
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Chip Pile"))
                    {
                        // Instantiate a chip in the users hand
                        if (!isHolding)
                        {
                            GameObject newChip = Instantiate(chip, endPosition, Quaternion.identity);
                            isHolding = true;
                            PickUpObject(newChip);
                        }
                    }
                    if (hit.collider.CompareTag("Chip"))
                    {
                        // Pick up the chip
                        if (!isHolding)
                        {
                            GameObject chipPrefab = hit.collider.gameObject;
                            isHolding = true;
                            PickUpObject(chipPrefab);
                        }
                    }
                    else if (hit.collider.CompareTag("Select"))
                    {
                        control = hit.collider.GetComponent<ISelectable>();
                        control.OnSelect(highlightMaterial);

                        // Check if select button is pressed
                        if (selectButton.GetStateUp(hand))
                        {
                            // Call the onPress method of the object
                            //control.OnDeSelect();
                            control.OnPress();
                        }
                    }
                }
            }
            else if (trigger.GetStateUp(hand))
            {
                laser.enabled = false;

                // Drop the object if user releases trigger
                if (isHolding)
                {
                    // control.OnDeSelect();
                    isHolding = false;

                    // Disconnect the object from the joint
                    Destroy(fixedJoint);
                    fixedJoint = null;
                    objectInHand = null;
                }
            }
            else
            {
                if (control != null)
                {
                    control.OnDeSelect();
                }
            }
        }
    }

    // Attach pickup object to controllers fixed joint so that it follows controller movement
    private void PickUpObject(GameObject obj)
    {
        Debug.Log("Pickup");
        objectInHand = obj;
        fixedJoint = gameObject.AddComponent<FixedJoint>();
        fixedJoint.breakForce = 10000;
        fixedJoint.connectedBody = objectInHand.GetComponent<Rigidbody>();
    }

    // check with raycast if laser intersects with any controls
    private RaycastHit RayCast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(ray, out hit, length);
        return hit;
    }
}

