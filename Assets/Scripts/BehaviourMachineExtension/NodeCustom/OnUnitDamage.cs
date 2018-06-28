//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//OnUnitDamage made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/06/28
//NOTICE:           Must be used with (CMSR) SUnitStater.
using UnityEngine;
using System.Collections;
using BehaviourMachine;
using CMSR;

namespace BehaviourMachine
{
    [NodeInfo(category = "Function/CMSR/",
        icon = "Function",
        description = "Tick when the SUnitStater has been damaged.")]
    public class OnUnitDamage : FunctionNode
    {

        [VariableInfo(requiredField = false, nullLabel = "Don't Store", canBeConstant = false, tooltip = "Stores the amount of damage")]
        public FloatVar damage; // Stores the amount of damage.

        public override void Reset()
        {
            base.Reset();
            damage = new ConcreteFloatVar();
        }

        // Called when the BehaviourTree is enabled
        public override void OnEnable()
        {
            if (enabled)
            {
                SUnitStater _stater = self.GetComponent<SUnitStater>();
                if (_stater == null)
                {
                    Debug.LogError(self.name + "/" + GetType().Name + ": cannot find SUnitStater, this function node will not work.");
                    enabled = false;
                    return;
                }
                _stater.OnUnitDamageEvent += OnDamageCallback;
                // Internal property used to check if the function node registered its callback
                m_Registered = true;
            }
        }

        // Called when the BehaviourTree is disabled
        public override void OnDisable()
        {
            SUnitStater _stater = self.GetComponent<SUnitStater>();
            if (_stater != null)
            {
                _stater.OnUnitDamageEvent -= OnDamageCallback;
            }
            // Internal property used to check if the function node registered its callback
            m_Registered = false;
        }

        // Callback registered to get the SUnitStater's OnUnitDamageEvent
        protected void OnDamageCallback(float damageParameter)
        {
            // Update the damage value
            damage.Value = damageParameter;
            // Tick children
            OnTick();
        }
    }
}
