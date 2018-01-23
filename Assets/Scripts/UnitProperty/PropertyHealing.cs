using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyHealing : PlayerProperty2D {

	private Rigidbody2D rb2d;
	public float healingPeriod = 1f;
	public float healingOthersPeriod = 4f;
	private float timer = 0f;
	public LayerMask ignoreTheseObjects = (1 << 9);
	public bool updateIfExists = true;
	public int maxHealth = 100;
	private List<OnTouchingPlayer> onTouchingList = new List<OnTouchingPlayer>();
	public GameObject healingGiver;

	// Use this for initialization
	protected override void Start()
	{
		base.Start();
		player.Circle.GetComponent<SpriteRenderer> ().color = healingGiver.GetComponent<SpriteRenderer>().color;
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (!enabled) return;
		if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer))) return;
		//notice that PropertyBurn has scripted to destroy each other on touch already.
		if(col.gameObject.tag == "Player"){
			onTouchingList.Add (new OnTouchingPlayer (col.gameObject.GetComponent<Player> ()));
		}

	}

	private void OnCollisionExit2D(Collision2D col){
		if (!enabled) return;
		if (col.gameObject.tag == "Player") {
			foreach (OnTouchingPlayer p in onTouchingList)
			{
				if (p.player == col.gameObject.GetComponent<Player>())
				{
					onTouchingList.Remove(p);
					return;
				}
			}
		}
	}
		
	
	// Update is called once per frame

	public void Update(){
		timer += Time.deltaTime;
		if (timer >= healingPeriod && player.healthPoint < maxHealth) {
			player.healthPoint++;
			Debug.Log("Player " + name + " is healing." + "HP is now " + player.healthPoint);
			timer -= healingPeriod;
		}

		foreach (OnTouchingPlayer p in onTouchingList){
			p.touchTime += Time.deltaTime;
			if (p.touchTime >= healingOthersPeriod && p.player.healthPoint < maxHealth)
			{
				p.player.healthPoint++;
				Debug.Log ("Player " + p.player.name + " is healing." + "HP is now " + p.player.healthPoint);
				p.touchTime = 0;
			}
		}
	}

}


