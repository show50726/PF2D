//Property Giver 2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/11/22
//Usage:            Use this to remove property on touch. Require collider or trigger.
using UnityEngine;
using System.Collections;

public class PropertyRemover2D : MonoBehaviour
{
    [Tooltip("If keep it nothing, it will remove all the property (purify).")]
    public UnitProperty[] removeProperty = new UnitProperty[0];
    private void OnTriggerEnter2D(Collider2D col)
    {
        DoAction(col.gameObject);
    }
    private void OnCollisionEnter(Collision col)
    {
        DoAction(col.gameObject);
    }
    private void DoAction(GameObject obj)
    {
        if (removeProperty.Length != 0)
        {
            RemovePropertyFrom(obj, removeProperty);
        }
        else
        {
            Debug.Log("remove everything!");
			if (obj.gameObject.tag == "Player") {
				Player p = obj.gameObject.GetComponent<Player> ();
				p.Circle.GetComponent<SpriteRenderer> ().color = Color.white;
			}
            ClearPropertyFrom(obj);
        }
    }
    protected bool RemovePropertyFrom(GameObject obj, UnitProperty[] removeProperty)
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            return false;
        }
        foreach (UnitProperty p in removeProperty)
        {
            objPropertyManager.RemoveProperty(p.GetType());
        }
        return true;
    }
    protected bool ClearPropertyFrom(GameObject obj)
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            return false;
        }
        return objPropertyManager.ClearProperty();
    }
}
