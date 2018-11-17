//Property Giver 2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/11/18
//Usage:            Use this to give property on touch. Require collider or trigger.


using UnityEngine;
using System.Collections;

public class PropertyGiver2D : STCMonoBehaviour
{
    public LayerMask ignoreTheseObjects = (1 << 8) | (1 << 9); //this format means the Layer 8. 9 are selected.

    public UnitProperty giveProperty;
    public bool updateInfoIfPropertyExists = true;
    public bool removeWhenLeave = false;
    //public bool giveAgainWhenStay = false;
    //[Tooltip("unit: seconds.")]
    //public float giveAgainAfterTime = 1;

    protected virtual void Start()
    {
        if (giveProperty == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: the giving property is missing, thus unable to work.");
            enabled = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
        GivePropertyTo(col.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
        GivePropertyTo(col.gameObject);
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
        if (removeWhenLeave)
        {
            RemovePropertyFrom(col.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (!enabled) return;
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
        if (removeWhenLeave)
        {
            RemovePropertyFrom(col.gameObject);
        }
    }

    private void GivePropertyTo(GameObject obj)
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: cannot find property manager of " + obj.name + ", which might be a bug. To make system keep running, script will add one.");
            objPropertyManager = obj.AddComponent<PropertyManager>();
        }
        if (giveProperty != null)
        {
            objPropertyManager.ApplyProperty(giveProperty, updateInfoIfPropertyExists);
        }
        else
        {
            //it might want to reset.
            objPropertyManager.CleanUp();
        }

    }
    private void RemovePropertyFrom(GameObject obj)
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: cannot find property manager of " + obj.name + ", which might be a bug and cause the property removal failed.");
            return;
        }
        if (giveProperty != null) objPropertyManager.RemoveProperty(giveProperty.GetType());
        else
        {
            DebugMessage(LogType.Error, "trying to remove null property. This is illegal. If you want to \"reload\" the previous property, you'll need to re-write the code.");
        }
    }


}
