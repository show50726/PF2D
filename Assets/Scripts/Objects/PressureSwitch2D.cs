﻿//PressureSwitch2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/08
//usage:            apply this on "button-like" objects to make pressure switches.
//NOTE:             Mechanism2D needed. Using collider instead of trigger.

using UnityEngine;

public class PressureSwitch2D : MonoBehaviour {

    public LayerMask ignoreTheseObjects = (1 << 4) | (1 << 5);
    public Mechanism2D[] connectedMechanism;

    [Tooltip("When checked, mechanisms will 'switch state' (on<->off) instead of turning on / off.")]
    public bool switchInsteadTurnOn = false;

    [Tooltip("When checked, pressure switch can't turn off once turn on.")]
    public bool willStayOn = false;
    
    public bool LayerIsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (LayerIsInLayerMask(col.gameObject.layer,ignoreTheseObjects) == false)
        {
            ActivateMechanism(true);
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (LayerIsInLayerMask(col.gameObject.layer, ignoreTheseObjects) == false)
        {
            if (willStayOn == false) ActivateMechanism(false);
        }
    }
    
    private void ActivateMechanism(bool isOn)
    {
        foreach (Mechanism2D m in connectedMechanism)
        {
            m.Activated = switchInsteadTurnOn ? !m.Activated : isOn;
        }
    }
    
	
}
