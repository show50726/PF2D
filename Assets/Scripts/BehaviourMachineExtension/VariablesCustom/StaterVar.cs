//made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23

using UnityEngine;
using CMSR;

namespace BehaviourMachine
{
    /// <summary>
    /// Base class to store Stater values.
    /// </summary>
    
    [System.Serializable]
    [ConcreteClass(typeof(ConcreteStaterVar))]
    public abstract class StaterVar : Variable
    {
        /// <summary>
        /// Variable value.
        /// </summary>
        public abstract SUnitStater Value { get; set; }

        /// <summary>
        /// A generic get and set value.
        /// </summary>
        public override object genericValue { get { return this.Value; } set { this.Value = (SUnitStater)value; } }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public StaterVar() : base() { }

        /// <summary>
        /// Constructor for Stater variables that will be added to a blackboard.
        /// <param name="name">The name of the variable.</param>
        /// <param name="blackboard">The variable blackboard.</param>
        /// <param name="id">The unique id of the variable</param>
        /// </summary>
        public StaterVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }

        /// <summary>
        /// User-defined conversion from KeycodeVar to Keycode
        /// </summary>
        public static implicit operator SUnitStater(StaterVar variable)
        {
            return variable.Value;
        }

        /// <summary>
        /// User-defined conversion from Keycode to KeycodeVar
        /// </summary>
        public static implicit operator StaterVar(SUnitStater value)
        {
            return new ConcreteStaterVar(value);
        }
    }

    /// <summary>
    /// Store Keycode values.
    /// </summary>
    [System.Serializable]
    public class ConcreteStaterVar : StaterVar
    {

        /// <summary>
        /// Serialized value.
        /// </summary>
        public SUnitStater value;

        /// <summary>
        /// Variable value.
        /// </summary>
        public override SUnitStater Value { get { return value; } set { this.value = value; } }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public ConcreteStaterVar() : base() { }

        /// <summary>
        /// Constructor for constants.
        /// <param name="value">The value of the variable.</param>
        /// </summary>
        public ConcreteStaterVar(SUnitStater value)
        {
            this.SetAsConstant();
            this.value = value;
        }

        /// <summary>
        /// Constructor for float variables that will be added to a blackboard.
        /// <param name="name">The name of the variable.</param>
        /// <param name="blackboard">The variable blackboard.</param>
        /// <param name="id">The unique id of the variable</param>
        /// </summary>
        public ConcreteStaterVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }
    }

}