using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRotation : MonoBehaviour {

    [SerializeField] GameObject labels;
    [SerializeField] Text objText;
    //This script takes in Empty Gameobject where origin location for pause camera will go

    public Camera cam;
    public Transform target;
    public Transform worldViewPos;
    private Transform origPos;
    private string origText;

    public float rotSpeed = 0.5f;

    //[Range(1.0f, 65f)]
    public float cameraDistance;

    //There are two options. Our current prototype uses Ortho, but if we need perspective, it's here too
    public float perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    public float orthoZoomSpeed = 0.5f;        // The rate of change of the orthographic size in orthographic mode.
    public Button worldViewButton;
    public bool worldViewActive = false;

    private float rotX = 0f;
    private float rotY = 0f;
    private float dir = -1;
    private Vector3 origRot;

    private AgentController agent;
    private CameraController cameraControl;
    private float origFOV;
    

    void Start(){

        //        cam.transform.position = (cam.transform.position - target.position).normalized * cameraDistance + target.position;

        agent = GameObject.FindGameObjectWithTag("Player").GetComponent<AgentController>();
        cameraControl = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        origRot = cam.transform.eulerAngles;
        rotX = origRot.x;
        rotY = origRot.y;
    }

    // Update is called once per frame
    void FixedUpdate(){

        GestureMovement();


    }

    void GestureMovement() {

        if (worldViewActive)
        {
            // ------------------------------- ROTATE
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                /*rotX += touchDeltaPosition.y * Time.deltaTime * 10.0f;
                rotY -= touchDeltaPosition.x * Time.deltaTime * 10.0f;
                Mathf.Clamp(rotX, -45f, 45f);
                cam.transform.eulerAngles = new Vector3(rotX, rotY, 0f); */

                if (!cam.orthographic)
                {
                    // cam.transform.Translate(-touchDeltaPosition.x * rotSpeed, -touchDeltaPosition.y * rotSpeed, 0);
                    // cam.transform.RotateAround(target.transform.position, target.transform.up, 100 * Time.deltaTime);
                    cam.transform.RotateAround(target.transform.position, -target.transform.up, -touchDeltaPosition.x * Time.deltaTime * 1.5f); // rotate along x-axis
                    cam.transform.RotateAround(target.transform.position, target.transform.right, -touchDeltaPosition.y * Time.deltaTime * 4f); // rotate along y-axis
                }
                else
                {
                    cam.transform.position = (cam.transform.position - target.position).normalized * cameraDistance + target.position;
                }
            }

            //----------------------------------- ZOOM
            if (Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                //calculating the distance between touches between each frame
                float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                //if this value is negative, then the fingers are moving apart and positive as they move together
                float deltaMagnitudediff = prevTouchDeltaMag - touchDeltaMag;

                if (cam.orthographic)
                {
                    cam.orthographicSize += deltaMagnitudediff * orthoZoomSpeed;
                    // cam.orthographicSize = Mathf.Max(cam.orthographicSize, .5f);
                    if (cam.orthographicSize > 260f) { cam.orthographicSize = 260f; }
                    if (cam.orthographicSize < 5f) { cam.orthographicSize = 5f; }
                }
                else
                {
                    // For perspective camera, world view position must be outside the brain and maximum zoom out can be to 100f to prevent warping

                    // +deltaMagnitude = zooming out
                    // -deltaMagnitude = zooming in
                    Debug.Log(deltaMagnitudediff);
                    Debug.Log(Vector3.Distance(target.position, cam.transform.position));
                    // max & min zooming
                    cam.fieldOfView += deltaMagnitudediff * perspectiveZoomSpeed;
                    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 10.0f, 100.0f);
                }
            }
            cam.transform.LookAt(target);
        }
    }

    IEnumerator ZoomOut() {

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

    public void ButtonClick()
    {
        if (!worldViewActive)
        {
            //worldViewButton.image = Resources.Load < "" >;
            origPos = cam.transform;
            origFOV = cam.fieldOfView;
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

        cam.fieldOfView = origFOV;
        worldViewActive = !worldViewActive;
        // cam.orthographic = worldViewActive;
        labels.SetActive(worldViewActive);
        agent.enabled = !worldViewActive;
        cameraControl.enabled = !worldViewActive;
        Toggle();
    }

    private void Toggle()
    {
        cam.cullingMask ^= 1 << LayerMask.NameToLayer("RenderToWV");
    }
}
