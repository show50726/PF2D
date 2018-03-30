//Finish made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/03/30
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
    //[Tooltip("If left empty or non-exist bool name, will just restart level.")]
    //public string directionConditionBool = "Go"; //Saved in Player, will be called by LevelManager
    [Tooltip("If left empty or not-saved scene name, will just restart level.")]
    public string goToThisScene = "Title";
    [Tooltip("Will transfer to the position in NEW scene. Except set to (0,0,0).")]
    public Vector3 goToThisPosition = Vector3.zero;

    [Header("Animator Detect")]
    [Tooltip("if set to nothing, it will skip checking.")]
    public string stateName = "";
    [Tooltip("0 is for base layer. Don't change it unless you know what you're doing.")]
    public int targetLayer = 0;
    internal int stateHashCode = 0;

    [Header("Animator Set")]
    [Tooltip("if set to nothing, it will skip updating.")]
    public string updateCondictionBoolName = "InFinish";

    internal bool CheckObjectInside(GameObject targetObj)
    {
        if (insideObjects.Count == 0) return false;
        foreach (GameObject obj in insideObjects)
            if (obj == targetObj)
                return true;
        return false;
    }

    internal void CheckAndAddObjectInside(GameObject targetObj)
    {
        if (CheckObjectInside(targetObj) == false)
        {
            insideObjects.Add(targetObj);
        }
    }

    internal void RefreshCheckedStatus()
    {
        foreach (bool b in switchCase)
        {
            if (b == false)
            {
                Debug.Log("The " + GetType().Name + " of " + name + " will not work now because its switchCase didn't full-open.");
                isFinished = false;
                return;
            }
        }
        if (insideObjects.Count == 0)
        {
            isFinished = false;
            return;
        }
        if (assignedPlayer == null && insideObjects.Count > 0)
        {
            isFinished = true;
            return;
        }
        foreach (GameObject obj in insideObjects)
        {
            if (obj == assignedPlayer)
            {
                isFinished = true;
                return;
            }
        }
        isFinished = false;
        return;
    }

    internal void RefreshCheckedStatus(GameObject targetObj)
    {
        foreach (bool b in switchCase)
        {
            if (b == false)
            {
                Debug.Log("The " + GetType().Name + " of " + name + " will not work now because its switchCase didn't full-open.");
                isFinished = false;
                return;
            }
        }
        if (targetObj == assignedPlayer || assignedPlayer == null)
        {
            isFinished = true;
            return;
        }
    }
    internal void GiveDirectionToPlayer(GameObject playerObj)
    {
        Player player = playerObj.GetComponentInParent<Player>(); //due to magical solution of controller (?)
        if (player == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: an non-player Object " + playerObj.name + " is trying to get direction. Did you forget to assign it Player component?");
            return;
        }
        player.nextScene = goToThisScene;
        if(goToThisPosition != Vector3.zero) player.nextPosition = goToThisPosition;
        //player.levelGoingDirectionConditionName = directionConditionBool;
        return;
    }

}
