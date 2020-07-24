using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ActivityPlayback : MonoBehaviour {

    private struct Activity
    {
        public Activity(float time, string name)
        {
            Timing = time;
            Action = name;
        }

        public float Timing;
        public string Action;
    }

    private Queue<Activity> ActivityQueue = new Queue<Activity>();
    private bool playing;
    private GuidedTourManager tour;

	// Use this for initialization
	void Start () {
        FillQueue();
        tour = GameObject.Find("GuidedTourManager").GetComponent<GuidedTourManager>();
        playing = true;
	}
	
	// Update is called once per frame
	void Update () {
        if(playing)
        {
            float time = ActivityQueue.Peek().Timing;
            if (Time.time >= time)
            {
                Activity activity = ActivityQueue.Dequeue();
                Debug.Log(time + ";" + activity.Action);
                tour.GetType().GetMethod(activity.Action).Invoke(tour, new object[] { });
            }
        }
	}

    void FillQueue()
    {

        using (StreamReader sr = new StreamReader("Playback.txt"))
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
                ActivityQueue.Enqueue(new Activity(time, name));
            }
        }
    }
}
