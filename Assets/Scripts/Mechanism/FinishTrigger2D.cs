//FinishTrigger2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/13
//usage:            this inherits from Mechanism2D, thus a pressure switch / other Mechanism2D-controller can trigger it.
//                  assign this to something (recommended the related pressure switch), and set control of Finish2D's switchCase.
//NOTE:             2D only. Works with Finish2D.

using UnityEngine;

public class FinishTrigger2D : Mechanism2D
{
    [Header("Finish Setting")]
    public Finish2D finish2d;
    [Tooltip("Look up to Finish2D.")]
    public int switchCaseIndex = 0;
    protected override void Start()
    {
        base.Start();
        if (finish2d == null)
        {
            Debug.LogError(GetType().Name + " of " + name + " warning: didn't assign finish2d. The script won't work.");
            enabled = false;
            return;
        }
        if (switchCaseIndex >= finish2d.switchCase.Length)
        {
            Debug.LogError(GetType().Name + " of " + name + " warning: switchCaseIndex is bigger than one the finish2d has! The script won't work.");
            enabled = false;
            return;
        }
    }

    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        finish2d.switchCase[switchCaseIndex] = isTurnOn;
    }


}
