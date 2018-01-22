//GameSystemStateMachineState made by STC
//Contact:          stc.ntu@gmail.com
//Last maintained:  2018/01/22
//Usage:            This is a basic S.M.S.Behaviour, you can use it or inherit it.

using UnityEngine;

public class GameSystemStateMachineState : StateMachineBehaviour {

    [Tooltip("If assigned & found, the script will \"Reset\" it.")]
    public string parameterEnterName = "Go";
    [Tooltip("If left empty, the script won't load any scene.")]
    public string loadSceneName = "Title";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        #region Security Check
        if (GameSystemManager.exist == null)
        {
            Debug.LogError(GetType().Name + " of animator on " + animator.gameObject.name + " warning: trying to access a GameSystemStateMachineState without GameSystemManager. This might cause bugs.");
        }
        #endregion
        AnimatorControllerParameter cp = null;
        if (parameterEnterName != "") cp = GetParameter(animator, parameterEnterName);
        if (cp != null) ResetParameter(animator, cp);

        //loading the scene of state.
        if (loadSceneName != "")
        {
            GameSystemManager.exist.LoadScene(loadSceneName);
            Debug.Log("Loading Scene " + loadSceneName + " by animator");

        }
    }
    public static AnimatorControllerParameter GetParameter(Animator animator, string name)
    {
        foreach (AnimatorControllerParameter cp in animator.parameters)
            if (cp.name == name)
                return cp;
        return null;
    }
    public static void ResetParameter(Animator animator, AnimatorControllerParameter parameter)
    {
        switch (parameter.type)
        {
            case AnimatorControllerParameterType.Float:
                animator.SetFloat(parameter.name, parameter.defaultFloat);
                break;
            case AnimatorControllerParameterType.Int:
                animator.SetInteger(parameter.name, parameter.defaultInt);
                break;
            case AnimatorControllerParameterType.Bool:
                animator.SetBool(parameter.name, parameter.defaultBool);
                break;
            case AnimatorControllerParameterType.Trigger:
                animator.SetTrigger(parameter.name);
                break;
            default:
                break;
        }
    }

}
