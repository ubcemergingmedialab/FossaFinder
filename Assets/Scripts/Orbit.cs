using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Created by Wayland.
/// Improved version of CameraRotation
/// </summary>

public class Orbit : MonoBehaviour
{
    [SerializeField] GameObject labels;
    [SerializeField] Text objText;

    public Camera cam;
    private AgentController agent;
    private CameraController cameraControl;
    public Transform worldViewPos;
    private Transform origPos;
    private string origText;

    public Transform target;
    public Vector3 targetOffset;
    public float distance;
    public float maxDistance = 1500;
    public float minDistance = 300;
    public float xSpeed = 5.0f;
    public float ySpeed = 5.0f;
    public int yMinLimit = -80;
    public int yMaxLimit = 80;
    public float zoomRate = 10.0f;
    public float panSpeed = 0.3f;
    public float zoomDampening = 5.0f;

    private float xDeg = 0.0f;
    private float yDeg = 0.0f;
    private float currentDistance;
    private float desiredDistance;
    private Quaternion currentRotation;
    private Quaternion desiredRotation;
    private Quaternion rotation;
    private Vector3 position;

    private Vector3 FirstPosition;
    private Vector3 SecondPosition;
    private Vector3 delta;
    private Vector3 lastOffset;
    private Vector3 lastOffsettemp;

    public bool worldViewActive = false;

    void Start() { CameraRotation(); }
    void OnEnable() { CameraRotation(); }

    public void CameraRotation()
    {
        agent = GameObject.FindGameObjectWithTag("Player").GetComponent<AgentController>();
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();

        if (worldViewActive)
        {
            //If there is no target, then craete one. This is not very neccessary for our build
            if (!target)
            {
                GameObject go = new GameObject("CamTarget");
                go.transform.position = cam.transform.position + (cam.transform.forward * distance);
                target = go.transform;
            }

            distance = Vector3.Distance(cam.transform.position, target.position);
            currentDistance = distance;
            desiredDistance = distance;

            //Grabbing the starting points
            position = cam.transform.position;
            rotation = cam.transform.rotation;
            currentRotation = cam.transform.rotation;
            desiredRotation = cam.transform.rotation;

            xDeg = Vector3.Angle(Vector3.right, cam.transform.right);
            yDeg = Vector3.Angle(Vector3.up, cam.transform.up);
        }
    }

    /*
      * Camera logic on LateUpdate to only update after all character movement logic has been handled.
      */
    void LateUpdate()
    {
        if (worldViewActive)
        {
            //If there are 2 touch inputs then we can start calcualting the distance from the object to the camera
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);



                Vector2 touchZeroPreviousPosition = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePreviousPosition = touchOne.position - touchOne.deltaPosition;



                float prevTouchDeltaMag = (touchZeroPreviousPosition - touchOnePreviousPosition).magnitude;
                float TouchDeltaMag = (touchZero.position - touchOne.position).magnitude;



                float deltaMagDiff = prevTouchDeltaMag - TouchDeltaMag;
                desiredDistance += deltaMagDiff * Time.deltaTime * zoomRate * 0.0025f * Mathf.Abs(desiredDistance);
            }
            // Used to calculate the degree instead of using LookAt or Rotate
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchposition = Input.GetTouch(0).deltaPosition;
                xDeg += touchposition.x * xSpeed * 0.002f;
                yDeg -= touchposition.y * ySpeed * 0.002f;
                yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);

            }

            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0);
            currentRotation = cam.transform.rotation;
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, Time.deltaTime * zoomDampening);
            cam.transform.rotation = rotation;

            
            if (Input.GetMouseButtonDown(1))
            {
                FirstPosition = Input.mousePosition;
                lastOffset = targetOffset;
            }



            //Orbit Position
            
            desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
            currentDistance = Mathf.Lerp(currentDistance, desiredDistance, Time.deltaTime * zoomDampening);

            position = target.position - (rotation * Vector3.forward * currentDistance);

            position = position - targetOffset;

            cam.transform.position = position;

        }
    }

    //limit the angle so we don't have clipping or very weird calculations occuring 
    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }

    
    IEnumerator ZoomOut()
    {

        while (cam.transform.position != worldViewPos.position)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, worldViewPos.position, Time.deltaTime * 3f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, Time.deltaTime * 3f);
            cam.transform.LookAt(target);
            yield return null;

        }
    }

    IEnumerator ZoomIn()
    {
        while (cam.transform.position != origPos.position)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, origPos.position, Time.deltaTime * 3f);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, Time.deltaTime * 3f);
            yield return null;
        }
    }
    //when the button is clicked then we zooooom out
    public void ButtonClick()
    {
        if (!worldViewActive)
        {
            //worldViewButton.image = Resources.Load < "" >;
            origPos = cam.transform;
            origText = objText.text;
            objText.text = "Zoom and rotate.";
            cam.transform.position = worldViewPos.position;

            // StartCoroutine(ZoomOut());

        }
        else
        {
            objText.text = origText;
            // StartCoroutine(ZoomIn());
        }

        worldViewActive = !worldViewActive;
        // cam.orthographic = worldViewActive;
        labels.SetActive(worldViewActive);
        agent.enabled = !worldViewActive;
        cameraControl.enabled = !worldViewActive;
        Toggle();
    }
    
    //to render the layermase RenderToWV to be viewable to the camera: Spheres for player and buggy location.
    private void Toggle()
    {
        cam.cullingMask ^= 1 << LayerMask.NameToLayer("RenderToWV");
    }
}
