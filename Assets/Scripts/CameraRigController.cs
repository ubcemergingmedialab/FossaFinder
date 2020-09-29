using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CameraRigController : MonoBehaviour {

    public GameObject mainCamera;
    public GameObject path;
    public GameObject pathFollower;

    bool needsToTeleport;
    bool isReadyToTrackPathFollower;
    float preservedHeightValue;

	// Use this for initialization
	void Start () {
        // Debug.Log(OVRManager.tracker.count);
        // InputTracking.Recenter();
        // Debug.Log(InputTracking.GetLocalPosition(XRNode.CenterEye));
        // Debug.Log(mainCamera.GetComponent<SteamVR_Camera>().head.localPosition);
        // Debug.Log(mainCamera.GetComponent<SteamVR_Camera>().origin.localPosition);
        // cameraRig.transform.position = new Vector3(0, 5, 2.5f) - InputTracking.GetLocalPosition(XRNode.CenterEye);
        // OVRManager.tracker.GetPose().position
        // InputTracking.disablePositionalTracking = true;
        // XRDevice.SetTrackingSpaceType(TrackingSpaceType.Stationary);
        // OVRManager.display.RecenterPose();
        StartCoroutine(Compensate());
    }

    IEnumerator Compensate()
    {
        yield return new WaitForSeconds(.5f);
        // InputTracking.Recenter();
        // Valve.VR.OpenVR.System.ResetSeatedZeroPose();
        // XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
        // OVRManager.display.RecenterPose();
        transform.position = new Vector3(0, 5, 1) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition;
        preservedHeightValue = transform.position.y;
        isReadyToTrackPathFollower = true;
        path.GetComponent<CPC_Path>().PlayPath(3);
    }

    // Update is called once per frame
    void Update () {
        // OVRManager.display.RecenterPose();

        // Debug.Log(OVRManager.tracker.GetPose().position);
        // Debug.Log(InputTracking.GetLocalPosition(XRNode.CenterEye));
        // Debug.Log(mainCamera.GetComponent<SteamVR_Camera>().head.localPosition);
        // Debug.Log(mainCamera.GetComponent<SteamVR_Camera>().origin.localPosition);
        if (isReadyToTrackPathFollower && path.GetComponent<CPC_Path>().IsPlaying())
        {
            transform.position = pathFollower.transform.position - new Vector3(0, pathFollower.transform.position.y - preservedHeightValue, 0); //  - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
            transform.rotation = pathFollower.transform.rotation; // rotation feels more natural ... but you are giving up user control
        }
    }
}
