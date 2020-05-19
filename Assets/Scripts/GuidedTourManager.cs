using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedTourManager : MonoBehaviour {

    public GameObject head;
    public GameObject cameraRig;
    public GameObject mainCamera;

    int currentSceneNumber;
    bool isSwitching;

    Vector2 defaultTopDownCameraCoordinates;
    Vector3 defaultCameraRigPosition;
    float distanceThreshold;
    
	// Use this for initialization
	void Start () {
        currentSceneNumber = 1;
        isSwitching = false;

        defaultTopDownCameraCoordinates = new Vector2(0, 1);
        distanceThreshold = 0.2f;

        StartCoroutine(Compensate());
    }

    IEnumerator Compensate()
    {
        yield return new WaitForSeconds(.5f);
        // InputTracking.Recenter();
        // Valve.VR.OpenVR.System.ResetSeatedZeroPose();
        // XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        // OVRManager.display.RecenterPose();
        cameraRig.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, 1) - mainCamera.GetComponent<SteamVR_Camera>().head.position; // mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.y, 1f) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
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

    public void SwitchToPreviousScene()
    {
        if (currentSceneNumber > 1)
        {
            currentSceneNumber -= 1;
            isSwitching = true;
        }
        // Update will handle the rest
    }

    public void SwitchToNextScene()
    {
        currentSceneNumber += 1;
        isSwitching = true;
        // Update will handle the rest
    }

    // Update is called once per frame
    void Update () {
        //Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        Debug.Log("Current distance from default: " + Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates));
        if (isSwitching)
        {
            TeleportBackToDefaultCameraPosition();
            SwitchToScene(currentSceneNumber);
        }
        // Debug.Log(isSwitching);
	}

    void TeleportBackToDefaultCameraPosition ()
    {
        Vector2 currentTopDownCameraCoordinates = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.z);
        // Debug.Log(Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates));
        if (Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates) > distanceThreshold)
        {
            // cameraRig.transform.position = new Vector3(0 - mainCamera.GetComponent<SteamVR_Camera>().head.position.x, cameraRig.transform.position.y, 1 - mainCamera.GetComponent<SteamVR_Camera>().head.position.z);
            // cameraRig.transform.position = new Vector3(0 + mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.x, cameraRig.transform.position.y, 1 + mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.z);
            cameraRig.transform.position = new Vector3(0, cameraRig.transform.position.y, 1) - new Vector3(mainCamera.GetComponent<SteamVR_Camera>().head.position.x-cameraRig.transform.position.x, 0, mainCamera.GetComponent<SteamVR_Camera>().head.position.z - cameraRig.transform.position.z);
            Debug.Log("Camera rig is now at: " + cameraRig.transform.position);
            Debug.Log("After teleportation: " + Vector2.Distance(currentTopDownCameraCoordinates, defaultTopDownCameraCoordinates).ToString());
        }
    }

    void SwitchToScene(int i)
    {
        float newEulerAngle = 0;
        switch (i) // gotta reset position every time
        {
            case 1:
                newEulerAngle = Mathf.MoveTowardsAngle(head.transform.eulerAngles.y, 0, 90 * Time.deltaTime);
                // Debug.Log(newEulerAngle);
                head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, newEulerAngle, head.transform.eulerAngles.z);
                if (Mathf.Abs(newEulerAngle) < 0.1f)
                {
                    isSwitching = false;
                }
                break;
            case 2:
                newEulerAngle = Mathf.MoveTowardsAngle(head.transform.eulerAngles.y, -90, 90 * Time.deltaTime);
                // Debug.Log(newEulerAngle);
                head.transform.eulerAngles = new Vector3(head.transform.eulerAngles.x, newEulerAngle, head.transform.eulerAngles.z);
                if (Mathf.Abs(newEulerAngle + 90) < 0.1f)
                {
                    isSwitching = false;
                }
                break;
        }
    }
}
