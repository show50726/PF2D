//Finish2D      made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/07
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
        if (CheckObjectIsLegal(col.gameObject) == false) return;
        if (stateName == "")
        {
            insideObjects.Add(col.gameObject);
            UpdateObjectAnimatorCondictionBool(col.gameObject, updateCondictionBoolName, true);
            GiveDirectionToPlayer(col.gameObject);
            RefreshCheckedStatus(col.gameObject);
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
