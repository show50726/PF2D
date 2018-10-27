//Velocity made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/10/27
//Usage:            Modify the speed when situation matches the condition.
//NOTE:             2D only...by now. You can extend it to 3D with ease.

using UnityEngine;

public class VelocityModifier : STCMonoBehaviour {

    private Rigidbody2D _rb;
    private Vector2 lastVel = Vector2.zero;

    [Tooltip("使用這四個點來定位左下、右下、左上、右上")]
    public Transform[] checkPoint = new Transform[4];
    [Tooltip("如果你只有設定一個Check Point, 可以用這個來當作腳的深度")]
    public float footDepth = 0.05f;
    public ModifyVelocityStatus modifyStatus = ModifyVelocityStatus.horizontalStop;
    [Tooltip("如果勾選，會每幀強制判斷。較吃效能，但是較穩定。")]
    public bool strictlyUpdate = false;

    private GameObject standOn;
    private bool IsOnGround
    {
        get
        {
            RaycastHit2D g;
            GameObject hitObj = null;
            Vector3 judgePoint1, judgePoint2;
            if (checkPoint.Length == 0)
            {
                DebugMessage(LogType.Error, "didn't set up enough point.");
                return true;
            }
            else if (checkPoint.Length == 1)
            {
                judgePoint1 = checkPoint[0].position;
                judgePoint2 = checkPoint[0].position + new Vector3(0, -footDepth);
            }
            else
            {
                judgePoint1 = checkPoint[0].position;
                judgePoint2 = checkPoint[1].position;
            }
            g = Physics2D.Linecast(judgePoint1, judgePoint2);
            if (g)
            {
                hitObj = g.collider.gameObject;
                //Debug.Log(debugTag + ": The foot is detected stand on " + hitObj.name);
                if (hitObj.transform == GetComponentInParent<Transform>())
                {
                    DebugMessage(LogType.Error, "the 'standing judge' find it stand on object itself. Fix it (by moving lower the check point) otherwise the justification will never work.");
                    return false;
                }
                standOn = hitObj;
                //hit ground. Not jumping / in mid air anymore.
            }
            else
            {
                //Debug.Log(debugTag + ": The foot is standing on nothing.");
                standOn = null;
            }
            return g;
        }
    }
    //Initialization
	void Start () {
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            DebugMessage(LogType.Error, "cannot find Rigidbody2D.");
            enabled = false;
            return;
        }
        lastVel = _rb.velocity;
	}

    private void OnVelocityChange()
    {
        if (_rb.velocity != lastVel && !IsOnGround) {
            ModifyVelocity();
        }
        lastVel = _rb.velocity;
    }
    private void ModifyVelocity()
    {
        switch (modifyStatus)
        {
            case ModifyVelocityStatus.horizontalStop:
                DebugMessage(LogType.Normal, "change x speed to zero.");
                _rb.velocity -= new Vector2(_rb.velocity.x, 0);
                break;
            default:
                break;
        }

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        OnVelocityChange();
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        OnVelocityChange();
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        OnVelocityChange();
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        OnVelocityChange();
    }

    // Update is called once per frame
    void Update () {
        if(strictlyUpdate) ModifyVelocity();
	}
}

public enum ModifyVelocityStatus
{
    horizontalStop, verticalStop, horizontalSlower, verticalSlower
}