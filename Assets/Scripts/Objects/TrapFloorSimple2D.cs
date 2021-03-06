﻿//TrapFloorSimple2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/15
//usage:            a basic trap floor, which you can set path and velocity.

using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TrapFloorSimple2D : Mechanism2D
{
    public Vector2 destination = new Vector2();
    public float speed = 3f;
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
        if (direction == jugdeD) return;
        direction = jugdeD;
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

}
