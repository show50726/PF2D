//Player Property 2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/03
//Usage:            This is made for player property 2D example. Inherit this to write a property script.

using UnityEngine;
using System.Collections;

public class PlayerProperty2D : UnitProperty
{
    public PF2DController controller2D;
    public Player player;
    public virtual void ApplyAppearanceChange(bool apply)
    {

    }
    public static void EraseAllChange(PropertyManager propertyManager)
    {

    }


    protected override void Start()
    {
        base.Start();
        if (player == null)
        {
            player = GetComponent<Player>();
            if (player == null)
            {
                Debug.LogWarning(GetType().Name + " warning: failed to find player on " + name + ". To avoid bugs, this property won't work.");
                enabled = false;
                return;
            }
        }
        if (controller2D == null)
        {
            controller2D = GetComponent<PF2DController>();
            if (controller2D == null)
            {
                Debug.LogWarning(GetType().Name + " warning: failed to find controller2D on " + name + ". To avoid bugs, this property won't work.");
                enabled = false;
                return;
            }
        }
    }

}
