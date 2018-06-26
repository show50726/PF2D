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
    public GameObject endPoint;
    private LineRenderer laserLine;
    [Tooltip("unit: melting factor(see PropertyFrozen). Set 0 to melt ice immediately.")]
    public float meltingIcePeriod = 1f;
    public float DestroyWoodPeriod = 3f;
    [SerializeField]
    private float timer_f = 0f;
    private float timer_w = 0f;
    private Vector2 lastPos;
    private Vector2 Pos;

    protected override void Start()
    {
        if (meltingIcePeriod < 0)
        {
            Debug.LogError(GetType().Name + " of " + name + " error: meltingIcePeriod is not allowed to set under 0. The script will not work if continues.");
            enabled = false;
            return;
        }
        laserLine = GetComponent<LineRenderer>();
        laserLine.enabled = true;
    }

    protected void ResetEffect()
    {
        timer_f = 0;
        timer_w = 0;
    }
    private GameObject lastHittingObj = null;
    protected void Update()
    {
        laserLine.positionCount = 2;
        Vector2 pos = new Vector2((this.transform.position.x + this.transform.localScale.y * 0.49f * Mathf.Sin(this.transform.eulerAngles.z / 180f * Mathf.PI)), (this.transform.position.y - this.transform.localScale.y * 0.49f * Mathf.Cos(this.transform.eulerAngles.z / 180f * Mathf.PI)));
        //float k = endPoint.transform.position.y > this.transform.position.y ? this.transform.position.y + 0.225f : this.transform.position.y - 0.225f;
        //Debug.Log(Mathf.Cos(this.transform.rotation.z / 180f * Mathf.PI));
        //Debug.Log (this.transform.rotation);
        laserLine.SetPosition(0, pos);
        laserLine.SetPosition(1, endPoint.transform.position);

        Vector2 l = new Vector2((this.transform.position.x + this.transform.localScale.y * 0.501f * Mathf.Sin(this.transform.eulerAngles.z / 180f * Mathf.PI)), (this.transform.position.y - this.transform.localScale.y * 0.501f * Mathf.Cos(this.transform.eulerAngles.z / 180f * Mathf.PI)));
        RaycastHit2D hit = Physics2D.Raycast(l, new Vector2(endPoint.transform.position.x - l.x, endPoint.transform.position.y - l.y), Vector2.Distance(l, endPoint.transform.position));

        bool needToReset = false;
        //Debug.Log (hit.collider.name);
        GameObject hitObj = null;
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

    private void Reflect()
    {

    }
}
