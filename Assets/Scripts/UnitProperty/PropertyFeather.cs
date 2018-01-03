//PropertyFeather made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2017/11/27
//Usage:            This is a specified property, which makes player run faster and jump higher.

using UnityEngine;
using System.Collections;

public class PropertyFeather : PlayerProperty2D
{
    public float moveSpeedMultiplier = 1.4f;
    public float jumpHeightMulitplier = 1.571428f;

    protected override void Start()
    {
        base.Start();
        ActivateEffect(true);
    }

    private void ActivateEffect(bool activate)
    {
        if (moveSpeedMultiplier == 0 || jumpHeightMulitplier == 0)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: one of multiplier has set to 0. For bug avoiding, this property won't work.");
            enabled = false;
            return;
        }
        if (controller2D == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: no controller assigned. For bug avoiding, this property won't work.");
            enabled = false;
            return;
        }

        if (activate)
        {
            controller2D.movingSpeed *= moveSpeedMultiplier;
            controller2D.jumpHeight *= jumpHeightMulitplier;
        }
        else
        {
            controller2D.movingSpeed /= moveSpeedMultiplier;
            controller2D.jumpHeight /= jumpHeightMulitplier;
        }
    }
    //尚未實裝：在水中會持續向上浮起

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (enabled == false) return;
        ActivateEffect(false);
    }

}
