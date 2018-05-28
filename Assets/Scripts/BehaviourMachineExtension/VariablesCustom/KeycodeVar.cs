//made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23


//namespace BehaviourMachine
//{
//    [ConcreteClass(typeof(ConcreteKeycodeVar))]
//    public abstract class KeycodeVar : Variable
//    {
//        public KeycodeVar();
//        public KeycodeVar(string name, InternalBlackboard blackboard, int id);

//        public abstract UnityEngine.KeyCode Value { get; set; }
//        public override object genericValue { get; set; }

//        public static implicit operator UnityEngine.KeyCode(KeycodeVar variable);
//        public static implicit operator KeycodeVar(UnityEngine.KeyCode value);
//    }

//    public class ConcreteKeycodeVar : KeycodeVar
//    {
//        public UnityEngine.KeyCode value;

//        public ConcreteKeycodeVar();
//        public ConcreteKeycodeVar(UnityEngine.KeyCode value);
//        public ConcreteKeycodeVar(string name, InternalBlackboard blackboard, int id);

//        public override UnityEngine.KeyCode Value { get; set; }
//    }
//}