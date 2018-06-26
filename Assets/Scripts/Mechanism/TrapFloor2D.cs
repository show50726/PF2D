//TrapFloor2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/11
//usage:            a basic trap floor, which you can set path and velocity.

using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
public class TrapFloor2D : Mechanism2D
{
    public TrapFloorStatus status = TrapFloorStatus.OneWay;
    public bool returnWhenTurnOff = true;
    [Tooltip("The start position will be automatic added to the path.")]
    public List<Vector2> Path;
    public float speed = 3f;

    #region In-Script Data
    [ReadOnly]
    public int lastPointOrder;
    [ReadOnly]
    public int theoricalPointOrder;
    public enum TrapFloorStatus { OneWay, GoAndBack, LoopPath }
    private Rigidbody2D rb2d;
    [ReadOnly]
    public Vector2 direction = new Vector2();


    #endregion

    #region Path Decider
    [ReadOnly]
    public bool isReturning = false; //trap floor status
    [ReadOnly]
    public bool needToStop = false; //this only use in OneWay mode. Maybe plus touches something?

    #endregion

    protected override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        Path.Insert(0, transform.position);
        if (Path.Count <= 1)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: path has less than 1 point, thus script won't work.");
            enabled = false;
        }
        if (!Activated)
        {
            needToStop = true;
        }
        lastPointOrder = 0;
        theoricalPointOrder = TheoricalNextPathPoint(0);
        //rb2d.gravityScale = 0;    
    }
    private void Update()
    {
        #region Security Check.
        if ((lastPointOrder < 0 || theoricalPointOrder < 0) && !needToStop && Activated)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: point order get wrong, this might be a bug. Has stoped moving forcibly.");
            needToStop = true;
        }

        #endregion
        if (needToStop) return;
        if (Activated)
        {
            if (JudgeIfDirectionNeedRefresh())
            {
                lastPointOrder = theoricalPointOrder;
                theoricalPointOrder = TheoricalNextPathPoint(lastPointOrder);
                if (theoricalPointOrder == lastPointOrder)
                {
                    //no next point.
                    Stop();
                    return;
                }
                RefreshDirection();
            }
        }
        if (!Activated && returnWhenTurnOff)
        {
            if (JudgeIfDirectionNeedRefresh())
            {
                lastPointOrder = theoricalPointOrder;
                theoricalPointOrder = TheoricalNextPathPoint(lastPointOrder);
                if (theoricalPointOrder == lastPointOrder)
                {
                    //no next point.
                    Stop();
                    return;
                }
                RefreshDirection();
            }
        }
        rb2d.velocity = direction * speed;

    }
    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        if (isTurnOn)
        {
            needToStop = false;
            RefreshDirection();
        }
        else
        {
            //make trap floor return.
            needToStop = true;
            if (returnWhenTurnOff && lastPointOrder == 0)
            {
                isReturning = true;
                theoricalPointOrder = lastPointOrder;
                lastPointOrder = TheoricalNextPathPoint(lastPointOrder);
                RefreshDirection();
            }
        }
    }
    private void RefreshDirection()
    {
        direction = (Path[theoricalPointOrder] - Path[lastPointOrder]).normalized;
    }

    private void Stop()
    {
        rb2d.velocity = Vector2.zero;
        needToStop = true;
    }
    private bool JudgeIfDirectionNeedRefresh()
    {
        Vector2 LastP = Path[lastPointOrder];
        Vector2 TheoP = Path[theoricalPointOrder];
        if (Vector2.Dot((Vector2)transform.position - LastP, TheoP - LastP) / (TheoP - LastP).sqrMagnitude >= 1)
        {
            //if >= 1: too close or gone too far. Need to find next point.
            return true;
        }
        return false;
    }
    private int TheoricalNextPathPoint(int pathPointOrder)
    {
        //TheoricalNextPathPoint() returns order of next theorical path point. 
        switch (status)
        {
            case TrapFloorStatus.OneWay:
                //if OneWay, return stop (in destination) or next path point.
                return pathPointOrder >= Path.Count - 1 ? pathPointOrder : pathPointOrder + 1;
            case TrapFloorStatus.GoAndBack:
                //if GoAndBack, return p+1 on go and p-1 on back.
                if (isReturning)
                {
                    if (pathPointOrder <= 0)
                    {
                        //(Re)start "go" trip.
                        isReturning = false;
                        return 1;
                    }
                    else return pathPointOrder - 1;
                }
                else
                {
                    if (pathPointOrder >= Path.Count - 1)
                    {
                        //Start "back" trip.
                        isReturning = true;
                        return Path.Count - 2;
                    }
                    else return pathPointOrder + 1;
                }
            case TrapFloorStatus.LoopPath:
                //if LoopPath, return p+1 (or 0 when in destination)
                return pathPointOrder >= Path.Count - 1 ? 0 : pathPointOrder + 1;
            default:
                break;
        }
        Debug.LogWarning(GetType().Name + " of " + name + " warning: You shall not see this warning. Check your code");
        return -2;
    }
}
