//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//NodeCustomExample made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/07/04
using UnityEngine;

namespace BehaviourMachine
{
    /// <summary>
    /// Get a bounds size of renderer.
    /// </summary>
    [NodeInfo(category = "Action/Renderer/",
        icon = "Renderer",
        description = "Get a bounds size of renderer.")]
    public class GetBoundSize : ActionNode
    {
        [VariableInfo(requiredField = false, nullLabel = "use self", tooltip = "The one with renderer.")]
        public GameObjectVar target;

        [VariableInfo(requiredField = true, tooltip = "Save the bounds.size.x here.")]
        public FloatVar saveX;

        //to call self, use self.
        public override void Reset()
        {
            //such as XXVar = new ConcreteXXVar();
            target = new ConcreteGameObjectVar();
            saveX = new ConcreteFloatVar();
        }
        
        // This function is called when the node is in execution
        public override Status Update()
        {
            // Do stuff
            Renderer rdr = target.isNone ? self.GetComponent<Renderer>() : target.Value.GetComponent<Renderer>();
            if (rdr == null)
            {
                return Status.Error;
            }
            saveX.Value = rdr.bounds.size.x;

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