//Laser Shoot made by STC
//contact: stc.ntu@gmail.com

using UnityEngine;
using System.Collections;

public class LaserShoot : MonoBehaviour
{

    public LineRenderer laserLineRenderer;
    public Transform StartPoint;
    public Transform endPoint;
    public bool UseDirection;
    private float timer_f;
    private float timer_w;
    public float meltingIcePeriod;
    public float DestroyWoodPeriod;

    public string tagOfFloor = "Floor";
    public string tagOfPlayer = "Player";
    

    private void OnEnable()
    {
        laserLineRenderer = GetComponent<LineRenderer>() == null ?
            gameObject.AddComponent<LineRenderer>() : GetComponent<LineRenderer>();
        laserLineRenderer.enabled = true;
    }
    
    // Update is called once per frame
    void Update()
    {

    }

    private Vector3 lastPos, Pos;
    private GameObject lastHittingObj;
    public void ShootLaser(Vector3 StartPosition, Vector3 Direction)
    {

        //Debug.Log("shoot");
        laserLineRenderer.positionCount = 2;
        laserLineRenderer.SetPosition(0, StartPoint.position);
        RaycastHit2D hit;
        if (UseDirection)
            hit = Physics2D.Raycast(StartPosition, Direction, Mathf.Infinity);
        else
        {
            Vector2 l = new Vector2(this.transform.position.x, this.transform.position.y);
            hit = Physics2D.Raycast(StartPosition, new Vector2(endPoint.transform.position.x - l.x, endPoint.transform.position.y - l.y), Mathf.Infinity);
        }
        bool needToReset = false;

        laserLineRenderer.SetPosition(1, hit.point);
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
            if (hitObj.tag == tagOfFloor)
            {

            }
            else if (hitObj.tag == tagOfPlayer)
            {
                PropertyFrosting frosting = hitObj.GetComponent<PropertyFrosting>();
                //PropertyMetal metal = hitObj.GetComponent<PropertyMetal>();
                Player p = hitObj.GetComponent<Player>();
                if (frosting)
                {
                    laserLineRenderer.SetPosition(1, hit.point);
                }
                else
                {
                    p.Death();
                }
            }
            else
            {
                laserLineRenderer.SetPosition(1, hit.point);
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
    protected void ResetEffect()
    {
        timer_f = 0;
        timer_w = 0;
    }

}
