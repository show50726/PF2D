//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------

using UnityEngine;
using CMSR;

namespace BehaviourMachine
{
    /// <summary>
    /// Add HP of Unit Stater.
    /// </summary>

    //for icons:
    //ActionState, Axis, BehaviourMachine_black, BehaviourTree, Blackboard, Button,
    //Constant, Empty, Function, GlobalBlackboard, Keyboard, Mouse, None,
    //Parallel, ParallelSelector, ParallelSequence, PriortySelector, PriortySequence,
    //RandomChild, RandomSelector, RandomSequence, Reflection, Reflection_10,
    //Selector, Sequence, StateMachine, Switch, {b}LogoDark, {b}LogoLight
    //or any other icon name by string
    //Recommend Size: greater than 16*16
    [NodeInfo(category = "Action/CMSR/",
        icon = "ActionState",
        description = "Add HP of Unit Stater.")]
    public class StaterHPAdd : ActionNode
    {

        public FloatVar addHP;
        /// <summary>
        /// If true, the amount will be done in a second; otherwise HP will add instaneously.
        /// </summary>
        [Tooltip("If true, the amount will be done in a second; otherwise HP will add instaneously.")]
        public bool perSecond = false;

        private SUnitStater stater;
        // Called once when the node is created
        public override void Awake() { }

        // Called when the owner (BehaviourTree or ActionState) is enabled
        public override void OnEnable() { }

        // Called when the node starts its execution
        public override void Start() {
            stater = self.GetComponent<SUnitStater>();
        }

        // This function is called when the node is in execution
        public override Status Update()
        {
            // Do stuff
            if (stater == null)
            {
                return Status.Error;
            }

            stater.Heal(addHP.Value);

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
            //such as XXVar = new ConcreteXXVar();
        }

        // Called when the script is loaded or a value is changed in the inspector (Called in the editor only)
        public override void OnValidate() { }
    }
}