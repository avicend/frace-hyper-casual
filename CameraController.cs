using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform[] views;
    public float transitionSpeed;
    Transform currentView;
    Transform currentView_fixed;
    public Transform playerTransform;

    public int cameraTransition = 0;

    // Use this for initialization
    void Start()
    {
      
    }

    void Update()
    {

        if (cameraTransition == 1)
        {
            currentView = views[0];
            
        }

        if (cameraTransition == 2)
        {
            currentView = views[1];
            
        }

        if (cameraTransition == 3)
        {
            currentView = views[2];
        }

        if (cameraTransition == 4)
        {
            currentView = views[3];
        }

        if (cameraTransition == 5)
        {
            currentView = views[4];
        }

    }


    void LateUpdate()
    {

        //Lerp position
        transform.position = Vector3.Lerp(transform.position, currentView.position, Time.deltaTime * transitionSpeed);

        Vector3 currentAngle = new Vector3(
         Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed),
         Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

        transform.eulerAngles = currentAngle;

    }
}