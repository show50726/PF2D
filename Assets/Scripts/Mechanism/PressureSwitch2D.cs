//PressureSwitch2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/19
//usage:            apply this on "button-like" objects to make pressure switches.
//NOTE:             Mechanism2D needed. Using collider instead of trigger.

using UnityEngine;

public class PressureSwitch2D : Mechanism2D {

    public LayerMask ignoreTheseObjects = (1 << 4) | (1 << 5);
    public Mechanism2D[] connectedMechanism;

    [Tooltip("When checked, mechanisms will 'switch state' (on<->off) instead of turning on / off.")]
    public bool switchInsteadTurnOn = false;

    public bool turnOffInsteadTurnOn = false;

    private System.Collections.Generic.List<GameObject> pressingObject
        = new System.Collections.Generic.List<GameObject>();

    protected override void Start()
    {
        #region Security Check.
        for (int i = 0; i < connectedMechanism.Length; i++)
        {
            if (connectedMechanism[i] == null)
            {
                Debug.LogError(GetType().Name + " of " + name + " error: you didn't assign some objects in connectedMechanism! To avoid bugs, the component won't work.");
                enabled = false;
                return;
            }
            if (connectedMechanism[i] == this)
            {
                Debug.LogError(GetType().Name + " of " + name + " error: connectedMechanism has assigned the component itself. You should not do this. In case of bugs, the component won't work.");
                enabled = false;
                return;
            }
        }
        #endregion
    }
    public bool LayerIsInLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!enabled) return;
        if (LayerIsInLayerMask(col.gameObject.layer,ignoreTheseObjects) == false)
        {
            pressingObject.Add(col.gameObject);
            Activated = !turnOffInsteadTurnOn;
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (!enabled || stayOnAfterActivated == true) return;
        if (LayerIsInLayerMask(col.gameObject.layer, ignoreTheseObjects) == false)
        {
            pressingObject.Remove(col.gameObject);
            if (pressingObject.Count == 0)
            {
                Activated = turnOffInsteadTurnOn;
            }
        }
    }
    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        ActivateMechanism(isTurnOn);
    }

    private void ActivateMechanism(bool isOn)
    {
        foreach (Mechanism2D m in connectedMechanism)
        {
            m.Activated = switchInsteadTurnOn ? !m.Activated : isOn;
        }
    }
    
	
}
