//Unit Property     made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/06/27
//Usage:            This is made for property example. Inherit this to write a property script.

using UnityEngine;
using System.Collections;

public class UnitProperty : MonoBehaviour
{
    //public string test = "hi";
    public bool debugMessage = false;

    #region String Saving Area

    protected string CanNotFindPropertyManagerSoAddOne(GameObject obj)
    {
        return GetType().Name + " of " + name + " warning: cannot find property manager of " + obj.name + ", which might be a bug. To make system keep running, script will add one.";
    }
    protected string CanNotFindPropertyManagerSoRemovalFailed(GameObject obj)
    {
        return GetType().Name + " of " + name + " warning: cannot find property manager of " + obj.name + ", which might be a bug and cause the property removal failed.";
    }
    protected string CanNotFindPropertyManagerSoCheckFailed(GameObject obj)
    {
        return GetType().Name + " of " + name + " warning: cannot find property manager of " + obj.name + ", which might be a bug and cause the property check failed.";
    }

    #endregion
    [ReadOnly]
    public PropertyManager propertyManager;
    protected virtual void Start()
    {
        if (propertyManager == null)
        {
            propertyManager = GetComponent<PropertyManager>();
            if (propertyManager == null)
            {
                Debug.Log(GetType().Name + " warning: the object " + name + " is missing PropertyManager. For safe property operation, script is creating one.");
                propertyManager = gameObject.AddComponent<PropertyManager>();
            }
        }
    }

    protected virtual void OnDestroy()
    {
        //don't do anything if it can't work normally.
        if (enabled == false) return;
        
    }
    protected bool GivePropertyTo(GameObject obj, UnitProperty giveProperty, bool updateInfoIfPropertyExists)
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            Debug.Log(CanNotFindPropertyManagerSoAddOne(obj));
            objPropertyManager = obj.AddComponent<PropertyManager>();
        }
        return objPropertyManager.ApplyProperty(giveProperty, updateInfoIfPropertyExists);

    }
    protected bool RemovePropertyFrom<T>(GameObject obj) where T: UnitProperty
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            Debug.LogWarning(CanNotFindPropertyManagerSoRemovalFailed(obj));
            return false;
        }
        return objPropertyManager.RemoveProperty<T>();
    }
    protected T GetProperty<T>(GameObject obj) where T: UnitProperty
    {
        PropertyManager objPropertyManager = obj.GetComponent<PropertyManager>();
        if (objPropertyManager == null)
        {
            Debug.LogWarning(CanNotFindPropertyManagerSoCheckFailed(obj));
            return null;
        }
        return objPropertyManager.GetProperty<T>();
    }


}
