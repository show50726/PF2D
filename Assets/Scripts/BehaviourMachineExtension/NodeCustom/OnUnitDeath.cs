//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//OnUnitDeath made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/01
//NOTICE:           Must be used with (CMSR) SUnitStater.
using UnityEngine;
using CMSR;

namespace BehaviourMachine
{
    [NodeInfo(category = "Function/CMSR/",
        icon = "Function",
        description = "Tick when the SUnitStater has died.")]
    public class OnUnitDeath : FunctionNode
    {
        
        public override void Reset()
        {
            base.Reset();
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
                _stater.OnUnitDeathEvent += OnDeathCallback;
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
                _stater.OnUnitDeathEvent -= OnDeathCallback;
            }
            // Internal property used to check if the function node registered its callback
            m_Registered = false;
        }

        // Callback registered to get the SUnitStater's OnUnitDamageEvent
        protected void OnDeathCallback()
        {
            // Tick children
            OnTick();
        }
    }
}
