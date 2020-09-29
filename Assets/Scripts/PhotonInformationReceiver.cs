using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;

public class PhotonInformationReceiver : MonoBehaviourPun {

    public GameObject mainCamera;
    // public GameObject cameraPrefab;

    // Use this for initialization
    void Start () {
        
	}

    public void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    public void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    private void OnEvent(EventData eventData)
    {
        if (eventData.Code == 1)
        {
            object[] receivedData = (object[])eventData.CustomData;
            Vector3 clientCameraPosition = (Vector3)receivedData[0];
            Quaternion clientCameraRotation = (Quaternion)receivedData[1];

            // Quaternion clientCameraViewRotation = Quaternion.Euler(0, -90, 0);
            GameObject copyOfClientCameraObjectInServer = new GameObject("ClientCameraObjectInServer", typeof(RectTransform), typeof(Camera));
            // copyOfClientCameraObjectInServer.GetComponent<RectTransform>().pivot = new Vector2(0, 0);
            copyOfClientCameraObjectInServer.GetComponent<RectTransform>().position = mainCamera.transform.position + clientCameraPosition;
            copyOfClientCameraObjectInServer.GetComponent<RectTransform>().rotation = Quaternion.LookRotation(Vector3.left, Vector3.up) * clientCameraRotation;
            // GameObject copyOfClientCameraObjectInServer = Instantiate(cameraPrefab, mainCamera.transform.localPosition + clientCameraPosition, Quaternion.Euler(0, -90, 0) * clientCameraRotation);
            RaycastHit hit;
            if (Physics.Raycast(copyOfClientCameraObjectInServer.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)), out hit))
            {
                Debug.Log("Hit!");
                // Debug.Log(copyOfClientCameraObjectInServer.GetComponent<RectTransform>().position);
                Debug.DrawLine(copyOfClientCameraObjectInServer.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)).origin, hit.point, Color.yellow, 10f);
                GameObject testSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                testSphere.transform.position = hit.point;
                testSphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                // Debug.DrawRay(mainCamera.transform.localPosition + clientCameraPosition, mainCamera.transform.localRotation * Vector3.forward * hit.distance, Color.yellow);
            }
            Destroy(copyOfClientCameraObjectInServer);
            // Debug.Log(clientCameraPosition.x.ToString() + " " + clientCameraPosition.y.ToString() + " " + clientCameraPosition.z.ToString() + " " + clientCameraRotation.ToString());
           
            // Debug.Log("Supppp");
        }
    }

    // Update is called once per frame
    void Update () {
        // Debug.DrawRay(mainCamera.transform.position, mainCamera.transform.forward, Color.yellow, 2f);

    }
}
