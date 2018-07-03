//Reset_PlayerState made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//usage:            in this script, write special effects (like unable to move) when a player has reset to default state.
//NOTE:             MUST be assigned on an Animator State, not GameObjects.

using UnityEngine;

public class Reset_PlayerState : PlayerState
{
    public bool resetParameters = true;
    [Tooltip("If set to nothing (size 0), parameters will be all reset.")]
    public string[] resetParametersName = new string[0];
    private void Awake()
    {
        if (resetParameters && resetParametersName.Length != 0)
        {
            foreach (string name in resetParametersName)
            {
                if (name == "")
                {
                    Debug.LogWarning(GetType().Name + " of " + name + " warning: not all of " + resetParametersName + " is assigned. To avoid bugs, resetParameters will set to false.");
                    resetParameters = false;
                    break;
                }
            }
        }
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, animatorStateInfo, layerIndex);
        
        //reset parameters
        if (resetParameters)
        {
            AnimatorControllerParameter[] parameterList = GetParameterList(animator);
            if (resetParametersName.Length == 0)
            {
                foreach (AnimatorControllerParameter param in parameterList)
                    ResetAnimatorContorllerParameter(animator, param.name, param.type);
            }
            else
            {
                foreach (string pName in resetParametersName)
                    foreach (AnimatorControllerParameter param in parameterList)
                        if (param.name == pName)
                        {
                            ResetAnimatorContorllerParameter(animator, pName, param.type);
                            break;
                        }
            }
        }

        //reset rigidbody
        if (_rb2d)
        {
            if (_rb2d.bodyType != psInfo.rbType)
            {
                Debug.Log(animator.gameObject.name + " has reset body type to " + psInfo.rbType);
                _rb2d.bodyType = psInfo.rbType;
            }
        }
        

    }

    private AnimatorControllerParameter[] GetParameterList(Animator animator)
    {
        AnimatorControllerParameter[] parameters = new AnimatorControllerParameter[animator.parameterCount];
        for (int i = 0; i < animator.parameterCount; i++)
        {
            parameters[i] = animator.GetParameter(i);
        }
        return parameters;
    }

    private void ResetAnimatorContorllerParameter(Animator animator, string resetParaName, AnimatorControllerParameterType type)
    {
        switch (type)
        {
            case AnimatorControllerParameterType.Float:
                animator.SetFloat(resetParaName, 0);
                break;
            case AnimatorControllerParameterType.Int:
                animator.SetInteger(resetParaName, 0);
                break;
            case AnimatorControllerParameterType.Bool:
                animator.SetBool(resetParaName, false);
                break;
            case AnimatorControllerParameterType.Trigger:
                break;
            default:
                break;
        }

    }

}
