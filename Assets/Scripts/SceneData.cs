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
}
