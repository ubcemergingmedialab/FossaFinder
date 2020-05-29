using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scene Data", order = 51)]
public class SceneData : ScriptableObject {
    public Vector3 endSkullPosition;
    public Vector3 endSkullRotation;
    public Vector3 endSkullScale;
    public string forwardAnimationClipName;
    public string backwardAnimationClipName;
    public GameObject forwardPathObject;
    public Vector3 forwardPathDefaultPosition;
    public GameObject backwardPathObject;
    public Vector3 backwardPathDefaultPosition;
    public float forwardPathTraversalSpeed;
    public float backwardPathTraversalSpeed;
}
