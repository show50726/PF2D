//Water made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/07
//Usage:            This is a simplified water, giving force to whom getting in.
//NOTE:             set collider to trigger. 2D only.

using UnityEngine;

public class Water : MonoBehaviour
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
        objRb2d.AddForce(new Vector2(0, area * density));
    }

}
