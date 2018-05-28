//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//Vector3CorrectByBool made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23

using UnityEngine;
namespace BehaviourMachine
{
    /// <summary>
    /// Corrects vector by considering multiple bools.
    /// </summary>
    [NodeInfo(category = "Action/Blackboard/",
                icon = "Blackboard",
                description = "Corrects vector by considering multiple bools.")]
    public class Vector3CorrectByBool : ActionNode
    {
        [VariableInfo(canBeConstant = false, requiredField = true, tooltip = "The target Vector3")]
        public Vector3Var target;

        [VariableInfo(requiredField = true, tooltip = "If set to false, vector.x will set to 0 when > 0")]
        public BoolVar XAllowP;
        [VariableInfo(requiredField = true, tooltip = "If set to false, vector.x will set to 0 when < 0")]
        public BoolVar XAllowN;
        [VariableInfo(requiredField = true, tooltip = "If set to false, vector.y will set to 0 when > 0")]
        public BoolVar YAllowP;
        [VariableInfo(requiredField = true, tooltip = "If set to false, vector.y will set to 0 when < 0")]
        public BoolVar YAllowN;
        [VariableInfo(requiredField = true, tooltip = "If set to false, vector.z will set to 0 when > 0")]
        public BoolVar ZAllowP;
        [VariableInfo(requiredField = true, tooltip = "If set to false, vector.z will set to 0 when < 0")]
        public BoolVar ZAllowN;

        public override void Reset()
        {
            target = new ConcreteVector3Var();

            XAllowP = new ConcreteBoolVar();
            XAllowN = new ConcreteBoolVar();
            YAllowP = new ConcreteBoolVar();
            YAllowN = new ConcreteBoolVar();
            ZAllowP = new ConcreteBoolVar();
            ZAllowN = new ConcreteBoolVar();
        }


        public override Status Update()
        {
            if (target.isNone || 
                XAllowP.isNone || XAllowN.isNone ||
                YAllowP.isNone || YAllowN.isNone ||
                ZAllowP.isNone || ZAllowN.isNone)
            {
                return Status.Error;
            }
            Vector3 newValue = target.Value;
            if (newValue.x > 0 && !XAllowP) newValue.x = 0;
            if (newValue.x < 0 && !XAllowN) newValue.x = 0;
            if (newValue.y > 0 && !YAllowP) newValue.y = 0;
            if (newValue.y < 0 && !YAllowN) newValue.y = 0;
            if (newValue.z > 0 && !ZAllowP) newValue.z = 0;
            if (newValue.z < 0 && !ZAllowN) newValue.z = 0;

            target.Value = newValue;
            return Status.Success;
        }

    }
}