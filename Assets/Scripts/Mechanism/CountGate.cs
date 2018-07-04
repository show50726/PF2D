//CountGate made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//usage:            can be used in situation that need to activate several time to be success.
//NOTE:             Mechanism2D needed. 

using UnityEngine;

public class CountGate : Mechanism2D {

    [Tooltip("The times need to be set activated to trigger.")]
    public int _CountNeeded = 3;
    [SerializeField, ReadOnly]
    [Tooltip("Value only used by script. Allowed to view as debug usage.")]
    public int _NowCount = 0;
    [Header("Setting to connected mechanism.")]
    public Mechanism2D[] connectedMechanism;

    [Tooltip("When checked, mechanisms will 'switch state' (on<->off) instead of turning on / off.")]
    public bool switchInsteadTurnOn = false;
    [Tooltip("(Prior than switch) When checked, mechanisms will 'turn off' instead of turning on/switch.")]
    public bool turnOffInsteadTurnOn = false;
    
    protected override void Start()
    {
        #region Security Check.
        for (int i = 0; i < connectedMechanism.Length; i++)
        {
            if (connectedMechanism[i] == null)
            {
                DebugMessage(LogType.Error, "you didn't assign some objects in connectedMechanism! To avoid bugs, the component won't work.");
                enabled = false;
                return;
            }
            if (connectedMechanism[i] == this)
            {
                DebugMessage(LogType.Error, "connectedMechanism has assigned the component itself. You should not do this. In case of bugs, the component won't work.");
                enabled = false;
                return;
            }
        }
        if (_CountNeeded <= 0)
        {
            DebugMessage(LogType.Error, "the Count Needed is less than 0. Unable to work.");
            enabled = false;
            return;
        }
        if (_NowCount < 0)
        {
            DebugMessage(LogType.Warning, "a strange bug occured where Now Count is less than 0. Auto correct.");
            _NowCount = 0;
        }
        else if (_NowCount > _CountNeeded)
        {
            DebugMessage(LogType.Error, " Now Count is greater than Count Needed, which should not happen. Assume as typing error, and will exchange the value of two if continues. ");
            int tmp = _NowCount;
            _NowCount = _CountNeeded;
            _CountNeeded = tmp;
        }
        #endregion
        base.Start();
    }

    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        //當設定啟動時
        //	開啟一個switchcase
        //  如果switchcase滿了
        //		啟動所有附屬機關
        //當設定關閉時
        //	如果switchcase滿了
        //		關閉所有附屬機關
        //	否則 如果switchcase有開
        //			關閉一個switchcase
        //	否則(都是全關的話)
        //		(不做事)
        if (stayOnAfterActivated && _NowCount >= _CountNeeded) return; //activated once. No need to turn off or reset.
        if (isTurnOn)
        {
            if (_NowCount < _CountNeeded) _NowCount++;
            if (_NowCount == _CountNeeded)
            {
                ActivateMechanism(true);
            }
            DebugMessage(LogType.Normal, "activated. Count now " + _NowCount +"/" + _CountNeeded + ", " + ( _NowCount >= _CountNeeded ? " connected mechanism activated." : ""));
        }
        else
        {
            DebugMessage(LogType.Normal, "deactivated. Count now " + _NowCount + "/" + _CountNeeded + ", " + (_NowCount >= _CountNeeded ? " deactivate connected mechanism." : ""));

            if (_NowCount >= _CountNeeded)
            {
                ActivateMechanism(false);
            }
            if (_NowCount > 0)
            {
                _NowCount--;
            }
        }

    }

    private void ActivateMechanism(bool isOn)
    {
        foreach (Mechanism2D m in connectedMechanism)
        {
            bool setValue = turnOffInsteadTurnOn ? !isOn : switchInsteadTurnOn ? !m.Activated : isOn;
            DebugMessage(LogType.Normal, "trying to " + (setValue ? "activate" : "deactivate") + " " + m.GetType().Name + " of " + m.gameObject.name + ".");
            m.Activated = setValue;
        }
    }
    
	
}
