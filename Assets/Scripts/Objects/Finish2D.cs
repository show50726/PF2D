//Finish2D      made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/18
//usage:            set this to an object, then when (assigned) player is in right status, it will be checked.
//NOTE:             use trigger. 2D only.
using UnityEngine;

public class Finish2D : Finish
{
    
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
    private void GiveDirectionToPlayer(GameObject playerObj)
    {
        Player player = playerObj.GetComponent<Player>();
        if (player==null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: an non-player Object is trying to get direction. Did you forget to assign it Player component?");
            return;
        }
        player.levelGoingDirectionConditionName = directionConditionBool;
        return;
    }

}
