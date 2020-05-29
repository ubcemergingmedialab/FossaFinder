using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class GuidedTourManager : MonoBehaviour {

    public GameObject head; 
    public GameObject cameraRig;  
    public GameObject mainCamera;
    public Animator anim;
    public SceneData[] sceneDataArray;
    public GameObject[] pathObjects;

    int currentSceneDestination; // the current scene number
    int lastVisitedScene; // the previous scene number after a scene transition
    bool isDuringSceneTransition; // whether a scene transition is taking place or not
    float distanceFromDefaultCameraPositionThreshold; // the maximum allowed distance from the default camera position before the user should be teleported
    bool isPastDistanceThreshold; // whether the user is past the aforementioned distance threshold
    Vector2 defaultTopDownCameraCoordinates; // the top down coordinates (x and z values only) of the default camera position

    string currentNameOfAnimationClip;
    GameObject currentPathObject;
    float distanceTravelled;
    float speed;

	// Use this for initialization
	void Start () {
        currentSceneDestination = 1;
        lastVisitedScene = 1;
        isDuringSceneTransition = false;
        distanceFromDefaultCameraPositionThreshold = 0.2f;
        isPastDistanceThreshold = false;
        defaultTopDownCameraCoordinates = new Vector2(0, .5f);
        distanceTravelled = 0;
        speed = 0.01f;

        StartCoroutine(Compensate());
    }

    // Defines the world position of the camera rig and the skull, after the position of the camera is set
    IEnumerator Compensate()
    {
        yield return new WaitForSeconds(.5f);
        cameraRig.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, .5f) - mainCamera.GetComponent<SteamVR_Camera>().head.position; // mainCamera.GetComponent<SteamVR_Camera>().head.localPosition.y, 1f) - mainCamera.GetComponent<SteamVR_Camera>().head.localPosition
        head.transform.position = new Vector3(0, mainCamera.GetComponent<SteamVR_Camera>().head.position.y, 0);

        //foreach (SceneData sceneData in sceneDataArray)
        //{
        //    sceneData.skullPosition.y += head.transform.position.y;
        //}
        foreach(GameObject pathObject in pathObjects)
        {
            pathObject.transform.position = new Vector3(pathObject.transform.position.x, pathObject.transform.position.y + head.transform.position.y, pathObject.transform.position.z);

        }
        sceneDataArray[3].forwardPathObject = pathObjects[0]; // this will likely be a foreach later on
        sceneDataArray[2].backwardPathObject = pathObjects[1];
        sceneDataArray[5].forwardPathObject = pathObjects[2];
        sceneDataArray[4].backwardPathObject = pathObjects[3];
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
            currentSceneDestination -= 1;
            // if Dante's idea doesn't work, check if transition is completed here
            isDuringSceneTransition = true;
            currentNameOfAnimationClip = sceneDataArray[currentSceneDestination - 1].backwardAnimationClipName;
            if (sceneDataArray[currentSceneDestination - 1].backwardPathObject != null)
            {
                currentPathObject = sceneDataArray[currentSceneDestination - 1].backwardPathObject;
                Debug.Log(currentPathObject.name);
                speed = sceneDataArray[currentSceneDestination - 1].backwardSpeed;
            } else
            {
                currentPathObject = null;
                speed = 0;
            }
            
            CheckIfPastDistanceThreshold();
        }
    }

    // Maintains all necessary variables for transitioning into the next scene (the scene with the greater scene number). Update() will then check these variables and handle the actual movement
    public void SetUpVariablesForNextSceneNumber()
    {
        lastVisitedScene = currentSceneDestination;
        currentSceneDestination += 1;
        // if Dante's idea doesn't work, check if transition is completed here
        isDuringSceneTransition = true;
        currentNameOfAnimationClip = sceneDataArray[currentSceneDestination - 1].forwardAnimationClipName;
        if (sceneDataArray[currentSceneDestination - 1].forwardPathObject != null)
        {
            currentPathObject = sceneDataArray[currentSceneDestination - 1].forwardPathObject;
            Debug.Log(currentPathObject.name);
            speed = sceneDataArray[currentSceneDestination - 1].forwardSpeed;
        } else
        {
            currentPathObject = null;
            speed = 0;
        }
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
        cameraRig.transform.position = new Vector3(0, cameraRig.transform.position.y, .5f) - new Vector3(mainCamera.GetComponent<SteamVR_Camera>().head.position.x - cameraRig.transform.position.x, 0, mainCamera.GetComponent<SteamVR_Camera>().head.position.z - cameraRig.transform.position.z);
        isPastDistanceThreshold = false;
    }

    // Update is called once per frame
    // If the user is in a scene transition, incrementally changes the transform value of the skull based on which scene the user wants to go to
    void Update () {
        if (isDuringSceneTransition)
        {
            TransitionToAnotherScene();
        }
        //distanceTravelled += Time.deltaTime * speed;
        //head.transform.position = pathObjects[0].GetComponent<PathCreator>().path.GetPointAtDistance(distanceTravelled);
	}

    // Checks whether the user needs to be teleported first. Then, incrementally changes the transform values of the skull based on which scene the user wants to go to
    void TransitionToAnotherScene()
    {
        if (isPastDistanceThreshold)
        {
            TeleportBackToDefaultCameraPosition();
        }

        // If Dante's idea doesn't work, this is where you check if previous scenes are completed

        if (!string.IsNullOrEmpty(currentNameOfAnimationClip))
        {
            anim.Play(currentNameOfAnimationClip);
        }

        if (currentPathObject != null)
        {
            // Debug.Log("sup");
            // Debug.Log("distance travelled: " + distanceTravelled);
            distanceTravelled += speed * Time.deltaTime;
            head.transform.position = currentPathObject.GetComponent<PathCreator>().path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        }

        // isDuringSceneTransition = false;
        // Debug.Log("current head position: " + head.transform.position);
        Vector3 endskullPosition = new Vector3(sceneDataArray[currentSceneDestination - 1].skullPosition.x, sceneDataArray[currentSceneDestination - 1].skullPosition.y + head.transform.position.y, sceneDataArray[currentSceneDestination - 1].skullPosition.z);
        //Debug.Log("Distance between current skull position and end skull position: " + Vector3.Distance(head.transform.position, endskullPosition));
        //Debug.Log("Abs delta angle between current skull rotation.x and end skull rotation.x: " + Mathf.DeltaAngle(head.transform.rotation.eulerAngles.x, sceneDataArray[currentSceneDestination - 1].skullRotation.x));
        //Debug.Log("Abs delta angle between current skull rotation.y and end skull rotation.y: " + Mathf.DeltaAngle(head.transform.rotation.eulerAngles.y, sceneDataArray[currentSceneDestination - 1].skullRotation.y));
        //Debug.Log("Abs delta angle between current skull rotation.x and end skull rotation.z: " + Mathf.DeltaAngle(head.transform.rotation.eulerAngles.z, sceneDataArray[currentSceneDestination - 1].skullRotation.z));
        //Debug.Log("Distance between current skull scale and end skull scale: " + Vector3.Distance(head.transform.localScale, sceneDataArray[currentSceneDestination - 1].skullScale));
        if (Vector3.Distance(head.transform.position, endskullPosition) <= 0.01f && 
            Mathf.Abs(Mathf.DeltaAngle(head.transform.rotation.eulerAngles.x, sceneDataArray[currentSceneDestination - 1].skullRotation.x)) <= 0.01f &&
            Mathf.Abs(Mathf.DeltaAngle(head.transform.rotation.eulerAngles.y, sceneDataArray[currentSceneDestination - 1].skullRotation.y)) <= 0.01f && 
            Mathf.Abs(Mathf.DeltaAngle(head.transform.rotation.eulerAngles.z, sceneDataArray[currentSceneDestination - 1].skullRotation.z)) <= 0.01f &&
            Vector3.Distance(head.transform.localScale, sceneDataArray[currentSceneDestination - 1].skullScale) <= 0.01f)
        {
            isDuringSceneTransition = false;
            distanceTravelled = 0;
        }
    }

    // Skips to a particular end state/scene of a transition 
    public void SkipToScene(int sceneNumber)
    {
        // Debug.Log("hello am i here yet");
        // head.GetComponent<Animator>().enabled = false;
        anim.Play(currentNameOfAnimationClip, -1, 1);
        Vector3 endskullPosition = new Vector3(sceneDataArray[currentSceneDestination - 1].skullPosition.x, sceneDataArray[currentSceneDestination - 1].skullPosition.y + head.transform.position.y, sceneDataArray[currentSceneDestination - 1].skullPosition.z);
        head.transform.position = endskullPosition;
        isDuringSceneTransition = false;
        distanceTravelled = 0;
    }
}
