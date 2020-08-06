using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class LabelRecording: MonoBehaviour
{

    public List<VRTK.Trigger_labels> labels;
    private ConcurrentQueue<string> messages = new ConcurrentQueue<string>();
    private int fileIndex;
    string filename;
    bool recordingData = false;
    int dataTimer = 0;
    int dataInterval = 60;
    int objectTrackingTimer = 0;
    int objectTrackingInterval = 1;

    // Use this for initialization

    void startRecordingData()
    {
        Debug.Log("recording data: ");
        filename = DateTime.Now.ToString("yyyy_MM_dd_HH_mm") + "_labels.txt";
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
                }
            });
        }
    }

    public void QueueMessage(string text)
    {
        messages.Enqueue(Time.time + ";" + text + "\r\n");
    }

    public void Start()
    {
        foreach(VRTK.Trigger_labels label in labels)
        {
            Debug.Log("injecting recorder");
            label.InjectRecorder(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!recordingData)
        {
            startRecordingData();
        }

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

    private void OnDestroy()
    {
        recordingData = false;
    }
}