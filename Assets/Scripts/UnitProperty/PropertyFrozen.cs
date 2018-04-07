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

    private System.Collections.Generic.List<PF2DController> thingsHasBeenSpeedUp =
        new System.Collections.Generic.List<PF2DController>();

    protected override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        if (rb2d)
        {
            originalPM2d = rb2d.sharedMaterial;
            rb2d.sharedMaterial.friction = 0;
        }
    }

    private void ModifyControllerSpeed(PF2DController controller, bool isMultiply)
    {
        //if isMultiply set to false, will divide then
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
            if (thingsHasBeenSpeedUp.Contains(controller) == true && isMultiply == true) return;//has been multiplied, don't do twice.
            if (thingsHasBeenSpeedUp.Contains(controller) == false && isMultiply == true)
            {
                Debug.Log("original move speed is " + controller.movingSpeed);
                controller.movingSpeed *= moveSpeedMultiplier;
                //p.AddVelocity(new Vector2((moveSpeedMultiplier-1)*rb.velocity.x, (moveSpeedMultiplier - 1) * rb.velocity.y));
                //rb.velocity *=  moveSpeedMultiplier * (p.isFacingRight ? 1f : -1f);
                thingsHasBeenSpeedUp.Add(controller);
                Debug.Log("set multiplied completed. now speed is " + controller.movingSpeed);
            }
            else if (thingsHasBeenSpeedUp.Contains(controller) == false && isMultiply == false)
            {
                Debug.LogWarning(GetType().Name + " warning: trying to divide speed of " + controller.gameObject.name + " which isn't multiplied before. To avoid bugs the speed will not divide. Check your flow.");
            }
            else if (thingsHasBeenSpeedUp.Contains(controller) == true && isMultiply == false)
            {
                controller.movingSpeed /= moveSpeedMultiplier;
                thingsHasBeenSpeedUp.Remove(controller);
                Debug.Log("set divided completed. now speed is " + controller.movingSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("Hey yo, " + col.gameObject.name + " has touched " + name + "!");
        PF2DController controller = col.gameObject.GetComponent<PF2DController>();
        if (controller)
        {
            ModifyControllerSpeed(controller, true);
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        Debug.Log("Hey yo, " + col.gameObject.name + " has left " + name + "!");
        PF2DController controller = col.gameObject.GetComponent<PF2DController>();
        if (controller)
        {
            ModifyControllerSpeed(controller, false);
        }
    }

    public void Melt()
    {
        Destroy(this);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(rb2d) rb2d.sharedMaterial = originalPM2d;
    }

}
