//Property Frosting made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2017/12/13
//Usage:            This is a specified property, which makes player "cool" and freeze other objects.

using UnityEngine;
using System.Collections;

public class PropertyFrosting : PropertyNegative
{
    public PropertyFrosting()
    {
        damagePeriod = 3f;
    }
    [Header("Freezing setting.")]
    [Tooltip("If not set, will apply as default.")]
    public PropertyFrozen freezePropertySample;
    public bool updateIfExists = true;
    [Tooltip("Objects in these layers won't be diffused.")]
    public LayerMask ignoreTheseObjects = (1 << 8); //this format means the Layer 8 are selected.



    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
        //notice that PropertyBurn has scripted to destroy each other on touch already.
        if (GetProperty<PropertyWooden>(col.gameObject) != null)
        {
            if (freezePropertySample) GivePropertyTo(col.gameObject, freezePropertySample, updateIfExists);
            else GivePropertyTo(col.gameObject, new PropertyFrozen(), updateIfExists);
        }

    }

}
