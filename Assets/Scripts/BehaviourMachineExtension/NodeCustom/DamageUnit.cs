//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//DamageUnit made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/02
//NOTICE:           Must be used with (CMSR) SUnitStater.
using UnityEngine;
using CMSR;

namespace BehaviourMachine
{
    /// <summary>
    /// Damage Specified unit. Need SUnitStater applied.
    /// </summary>

    [NodeInfo(category = "Action/CMSR/",
        icon = "Blackboard",
        description = "Damage Specified unit. Need SUnitStater applied.")]
    public class DamageUnit : ActionNode
    {
        /// <summary>
        /// The object that will be damaged.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "Use Self", tooltip = "The object that will be damaged. SUnitStater is NEEDED.")]
        public GameObjectVar unitToDamage;

        /// <summary>
        /// The amount of damage.
        /// </summary>
        [VariableInfo(requiredField = true, tooltip = "The amount of damage.")]
        public FloatVar damageAmount;

        private SUnitStater target;

        /// <summary>
        /// If checked, damage will be multiplied with time passed.
        /// </summary>
        [VariableInfo(requiredField = false, nullLabel = "false", tooltip = "If checked, damage will be multiplied with time passed.")]
        public BoolVar isPerSecond;
        

        // Called when the node starts its execution
        public override void Start() {
            if (unitToDamage.isNone)
                target = self.GetComponent<SUnitStater>();
            else
                target = unitToDamage.Value.GetComponent<SUnitStater>();
            if (target == null)
            {
                Debug.LogError(self.name + "/{b}/" + tree.name + "/" + name + ": cannot find SUnitStater.");
                return;
            }
        }

        // This function is called when the node is in execution
        public override Status Update()
        {
            // Do stuff
            if (target == null)
            {
                return Status.Error;
            }

            if (isPerSecond.isNone || isPerSecond.Value == false)
            {
                target.Damage(damageAmount);
            }
            else
            {
                //damage per second.
                target.Damage(damageAmount * Time.deltaTime);
            }

            // Never forget to set the node status
            return Status.Success;
            //Ready: 尚未被執行。
            //Success: 動作執行成功。
            //Failure: 動作執行失敗。
            //Error: 執行錯誤。
            //Running: 動作還需要更多Frame執行，像是等待時間的就是利用這個狀態檢查倒數，所以才一定要放在Update()底下
        }
        

        // This function is called to reset the default values of the node
        public override void Reset() {
            unitToDamage = new ConcreteGameObjectVar();
            damageAmount = new ConcreteFloatVar();
        }
        
    }
}