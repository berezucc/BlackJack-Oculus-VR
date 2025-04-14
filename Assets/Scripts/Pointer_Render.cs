using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Valve.VR;

public class Pointer_Render : MonoBehaviour
{
    // length of laser
    public float length = 20.0f;

    // line render object 
    private LineRenderer laser;

    // reference to hand position
    public SteamVR_Behaviour_Pose controllerPose;

    // reference to left hand and button
    public SteamVR_Input_Sources hand;
    public SteamVR_Action_Boolean trigger;
    public SteamVR_Action_Boolean buttonTest;

    public SteamVR_Action_Boolean teleportButton;

    // Prefab for ball at laser intesrection with ground
    public GameObject laserLookAt;
    public GameObject laserPrefabInstance;
    public RaycastHit hit;

    // Camerarig object to teleport player
    public GameObject cameraRigPos;
    public GameObject cameraPos;

    private void Start()
    {
        // checker for if user is triggering/shooting the laser
        laser = GetComponent<LineRenderer>();
        laser.enabled = false;
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
            laser.enabled = true;

            hit = RayCast(length);

            Vector3 endPosition = transform.position + (transform.forward * length);

            laser.SetPosition(0, transform.position);
            laser.SetPosition(1, endPosition);


            // Draw circle where to teleport
            if (hit.collider != null && hit.collider.CompareTag("Ground"))
            {
                DrawCircle(hit);
            }

            // Teleport camera rig to target
            if (teleportButton.GetStateDown(hand))
            {
                // fade screen black
                // wait 1 second to teleport camera
                // fade back to clear screen after 2 seconds
                Invoke(nameof(FadeToBlack), 0f);
                TransformCamera(hit);
                Invoke(nameof(FadeFromBlack), 2f);
            }
        }
    }

    // check with raycast if laser intersects with any controls
    private RaycastHit RayCast(float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(this.transform.position, this.transform.forward);
        Physics.Raycast(ray, out hit, length);

        return hit;
    }

    // draw circle where user is aiming at
    private void DrawCircle(RaycastHit hit)
    {
        if (laserPrefabInstance == null)
        {
            // instantiate the object at the hit point if it doesn't exist
            laserPrefabInstance = Instantiate(laserLookAt, hit.point, Quaternion.identity);
        }
        else
        {
            // move the object to the hit point if it already exists
            laserPrefabInstance.transform.position = hit.point;
        }
    }

    // transform entire camera right to the target position of the laser and ground
    private void TransformCamera(RaycastHit hit)
    {
        if (cameraRigPos != null && cameraPos != null)
        {
            Vector3 offset = cameraPos.transform.position - cameraRigPos.transform.position;
            offset.y = 0.2f;
            // factors in offset if camera moved off the rig center
            cameraRigPos.transform.position = hit.point + offset;
        }
    }

    // **Bottom 2 functions inspired by code found online**
    // fade screen to black
    private void FadeToBlack()
    {
        SteamVR_Fade.View(Color.black, 1);
    }
    // remove black fade back to normal
    private void FadeFromBlack()
    {
        SteamVR_Fade.View(Color.clear, 1);
    }
}