//PlayerState       made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/19
//usage:            This is a BASE class, inherit this to make custom one.
//NOTE:             MUST be assigned on an Animator State, not GameObjects.

using UnityEngine;

public class PlayerState : StateMachineBehaviour {

    protected GameObject _obj;
    protected Rigidbody2D _rb2d;
    protected Collider2D _col2d;
    protected PF2DController _controllerPF2d;
    public static PlayerStateInfo psInfo = new PlayerStateInfo();


    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        _obj = animator.gameObject;
        _rb2d = _obj.GetComponent<Rigidbody2D>();
        _col2d = _obj.GetComponent<Collider2D>();
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
public class PlayerStateInfo
{
    public RigidbodyType2D rbType = new RigidbodyType2D();

}
