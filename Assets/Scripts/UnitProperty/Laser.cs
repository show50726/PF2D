using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : PlayerProperty2D {

	public GameObject endPoint;
	private LineRenderer laserLine;
	public float RemoveFrozenPeriod = 1f;
	public float DestroyWoodPeriod = 3f;
	private float timer_f = 0f;
	private float timer_w = 0f;
	private Vector2 lastPos;
	private Vector2 Pos;

	// Use this for initialization
	protected override void Start () {
		laserLine = GetComponent<LineRenderer> ();
		laserLine.enabled = true;
	}

	// Update is called once per frame
	protected void Update () {
		laserLine.positionCount = 2;
		float k = endPoint.transform.position.y > this.transform.position.y ? this.transform.position.y + 0.225f : this.transform.position.y - 0.225f;
		laserLine.SetPosition (0, new Vector2(this.transform.position.x, k));
		laserLine.SetPosition (1, endPoint.transform.position);

		float l = endPoint.transform.position.y > this.transform.position.y ? this.transform.position.y + 0.4f : this.transform.position.y - 0.4f;
		RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, l), new Vector2(endPoint.transform.position.x - this.transform.position.x, endPoint.transform.position.y - l), Mathf.Infinity);  
		//Debug.Log (hit.collider.name);
		if (hit.collider.tag == "Floor") {
				
		}
		else if (hit.collider.tag == "Player") {
			PropertyFrosting frosting = hit.collider.gameObject.GetComponent<PropertyFrosting>();
			//PropertyMetal metal = hit.collider.gameObject.GetComponent<PropertyMetal>();
			Player p = hit.collider.gameObject.GetComponent<Player> ();
			if (frosting) {
				laserLine.SetPosition (1, hit.point);
			} 
			else {
				p.Death ();
			}
		} 
		else {
			laserLine.SetPosition (1, hit.point);
			PropertyFrozen frozen = hit.collider.gameObject.GetComponent<PropertyFrozen>();
			PropertyWooden wooden = hit.collider.gameObject.GetComponent<PropertyWooden>();
			//PropertyMetal metal = hit.collider.gameObject.GetComponent<PropertyMetal>();
			if (frozen) {
				timer_f += Time.deltaTime;
				if (timer_f >= RemoveFrozenPeriod) {
					propertyManager.RemoveProperty (this.GetType ());
					timer_f = 0;
				}
			} 
			else{
				timer_f = 0;
			}
			if (wooden) {
				timer_w += Time.deltaTime;
				Pos = hit.collider.gameObject.transform.position;
				if (Pos == lastPos) {
					timer_w += Time.deltaTime;
				}
				else {
					timer_w = 0;
				}
				if (timer_w >= DestroyWoodPeriod) {
					Destroy (hit.collider.gameObject);
					timer_w = 0;
				}
				lastPos = Pos;
			}
			//if (metal) {
			//	Reflect ();
			//}

		}
	}

	private void Reflect(){

	}

}
