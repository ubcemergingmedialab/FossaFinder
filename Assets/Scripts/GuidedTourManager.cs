using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedTourManager : MonoBehaviour {

    public GameObject head;
    public GameObject cameraRig;
    public GameObject mainCamera;

    int currentSceneNumber;
    int previousSceneNumber;
    bool isDuringSceneTransition;
    float distanceFromDefaultCameraPositionThreshold;
    bool isPastDistanceThreshold;
    Vector2 defaultTopDownCameraCoordinates;
    Vector3 defaultCameraRigPosition;
    
	// Use this for initialization
	void Start () {
        currentSceneNumber = 1;
        previousSceneNumber = 1;
        isDuringSceneTransition = false;
        distanceFromDefaultCameraPositionThreshold = 0.2f;
        isPastDistanceThreshold = false;
        defaultTopDownCameraCoordinates = new Vector2(0, .5f);
        
        StartCoroutine(Compensate());
    }

    IEnumerator Compensate()
    {
        yield return new WaitForSeconds(.5f);
        // InputTracking.Recenter();
        // Valve.VR.OpenVR.System.ResetSeatedZeroPose();
        // XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        // OVRManager.display.RecenterPose();
        cameraRig.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, .5f) - mainCamera.GetComponent<SteamVR_Camera>().head.position; // mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.y, 1f) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
        defaultCameraRigPosition = cameraRig.transform.position;
        head.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, 0);
        //preservedHeightValue = transform.position.y;
        //isReadyToTrackPathFollower = true;
        //path.GetComponent<CPC_Path>().PlayPath(3);
    }

    public int GetCurrentSceneNumber()
    {
        return currentSceneNumber;
    }

    public bool GetIsDuringSceneTransition()
    {
        return isDuringSceneTransition;
    }

    public void SetUpVariablesForPreviousScene()
    {
        if (currentSceneNumber > 1)
        {
            previousSceneNumber = currentSceneNumber;
            Debug.Log(previousSceneNumber);
            currentSceneNumber -= 1;
            // check if transition is completed here
            isDuringSceneTransition = true;
            CheckIfPastDistanceThreshold();
        }
        // Update will handle the rest
    }

    public void SetUpVariablesForNextScene()
    {
        previousSceneNumber = currentSceneNumber;
        Debug.Log(previousSceneNumber);
        currentSceneNumber += 1;
        // check if transition is completed here
        isDuringSceneTransition = true;
        CheckIfPastDistanceThreshold();
        // Update will handle the rest
    }

    void CheckIfPastDistanceThreshold()
    {
        Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        if (Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates) > distanceFromDefaultCameraPositionThreshold)
        {
            isPastDistanceThreshold = true;
        }
    }

    void TeleportBackToDefaultCameraPosition()
    {
        Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        // Debug.Log(Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates));
        // cameraRig.transform.position = new Vector3(0 - mainCamera.GetComponent<SteamVR_Camera>().head.position.x, cameraRig.transform.position.y, 1 - mainCamera.GetComponent<SteamVR_Camera>().head.position.z);
        // cameraRig.transform.position = new Vector3(0 + mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.x, cameraRig.transform.position.y, 1 + mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.z);
        cameraRig.transform.position = new Vector3(0, cameraRig.transform.position.y, .5f) - new Vector3(mainCamera.GetComponent<SteamVR_Camera>().head.position.x - cameraRig.transform.position.x, 0, mainCamera.GetComponent<SteamVR_Camera>().head.position.z - cameraRig.transform.position.z);
        //Debug.Log("Camera rig is now at: " + cameraRig.transform.position);
        //Debug.Log("After teleportation: " + Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates).ToString());
        isPastDistanceThreshold = false;
    }

    // Update is called once per frame
    void Update () {
        //Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        //Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        //Debug.Log("Current distance from default: " + Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates));
        // Debug.Log(previousSceneNumber);
        if (isDuringSceneTransition)
        {
            TransitionToScene(currentSceneNumber);
        }
        // Debug.Log(isSwitching);
	}

    void TransitionToScene(int sceneNumber)
    {
        if (isPastDistanceThreshold)
        {
            TeleportBackToDefaultCameraPosition();
        }

        // make sure previous scenes are done
        
        switch (sceneNumber) // gotta reset position every time
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

    void TransitionToScene3()
    {
        if (previousSceneNumber == 2)
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

    void TransitionToScene4()
    {
        if (previousSceneNumber == 3)
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

    void TransitionToScene5()
    {
        if (previousSceneNumber == 4)
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

    void TransitionToScene6()
    {
        Vector3 newLocalScale = new Vector3(5, 5, 5);
        head.transform.localScale = Vector3.MoveTowards(head.transform.localScale, newLocalScale, Mathf.Sqrt(27) * Time.deltaTime);
        if (Mathf.Abs(newLocalScale.x - head.transform.localScale.x) < 0.01f)
        {
            isDuringSceneTransition = false;
        }
    }

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

    void SkipToScene1()
    {
        head.transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void SkipToScene2()
    {
        head.transform.eulerAngles = new Vector3(0, -90, 0);
    }

    void SkipToScene3()
    {
        head.transform.localScale = new Vector3(1, 1, 1);
    }

    void SkipToScene4()
    {
        head.transform.eulerAngles = new Vector3(0, -90, 0);
        head.transform.localScale = new Vector3(2, 2, 2);
    }

    void SkipToScene5()
    {
        head.transform.eulerAngles = new Vector3(0, -90, 45);
        head.transform.localScale = new Vector3(2, 2, 2);
    }

    void SkipToScene6()
    {
        head.transform.localScale = new Vector3(5, 5, 5);
    }
}
