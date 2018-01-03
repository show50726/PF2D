//PropertyBurn made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2017/12/17
//Usage:            This is a specified property, which makes player burn.

using UnityEngine;
using System.Collections.Generic;

public class PropertyBurn : PropertyNegative
{
    [Tooltip("unit: seconds. Set 0 to melt ice immediately.")]
    public float meltIceAfterTime = 3f;
    [Tooltip("Objects in these layers won't be diffused.")]
    public LayerMask ignoreTheseObjects = (1 << 8); //this format means the Layer 8 are selected.

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer) ) ) return;
        GameObject objTouched = col.gameObject;
        //remove frozen and itself at the same time.
        if (RemovePropertyFrom<PropertyFrosting>(objTouched))
        {
            propertyManager.RemoveProperty(this.GetType());
        }
        if (GetProperty<PropertyWooden>(objTouched) != null)
        {
            Destroy(objTouched);
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
        PropertyFrozen p = col.gameObject.GetComponent<PropertyFrozen>();
        if (p)
        {
            p.meltingFactor += Time.deltaTime;
            if (p.meltingFactor > meltIceAfterTime)
            {
                p.Melt();
            }
        }

    }

}
