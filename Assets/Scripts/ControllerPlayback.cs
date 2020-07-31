using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ControllerPlayback : MonoBehaviour {
    private struct ControllerMovement
    {
        public ControllerMovement(float time, Vector3 left, Vector3 right, Quaternion leftQuat, Quaternion rightQuat)
        {
            Timing = time;
            LeftPosition = left;
            LeftOrientation = leftQuat;
            RightPosition = right;
            RightOrientation = rightQuat;
        }

        public float Timing;
        public Vector3 LeftPosition;
        public Vector3 RightPosition;
        public Quaternion LeftOrientation;
        public Quaternion RightOrientation;
    }

    private Queue<ControllerMovement> ActivityQueue = new Queue<ControllerMovement>();
    private bool playing;
    public Transform left;
    public Transform right;

    // Use this for initialization
    void Start()
    {
        FillQueue();
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
                ControllerMovement movement = ActivityQueue.Dequeue();
                right.position = movement.RightPosition;
                right.rotation = movement.RightOrientation;
            }
        }
    }

    void FillQueue()
    {
        using (StreamReader sr = new StreamReader("Playback_controllers.txt"))
        {
            string line;
            // Read and display lines from the file until the end of
            // the file is reached.
            while ((line = sr.ReadLine()) != null)
            {
                string trimmed = line.Trim();
                string[] separated = trimmed.Split(new char[] { ';' });
                float time = float.Parse(separated[0]);
                Vector3 leftPosition = ParseVector3(separated[1]);
                Vector3 rightPosition = ParseVector3(separated[2]);
                Quaternion leftOrientation = ParseQuaternion(separated[3]);
                Quaternion rightOrientation = ParseQuaternion(separated[4]);
                ActivityQueue.Enqueue(new ControllerMovement(time, leftPosition, rightPosition, leftOrientation, rightOrientation));
            }
        }
    }

    /// <summary>
    /// helper function to parse a string in the format "(0.0,0.0,0.0)" into a vector3
    /// </summary>
    /// <param name="s">string assumed in the format "(0.0,0.0,0.0)"</param>
    /// <returns>A Vector3</returns>
    Vector3 ParseVector3(string s)
    {
        string trimmed = s.TrimStart('(').TrimEnd(')');
        string[] elements = trimmed.Split(new char[] { ',' });
        float x = float.Parse(elements[0]);
        float y = float.Parse(elements[1]);
        float z = float.Parse(elements[2]);
        return new Vector3(x, y, z);
    }

    /// <summary>
    /// helper function to parse a string in the format "(0.0,0.0,0.0,0.0)" into a vector3
    /// </summary>
    /// <param name="s">string assumed in the format "(0.0,0.0,0.0)"</param>
    /// <returns>A Vector3</returns>
    Quaternion ParseQuaternion(string s)
    {
        string trimmed = s.TrimStart('(').TrimEnd(')');
        string[] elements = trimmed.Split(new char[] { ',' });
        float x = float.Parse(elements[0]);
        float y = float.Parse(elements[1]);
        float z = float.Parse(elements[2]);
        float w = float.Parse(elements[3]);
        return new Quaternion(x, y, z, w);
    }
}
