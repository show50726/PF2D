//UIPropertyDisplayer made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/30
//usage:            works with PropertyManager. It shows the property name (and futhermore information if you want).

using UnityEngine;
using UnityEngine.UI;

public class UIPropertyDisplayer : MonoBehaviour
{
    public Text textDisplayer;
    public string formerDisplay = "Property: ";

    private void OnEnable()
    {
        if (textDisplayer == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name +" warning: the textDisplayer isn't assigned. The script won't work.");
            enabled = false;
            return;
        }
    }

    public void DisplayProperty(PropertyManager manager)
    {
        Debug.Log("yooooooo");
        UnitProperty[] list = manager.GetPropertyList();
        string propertyInfo = "";
        if (list == null) propertyInfo = "None";
        else if (list.Length > 1)
        {
            Debug.Log("length is " + list.Length);
            for (int i = 0; i < list.Length; i++)
            {
                propertyInfo += list[i].GetType().Name;
                Debug.Log("getting " + list[i].GetType().Name);
                if (i < list.Length - 1)
                {
                    propertyInfo += ", ";
                }
            }
        }
        else propertyInfo = list[0].GetType().Name;
        textDisplayer.text = formerDisplay + propertyInfo;
        Debug.Log("Property info of player " + manager.gameObject.name + " is " + propertyInfo);
    }

}
