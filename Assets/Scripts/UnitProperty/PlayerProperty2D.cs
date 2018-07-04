//Player Property 2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//Usage:            This is made for player property 2D example. Inherit this to write a property script.

using UnityEngine;
using CMSR;
public class PlayerProperty2D : UnitProperty
{
    [Header("Change Appearance")]
    [Tooltip("This will set on when enabled AND disabled.")]
    public string[] _SetAnimatorBool = { "changeAppearance" };
    [Tooltip("This will be set true when enabled, and false when disabled.")]
    public string[] _TurnOnAnimatorBool = { };
    [Tooltip("This will be set false when enabled, and true when disabled.")]
    public string[] _TurnOffAnimatorBool = { "PropertyNone" };
    public Color showingColor;
    /// <summary>
    /// to tell UIManager to change special picture if player has property. See SUnitStater._SpecialOutfit
    /// </summary>
    public string _SetOutfitKeyword = "";
    private string originalOutfitKeyword = "";

    [Header("Setting Data")]
    public PF2DController controller2D;
    public Player player;
    /// <summary>
    /// call when property on enable (true) & disable (false)
    /// </summary>
    /// <param name="apply">to tell if is apply(true) or unapply(false)</param>
    public virtual void ApplyAppearanceChange(bool apply)
    {
        #region Animator
        Animator anim = GetComponent<Animator>();
        if (anim != null && anim.isActiveAndEnabled)
        {
            //have to check if there's AC running, otherwise many warning comes out.
            //see info here: https://answers.unity.com/questions/1337226/index.html
            if (apply)
            {
                DebugMessage(LogType.Normal, "apply changes.");
                foreach (string s in _TurnOnAnimatorBool)
                {
                    anim.SetBool(s, true);
                    DebugMessage(LogType.Normal, "set " + s + " to true.");
                }
                foreach (string s in _TurnOffAnimatorBool)
                {
                    anim.SetBool(s, false);
                    DebugMessage(LogType.Normal, "set " + s + " to false.");
                }
            }
            else
            {
                DebugMessage(LogType.Normal, "unapply changes.");
                foreach (string s in _TurnOnAnimatorBool)
                {
                    anim.SetBool(s, false);
                    DebugMessage(LogType.Normal, "set " + s + " to false.");
                }
                foreach (string s in _TurnOffAnimatorBool)
                {
                    anim.SetBool(s, true);
                    DebugMessage(LogType.Normal, "set " + s + " to true.");
                }
            }
            foreach (string s in _SetAnimatorBool)
            {
                anim.SetBool(s, true);
                DebugMessage(LogType.Normal, "set " + s + " to true.");
            }
        }
        #endregion

        #region UI Outfit
        SUnitStater stater = GetComponent<SUnitStater>();
        if (stater != null && _SetOutfitKeyword != "")
        {
            if (apply)
            {
                originalOutfitKeyword = stater._SpecialOutfit;
                stater._SpecialOutfit = _SetOutfitKeyword;
            }
            else
            {
                stater._SpecialOutfit = originalOutfitKeyword;
            }
        }

        #endregion
    }

    protected virtual void OnEnable()
    {
        ApplyAppearanceChange(true);
    }
    protected override void Start()
    {
        base.Start();
        if (player == null)
        {
            player = GetComponent<Player>();
            if (player == null)
            {
                Debug.LogWarning(GetType().Name + " warning: failed to find player on " + name + ". To avoid bugs, this property won't work.");
                enabled = false;
                return;
            }
        }
        if (controller2D == null)
        {
            controller2D = GetComponent<PF2DController>();
            if (controller2D == null)
            {
                Debug.LogWarning(GetType().Name + " warning: failed to find controller2D on " + name + ". To avoid bugs, this property won't work.");
                enabled = false;
                return;
            }
        }
    }
    protected virtual void OnDisable()
    {
        ApplyAppearanceChange(false);
    }

}
