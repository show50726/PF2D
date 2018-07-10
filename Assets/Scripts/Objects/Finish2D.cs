//Finish2D      made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/10
//usage:            set this to an object, then when (assigned) player is in right status, it will be checked.
//NOTE:             use trigger. 2D only.
using UnityEngine;

public class Finish2D : Finish
{
    /// <summary>
    /// If next scene contains this door (by name), it will be opened. Assign the DOORSET name, not the door.
    /// </summary>
    [Tooltip("If next scene contains this door (by name), it will be opened. Assign the DOORSET name, not the door.")]
    public string openThisDoor = "FinishSet";

    private void SaveDoorOnPlayer(GameObject playerObj, string doorName)
    {
        Player p = playerObj.GetComponent<Player>();
        if (p == null)
        {
            Debug.LogWarning(name + "/" + GetType().Name + " warning: trying to update a doorName onto non-player object " + playerObj.name + "." +
                "Check your code.");
            return;
        }
        p.openThisDoorInNextScene = doorName;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (CheckObjectIsLegal(col.gameObject) == false) return;
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
            UpdateObjectAnimatorCondictionBool(col.gameObject, updateCondictionBoolName, true);
            GiveDirectionToPlayer(col.gameObject);
            RefreshCheckedStatus(col.gameObject);
            SaveDoorOnPlayer(col.gameObject, openThisDoor);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        if (stateName == "") return; //see OnTriggerEnter2D
        if (CheckObjectIsLegal(col.gameObject) == false) return;
        Animator anim = col.GetComponent<Animator>();
        stateHashCode = Animator.StringToHash(anim.GetLayerName(targetLayer) + "." + stateName);
        if (anim != null)
        {
            //hash code is equal when part of it is equal.
            if (anim.GetCurrentAnimatorStateInfo(targetLayer).fullPathHash == stateHashCode)
            {
                GiveDirectionToPlayer(col.gameObject);
                CheckAndAddObjectInside(col.gameObject);
                RefreshCheckedStatus(col.gameObject);
                SaveDoorOnPlayer(col.gameObject, openThisDoor);
                if (updateCondictionBoolName != "") anim.SetBool(updateCondictionBoolName, true);
                return;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (CheckObjectIsLegal(col.gameObject) == false) return;
        if (CheckObjectInside(col.gameObject))
        {
            insideObjects.Remove(col.gameObject);
            UpdateObjectAnimatorCondictionBool(col.gameObject, updateCondictionBoolName, false);
            RefreshCheckedStatus();
        }
    }
    private void UpdateObjectAnimatorCondictionBool(GameObject obj, string boolName, bool setValue)
    {
        Animator anim = obj.GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to update " + obj.name + " animator, but animator not exists. Will do nothing, and this might cause bugs.");
            return;
        }
        if (boolName == null)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to update " + obj.name + " animator parameter with empty name. Did you forget to set it?");
            return;
        }
        anim.SetBool(boolName, setValue);
    }

}
