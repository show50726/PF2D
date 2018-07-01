//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//DamageUnit made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/01
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

        // Called once when the node is created
        public override void Awake() { }

        // Called when the owner (BehaviourTree or ActionState) is enabled
        public override void OnEnable() { }

        // Called when the node starts its execution
        public override void Start() {
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

        // Called when the node ends its execution
        public override void End() { }

        // Called when the owner (BehaviourTree or ActionState) is disabled
        public override void OnDisable() { }

        // This function is called to reset the default values of the node
        public override void Reset() {
            unitToDamage = new ConcreteGameObjectVar();
            damageAmount = new ConcreteFloatVar();
        }

        // Called when the script is loaded or a value is changed in the inspector (Called in the editor only)
        public override void OnValidate() { }
    }
}