//made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/06/05

using UnityEngine;

namespace BehaviourMachine
{
    /// <summary>
    /// Base class to store LevelManager values.
    /// </summary>
    [System.Serializable]
    [ConcreteClass(typeof(ConcreteLevelManagerVar))]
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
        /// Constructor for LevelManager variables that will be added to a blackboard.
        /// <param name="name">The name of the variable.</param>
        /// <param name="blackboard">The variable blackboard.</param>
        /// <param name="id">The unique id of the variable</param>
        /// </summary>
        public LevelManagerVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }

        /// <summary>
        /// User-defined conversion from LevelManagerVar to LevelManager
        /// </summary>
        public static implicit operator LevelManager(LevelManagerVar variable)
        {
            return variable.Value;
        }

        /// <summary>
        /// User-defined conversion from LevelManager to LevelManagerVar
        /// </summary>
        public static implicit operator LevelManagerVar(LevelManager value)
        {
            return new ConcreteLevelManagerVar(value);
        }
    }

    /// <summary>
    /// Store LevelManager values.
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
        /// Constructor for LevelManager variables that will be added to a blackboard.
        /// <param name="name">The name of the variable.</param>
        /// <param name="blackboard">The variable blackboard.</param>
        /// <param name="id">The unique id of the variable</param>
        /// </summary>
        public ConcreteLevelManagerVar(string name, InternalBlackboard blackboard, int id) : base(name, blackboard, id) { }
    }
}