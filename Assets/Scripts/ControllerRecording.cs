using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using UnityEngine;
using System;
using System.IO;
using System.Threading.Tasks;

public class ControllerRecording : MonoBehaviour {

    string filename;
    int dataTimer = 0;
    float dataInterval = 60;
    private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
    bool recordingData = false;
    public SteamVR_TrackedObject left;
    public SteamVR_TrackedObject right;
    public SteamVR_ControllerManager manager;

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
        QueueMessage("position;" + left.transform.position.ToString() + ";" + right.transform.position.ToString());


        if (recordingData)
        {
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
        Debug.Log("recording data: ");
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
                        Debug.Log("message: " + text);
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
        Debug.Log("NEW MESSAGE");
        messages.Enqueue(Time.time + ";" + text + "\r\n");
    }
}
