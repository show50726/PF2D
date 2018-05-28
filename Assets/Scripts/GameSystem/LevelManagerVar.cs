//made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23

using UnityEngine;

namespace BehaviourMachine
{
    /// <summary>
    /// Base class to store LevelManager values.
    /// </summary>
    [System.Serializable]
    [ConcreteClass(typeof(ConcreteFloatVar))]
    public abstract class LevelManagerVar : Variable
    {

        /// <summary>
        /// Variable value.
        /// </summary>
        public abstract LevelManager Value { get; set; }

        /// <summary>
        /// A generic get and set value.
        /// </summary>
        public override object genericValue { get { return this.Value; } set { this.Value = (LevelManager)value; } }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public LevelManagerVar() : base() { }

        /// <summary>
        /// Constructor for float variables that will be added to a blackboard.
        /// <param name="name">The name of the variable.</param>
        /// <param name="blackboard">The variable blackboard.</param>
        /// <param name="id">The unique id of the variable</param>
        /// </summary>
        public LevelManagerVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }

        /// <summary>
        /// User-defined conversion from FloatVar to float
        /// </summary>
        public static implicit operator LevelManager(LevelManagerVar variable)
        {
            return variable.Value;
        }

        /// <summary>
        /// User-defined conversion from float to FloatVar
        /// </summary>
        public static implicit operator LevelManagerVar(LevelManager value)
        {
            return new ConcreteLevelManagerVar(value);
        }
    }

    /// <summary>
    /// Store float values.
    /// </summary>
    [System.Serializable]
    public class ConcreteLevelManagerVar : LevelManagerVar
    {

        /// <summary>
        /// Serialized value.
        /// </summary>
        public LevelManager value;

        /// <summary>
        /// Variable value.
        /// </summary>
        public override LevelManager Value { get { return value; } set { this.value = value; } }

        /// <summary>
        /// Constructor for none variables.
        /// </summary>
        public ConcreteLevelManagerVar() : base() { }

        /// <summary>
        /// Constructor for constants.
        /// <param name="value">The value of the variable.</param>
        /// </summary>
        public ConcreteLevelManagerVar(LevelManager value)
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
        public ConcreteLevelManagerVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }
    }
}