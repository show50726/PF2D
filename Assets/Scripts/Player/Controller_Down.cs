//Controller_Down   made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/12
//usage:            a controller to make player down.

using UnityEngine;

public class Controller_Down : MonoBehaviour {

    public KeyCode down = KeyCode.S;
    public Animator targetSM;
    public string conditionBoolName = "HoldingDown";

    private bool checkedAnimatorAvailable = false;

    private void Start()
    {
        if (targetSM != null)
        {
            foreach (AnimatorControllerParameter p in targetSM.parameters)
            {
                if (p.name == conditionBoolName) checkedAnimatorAvailable = true;
            }
        }
    }
    private void Update()
    {
        if (Input.GetKey(down))
        {
            if (checkedAnimatorAvailable) targetSM.SetBool(conditionBoolName, true);
        }
        else
        {
            if (checkedAnimatorAvailable) targetSM.SetBool(conditionBoolName, false);
        }

    }

}
