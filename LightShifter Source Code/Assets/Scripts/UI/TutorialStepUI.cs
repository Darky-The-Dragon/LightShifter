using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StepUIReferences {
    public TutorialStep step;

    // Each array can have multiple icon prefabs:
    public GameObject[] keyboardUI; 
    public GameObject[] playStationUI;
    public GameObject[] xboxUI;
    public GameObject[] switchUI;
}


public enum TutorialStep
{
    Walk,
    Jump,
    DoubleJump,
    HoldJump,
    LightShift,
    WallJump
}
