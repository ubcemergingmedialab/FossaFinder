using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;
using Valve.VR;

public class ControllerRecording : MonoBehaviour {

    string filename;
    int dataTimer = 0;
    float dataInterval = 60;
    private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
    bool recordingData = false;
    public SteamVR_TrackedObject left;
    public SteamVR_TrackedObject right;
    public SteamVR_ControllerManager manager;
    public SteamVR_Camera steamCamera;

    private void Start()
    {
        if(left == null || right == null || manager == null)
        {
            Debug.Log("[ControllerRecording]: please connect controller SteamVR_TrackedObjects and SteamVR_ControllerManager in Inspector");
            return;
        }

        if (!recordingData)
        {
            startRecordingData();
        }

    }

    private void TriggerHandler(object sender, ClickedEventArgs e)
    {
        Debug.Log("Trigger");
    }

    private void Update()
    {
        if (recordingData)
        {
            Vector3 leftPosition = left.transform.position - steamCamera.head.transform.position;
            Vector3 rightPosition = right.transform.position - steamCamera.head.transform.position;
            QueueMessage(left.transform.position.ToString("0.000") + ";" + right.transform.position.ToString("0.000") + ";" + left.transform.rotation.ToString("0.000") + ";" + right.transform.rotation.ToString("0.000"));
            if (dataTimer >= dataInterval)
            {
                writeToDataFile();
                dataTimer = 0;
            }
            else
            {
                dataTimer += 1;
            }
        }
    }

    void startRecordingData()
    {
        filename = DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_controllers.txt";
        recordingData = true;
    }

    async void writeToDataFile()
    {
        if (recordingData)
        {
            //Debug.Log("writing data");
            string text;
            await Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(filename, true))
                {
                    while (messages.TryDequeue(out text))
                    {
                        sw.Write(text);
                    }
                    sw.Flush();
                    sw.Close();
                    Debug.Log("empty queue");
                }
                Debug.Log("finished task");
            });
        }
    }

    public void QueueMessage(string text)
    {
        messages.Enqueue(Time.time + ";" + text + "\r\n");
    }
}
