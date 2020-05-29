using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scene Data", order = 51)]
public class SceneData : ScriptableObject {
    public Vector3 skullPosition; // endOfPathPosition; remove the rest
    public Vector3 skullRotation;
    public Vector3 skullScale;
    public string forwardAnimationClipName;
    public string backwardAnimationClipName;
    public GameObject forwardPathObject;
    public GameObject backwardPathObject;
    public float forwardSpeed;
    public float backwardSpeed;
}
