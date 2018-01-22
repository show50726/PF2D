//Finish made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/13
//usage:            this is a BASE class. Inherit this to make a custom one.

using UnityEngine;

public class Finish : MonoBehaviour
{
    [ReadOnly, Tooltip("Don't modifiy it, as it is used to system auto judge.")]
    public bool isFinished = false;
    [Header("Basic Setting")]
    public bool[] switchCase = { true };
    [Tooltip("If not assigned, it will be finished as any gameobject with right status.")]
    public GameObject assignedPlayer;

    internal System.Collections.Generic.List<GameObject> insideObjects = new System.Collections.Generic.List<GameObject>();

    [Header("Portal Setting")]
    [Tooltip("If left empty or non-exist bool name, will just restart level.")]
    public string directionConditionBool = "Go"; //Saved in Player, will be called by LevelManager

    [Header("Animator Detect")]
    [Tooltip("if set to nothing, it will skip checking.")]
    public string stateName = "";
    [Tooltip("0 is for base layer. Don't change it unless you know what you're doing.")]
    public int targetLayer = 0;
    internal int stateHashCode = 0;

    [Header("Animator Set")]
    [Tooltip("if set to nothing, it will skip updating.")]
    public string updateCondictionBoolName = "InFinish";

}
