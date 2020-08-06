using UnityEngine;

[CreateAssetMenu(fileName = "NewSceneData", menuName = "Scene Data", order = 51)]
public class SceneData : ScriptableObject {
    public string forwardAnimationClipName;
    public float forwardAnimationClipLength;
    public string backwardAnimationClipName;
    public float backwardAnimationClipLength;
    public string ZoomOutAnimationClipName;
    public float ZoomOutAnimationClipLength;
    public string ZoomInAnimationClipName;
    public float ZoomInAnimationClipLength;
    public string[] highlights;
    public string[] boundaries;
    public string[] lights;
    public string[] nerves;
    public Animator animator;

    public void Assign(GuidedTourManager manager)
    {
        manager.animator = animator;
    }
}
