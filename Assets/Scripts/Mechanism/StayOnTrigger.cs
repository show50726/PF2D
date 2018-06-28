//StayOnTrigger made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/25
//usage:            This can be a condition for other mechanisms. For example, if you want to make a union-pair-stay-on mechanism (activate forever ONLY when both of them are triggered), you can apply this on both of them, and set the switchToStayOn to each others.
using UnityEngine;
using System.Collections;

public class StayOnTrigger : Mechanism2D
{
    internal class MechanismAndBoolSet
    {
        public Mechanism2D mechanism;
        public bool originStayOnSet = false;
    }
    public Mechanism2D[] switchToStayOn = new Mechanism2D[0];
    private MechanismAndBoolSet[] sets;
    protected override void Start()
    {
        #region Security Check
        if (switchToStayOn.Length == 0)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: didn't assign any of switchToStayOn. The component will be useless.");
        }
        for (int i = 0; i < switchToStayOn.Length; i++)
        {
            if (switchToStayOn[i] == null)
            {
                Debug.LogError(GetType().Name + " of " + name + " error: didn't assign the right amount of " + switchToStayOn + "! Component will turn off in case of bugs.");
                enabled = false;
                return;
            }
            if (switchToStayOn[i] == this)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: switchToStayOn has assigned the component itself. You should not do this. In case of bugs, the component won't work.");
                enabled = false;
                return;
            }
        }
        #endregion
        sets = new MechanismAndBoolSet[switchToStayOn.Length];
        for (int i = 0; i < sets.Length; i++)
        {
            sets[i] = new MechanismAndBoolSet {
                mechanism = switchToStayOn[i],
                originStayOnSet = switchToStayOn[i].stayOnAfterActivated };
            if (sets[i].originStayOnSet == true)
            {
                Debug.Log(GetType().Name + " of " + name + ": you assign a mechanism to stay activated when triggered, while that is already set to stay activated. Thus, nothing will change.");
            }
        }
        base.Start();
    }
    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        if (isTurnOn)
        {
            foreach (MechanismAndBoolSet m in sets)
            {
                m.mechanism.stayOnAfterActivated = true;
            }
        }
        else
        {
            foreach (MechanismAndBoolSet m in sets)
            {
                m.mechanism.stayOnAfterActivated = m.originStayOnSet;
            }
        }

    }
    
}
