//Property Frozen   made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2017/11/25
//Usage:            This is a specified property, which makes things become frozen.

using UnityEngine;
using System.Collections;

public class PropertyFrozen : UnitProperty
{
    [ReadOnly, Tooltip("This is used (only in script) to determine when to \"melt\".")]
    public float meltingFactor = 0;

    [Header("Interact Aettings")]
    public float friction = 0;
    public float moveSpeedMultiplier = 1.1f;

    private Rigidbody2D rb2d;
    private PhysicsMaterial2D originalPM2d;

    protected override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d)
        {
            originalPM2d = rb2d.sharedMaterial;
            rb2d.sharedMaterial = new PhysicsMaterial2D();
            rb2d.sharedMaterial.friction = friction;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        PF2DController controller = col.gameObject.GetComponent<PF2DController>();
        if (controller)
        {
            if (moveSpeedMultiplier < 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: moveSpeedMultiplier set < 0. This might cause strange move.");
            }
            if (moveSpeedMultiplier == 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: moveSpeedMultiplier set to 0. To avoid bugs, the script will not modify movingSpeed.");
            }
            else
            {
                controller.movingSpeed *= moveSpeedMultiplier;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        PF2DController controller = col.gameObject.GetComponent<PF2DController>();
        if (controller)
        {
            if (moveSpeedMultiplier < 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: moveSpeedMultiplier set < 0. This might cause strange move.");
            }
            if (moveSpeedMultiplier == 0)
            {
                Debug.LogWarning(GetType().Name + " of " + name + " warning: moveSpeedMultiplier set to 0. To avoid bugs, the script will not modify movingSpeed.");
            }
            else
            {
                controller.movingSpeed /= moveSpeedMultiplier;
            }
        }
    }

    public void Melt()
    {
        Destroy(this);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        rb2d.sharedMaterial = originalPM2d;
    }

}
