//DoActionWhenSpeed made by STC
//contact:  stc.ntu@gmail.com
//usage:    do action when speed changed.

using UnityEngine;
using System.Collections;

public class DoActionWhenSpeed : STCMonoBehaviour
{

    [Tooltip("If null, use self.")]
    public GameObject target = null;
    public SpeedTrigger monitorStatus = SpeedTrigger.OnDrop;
    public bool is2D = true;

    private Rigidbody2D targetRb2D;
    private Rigidbody targetRb;
    private Vector3 lastSpeed, nowSpeed;

    // Use this for initialization
    void Start()
    {
        enabled = SetRigidbody();
    }
    private bool SetRigidbody(GameObject target)
    {
        if (is2D)
        {
            targetRb2D = target.GetComponent<Rigidbody2D>();
            if (targetRb2D == null)
            {
                DebugMessage(LogType.Error, "cannot get RigidBody2D of " + target.name + " while it is checked as 2D.");
                return false;
            }
            nowSpeed = targetRb2D.velocity;

            return true;
        }
        else
        {
            targetRb = target.GetComponent<Rigidbody>();
            if (targetRb == null)
            {
                DebugMessage(LogType.Error, "cannot get RigidBody of " + target.name + " while it is checked as 3D.");
                return false;
            }
            return true;
        }
    }
    private bool SetRigidbody()
    {
        if (target!=null)
        {
            return SetRigidbody(target);
        }
        else
        {
            return SetRigidbody(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;
        switch (monitorStatus)
        {
            case SpeedTrigger.OnDrop:

                break;
            case SpeedTrigger.OnHit:
                break;
            default:
                break;
        }
    }
}

public enum SpeedTrigger
{
    OnDrop, OnHit,
}

