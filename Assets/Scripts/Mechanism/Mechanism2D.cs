//Mechanism2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/08/01
//usage:            this is base of any 2D mechanism. Inherit this to write a new mechanism that can be controlled by outside.

using UnityEngine;

public class Mechanism2D : STCMonoBehaviour {
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
        //WhenActivate(activated); // to do Activate effect
        Activated = activated; // to do Activate effect
    }

    /// <summary>
    /// Called when Activated value is changed(set). Override it to make custom action.
    /// </summary>
    /// <param name="isTurnOn">Determine the situation.</param>
    protected virtual void WhenActivate(bool isTurnOn)
    {
        DebugMessage(LogType.Normal, (isTurnOn ? "activated." : "deactivated."));
        if (isTurnOn == false && stayOnAfterActivated == true)
        {
            //trying to turn off, but should be stay on. Reverse it!
            activated = true;
        }
    }


}
