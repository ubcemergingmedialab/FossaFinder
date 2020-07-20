using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Triggers respawn - if player misses the target when flying through the brain slice. Also handles transparency of the slice.
/// </summary>
public class BrainHitEvent : MonoBehaviour
{
    [SerializeField] TutorialEvents tutorial;
    [SerializeField] TutorialUI uiManager;

    private bool transparent = false;
    private Color matColor;
    private Renderer render;

    private void Start()
    {
        render = GetComponent<Renderer>();
        matColor = render.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && transparent && tutorial.GetMode() == TutorialMode.Skip)
        {
            // positive feedback if successfully pass through a skipped slice on the tutorial state for skipping
            uiManager.PlayPositiveFeedback();
        } else
        {
            uiManager.PlayNegativeFeedback();
            tutorial.RespawnPlayer();
        }
    }

    /// <summary>
    /// Called by the skip/unskip button. Toggles the transparent property of the slice and creates an animation of fading the material.
    /// </summary>
    public void ToggleSkipSlice()
    {
        StartCoroutine(ToggleFade(!transparent));
        transparent = !transparent;
    }

    /// <summary>
    /// Animation of fading the material to transparency
    /// </summary>
    /// <param name="transparent">Determines whether to fade in (true) or fade out (false) the slice material</param>
    /// <returns></returns>
    IEnumerator ToggleFade(bool transparent)
    {
        if (!transparent)
        {
            while (matColor.a < 1)
            {
                matColor.a += 0.1f;
                render.material.color = matColor;
                yield return null;
            }
        }
        else
        {
            while (matColor.a > 0)
            {
                matColor.a -= 0.1f;
                render.material.color = matColor;
                yield return null;
            }
        }
    }

    /// <summary>
    /// Allows other classes to access the transparent property of the slice
    /// </summary>
    /// <returns>bool of the transparent state of the slice</returns>
    public bool GetTransparent()
    {
        return transparent;
    }

    /// <summary>
    /// Allows other classes to set the transparent property of the slice
    /// </summary>
    /// <param name="setTransparent">The setting of the transparent property</param>
    public void SetTransparent(bool setTransparent)
    {
        transparent = setTransparent;
    }
}
