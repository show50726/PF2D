//S(Stater) Unit Stater by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//Usage:            State the info of generic unit, such as hp. Assign it to any GO that needs these info.

using UnityEngine;
namespace CMSR
{
    public delegate void OnUnitDamageDelegate(float dmg);
    public delegate void OnUnitDeathDelegate();

    public class SUnitStater : STCMonoBehaviour
    {
        public bool debugMessage = false;
        /// <summary>
        /// consider when changing outfit, use this to tell UIM to change picture (in UI).
        /// </summary>
        [ReadOnly] [Tooltip("Controlled by script. Don't modify from editor.")]
        public string _SpecialOutfit = "";
        public event OnUnitDamageDelegate OnUnitDamageEvent;
        public event OnUnitDeathDelegate OnUnitDeathEvent;

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

        public float healthPoint = 100;
        public float healthPointLimit = 100;

        protected virtual void Reset()
        {
            healthPoint = 100;
            healthPointLimit = 100;
        }

        /// <summary>
        /// Use this function to correct data.
        /// </summary>
        protected virtual bool DataCorrect()
        {
            if (healthPoint < 0)
            {
                Debug.LogError(LogTitle(LogType.Error) + "healthPoint cannot be negative! " +
                    "(Will set to positive if continues.)");
                healthPoint = -healthPoint;
            }
            if (healthPoint == 0)
            {
                Debug.Log(LogTitle(LogType.Normal) + "");
            }
            if (healthPointLimit <= 0)
            {
                Debug.LogError(LogTitle(LogType.Error) + "healthPoint Limit cannot be negative! " +
                    "(Will set to healthPoint / 100 if continues)");
                healthPointLimit = healthPoint > 0 ? healthPoint : 100;
            }
            if (true)
            {

            }

            return true;
        }

        /// <summary>
        /// Heal() will heal to full healthPoint.
        /// </summary>
        public void Heal()
        {
            Heal(healthPointLimit);
            //DataCorrect();
            //healthPoint = healthPointLimit;
            //Debug.Log(LogTitle(LogType.Normal) + " successfully healed.");
        }
        /// <summary>
        /// Heal(healAmount) will heal specified amount until reach limit.
        /// </summary>
        public void Heal(float healAmount)
        {
            DataCorrect();
            healthPoint = (healthPointLimit - healthPoint) <= healAmount ? healthPointLimit : healthPoint + healAmount;
            if (healthPoint <= 0) healthPoint = 0;
            if(debugMessage) Debug.Log(LogTitle(LogType.Normal) + " successfully healed " + healAmount + " hp.");

        }
        /// <summary>
        /// Damage(damageAmount) will decrease HP. If HP is less than 0, Die() will be triggered.
        /// </summary>
        public void Damage(float damageAmount)
        {
            float actualDmg = (healthPoint - damageAmount) <= 0 ? healthPoint : damageAmount;
            healthPoint -= actualDmg;
            if (debugMessage) Debug.Log(LogTitle(LogType.Normal) + " got " + actualDmg + " damage(s).");
            if (OnUnitDamageEvent != null) OnUnitDamageEvent.Invoke(damageAmount);
            if (healthPoint <= 0)
            {
                Die();
            }
        }
        public void Die()
        {
            if (debugMessage) Debug.Log(LogTitle(LogType.Normal) + " is dead.");
            if (OnUnitDeathEvent != null) OnUnitDeathEvent.Invoke();
        }

    }
}