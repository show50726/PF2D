//made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23

using UnityEngine;

namespace BehaviourMachine
{
    /// <summary>
    /// Base class to store Keycode values.
    /// </summary>
    [System.Serializable]
    [ConcreteClass(typeof(ConcreteKeycodeVar))]
    public abstract class KeycodeVar : Variable
    {

        /// <summary>
        /// Variable value.
        /// </summary>
        public abstract KeyCode Value { get; set; }

        /// <summary>
        /// A generic get and set value.
        /// </summary>
        public override object genericValue { get { return this.Value; } set { this.Value = (KeyCode)value; } }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public KeycodeVar() : base() { }

        /// <summary>
        /// Constructor for keycode variables that will be added to a blackboard.
        /// <param name="name">The name of the variable.</param>
        /// <param name="blackboard">The variable blackboard.</param>
        /// <param name="id">The unique id of the variable</param>
        /// </summary>
        public KeycodeVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }

        /// <summary>
        /// User-defined conversion from KeycodeVar to Keycode
        /// </summary>
        public static implicit operator KeyCode(KeycodeVar variable)
        {
            return variable.Value;
        }

        /// <summary>
        /// User-defined conversion from Keycode to KeycodeVar
        /// </summary>
        public static implicit operator KeycodeVar(KeyCode value)
        {
            return new ConcreteKeycodeVar(value);
        }
    }

    /// <summary>
    /// Store Keycode values.
    /// </summary>
    [System.Serializable]
    public class ConcreteKeycodeVar : KeycodeVar
    {

        /// <summary>
        /// Serialized value.
        /// </summary>
        public KeyCode value;

        /// <summary>
        /// Variable value.
        /// </summary>
        public override KeyCode Value { get { return value; } set { this.value = value; } }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public ConcreteKeycodeVar() : base() { }

        /// <summary>
        /// Constructor for constants.
        /// <param name="value">The value of the variable.</param>
        /// </summary>
        public ConcreteKeycodeVar(KeyCode value)
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
        public ConcreteKeycodeVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }
    }

}