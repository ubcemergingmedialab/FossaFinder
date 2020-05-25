using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedTourManager : MonoBehaviour {

    public GameObject head; 
    public GameObject cameraRig;  
    public GameObject mainCamera; 

    int currentSceneDestination; // the current scene number
    int lastVisitedScene; // the previous scene number after a scene transition
    bool isDuringSceneTransition; // whether a scene transition is taking place or not
    float distanceFromDefaultCameraPositionThreshold; // the maximum allowed distance from the default camera position before the user should be teleported
    bool isPastDistanceThreshold; // whether the user is past the aforementioned distance threshold
    Vector2 defaultTopDownCameraCoordinates; // the top down coordinates (x and z values only) of the default camera position
    
	// Use this for initialization
	void Start () {
        currentSceneDestination = 1;
        lastVisitedScene = 1;
        isDuringSceneTransition = false;
        distanceFromDefaultCameraPositionThreshold = 0.2f;
        isPastDistanceThreshold = false;
        defaultTopDownCameraCoordinates = new Vector2(0, .5f);
        
        StartCoroutine(Compensate());
    }

    // Defines the world position of the camera rig and the skull, after the position of the camera is set
    IEnumerator Compensate()
    {
        // InputTracking.Recenter();
        // Valve.VR.OpenVR.System.ResetSeatedZeroPose();
        // XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        // OVRManager.display.RecenterPose();
        // preservedHeightValue = transform.position.y;
        // isReadyToTrackPathFollower = true;
        // path.GetComponent<CPC_Path>().PlayPath(3);
        // defaultCameraRigPosition = cameraRig.transform.position;
        yield return new WaitForSeconds(.5f);
        cameraRig.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, .5f) - mainCamera.GetComponent<SteamVR_Camera>().head.position; // mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.y, 1f) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
       
        head.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, 0);   
    }

    // Returns the current scene number.
    public int GetCurrentSceneDestination()
    {
        return currentSceneDestination;
    }

    // Returns whether a scene transition is currently taking place.
    public bool GetIsDuringSceneTransition()
    {
        return isDuringSceneTransition;
    }

    // Maintains all necessary variables for transitioning into the previous scene (the scene with the smaller scene number). Update() will then check these variables and handle the actual movement
    public void SetUpVariablesForPreviousSceneNumber()
    {
        if (currentSceneDestination > 1)
        {
            lastVisitedScene = currentSceneDestination;
            // Debug.Log(lastVisitedScene);
            currentSceneDestination -= 1;
            // if Dante's idea doesn't work, check if transition is completed here
            isDuringSceneTransition = true;
            CheckIfPastDistanceThreshold();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). Update() will then check these variables and handle the actual movement
    public void SetUpVariablesForNextSceneNumber()
    {
        lastVisitedScene = currentSceneDestination;
        // Debug.Log(lastVisitedScene);
        currentSceneDestination += 1;
        // if Dante's idea doesn't work, check if transition is completed here
        isDuringSceneTransition = true;
        CheckIfPastDistanceThreshold();
    }

    // Checks whether the user is past the distance threshold based on the default position of the user (positoin of user = position of camera)
    void CheckIfPastDistanceThreshold()
    {
        Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        if (Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates) > distanceFromDefaultCameraPositionThreshold)
        {
            isPastDistanceThreshold = true;
        }
    }

    // Teleports the user back to its default position
    void TeleportBackToDefaultCameraPosition()
    {
        // Debug.Log(Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates));
        // cameraRig.transform.position = new Vector3(0 - mainCamera.GetComponent<SteamVR_Camera>().head.position.x, cameraRig.transform.position.y, 1 - mainCamera.GetComponent<SteamVR_Camera>().head.position.z);
        // cameraRig.transform.position = new Vector3(0 + mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.x, cameraRig.transform.position.y, 1 + mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.z);
        //Debug.Log("Camera rig is now at: " + cameraRig.transform.position);
        //Debug.Log("After teleportation: " + Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates).ToString());
        cameraRig.transform.position = new Vector3(0, cameraRig.transform.position.y, .5f) - new Vector3(mainCamera.GetComponent<SteamVR_Camera>().head.position.x - cameraRig.transform.position.x, 0, mainCamera.GetComponent<SteamVR_Camera>().head.position.z - cameraRig.transform.position.z);
        isPastDistanceThreshold = false;
    }

    // Update is called once per frame
    // If the user is in a scene transition, incrementally changes the transform value of the skull based on which scene the user wants to go to
    void Update () {
        // Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        // Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        // Debug.Log("Current distance from default: " + Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates));
        // Debug.Log(previousSceneNumber);
        // Debug.Log(isSwitching);
        if (isDuringSceneTransition)
        {
            TransitionToScene(currentSceneDestination);
        }
	}

    // Checks whether the user needs to be teleported first. Then, incrementally changes the transform values of the skull based on which scene the user wants to go to
    void TransitionToScene(int sceneNumber)
    {
        if (isPastDistanceThreshold)
        {
            TeleportBackToDefaultCameraPosition();
        }

        // If Dante's idea doesn't work, this is where you check if previous scenes are completed
        
        switch (sceneNumber)
        {
            case 1:
                TransitionToScene1();
                break;
            case 2:
                TransitionToScene2();
                break;
            case 3:
                TransitionToScene3();
                break;
            case 4:
                TransitionToScene4();
                break;
            case 5:
                TransitionToScene5();
                break;
            case 6:
                TransitionToScene6();
                break;
        }
    }

    // Incrementally changes the transform values of the skull to where the skull should be in scene 1
    void TransitionToScene1()
    {
        float newEulerAngle = Mathf.MoveTowardsAngle(head.transform.eulerAngles.y, 0, 90 * Time.deltaTime);
        // Debug.Log(newEulerAngle);
        head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, newEulerAngle, head.transform.eulerAngles.z);
        if (newEulerAngle < 0.1f)
        {
            isDuringSceneTransition = false;
        }
    }

    // Incrementally changes the transform values of the skull to where the skull should be in scene 2
    void TransitionToScene2()
    {
        head.transform.GetChild(0).gameObject.SetActive(true);

        float newEulerAngle = Mathf.MoveTowardsAngle(head.transform.eulerAngles.y, -90, 90 * Time.deltaTime);
        // Debug.Log(newEulerAngle);
        head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, newEulerAngle, head.transform.eulerAngles.z);
        if (Mathf.Abs(newEulerAngle + 90) < 0.1f)
        {
            isDuringSceneTransition = false;
        }
    }

    // Incrementally changes the transform values of the skull to where the skull should be in scene 3
    void TransitionToScene3()
    {
        if (lastVisitedScene == 2)
        {
            head.transform.GetChild(0).gameObject.SetActive(false);
            isDuringSceneTransition = false;
        } else
        {
            Vector3 newLocalScale = new Vector3(1, 1, 1);
            head.transform.localScale = Vector3.MoveTowards(head.transform.localScale, newLocalScale, Mathf.Sqrt(3) * Time.deltaTime);
            if (Mathf.Abs(newLocalScale.x - head.transform.localScale.x) < 0.01f)
            {
                isDuringSceneTransition = false;
            }
        }
    }

    // Incrementally changes the transform values of the skull to where the skull should be in scene 4
    void TransitionToScene4()
    {
        if (lastVisitedScene == 3)
        {
            Vector3 newLocalScale = new Vector3(2, 2, 2);
            head.transform.localScale = Vector3.MoveTowards(head.transform.localScale, newLocalScale, Mathf.Sqrt(3) * Time.deltaTime);
            if ((newLocalScale.x - head.transform.localScale.x) < 0.01f)
            {
                isDuringSceneTransition = false;
            }
        } else
        {
            float newEulerAngle = Mathf.MoveTowardsAngle(head.transform.eulerAngles.z, 0, 45 * Time.deltaTime);
            head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, head.transform.eulerAngles.y, newEulerAngle);
            if (newEulerAngle < 0.1f)
            {
                isDuringSceneTransition = false;
            }
        }
    }

    // Incrementally changes the transform values of the skull to where the skull should be in scene 5
    void TransitionToScene5()
    {
        if (lastVisitedScene == 4)
        {
            float newEulerAngle = Mathf.MoveTowardsAngle(head.transform.eulerAngles.z, 45, 45 * Time.deltaTime);
            // Debug.Log(newEulerAngle);
            head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, head.transform.eulerAngles.y, newEulerAngle);
            if (Mathf.Abs(newEulerAngle - 45) < 0.1f)
            {
                isDuringSceneTransition = false;
            }
        } else
        {
            Vector3 newLocalScale = new Vector3(2, 2, 2);
            head.transform.localScale = Vector3.MoveTowards(head.transform.localScale, newLocalScale, Mathf.Sqrt(27) * Time.deltaTime);
            if (Mathf.Abs(newLocalScale.x - head.transform.localScale.x) < 0.01f)
            {
                isDuringSceneTransition = false;
            }
        } 
    }

    // Incrementally changes the transform values of the skull to where the skull should be in scene 6
    void TransitionToScene6()
    {
        Vector3 newLocalScale = new Vector3(5, 5, 5);
        head.transform.localScale = Vector3.MoveTowards(head.transform.localScale, newLocalScale, Mathf.Sqrt(27) * Time.deltaTime);
        if (Mathf.Abs(newLocalScale.x - head.transform.localScale.x) < 0.01f)
        {
            isDuringSceneTransition = false;
        }
    }

    // Skips to a particular end state/scene of a transition 
    public void SkipToScene(int sceneNumber)
    {
        switch (sceneNumber)
        {
            case 1:
                SkipToScene1();
                break;
            case 2:
                SkipToScene2();
                break;
            case 3:
                SkipToScene3();
                break;
            case 4:
                SkipToScene4();
                break;
            case 5:
                SkipToScene5();
                break;
            case 6:
                SkipToScene6();
                break;
        }
    }

    // Defines what the transform values of the skull should be in scene 1
    void SkipToScene1()
    {
        head.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    // Defines what the transform values of the skull should be in scene 2
    void SkipToScene2()
    {
        head.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    // Defines what the transform values of the skull should be in scene 3
    void SkipToScene3()
    {
        head.transform.localScale = new Vector3(1, 1, 1);
    }

    // Defines what the transform values of the skull should be in scene 4
    void SkipToScene4()
    {
        head.transform.eulerAngles = new Vector3(0, -90, 0);
        head.transform.localScale = new Vector3(2, 2, 2);
    }

    // Defines what the transform values of the skull should be in scene 5
    void SkipToScene5()
    {
        head.transform.eulerAngles = new Vector3(0, -90, 45);
        head.transform.localScale = new Vector3(2, 2, 2);
    }

    // Defines what the transform values of the skull should be in scene 6
    void SkipToScene6()
    {
        head.transform.localScale = new Vector3(5, 5, 5);
    }
}
