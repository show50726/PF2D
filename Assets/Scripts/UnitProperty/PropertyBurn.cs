//PropertyBurn made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/08
//Usage:            This is a specified property, which makes player burn.

using UnityEngine;

public class PropertyBurn : PropertyNegative
{
    [Tooltip("unit: melting factor(see PropertyFrozen). Set 0 to melt ice immediately.")]
    public float meltingIcePeriod = 3f;
    [Tooltip("Objects in these layers won't be diffused.")]
    public LayerMask ignoreTheseObjects = (1 << 8); //this format means the Layer 8 are selected.
    protected override void Start()
    {
        if (meltingIcePeriod < 0)
        {
            Debug.LogError(GetType().Name + " of " + name + " error: meltingIcePeriod is not allowed to set under 0. The script will not work if continues.");
            enabled = false;
        }
    }

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
        if (p) p.Melt(meltingIcePeriod);

    }

}
