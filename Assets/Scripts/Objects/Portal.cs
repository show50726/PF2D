//Portal            made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/12
//usage:            set this to an object, then when player is in right status, it will be portalled.
//NOTE:             use trigger. 2D only.
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Basic Setting")]
    public bool[] switchCase = { true };

    [Header("Animator Detect")]
    public string stateName = "HoldingDown";
    [Tooltip("0 is for base layer. Don't change it unless you .")]
    public int targetLayer = 0;
    private int stateHashCode = 0;
    
    private void OnTriggerStay2D(Collider2D col)
    {
        foreach (bool b in switchCase)
        {
            if (b == false)
            {
                Debug.Log("The portal " + name + " will not work because its switchCase didn't full-open.");
                return;
            }
        }
        Animator anim = col.GetComponent<Animator>();
        if (anim != null)
        {
            if (stateName == "")
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: trying to check state with state name not assigned.");
                return;
            }
            stateHashCode = Animator.StringToHash(anim.GetLayerName(targetLayer) + "." + stateName);
            //hash code is equal when part of it is equal.
            if (anim.GetCurrentAnimatorStateInfo(targetLayer).fullPathHash == stateHashCode)
            {
                DoTransport(col.gameObject);
            }
        }
    }
    private void DoTransport(GameObject objTriggered)
    {

    }


}
