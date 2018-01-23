//PropertyFeather made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/07
//Usage:            This is a specified property, which makes player run faster and jump higher.

using UnityEngine;

public class PropertyFeather : PlayerProperty2D
{
    public float moveSpeedMultiplier = 1.4f;
    public float jumpHeightMultiplier = 1.571428f;
    public float weightMultiplier = 0.5f;
    private Rigidbody2D rb2d;
    private float originalGravityScale;
	public GameObject featherGiver;

    protected override void Start()
    {
        base.Start();
		rb2d = GetComponent<Rigidbody2D> ();
        ActivateEffect(true);
		player.Circle.GetComponent<SpriteRenderer> ().color = featherGiver.GetComponent<SpriteRenderer>().color;
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
            if (weightMultiplier <= 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: weightMultiplier has been set to 0. For bug avoiding, The mass won't change.");
            }
            else rb2d.mass /= weightMultiplier;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (enabled == false) return;
		ActivateEffect(false);
    }

}
