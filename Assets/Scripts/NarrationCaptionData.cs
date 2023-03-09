using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewNarrationCaptionData", menuName = "Narration Caption Data", order = 53)]
public class NarrationCaptionData : ScriptableObject
{
    [TextArea(2, 5)]
    public string captionString;
    public float timing;
}
