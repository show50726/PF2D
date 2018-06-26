//Mechanism2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/26
//usage:            this is base of any 2D mechanism. Inherit this to write a new mechanism that can be controlled by outside.

using UnityEngine;

public class Mechanism2D : MonoBehaviour {
    public bool activated = false;
    public bool stayOnAfterActivated = false;
    public bool Activated
    {
        get
        {
            return activated;
        }
        set
        {
            activated = value;
            WhenActivate(value);
        }
    }
    protected virtual void Start()
    {
        WhenActivate(activated); // to do Activate effect
    }

    protected virtual void WhenActivate(bool isTurnOn)
    {
        if (isTurnOn == false && stayOnAfterActivated == true)
        {
            //trying to turn off, but should be stay on. Reverse it!
            activated = true;
        }
    }


}
