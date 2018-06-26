//Living Property 2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/06/27
//Usage:            This is made for living property 2D example. Inherit this to write a property script.

using UnityEngine;
using CMSR;

public class LivingProperty2D : UnitProperty
{
    public SLivingStater stater;
    protected override void Start()
    {
        base.Start();
        if (stater == null)
        {
            stater = GetComponent<SLivingStater>();
            if (stater == null)
            {
                Debug.LogWarning(GetType().Name + " warning: failed to find stater on " + name + ". To avoid bugs, this property won't work.");
                enabled = false;
                return;
            }
        }
    }

}
