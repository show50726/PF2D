//Finish2D      made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/13
//usage:            set this to an object, then when (assigned) player is in right status, it will be checked.
//NOTE:             use trigger. 2D only.
using UnityEngine;

public class Finish2D : Finish
{
    [Header("Basic Setting")]
    public bool[] switchCase = { true };
    [Tooltip("If not assigned, it will be finished as any gameobject with right status.")]
    public GameObject assignedPlayer;

    private System.Collections.Generic.List<GameObject> insideObjects = new System.Collections.Generic.List<GameObject>();

    [Header("Animator Detect")]
    [Tooltip("if set to nothing, it will skip checking.")]
    public string stateName = "";
    [Tooltip("0 is for base layer. Don't change it unless you know what you're doing.")]
    public int targetLayer = 0;
    private int stateHashCode = 0;

    [Header("Animator Set")]
    [Tooltip("if set to nothing, it will skip updating.")]
    public string updateCondictionBoolName = "InFinish";


    private void OnTriggerEnter2D(Collider2D col)
    {
        foreach (bool b in switchCase)
        {
            if (b == false)
            {
                Debug.Log("The " + GetType().Name + " of " + name + " will not work now because its switchCase didn't full-open.");
                return;
            }
        }
        if (stateName == "")
        {
            insideObjects.Add(col.gameObject);
            if (updateCondictionBoolName != "")
            {
                Animator anim = col.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetBool(updateCondictionBoolName, true);
                }
            }
            RefreshCheckedStatus(col.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (stateName == "") return; //see OnTriggerEnter2D
        Animator anim = col.GetComponent<Animator>();
        stateHashCode = Animator.StringToHash(anim.GetLayerName(targetLayer) + "." + stateName);
        if (anim != null)
        {
            //hash code is equal when part of it is equal.
            if (anim.GetCurrentAnimatorStateInfo(targetLayer).fullPathHash == stateHashCode)
            {
                CheckAndAddObjectInside(col.gameObject);
                RefreshCheckedStatus(col.gameObject);
                if (updateCondictionBoolName != "") anim.SetBool(updateCondictionBoolName, true);
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (CheckObjectInside(col.gameObject))
        {
            insideObjects.Remove(col.gameObject);
            if (updateCondictionBoolName != "")
            {
                Animator anim = col.GetComponent<Animator>();
                if (anim != null)
                {
                    anim.SetBool(updateCondictionBoolName, true);
                }
            }
            RefreshCheckedStatus();
        }
    }
    private bool CheckObjectInside(GameObject targetObj)
    {
        if (insideObjects.Count == 0) return false;
        foreach (GameObject obj in insideObjects)
            if (obj == targetObj)
                return true;
        return false;
    }
    private void CheckAndAddObjectInside(GameObject targetObj)
    {
        if (CheckObjectInside(targetObj) == false)
        {
            insideObjects.Add(targetObj);
        }
    }
    private void RefreshCheckedStatus()
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
    private void RefreshCheckedStatus(GameObject targetObj)
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

}
