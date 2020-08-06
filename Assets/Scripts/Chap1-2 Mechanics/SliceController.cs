using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Written By Nicco Pucci - Refactored by Oliver Vennike Riisager
/// </summary>
public class SliceController : MonoBehaviour, IComparable<SliceController>
{
    public static UnityEvent positiveFeedback = new UnityEvent();
    public static UnityEvent negativeFeedback = new UnityEvent();
    public static UnityEvent emptyHintIndication = new UnityEvent();

    [SerializeField]
    Texture miniMapTexture;
    public Texture MiniMapTexture { get { return miniMapTexture; } }

    [SerializeField] GameObject indicator;
    [SerializeField] GameObject animateSkip;
    [SerializeField] GameObject regularSkip;

    Material material;
    Collider collTrigger;

    const float SKIP_TARGET_ALPHA = 0.4f;
    const float OPAQUE_ALPHA = 1f;
    const float FADE_TIME_SEC = 1f;

    bool coroutineRunning = false;
    public bool Skipped { get; private set; }
    public bool Disabled { get; private set; }
    float targetAlpha;

    public bool HintActive { get; private set; }

    public Objective[] SliceObjectives { get; private set; }
    public Collider[] SliceObjectiveColliders { get; private set; }
    public Objective ActiveObjective { get; protected set; }
    public List<Objective> ActiveObjectives { get; protected set; } = new List<Objective>();

    void Start()
    {
        FindObjectives();
        HintActive = false;
        if (miniMapTexture == null)
        {
            Debug.LogWarning("Slice Controller missing MiniMap Texture - Is this intentional?");
        }

        Renderer renderer = GetComponent<Renderer>();
        material = renderer.material;

        collTrigger = GetComponent<Collider>();
    }

    /// <summary>
    /// Finds all objectives connected to this gameobject
    /// also sets the active objective depending on the level
    /// </summary>
    private void FindObjectives()
    {
        List<Objective> objectives = new List<Objective>(); //Create list to hold children
        List<Collider> colliders = new List<Collider>(); //Create list to hold children colliders
        var sliceParent = transform.parent.transform;
        for (int i = 0; i < sliceParent.childCount; i++)
        {
            var childTrans = sliceParent.GetChild(i);
            if (childTrans.tag != "Objective")
            {
                continue;
            }

            Objective currentChild = childTrans.GetComponent<Objective>(); //Get the childs objective component
            string childObjective = currentChild.GetObjectiveName(); //Get the current objective name
            objectives.Add(currentChild);//Add everything else
            colliders.AddRange(currentChild.GetComponentsInChildren<Collider>());

            if (childObjective == ObjectiveUtility.CurrentObjective)
            {
                ActiveObjective = currentChild.GetComponent<Objective>(); //If we find one with the same tag as currentlevels objective
                ActiveObjectives.Add(currentChild.GetComponent<Objective>());
            }
        }
        SliceObjectiveColliders = colliders.ToArray();
        SliceObjectives = objectives.ToArray();
    }

    public void DisableSlice()
    {
        Disabled = true;
        collTrigger.enabled = false;

        targetAlpha = SKIP_TARGET_ALPHA;
        StartFadeCoroutine();
    }

    /// <summary>
    /// Toggles wether or not a slice should be skipped
    /// </summary>
    public void ToggleSkip()
    {
        if (Disabled) //Do nothing if hint is active or slice is disabled
            return;

        if (Skipped) // NEED TO UN-SKIP
        {
            targetAlpha = OPAQUE_ALPHA;
            Skipped = false;
        }
        else // NEED TO SKIP
        {
            targetAlpha = SKIP_TARGET_ALPHA;
            Skipped = true;
        }
        StartFadeCoroutine();
    }

    /// <summary>
    /// Enables hint on this slice
    /// </summary>
    public void EnableHint()
    {
        if (ActiveObjective != null)
        {
            if (ActiveObjectives != null)
            {
                foreach (var objective in ActiveObjectives)
                {
                    objective.Highlight();
                }
            }
            else
            {
                ActiveObjective.Highlight();
            }
        }
        else
        {
            emptyHintIndication.Invoke();
        }
        HintActive = true;
    }

    /// <summary>
    /// Resets the slices boolean values.
    /// Also unskips if it has been skipped
    /// </summary>
    public void ResetSlice()
    {
        HintActive = false;
        if (Disabled)
            ToggleObjectiveColliders();
        if (Skipped)
            ToggleSkip();
        Skipped = false;
    }

    public void ResetSliceAndObjectives()
    {
        ResetSlice();
        for (int i = 0; i < SliceObjectives.Length; i++)
        {
            SliceObjectives[i].RemoveHighlight();
        }
    }

    public void ToggleObjectiveColliders()
    {
        if (!Disabled)
        {
            for (int i = 0; i < SliceObjectiveColliders.Length; i++)
            {
                SliceObjectiveColliders[i].enabled = false;
            }
            collTrigger.enabled = false;
            Disabled = true;
        }
        else
        {
            for (int i = 0; i < SliceObjectiveColliders.Length; i++)
            {
                SliceObjectiveColliders[i].enabled = true;
            }
            collTrigger.enabled = true;
            Disabled = false;
        }
    }

    public void DisableObjectiveColliders()
    {
        for (int i = 0; i < SliceObjectiveColliders.Length; i++)
        {
            SliceObjectiveColliders[i].enabled = false;
        }
    }

    /// <summary>
    /// Tells the activeobjective that is is correctly hit
    /// </summary>
    public void Correct()
    {
        if (ActiveObjectives != null)
        {
            foreach (var objective in ActiveObjectives)
            {
                objective.Correct();
            }
        }
        else
            ActiveObjective?.Correct();

        ToggleObjectiveColliders();
    }

    /// <summary>
    /// Tells the activeobjective to be missed
    /// </summary>
    public void Missed()
    {
        if (ActiveObjectives != null)
        {
            foreach (var objective in ActiveObjectives)
            {
                objective.Missed();
            }
        }
        else
            ActiveObjective?.Missed();

        ToggleObjectiveColliders();
        indicator.GetComponent<Indicators>().SetNegative();
    }

    /// <summary>
    /// Sorts slices depending on their Z position.
    /// </summary>
    public int CompareTo(SliceController other)
    {
        var otherZ = other.transform.position.z;
        var myZ = transform.position.z;
        if (otherZ < myZ)
        {
            return 1;
        }
        else if (otherZ == myZ)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    void StartFadeCoroutine()
    {
        if (coroutineRunning)
        {
            return;
        }

        coroutineRunning = true;
        StartCoroutine(UpdateRoutine());
    }

    IEnumerator UpdateRoutine()
    {
        if (material == null)
        {
            Debug.LogError("Material is null");
            yield break;
        }

        Color color = material.color;

        bool reachedTarget = false;
        while (!reachedTarget)
        {
            float alphaChangeAmount = (Time.deltaTime / FADE_TIME_SEC);

            if (targetAlpha == OPAQUE_ALPHA)
            {
                color.a += alphaChangeAmount;
            }
            else
            {
                color.a -= alphaChangeAmount;
            }

            color.a = Mathf.Clamp(color.a, SKIP_TARGET_ALPHA, OPAQUE_ALPHA);

            material.color = color;

            bool reachLowerTarget = (color.a == SKIP_TARGET_ALPHA);
            bool reachUpperTarget = (color.a == OPAQUE_ALPHA);

            reachedTarget = (reachLowerTarget || reachUpperTarget);

            yield return null;
        }

        coroutineRunning = false;
    }
}
