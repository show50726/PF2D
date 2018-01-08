//LiquidFloat made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/08
//Usage:            This is a simplified water, giving force to whom getting in.
//NOTE:             set collider to trigger. 2D only.
//NOTE:             the float force has gravity-normalized based on object.

using UnityEngine;

public class LiquidFloat : MonoBehaviour
{

    public float density = 1f;

    private void OnTriggerStay2D(Collider2D col)
    {
        Rigidbody2D objRb2d = col.GetComponent<Rigidbody2D>();
        if (objRb2d == null) return;
        float area = 1;
        BoxCollider2D objBCol2d = col.GetComponent<BoxCollider2D>();
        if (objBCol2d != null)
        {
            area = objBCol2d.size.x * objBCol2d.size.y;
        }
        //objRb2d.AddForce(new Vector2(0, area * density * objRb2d.gravityScale * - Physics2D.gravity.y));
        objRb2d.AddForce(-Physics2D.gravity * objRb2d.gravityScale * area * density);
    }

}
