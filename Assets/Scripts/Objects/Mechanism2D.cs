//Mechanism2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/08
//usage:            this is base of any 2D mechanism. Inherit this to write a new mechanism that can be controlled by outside.

using UnityEngine;

public class Mechanism2D : MonoBehaviour {
    [ReadOnly]
    public bool activated = false;
    public bool Activated
    {
        get
        {
            return activated;
        }
        set
        {
            WhenActivate(value);
            activated = value;
        }
    }
    protected virtual void WhenActivate(bool isTurnOn)
    {
        //don't assign "activated" value here, as accessor has done this (after this function).
    }


}
