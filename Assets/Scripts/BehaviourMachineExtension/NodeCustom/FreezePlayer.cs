//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//NodeCustomExample made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/07/02


using UnityEngine;

namespace BehaviourMachine
{
    /// <summary>
    /// Freezes Player Control. PF2DController is NEEDED.
    /// </summary>
    [NodeInfo(category = "Action/Custom/",
        icon = "Keyboard",
        description = "Freezes Player Control. PF2DController is NEEDED.")]
    public class FreezePlayer : ActionNode
    {

        [VariableInfo(requiredField = false, nullLabel = "use self", tooltip = "The player GO to freeze.")]
        public GameObjectVar playerObj;

        [VariableInfo(requiredField = false, nullLabel = "toggle", tooltip = "When true, player will not be able to move.")]
        public BoolVar setFreezed;

        [VariableInfo(requiredField = false, nullLabel = "forever", tooltip = "Freeze time. Notice 1: will IGNORE setFreezed (always \"freezed\" a finite time). Notice 2: several intend to freeze finite time to same controller might cause controller to unfreeze at wrong time.")]
        public FloatVar freezeTime;

        private PF2DController controller;

        // This function is called to reset the default values of the node
        public override void Reset()
        {
            playerObj = new ConcreteGameObjectVar();
            setFreezed = new ConcreteBoolVar();
            freezeTime = new ConcreteFloatVar();
        }

        // Called when the node starts its execution
        public override void Start() {
            GameObject targetObj = playerObj.isNone ? self : playerObj.Value;
            controller = targetObj.GetComponent<PF2DController>();
            if (controller == null)
            {
                Debug.LogError(self.name + "/{b}/" + tree.name + "/" + name + " error: cannot find controller on " + targetObj.name + ".");
                return;
            }

        }

        // This function is called when the node is in execution
        public override Status Update()
        {
            // Do stuff
            if (controller == null)
            {
                return Status.Error;
            }
            if (setFreezed.isNone)
            {
                //toggle
                controller.Freezed = !controller.Freezed;
            }
            else
            {
                controller.Freezed = setFreezed.Value;
            }
            if (!freezeTime.isNone)
            {
                controller.FreezeControl(freezeTime.Value);
            }
            

            // Never forget to set the node status
            return Status.Success;
            //Ready: 尚未被執行。
            //Success: 動作執行成功。
            //Failure: 動作執行失敗。
            //Error: 執行錯誤。
            //Running: 動作還需要更多Frame執行，像是等待時間的就是利用這個狀態檢查倒數，所以才一定要放在Update()底下
        }

        
    }
}