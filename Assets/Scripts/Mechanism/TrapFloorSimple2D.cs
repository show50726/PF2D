//TrapFloorSimple2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/12/22
//usage:            a basic trap floor, which you can set path and velocity.

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrapFloorSimple2D : Mechanism2D
{
    public Vector2 destination = new Vector2();
    public float speed = 3f;
    private float setSpeed = 0;
    private Vector2 start = new Vector2();
    private Rigidbody2D rb2d;
    public bool returnWhenOff = true;
    private Vector2 direction = Vector2.zero;
    private Vector2 originPos = new Vector2();
    
    protected override void Start()
    {
        base.Start();
        originPos = transform.localPosition;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        //rb2d.bodyType = RigidbodyType2D.Dynamic;
        rb2d.gravityScale = 0;

        start = transform.localPosition;


    }

    private void Update()
    {
        if (Activated)
        {
            SetMove();
        }
        else if (returnWhenOff)
        {
            SetMove();
        }
    }
    private void SetMove()
    {
        Vector2 jugdeD = JudgeMoveDirection();
        //if (direction == jugdeD) return;
        direction = jugdeD;
        //if (direction.magnitude != 0)
        //{
        //    rb2d.bodyType = RigidbodyType2D.Dynamic;
        //}
        //else
        //{
        //    rb2d.bodyType = RigidbodyType2D.Kinematic;
        //}
        rb2d.velocity = direction * speed;

    }
    private Vector2 JudgeMoveDirection()
    {
        if (Activated)
        {
            if (((Vector2)transform.localPosition - start).magnitude < destination.magnitude)
            {
                return destination.normalized;
            }
            else
            {
                //too far
                return Vector2.zero;
            }
        }
        else if (returnWhenOff)
        {
            if (((Vector2)transform.localPosition - (originPos + destination)).magnitude < destination.magnitude)
            {
                return -destination.normalized;
            }
            else
            {
                //too far
                return Vector2.zero;
            }
        }
        //since not activated nor need to return, no need to move
        return Vector2.zero;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //TODO: 這邊要想辦法判定是不是可推動的物體
        //DEV NOTE: OnCollisionEnter2D只會判定到有RB的物體...頂到地板或者無RB時就會失效。
        //DebugMessage(LogType.Normal, col.contacts[0].ToString());
    }

}
