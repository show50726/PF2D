//Laser made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//usage:            Laser shoots a deadly laser. Can be treats as an ablility.
using UnityEngine;
using CMSR;

public class Laser : STCMonoBehaviour
{
    [Header("Basic Setting")]
    public LineRenderer laserLineRenderer;
    public Transform startPoint;
    public Transform endPoint;
    [Tooltip("When enable this, laser will go infinite far along startPoint -> endPoint.")]
    public bool UseDirection;
    public LayerMask ignoreTheseObjects = (1 << 2) | (1 << 5) | (1 << 8) | (1 << 9) | (1 << 12) | (1 << 13); //this format means the Layer 2,5,8,9 are selected.
    public float damageToPlayer = 200;
    public float damageToUnit = 10;

    [Header("Special Function")]
    public float meltingIcePeriod;
    private float timer_f;
    public float DestroyWoodPeriod;
    private float timer_w;

    private bool resetTimer = false; //used by timers.

    public string tagOfFloor = "Floor";
    public string tagOfPlayer = "Player";

    /// <summary>
    /// used to check if need to re-draw line.
    /// </summary>
    private Vector2 lastHit;

    private void OnEnable()
    {
        if (startPoint == null) startPoint = transform;
        if (endPoint == null)
        {
            DebugMessage(LogType.Error, "endPoint not assigned. This laser will not work.");
            enabled = false;
            return;
        }
        if (laserLineRenderer == null)
            laserLineRenderer = GetComponent<LineRenderer>() == null ? gameObject.AddComponent<LineRenderer>() : GetComponent<LineRenderer>();
        //basic draw laser.
        lastHit = endPoint.position;
        DrawLaser(new Vector3[] { startPoint.position, endPoint.position });
    }
    private void OnDisable()
    {
        if (laserLineRenderer == null)
            laserLineRenderer = GetComponent<LineRenderer>() == null ? gameObject.AddComponent<LineRenderer>() : GetComponent<LineRenderer>();
        laserLineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShootLaser(startPoint.position, endPoint.position - startPoint.position);
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
        string pointsString = "";
        foreach (Vector3 p in points)
        {
            pointsString += p + ", ";
        }
        pointsString.Remove(pointsString.Length - 2, 2);
        if (_showDebugMessage) DebugMessage(LogType.Normal, "line drawn. Draw points are " + pointsString);
    }

    /// <summary>
    /// used by checking if wooden box is moved.
    /// </summary>
    private Vector3 objLastPos, objPos;
    private GameObject lastHittingObj = null;
    public void ShootLaser(Vector3 startPosition, Vector3 direction)
    {
        //Raycast.
        RaycastHit2D hit;
        if (UseDirection)
            hit = Physics2D.Raycast(startPosition, direction, Mathf.Infinity, ~ignoreTheseObjects);
        else
            hit = Physics2D.Raycast(startPosition, direction, direction.magnitude, ~ignoreTheseObjects);

        GameObject hitObj = hit.collider != null ? hit.collider.gameObject : null;

        if (hitObj == null) hit.point = startPosition + (UseDirection ? 1000 : 1) * direction;
        if (hitObj == gameObject)
        {
            DebugMessage(LogType.Error, " hit itself. Make sure that startPoint is NOT inside the shooter.");
        }

        if (hitObj != lastHittingObj)
        {
            resetTimer = true;
            lastHittingObj = hitObj;
            DrawLaser(new Vector3[] { startPosition, hit.point });
            if (hitObj) DebugMessage(LogType.Normal, "hit " + hitObj.name);
            else DebugMessage(LogType.Normal, "hit nothing");
        }
        else if (hit.point != lastHit)
        {
            DrawLaser(new Vector3[] { startPosition, hit.point });
            lastHit = hit.point;
        }
        if (hitObj != null)
        {
            PropertyManager objPropertyManager = hitObj.GetComponent<PropertyManager>();
            SUnitStater unit = hitObj.GetComponent<SUnitStater>();

            if (hitObj.tag == tagOfPlayer)
            {
                //design of player.
                PropertyFrosting frosting = hitObj.GetComponent<PropertyFrosting>();
                if (frosting == null)
                {
                    //if (unit.GetType() == typeof(Player))
                    //{
                    //    unit = (Player) unit;
                    //    unit.Death();
                    //}
                    //else
                    //{
                    //    DebugMessage(LogType.Warning, "hit object " + hitObj.name + " has tag of player but didn't assign Player(Script). This might be a bug and laser will not kill it.");
                    //}
                    unit.Damage(damageToPlayer);
                }
            }
            else if (unit != null)
            {
                unit.Damage(damageToUnit);
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
                        objPos = hitObj.transform.position;
                        if (objPos == objLastPos)
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
                        objLastPos = objPos;
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
