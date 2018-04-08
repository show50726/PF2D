using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	public GameObject endPoint;
	private LineRenderer laserLine;
	public float RemoveFrozenPeriod = 1f;
	public float DestroyWoodPeriod = 3f;
	private float timer_f = 0f;
	private float timer_w = 0f;
	private Vector2 lastPos;
	private Vector2 Pos;

	// Use this for initialization
	protected void Start () {
		laserLine = GetComponent<LineRenderer> ();
		laserLine.enabled = true;
	}

	// Update is called once per frame
	protected void Update () {
		laserLine.positionCount = 2;
		Vector2 pos = new Vector2 ((this.transform.position.x + this.transform.localScale.y * 0.49f * Mathf.Sin(this.transform.eulerAngles.z / 180f * Mathf.PI)), (this.transform.position.y - this.transform.localScale.y * 0.49f * Mathf.Cos(this.transform.eulerAngles.z / 180f * Mathf.PI)));
		//float k = endPoint.transform.position.y > this.transform.position.y ? this.transform.position.y + 0.225f : this.transform.position.y - 0.225f;
		//Debug.Log(Mathf.Cos(this.transform.rotation.z / 180f * Mathf.PI));
		//Debug.Log (this.transform.rotation);
		laserLine.SetPosition (0, pos);
		laserLine.SetPosition (1, endPoint.transform.position);

		Vector2 l = new Vector2 ((this.transform.position.x + this.transform.localScale.y * 0.501f * Mathf.Sin(this.transform.eulerAngles.z / 180f * Mathf.PI)), (this.transform.position.y - this.transform.localScale.y * 0.501f * Mathf.Cos(this.transform.eulerAngles.z / 180f * Mathf.PI)));
		RaycastHit2D hit = Physics2D.Raycast(l, new Vector2(endPoint.transform.position.x - l.x, endPoint.transform.position.y - l.y), Mathf.Infinity);
        //Debug.Log (hit.collider.name);
        GameObject hitObj = hit.collider.gameObject;
		if (hitObj.tag == "Floor") {
				
		}
		else if (hitObj.tag == "Player") {
			PropertyFrosting frosting = hitObj.GetComponent<PropertyFrosting>();
			//PropertyMetal metal = hitObj.GetComponent<PropertyMetal>();
			Player p = hitObj.GetComponent<Player> ();
			if (frosting) {
				laserLine.SetPosition (1, hit.point);
			} 
			else {
				p.Death ();
			}
		} 
		else {
			laserLine.SetPosition (1, hit.point);
			//PropertyFrozen frozen = hitObj.GetComponent<PropertyFrozen>();
			//PropertyWooden wooden = hitObj.GetComponent<PropertyWooden>();
            //PropertyMetal metal = hitObj.GetComponent<PropertyMetal>();
            PropertyManager objPropertyManager = hitObj.GetComponent<PropertyManager>();
            if (objPropertyManager != null)
            {
                if (objPropertyManager.GetProperty<PropertyFrozen>())
                {
                    timer_f += Time.deltaTime;
                    if (timer_f >= RemoveFrozenPeriod)
                    {
                        objPropertyManager.RemoveProperty<PropertyFrozen>();
                        timer_f = 0;
                    }
                }
                else
                {
                    timer_f = 0;
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
                //if (metal) {
                //	Reflect ();
                //}
            }
		}
	}

	private void Reflect(){

	}

}
