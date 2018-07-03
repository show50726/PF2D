//PropertyFeather made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//Usage:            This is a specified property, which makes player run faster and jump higher.

using UnityEngine;

public class PropertyFeather : PlayerProperty2D
{
    public PropertyFeather()
    {
        _TurnOnAnimatorBool = new string[] { "PropertyFeather" };
        showingColor = new Color32(79, 44, 167, 255);
    }
    public float moveSpeedMultiplier = 1.4f;
    public float jumpHeightMultiplier = 1.571428f;
    public float weightMultiplier = 0.5f;
    private Rigidbody2D rb2d;
    private PF2DController pc;
    private float originalGravityScale;

    private LayerMask o_JumpLayer;
    private bool o_JumpLayerIsAllow = true;
    public LayerMask allowJumpOnTheseThing = (1 << 4);

    protected override void Start()
    {
        base.Start();
		rb2d = GetComponent<Rigidbody2D> ();
        pc = GetComponent<PF2DController>();
        ActivateEffect(true);
        player.Circle.GetComponent<SpriteRenderer>().color = showingColor;
    }

    private void ActivateEffect(bool activate)
    {
        if (moveSpeedMultiplier == 0 || jumpHeightMultiplier == 0)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: one of multiplier has been set to 0. For bug avoiding, this property won't work.");
            enabled = false;
            return;
        }
        if (controller2D == null)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: no controller assigned. For bug avoiding, this property won't work.");
            enabled = false;
            return;
        }

        if (activate)
        {
            controller2D.movingSpeed *= moveSpeedMultiplier;
            controller2D.jumpHeight *= jumpHeightMultiplier;
            if (~pc.onlyJumpOnTheseThing != 0)
            {
                o_JumpLayer = pc.onlyJumpOnTheseThing;
                o_JumpLayerIsAllow = true;
                pc.onlyJumpOnTheseThing |= allowJumpOnTheseThing;
            }
            else
            {
                o_JumpLayer = pc.cannotJumpOnTheseThing;
                o_JumpLayerIsAllow = false;
                pc.cannotJumpOnTheseThing &= ~(allowJumpOnTheseThing);
            }
            if (weightMultiplier <= 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: weightMultiplier has been set to 0. For bug avoiding, The mass won't change.");
            }
            else rb2d.mass *= weightMultiplier;
        }
        else
        {
            controller2D.movingSpeed /= moveSpeedMultiplier;
            controller2D.jumpHeight /= jumpHeightMultiplier;
            if (o_JumpLayerIsAllow)
            {
                pc.onlyJumpOnTheseThing = o_JumpLayer; 
            }
            else
            {
                pc.cannotJumpOnTheseThing = o_JumpLayer;
            }
            if (weightMultiplier <= 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: weightMultiplier has been set to 0. For bug avoiding, The mass won't change.");
            }
            else rb2d.mass /= weightMultiplier;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ActivateEffect(false);
    }

}
