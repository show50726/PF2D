//Finish_PlayerState made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/13
//usage:            in this script, write special effects (like unable to move) when a player has finished.
//NOTE:             MUST be assigned on an Animator State, not GameObjects.

using UnityEngine;

public class Finish_PlayerState : PlayerState
{
    protected PlayerStateInfo psInfo = new PlayerStateInfo();

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        if (_rb2d)
        {
            psInfo.rbType = _rb2d.bodyType;
            _rb2d.bodyType = RigidbodyType2D.Static;
        }
        if (_col2d)
        {
            _col2d.isTrigger = true;
        }
        if (_controllerPF2d)
        {
            _controllerPF2d.FreezeControl();
        }
        if (PFManager.exist)
        {
            PFManager.exist.APlayerIsFinished(_obj);
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if (_rb2d)
        {
            _rb2d.bodyType = psInfo.rbType;
        }
        if (_col2d)
        {
            _col2d.isTrigger = false;
        }
        if (_controllerPF2d)
        {
            _controllerPF2d.UnFreezeControl();
        }
    }


}
public class PlayerStateInfo
{
    public RigidbodyType2D rbType;

}
