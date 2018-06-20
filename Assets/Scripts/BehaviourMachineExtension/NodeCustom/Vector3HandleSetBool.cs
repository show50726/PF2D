//----------------------------------------------
//            Behaviour Machine
// Copyright © 2014 Anderson Campos Cardoso
//----------------------------------------------
//Vector3HandleSetBool made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23

namespace BehaviourMachine
{
    /// <summary>
    /// Change multiple bool when vector is not 0 in all direction. Comes in particular usage....
    /// </summary>
    [NodeInfo(category = "Action/Blackboard/",
                icon = "Blackboard",
                description = "Change multiple bool when vector is not 0 in all direction. Comes in particular usage....")]
    public class Vector3HandleSetBool : ActionNode
    {

        /// <summary>
        /// The reference vector.
        /// </summary>
        [VariableInfo(canBeConstant = true, requiredField = true, tooltip = "The reference Vector3")]
        public Vector3Var referVector;

        /// <summary>
        /// This bool changes value when vector.x > 0
        /// </summary>
        [VariableInfo(canBeConstant = false, requiredField = false, nullLabel = "Ignore", tooltip = "This bool changes value when vector.x > 0")]
        public BoolVar whenXIsPst;
        [VariableInfo(requiredField = false, nullLabel = "Toggle", tooltip = "The new variable value. If Toggle is selected the value of \"variable\" is flipped")]
        public BoolVar newValueXP;
        /// <summary>
        /// This bool changes value when vector.x is negative
        /// </summary>
        [VariableInfo(canBeConstant = false, requiredField = false, nullLabel = "Ignore", tooltip = "This bool changes value when vector.x < 0")]
        public BoolVar whenXIsNgt;
        [VariableInfo(requiredField = false, nullLabel = "Toggle", tooltip = "The new variable value. If Toggle is selected the value of \"variable\" is flipped")]
        public BoolVar newValueXN;
        /// <summary>
        /// This bool changes value when vector.y > 0
        /// </summary>
        [VariableInfo(canBeConstant = false, requiredField = false, nullLabel = "Ignore", tooltip = "This bool changes value when vector.y > 0")]
        public BoolVar whenYIsPst;
        [VariableInfo(requiredField = false, nullLabel = "Toggle", tooltip = "The new variable value. If Toggle is selected the value of \"variable\" is flipped")]
        public BoolVar newValueYP;
        /// <summary>
        /// This bool changes value when vector.y is negative
        /// </summary>
        [VariableInfo(canBeConstant = false, requiredField = false, nullLabel = "Ignore", tooltip = "This bool changes value when vector.y < 0")]
        public BoolVar whenYIsNgt;
        [VariableInfo(requiredField = false, nullLabel = "Toggle", tooltip = "The new variable value. If Toggle is selected the value of \"variable\" is flipped")]
        public BoolVar newValueYN;
        /// <summary>
        /// This bool changes value when vector.z > 0
        /// </summary>
        [VariableInfo(canBeConstant = false, requiredField = false, nullLabel = "Ignore", tooltip = "This bool changes value when vector.z > 0")]
        public BoolVar whenZIsPst;
        [VariableInfo(requiredField = false, nullLabel = "Toggle", tooltip = "The new variable value. If Toggle is selected the value of \"variable\" is flipped")]
        public BoolVar newValueZP;
        /// <summary>
        /// This bool changes value when vector.z is negative
        /// </summary>
        [VariableInfo(canBeConstant = false, requiredField = false, nullLabel = "Ignore", tooltip = "This bool changes value when vector.z < 0")]
        public BoolVar whenZIsNgt;
        [VariableInfo(requiredField = false, nullLabel = "Toggle", tooltip = "The new variable value. If Toggle is selected the value of \"variable\" is flipped")]
        public BoolVar newValueZN;

        public override void Reset()
        {
            whenXIsPst = new ConcreteBoolVar();
            newValueXP = new ConcreteBoolVar();
            whenXIsNgt = new ConcreteBoolVar();
            newValueXN = new ConcreteBoolVar();

            whenYIsPst = new ConcreteBoolVar();
            newValueYP = new ConcreteBoolVar();
            whenYIsNgt = new ConcreteBoolVar();
            newValueYN = new ConcreteBoolVar();

            whenZIsPst = new ConcreteBoolVar();
            newValueZP = new ConcreteBoolVar();
            whenZIsNgt = new ConcreteBoolVar();
            newValueZN = new ConcreteBoolVar();
        }

        public override Status Update()
        {
            // Validate members
            if ((whenXIsPst.isNone && !newValueXP.isNone) || (whenXIsNgt.isNone && !newValueXN.isNone) ||
                (whenYIsPst.isNone && !newValueYP.isNone) || (whenYIsNgt.isNone && !newValueYN.isNone) ||
                (whenZIsPst.isNone && !newValueZP.isNone) || (whenZIsNgt.isNone && !newValueZN.isNone))
            {
                return Status.Error;
            }
            // Flip value?
            if (!whenXIsPst.isNone) whenXIsPst.Value = newValueXP.isNone ? !whenXIsPst.Value : newValueXP.Value;
            if (!whenXIsNgt.isNone) whenXIsNgt.Value = newValueXN.isNone ? !whenXIsNgt.Value : newValueXN.Value;

            if (!whenYIsPst.isNone) whenYIsPst.Value = newValueYP.isNone ? !whenYIsPst.Value : newValueYP.Value;
            if (!whenYIsNgt.isNone) whenYIsNgt.Value = newValueYN.isNone ? !whenYIsNgt.Value : newValueYN.Value;

            if (!whenZIsPst.isNone) whenZIsPst.Value = newValueZP.isNone ? !whenZIsPst.Value : newValueZP.Value;
            if (!whenZIsNgt.isNone) whenZIsNgt.Value = newValueZN.isNone ? !whenZIsNgt.Value : newValueZN.Value;

            return Status.Success;
        }

    }
}