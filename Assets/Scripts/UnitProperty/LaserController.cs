using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {
    public KeyCode keyOfShooting = KeyCode.Space;
    public KeyCode keyOfStopShooting = KeyCode.S;
    public Transform StartPoint;
    public Vector2 Direction;
    public float Duration = 3f;
    private LineRenderer laserLine;
    public float meltingIcePeriod = 1f;
    public float DestroyWoodPeriod = 3f;
    private float timer_f = 0f;
    private float timer_w = 0f;
    private float timer = 0f;
    private Vector2 lastPos;
    private Vector2 Pos;
    public bool UseDirection = true;
    public Transform endPoint;
    // Use this for initialization
    void Start () {
        if (meltingIcePeriod < 0)
        {
            Debug.LogError(GetType().Name + " of " + name + " error: meltingIcePeriod is not allowed to set under 0. The script will not work if continues.");
            enabled = false;
            return;
        }
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(keyOfShooting))
        {
            timer += Time.deltaTime;
        }
        if (timer < Duration && timer > 0)
        {
            ShootLaser(StartPoint.position, Direction);
            timer += Time.deltaTime;
            if (Input.GetKeyDown(keyOfStopShooting))
            {
                StopShooting();
                timer = 0;
            }
        }
        else if(timer >= Duration)
        {
            StopShooting();
            timer = 0;
        }
        
	}

    protected void ResetEffect()
    {
        timer_f = 0;
        timer_w = 0;
    }
    private GameObject lastHittingObj = null;
    public void ShootLaser(Vector3 StartPosition, Vector3 Direction)
    {

        //Debug.Log("shoot");
        laserLine.positionCount = 2;
        laserLine.SetPosition(0, StartPoint.position);
        RaycastHit2D hit;
        if (UseDirection)
            hit = Physics2D.Raycast(StartPosition, Direction, Mathf.Infinity);
        else
        {
            Vector2 l = new Vector2(this.transform.position.x , this.transform.position.y);
            hit = Physics2D.Raycast(StartPosition, new Vector2(endPoint.transform.position.x - l.x, endPoint.transform.position.y - l.y), Mathf.Infinity);
        }
        bool needToReset = false;

        laserLine.SetPosition(1, hit.point);
        GameObject hitObj = null;
        //Debug.Log (hit.collider.name);
        if (hit.collider != null)
            hitObj = hit.collider.gameObject;
        if (hitObj != lastHittingObj || hitObj == null)
        {
                needToReset = true;
                lastHittingObj = hitObj;
        }
        if (hitObj != null)
        {
            if (hitObj.tag == "Floor")
            {

            }
            else if (hitObj.tag == "Player")
            {
                PropertyFrosting frosting = hitObj.GetComponent<PropertyFrosting>();
                //PropertyMetal metal = hitObj.GetComponent<PropertyMetal>();
                Player p = hitObj.GetComponent<Player>();
                if (frosting)
                {
                    laserLine.SetPosition(1, hit.point);
                }
                else
                {
                    p.Death();
                }
            }
            else
            {
                laserLine.SetPosition(1, hit.point);
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
                            timer_f = 0; //DEV NOTE: the nasty sol. of strange setting of Laser.
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
        if (needToReset)
        {
            ResetEffect();
        }

    }

    public void StopShooting()
    {
        Debug.Log("Stop");
        laserLine.positionCount = 1;
    }


}
