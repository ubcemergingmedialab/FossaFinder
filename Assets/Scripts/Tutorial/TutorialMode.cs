using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as an identifier for a Tutorial Modes/events.
/// </summary>
/// 

/*
 * NOTE: DO NOT CHANGE NUMBERS ASSOCIATED WITH ENUM VALUES!
 * You CAN change the ORDER of the list, just as long as 
 * the value-number pairs are UNCHANGED.
 * The numerical values do NOT depict ordering in the list, but
 * rather, the ID of each enum value, which is used by the
 * Unity Editor to keep track of selected values for scene components.
 * */
public enum TutorialMode
{
    None,
    Intro,
    Steering,
    Target,
    Boost,
    Hidden,
    Skip,
    Unskip,
    Hint,
    Finish,
}
