//TrapFloor2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/08
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
    private Vector2 direction = new Vector2();


    #endregion

    #region Path Decider
    private bool isReturning = false;
    private bool needToStop = false; //this only use in OneWay mode. Maybe plus touches something?

    #endregion

    private void Start()
    {
        Path.Insert(0, transform.position);
        if (Path.Count <= 1)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: path has less than 1 point, thus script won't work.");
            enabled = false;
        }
        lastPointOrder = 0;
        theoricalPointOrder = TheoricalNextPathPoint(0);
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.bodyType = RigidbodyType2D.Kinematic;
        //rb2d.gravityScale = 0;
        
    }
    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        if (!isTurnOn)
        {
            //make trap floor return.
            if (returnWhenTurnOff) isReturning = true;
        }
    }
    
    private void PathMoving(int NextPointOrder, float speed)
    {
        if (NextPointOrder < 0 || (!Activated && !returnWhenTurnOff))
        {
            //no need to move.
            rb2d.velocity = Vector2.zero;
            return;
        }
        //if (!Activated && !returnWhenTurnOff) { }
        Vector2 NextP = Path[NextPointOrder];
        Vector2 direction = (NextP - (Vector2)transform.position).normalized;
        rb2d.velocity = direction * speed;
    }
    private int FindNextPathPoint()
    {
        if (theoricalPointOrder < 0) return -1;
        //FindNextPathPoint() returns order of next point need to go. 
        Vector2 LastP = Path[lastPointOrder];
        Vector2 TheoP = Path[theoricalPointOrder];
        if (Vector2.Dot((Vector2)transform.position - LastP, TheoP - LastP) / (TheoP - LastP).sqrMagnitude >=0.95)
        {
            //if >=1: go too far. Should find next point.
            lastPointOrder = theoricalPointOrder;
            return TheoricalNextPathPoint(theoricalPointOrder);
        }
        else
        {
            return theoricalPointOrder;
        }
    }
    

    //=====================line of refresh===================================
    private void Update()
    {
        if (needToStop) return;
        if (Activated)
        {
            if (JudgeIfDirectionNeedRefresh())
            {
                lastPointOrder = theoricalPointOrder;
                theoricalPointOrder = TheoricalNextPathPoint(lastPointOrder);
                if (theoricalPointOrder < 0)
                {
                    //no next point.
                    Stop();
                    return;
                }
                direction = Path[theoricalPointOrder] - Path[lastPointOrder];
            }
        }
        if (!Activated && returnWhenTurnOff)
        {
            if (lastPointOrder == 0)
            {
                //has returned.
                Stop();
                return;
            }

        }


    }
    private void Stop()
    {
        needToStop = true;
        rb2d.velocity = Vector2.zero;
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
        //Returns -1 if no need to move. Return -2 when error occurs.
        if (!Activated && isReturning)
        {
            //the trap floor is un-activated and need to return.
            return pathPointOrder - 1;
        }
        switch (status)
        {
            case TrapFloorStatus.OneWay:
                //if OneWay, return stop (in destination) or next path point.
                return pathPointOrder >= Path.Count - 1 ? -1 : pathPointOrder + 1;
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
