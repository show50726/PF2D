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
            GiveDirectionToPlayer(col.gameObject);
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


}
