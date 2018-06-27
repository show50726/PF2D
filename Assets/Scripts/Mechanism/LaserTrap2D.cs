//LaserTrap2D made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/06/27
//usage:            Laser Trap shoots laser and can be treated like a mechanism (such as button).
//NOTE:             Mechanism2D needed.

using UnityEngine;

public class LaserTrap2D : Mechanism2D {

    //protected override void Start()
    //{
    //    if (Activated)
    //    {
    //        //do something
    //    }

    //}
    [Header("Basic Setting")]
    public LineRenderer laserLineRenderer;
    public Transform startPoint;
    public Transform endPoint;
    [Tooltip("When enable this, laser will go infinite far along startPoint -> endPoint.")]
    public bool UseDirection;

    [Header("Special Function")]
    public float meltingIcePeriod;
    private float timer_f;
    public float DestroyWoodPeriod;
    private float timer_w;

    private bool resetTimer = false; //used by timers.

    public string tagOfFloor = "Floor";
    public string tagOfPlayer = "Player";

    protected override void WhenActivate(bool isTurnOn)
    {
        base.WhenActivate(isTurnOn);
        if (laserLineRenderer == null)
            laserLineRenderer = GetComponent<LineRenderer>() == null ? gameObject.AddComponent<LineRenderer>() : GetComponent<LineRenderer>();
        if (isTurnOn)
        {
            //basic draw laser.
            DrawLaser(new Vector3[] { startPoint.position, endPoint.position });
        }
        else
        {
            laserLineRenderer.enabled = false;
        }
    }

    protected override void Start()
    {
        if (startPoint == null) startPoint = transform;
        if (endPoint == null)
        {
            DebugMessage(LogType.Error, "endPoint not assigned. This laser will not work.");
            Activated = false;
            return;
        }
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(Activated) ShootLaser(startPoint.position, endPoint.position - startPoint.position);
        if (resetTimer) ResetTimerCount();
    }
    
    private void DrawLaser(Vector3[] points)
    {
        laserLineRenderer.enabled = true;
        laserLineRenderer.positionCount = points.Length;
        for (int i = 0; i < points.Length; i++)
        {
            laserLineRenderer.SetPosition(i, points[i]);
        }
        if (_showDebugMessage) DebugMessage(LogType.Normal, "line drawn.");
    }

    private Vector3 lastPos, Pos;
    private GameObject lastHittingObj = null;
    public void ShootLaser(Vector3 startPosition, Vector3 direction)
    {
        //Raycast.
        RaycastHit2D hit;
        if (UseDirection)
            hit = Physics2D.Raycast(startPosition, direction);
        else
            hit = Physics2D.Raycast(startPosition, direction, direction.magnitude);
        
        GameObject hitObj = hit.collider.gameObject;
        if (hitObj != lastHittingObj)
        {
            resetTimer = true;
            lastHittingObj = hitObj;
            DrawLaser(new Vector3[] { startPosition, hit.point });
        }
        if (hitObj != null)
        {
            if (hitObj.tag == tagOfPlayer)
            {
                //design of player.
                PropertyFrosting frosting = hitObj.GetComponent<PropertyFrosting>();
                Player p = hitObj.GetComponent<Player>();
                if (frosting == null)
                {
                    if(p) p.Death();
                    else
                    {
                        DebugMessage(LogType.Warning, "hit object " + hitObj.name + " has tag of player but didn't assign Player(Script). This might be a bug and laser will not kill it.");
                    }
                }
            }
            else
            {
                PropertyManager objPropertyManager = hitObj.GetComponent<PropertyManager>();
                //Debug.Log("Have got property manager of " + hitObj.name + ": " + (objPropertyManager!=null));
                if (objPropertyManager != null)
                {
                    PropertyFrozen frozenProperty = objPropertyManager.GetProperty<PropertyFrozen>();
                    if (frozenProperty)
                    {
                        timer_f += Time.deltaTime * 2; //DEV NOTE: the nasty sol. of Unity's broken collision detect.
                        if (frozenProperty.immortalize)
                        {
                            timer_f = 0; //DEV NOTE: the nasty sol. of strange design of Laser.
                        }
                        if (timer_f >= meltingIcePeriod)
                        {
                            frozenProperty.Melt();
                        }
                    }
                    if (objPropertyManager.GetProperty<PropertyWooden>())
                    {
                        timer_w += Time.deltaTime;
                        Pos = hitObj.transform.position;
                        if (Pos == lastPos)
                        {
                            timer_w += Time.deltaTime;
                        }
                        else
                        {
                            timer_w = 0;
                        }
                        if (timer_w >= DestroyWoodPeriod)
                        {
                            Destroy(hitObj);
                            timer_w = 0;
                        }
                        lastPos = Pos;
                    }
                    //if (objPropertyManager.GetProperty<PropertyMetal>())
                    //{
                    //    Reflect();
                    //}
                }
            }
        }
    }
    protected void ResetTimerCount()
    {
        timer_f = 0;
        timer_w = 0;
        resetTimer = false;
    }
}
