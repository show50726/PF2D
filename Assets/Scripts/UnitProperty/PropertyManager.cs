//Property Manager  PROUDLY made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/08
//Usage:            This helps managing properties. Make sure you apply it to ANY unit that will be involved by properties.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PropertyManager : MonoBehaviour {

	public int MaxProperty = 1;    
    public List<UnitProperty> propertyList = new List<UnitProperty>();
    public Player player;
    public PF2DController controller2D;

    private void Start()
    {
        if (player == null)
        {
            Debug.Log(GetType().Name + " of " + name + ": looks like player isn't assigned. Script will try to find one.");
            player = GetComponent<Player>();
        }
        if (controller2D == null)
        {
            Debug.Log(GetType().Name + " of " + name + ": looks like controller2D isn't assigned. Script will try to find one.");
            controller2D = GetComponent<PF2DController>();
        }
        UnitProperty[] checkList = GetComponents<UnitProperty>();
        foreach (UnitProperty p in checkList)
        {
            if (propertyList.Contains(p)!=true)
            {
                propertyList.Add(p);
            }
        }
    }
    public T GetProperty<T>() where T : UnitProperty
    {
        foreach (UnitProperty p in propertyList)
        {
            if (p.GetType().Name == typeof(T).Name)
            {
                return p as T;
            }
        }
        return null;
    }
    public UnitProperty GetProperty(System.Type type)
    {
        foreach (UnitProperty p in propertyList)
        {
            if (p.GetType().Name == type.Name)
            {
                return p;
            }
        }
        return null;
    }
    public UnitProperty[] GetPropertyList()
    {
        if (propertyList.Count == 0) return null;
        UnitProperty[] list = new UnitProperty[propertyList.Count];
        for (int i = 0; i < propertyList.Count; i++)
        {
            list[i] = propertyList[i];
        }
        return list;

    }

    public bool ApplyProperty(UnitProperty unitProperty)
    {
        return ApplyProperty(unitProperty, true);
    }
    public bool ApplyProperty(UnitProperty property, bool updateInfoIfAlreadyExists)
    {
        //add property
        System.Type propertyType = property.GetType();
        if (GetProperty(propertyType) != null)
        {
            //Debug.Log(property.GetType().Name + " exists on " + gameObject);
            if (updateInfoIfAlreadyExists)
            {
                Debug.Log(GetType().Name + " of " + name + ": " + propertyType.Name + " already exists. Script will UPDATE the existing property to be the new assigned one.");
            }
            else
            {
                Debug.Log(GetType().Name + " of " + name + ": " + propertyType.Name + " has cancelled updating / re-applying due to script setting.");
                return false;
            }
        }
        else
        {
            UnitProperty p = gameObject.AddComponent(propertyType) as UnitProperty;
			if(propertyList.Count < MaxProperty)
            	propertyList.Add(p); //it's a new property, record it on the list.
			else{
				Debug.Log(name + " has lost the property " + propertyList[0].GetType().Name);
				RemoveProperty (propertyList[0].GetType());
				propertyList.Add (p);
			}
            foreach (UnitProperty pe in propertyList)
            {
                Debug.Log(name + " has property " + pe.GetType().Name);
            }
        }
        //update(copy) the value / setting of original one. 
        var dstProperty = GetProperty(propertyType) as UnitProperty;
        //var dstProperty = GetComponent(propertyType) as UnitProperty;
        if (dstProperty == null)
        {
            Debug.LogWarning("Warning: did not get the final property.");
            return false;
        }
        var fields = propertyType.GetFields();
        foreach (var field in fields)
        {
            if (field.IsStatic) continue;
            if (field.Name == "player" || field.Name == "controller2D") continue;
            //Debug.Log(field.Name + " is setting to " + field.GetValue(property));
            field.SetValue(dstProperty, field.GetValue(property));
        }
        dstProperty.propertyManager = this;
        dstProperty.enabled = true;
        //if everything is ok, return true.
        return true;
    }
    
    public bool RemoveProperty<T>() where T : UnitProperty
    {
        T originalProperty = GetProperty<T>();
        if (originalProperty == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: trying to remove an non-exist / non-recorded property. This might be a bug. The removal has been stopped.");
            return false;
        }
        //start removal.
        propertyList.Remove(originalProperty);
        Destroy(originalProperty);
        //Debug.Log(typeof(T).Name + " of " + name + " has been successfully removed.");
        return true;
    }
    public bool RemoveProperty(System.Type type)
    {
        var originalProperty = GetProperty(type);
        if (originalProperty == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: trying to remove an non-exist / non-recorded property. This might be a bug. The removal has been stopped.");
            return false;
        }
        //start removal.
        propertyList.Remove(originalProperty);
        Destroy(originalProperty);
        //Debug.Log(typeof(T).Name + " of " + name + " has been successfully removed.");
        return true;
    }
    
    public bool ClearProperty()
    {
        while (propertyList.Count != 0)
        {
            RemoveProperty(propertyList[0].GetType());
        }
        return true;
    }
    
}
