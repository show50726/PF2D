//Finish_PlayerState made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//usage:            in this script, write special effects (like unable to move) when a player has finished.
//NOTE:             MUST be assigned on an Animator State, not GameObjects.

using UnityEngine;

public class Finish_PlayerState : PlayerState
{
    /// <summary>
    /// This is helped to prevent player into ground when comes out the door.
    /// </summary>
    [Tooltip("This is helped to prevent player into ground when comes out the door.")]
    public Vector3 fixPos = new Vector3(0, 4, 0);

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        if (_rb2d)
        {
            psInfo.rbType = _rb2d.bodyType;
            _rb2d.bodyType = RigidbodyType2D.Kinematic;
            //_rb2d.simulated = false;
        }
        if (_col2d)
        {
            _col2d.isTrigger = true;
        }
        if (_controllerPF2d)
        {
            _controllerPF2d.Freezed = true;
        }
        if (_spr)
        {
            _spr.color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, 0);
            int c = _obj.transform.childCount;
            for (int i = 0; i < c; i++) {
                Transform child = _obj.transform.GetChild(i);
                SpriteRenderer sp = child.GetComponent<SpriteRenderer>();
                if (sp)
                {
                    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 0);
                    //Debug.Log(sp.color.r);
                }
                else if (child.childCount > 0)
                {
                    int cc = child.childCount;
                    for (int j = 0; j < cc; j++)
                    {
                        Transform childc = child.transform.GetChild(j);
                        SpriteRenderer childsp = childc.GetComponent<SpriteRenderer>();
                        if (childsp)
                            childsp.color = new Color(childsp.color.r, childsp.color.g, childsp.color.b, 0);
                    }
                }
            }
        
        }
        if (PFManager.exist)
        {
            PFManager.exist.APlayerIsFinished(_obj);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _obj.transform.position += fixPos;
        if (_rb2d)
        {
            _rb2d.bodyType = psInfo.rbType;
            _rb2d.simulated = true;
            _rb2d.velocity = Vector2.zero;
        }
        if (_col2d)
        {
            _col2d.isTrigger = false;
        }
        if (_controllerPF2d)
        {
            _controllerPF2d.Freezed = false;
        }
        if (_spr)
        {
            _spr.color = new Color(_spr.color.r, _spr.color.g, _spr.color.b, 255);
            int c = _obj.transform.childCount;
            for (int i = 0; i < c; i++)
            {
                Transform child = _obj.transform.GetChild(i);
                SpriteRenderer sp = child.GetComponent<SpriteRenderer>();
                if (sp)
                    sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 255);
                else if (child.childCount > 0)
                {
                    int cc = child.childCount;
                    for (int j = 0; j < cc; j++)
                    {
                        Transform childc = child.transform.GetChild(j);
                        SpriteRenderer childsp = childc.GetComponent<SpriteRenderer>();
                        if (childsp)
                            childsp.color = new Color(childsp.color.r, childsp.color.g, childsp.color.b, 255);
                    }
                }
            }
            
        }
    }


}

