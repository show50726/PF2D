//S(Stater) Unit Stater by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/05/21
//Usage:            State the info of generic unit, such as hp. Assign it to any GO that needs these info.

using System;
using UnityEngine;
namespace CMSR
{
    public class SUnitStater : MonoBehaviour
    {
        public string LogTitle(LogType logType)
        {
            string logHint = "";
            switch (logType)
            {
                case LogType.Normal:
                    break;
                case LogType.Warning:
                    logHint = " warning";
                    break;
                case LogType.Error:
                    logHint = " error";
                    break;
                default:
                    break;
            }

            return name + "/" + GetType().Name + logHint + ":";
        }

        public float HP = 100;
        public float HPLimit = 100;

        protected virtual void Reset()
        {
            HP = 100;
            HPLimit = 100;
        }

        /// <summary>
        /// Use this function to correct data.
        /// </summary>
        protected virtual bool DataCorrect()
        {
            if (HP < 0)
            {
                Debug.LogError(LogTitle(LogType.Error) + "HP cannot be negative! " +
                    "(Will set to positive if continues.)");
                HP = -HP;
            }
            if (HP == 0)
            {
                Debug.Log(LogTitle(LogType.Normal) + "");
            }
            if (HPLimit <= 0)
            {
                Debug.LogError(LogTitle(LogType.Error) + "HP Limit cannot be negative! " +
                    "(Will set to HP / 100 if continues)");
                HPLimit = HP > 0 ? HP : 100;
            }
            if (true)
            {

            }

            return true;
        }

        /// <summary>
        /// Heal() will heal to full HP.
        /// </summary>
        public void Heal()
        {
            DataCorrect();
            HP = HPLimit;
            Debug.Log(LogTitle(LogType.Normal) + " successfully healed.");
        }
        /// <summary>
        /// Heal(healAmount) will heal specified amount until reach limit.
        /// </summary>
        public void Heal(float healAmount)
        {
            DataCorrect();
            HP = (HPLimit - HP) <= healAmount ? HPLimit : HP + healAmount;
            if (HP <= 0) HP = 0;
            Debug.Log(LogTitle(LogType.Normal) + " successfully healed.");

        }


    }
    public enum LogType { Normal, Warning, Error }
}