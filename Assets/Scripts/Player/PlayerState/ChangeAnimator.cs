//ChangeAnimator    made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/08/16
//usage:            Use this in an animator to change animator.
using UnityEngine;

public class ChangeAnimator : StateMachineBehaviour
{

    protected GameObject _obj;
    protected Rigidbody2D _rb2d;
    protected Collider2D _col2d;
    protected PF2DController _controllerPF2d;
    protected SpriteRenderer _spr;
    public static PlayerStateInfo psInfo = new PlayerStateInfo();


    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _obj = animator.gameObject;
        _rb2d = _obj.GetComponent<Rigidbody2D>();
        _col2d = _obj.GetComponent<Collider2D>();
        _spr = _obj.GetComponent<SpriteRenderer>();
        _controllerPF2d = _obj.GetComponent<PF2DController>();
        if (_col2d == null) Debug.LogWarning(WarningOfMissingTypes(_col2d.GetType().Name));
        if (_rb2d == null) Debug.LogWarning(WarningOfMissingTypes(_rb2d.GetType().Name));
        if (_controllerPF2d == null) Debug.LogWarning(WarningOfMissingTypes(_controllerPF2d.GetType().Name));
    }
    public string WarningOfMissingTypes(string typeName)
    {
        return GetType().Name + " of " + _obj.name + "'s Animator warning: didn't find its " + typeName + ". This might cause some functions unabled.";
    }

}