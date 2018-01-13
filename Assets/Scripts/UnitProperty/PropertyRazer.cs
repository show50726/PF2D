using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyRazer : MonoBehaviour {

	public GameObject endPoint;
	private LineRenderer lazerLine;

	// Use this for initialization
	void Start () {
		lazerLine = GetComponent<LineRenderer> ();
		lazerLine.enabled = true;
	}

	// Update is called once per frame
	void Update () {
		lazerLine.positionCount = 2;
		lazerLine.SetPosition (0, new Vector2(this.transform.position.x, this.transform.position.y - 0.225f));
		lazerLine.SetPosition (1, endPoint.transform.position);

		RaycastHit2D hit = Physics2D.Raycast(new Vector2(this.transform.position.x, (this.transform.position.y - 0.4f)), new Vector2(endPoint.transform.position.x - this.transform.position.x, endPoint.transform.position.y - (this.transform.position.y - 0.376f)), Mathf.Infinity);  
		Debug.Log (hit.collider.name);
		if (hit.collider.tag == "Player") {  
			Player p = hit.collider.gameObject.GetComponent<Player> ();
			//lazerLine.SetPosition(1, p.transform.position);
			p.Death();
		} 
			
	}
}
