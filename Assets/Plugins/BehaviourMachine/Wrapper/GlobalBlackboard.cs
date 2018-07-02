//----------------------------------------------
//            Behaviour Machine
//     Auto-Generated Code, edit carefully
//----------------------------------------------

using UnityEngine;
using BehaviourMachine;

namespace BehaviourMachine {
    [AddComponentMenu("")]
    public class GlobalBlackboard : InternalGlobalBlackboard {
        /// <summary>
        /// The GlobalBlackboard instance.
        /// </summary>
        public static new GlobalBlackboard Instance {get {return InternalGlobalBlackboard.Instance as GlobalBlackboard;}}

        // BoolVars
        public static readonly int JUST_DAMAGED = -842839698;
        // StringVars
        public static readonly int Tag_Player = -729775018;
        public static readonly int Tag_Wall_Collider = -977670856;
        // FsmEvents
        public static readonly int APPLICATION_FOCUS = -1;
        public static readonly int APPLICATION_PAUSE = -2;
        public static readonly int APPLICATION_QUIT = -3;
        public static readonly int BECAME_INVISIBLE = -4;
        public static readonly int BECAME_VISIBLE = -5;
        public static readonly int COLLISION_ENTER = -6;
        public static readonly int COLLISION_ENTER_2D = -7;
        public static readonly int COLLISION_EXIT = -8;
        public static readonly int COLLISION_EXIT_2D = -9;
        public static readonly int FINISHED = -10;
        public static readonly int JOINT_BREAK = -11;
        public static readonly int MOUSE_ENTER = -12;
        public static readonly int MOUSE_EXIT = -13;
        public static readonly int MOUSE_DOWN = -14;
        public static readonly int MOUSE_UP = -15;
        public static readonly int MOUSE_UP_BUTTON = -16;
        public static readonly int TRIGGER_ENTER = -17;
        public static readonly int TRIGGER_ENTER_2D = -18;
        public static readonly int TRIGGER_EXIT = -19;
        public static readonly int TRIGGER_EXIT_2D = -20;
        public static readonly int ToIdle = -438203413;
        public static readonly int OnDamage = -269976692;
        public static readonly int TO_DIE = -818647525;
        public static readonly int TO_DASH = -620005631;
        public static readonly int TO_DAMAGED = -1777797933;
    }
}
