
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LabelPlayback : MonoBehaviour
{

    private struct Activity
    {
        public Activity(float time, string name, bool on)
        {
            Timing = time;
            Target = name;
            Active = on;
        }

        public float Timing;
        public string Target;
        public bool Active;
    }

    private Queue<Activity> ActivityQueue = new Queue<Activity>();
    private bool playing;
    private LabelManager labelManager;

    // Use this for initialization
    void Start()
    {
        FillQueue();
        labelManager = GameObject.Find("Label").GetComponent<LabelManager>();
        playing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (playing && ActivityQueue.Count > 0)
        {
            float time = ActivityQueue.Peek().Timing;
            if (Time.time >= time)
            {
                Activity activity = ActivityQueue.Dequeue();

                //this iteration can be made into O(1) by putting a dictionary in LabelManager
                foreach(GameObject label in labelManager.availableLabels)
                {
                    if(label.name == activity.Target)
                    {
                        label.SetActive(activity.Active);
                    }
                }
            }
        }
    }

    void FillQueue()
    {

        using (StreamReader sr = new StreamReader("Playback_labels.txt"))
        {
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                string trimmed = line.Trim();
                string[] separated = trimmed.Split(new char[] { ';' });
                float time = float.Parse(separated[0]);
                string name = separated[1];
                bool active = bool.Parse(separated[2]);
                ActivityQueue.Enqueue(new Activity(time, name, active));
            }
        }
    }
}
