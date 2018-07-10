//Controller_GoIn   made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/11
//usage:            a controller to make player "go in / go out" some room.

using UnityEngine;
using UnityExtensions;
public class Controller_GoIn : STCMonoBehaviour {

    public KeyCode keyOfIn = KeyCode.S;
    public Animator targetSM;
    [Tooltip("Check happens before update. Left nothing to skip check.")]
    public string checkConditionBoolName = "InFinish";
    public string updateConditionBoolName = "HoldingIn";

    /// <summary>
    /// The player cannot go out if there's something in these layers block the path.
    /// </summary>
    [Tooltip("The player cannot go out if there's something in these layers block the path.")]
    public LayerMask stopByTheseObjects = (1 << 0) | (1 << 10);
    /// <summary>
    /// Used to check if there's anything blocked the out path.
    /// </summary>
    private System.Collections.Generic.List<GameObject> blockList = new System.Collections.Generic.List<GameObject>();
    private bool GoOut = true;
    private bool checkedAnimatorAvailable = false;

    private void Start()
    {
        if (targetSM == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: didn't set targetSM. The script will not work.");
            enabled = false;
            return;
        }
        foreach (AnimatorControllerParameter p in targetSM.parameters)
        {
            if (p.name == updateConditionBoolName && p.type == AnimatorControllerParameterType.Bool)
            {
                if (checkConditionBoolName == "") checkedAnimatorAvailable = true;
                else
                {
                    foreach (AnimatorControllerParameter pa in targetSM.parameters)
                    {
                        if (pa.name == checkConditionBoolName && pa.type == AnimatorControllerParameterType.Bool)
                        {
                            checkedAnimatorAvailable = true;
                            break;
                        }
                    }
                }
                break;
            }
        }
        if (checkedAnimatorAvailable == false)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: didn't find match updateConditionBoolName and won't update. Do you type it right and set it to bool?");
        }
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(keyOfIn))
        {
            if (GetComponent<Rigidbody2D>().velocity.magnitude != 0)
            {
                Debug.Log(name + " cannot \"go in\" while in action.");
                return;
            }
            if (checkedAnimatorAvailable)
            {
                if (checkConditionBoolName == "" || targetSM.GetBool(checkConditionBoolName))
                {
                    if(targetSM.GetBool(updateConditionBoolName) && GoOut)
                        targetSM.SetBool(updateConditionBoolName, !targetSM.GetBool(updateConditionBoolName));
                    else if(!targetSM.GetBool(updateConditionBoolName))
                        targetSM.SetBool(updateConditionBoolName, !targetSM.GetBool(updateConditionBoolName));
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        //if(targetSM.GetBool(updateConditionBoolName) && col.gameObject.tag == "Player")
        //{
        //    GoOut = false;
        //    //Debug.Log("Cannot go out now!");
        //}
        if (targetSM.GetBool(updateConditionBoolName) && LayerMaskExtensions.IsInLayerMask(col.gameObject, stopByTheseObjects))
        {
            blockList.Add(col.gameObject);
        }
        if (blockList.Count != 0)
        {
            GoOut = false;
            DebugMessage(LogType.Normal, "cannot go out now.");
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        //if (targetSM.GetBool(updateConditionBoolName) && col.gameObject.tag == "Player")
        //{
        //    GoOut = true;
        //    //Debug.Log("Can go out now!");
        //}
        if (blockList.Contains(col.gameObject))
        {
            blockList.Remove(col.gameObject);
        }
        if (targetSM.GetBool(updateConditionBoolName) && blockList.Count == 0)
        {
            GoOut = true;
            DebugMessage(LogType.Normal, "can go out now.");
        }

    }


}
