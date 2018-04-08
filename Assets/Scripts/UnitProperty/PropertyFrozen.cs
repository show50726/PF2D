//Property Frozen   made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/08
//Usage:            This is a specified property, which makes things become frozen.

using UnityEngine;
public class PropertyFrozen : UnitProperty
{
    [ReadOnly]
    [Tooltip("This is used (only in script) to determine when to \"melt\". Think as negative HP of ice.")]
    public float meltingFactorStorage = 0;
    [Tooltip("If set to true, ice will never melt.")]
    public bool immortalize = false;

    [Header("Interact Aettings")]
    public float friction = 0;
    public float moveSpeedMultiplier = 1.4f;

    private Rigidbody2D rb2d;
    private PhysicsMaterial2D originalPM2d;
    private float initialMeltingFactorStorage;

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
        initialMeltingFactorStorage = meltingFactorStorage;
    }
    public void StayFrozen(bool isTurnOn)
    {
        if (isTurnOn)
        {
            immortalize = true;
            meltingFactorStorage = initialMeltingFactorStorage;
        }
        else
        {
            immortalize = false;
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

    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("Hey yo, " + col.gameObject.name + " has touched " + name + "!");
        PF2DController controller = col.gameObject.GetComponent<PF2DController>();
        if (controller)
        {
            ModifyControllerSpeed(controller, true);
        }
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        Debug.Log("Hey yo, " + col.gameObject.name + " has left " + name + "!");
        PF2DController controller = col.gameObject.GetComponent<PF2DController>();
        if (controller)
        {
            ModifyControllerSpeed(controller, false);
        }
    }
    /*
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
    */
    public void Melt()
    {
        //Destroy the property, or even the whole gameobject
        if (immortalize) return;
        if (tag == "Fragile")
        {
            Destroy(gameObject);
        }
        else propertyManager.RemoveProperty(GetType());
    }
    public void Melt(float meltingFactor)
    {
        //when meltingFactor is set to <= 0, melt immediately.
        meltingFactorStorage += Time.deltaTime;
        Debug.Log(name + ": melting factor is now " + meltingFactorStorage);
        if (meltingFactorStorage >= meltingFactor)
        {
            Melt();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(rb2d) rb2d.sharedMaterial = originalPM2d;
    }

}
