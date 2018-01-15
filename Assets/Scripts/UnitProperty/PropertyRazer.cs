using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyRazer : UnitProperty {

	public GameObject endPoint;
	private LineRenderer lazerLine;
	public float FrozenTime = 1f;
	public float WoodenTime = 1f;
	private float timer_f = 0f;
	private float timer_w = 0f;
	private Vector2 lastPos;
	private Vector2 Pos;

	// Use this for initialization
	protected override void Start () {
		lazerLine = GetComponent<LineRenderer> ();
		lazerLine.enabled = true;
	}

	// Update is called once per frame
	protected void Update () {
		lazerLine.positionCount = 2;
		lazerLine.SetPosition (0, new Vector2(this.transform.position.x, this.transform.position.y - 0.225f));
		lazerLine.SetPosition (1, endPoint.transform.position);

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, (this.transform.position.y - 0.4f)), new Vector2(endPoint.transform.position.x - this.transform.position.x, endPoint.transform.position.y - (this.transform.position.y - 0.4f)), Mathf.Infinity);  
		Debug.Log (hit.collider.name);
		if (hit.collider.tag == "Floor") {
				
		}
		else if (hit.collider.tag == "Player") {
			PropertyFrosting frosting = hit.collider.gameObject.GetComponent<PropertyFrosting>();
			Player p = hit.collider.gameObject.GetComponent<Player> ();
			if (frosting) {
				lazerLine.SetPosition (1, hit.collider.transform.position);
			} 
			else {
				p.Death ();
			}
		} 
		else {
			lazerLine.SetPosition (1, hit.collider.transform.position);
			PropertyFrozen frozen = hit.collider.gameObject.GetComponent<PropertyFrozen>();
			PropertyWooden wooden = hit.collider.gameObject.GetComponent<PropertyWooden>();
			if (frozen) {
				timer_f += Time.deltaTime;
				if (timer_f >= FrozenTime) {
					propertyManager.RemoveProperty (this.GetType ());
					timer_f = 0;
				}
			} 
			else if (wooden) {
				timer_w += Time.deltaTime;
				Pos = hit.collider.gameObject.transform.position;
				if (Pos == lastPos) {
					timer_w += Time.deltaTime;
				}
				if (timer_w >= WoodenTime) {
					Destroy (hit.collider.gameObject);
					timer_f = 0;
				}
				lastPos = Pos;
			}
		}
	}

}
