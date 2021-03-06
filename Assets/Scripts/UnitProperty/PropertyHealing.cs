﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyHealing : PlayerProperty2D {

	private Rigidbody2D rb2d;
    [Header("Healing Setting")]
	public float healingPeriod = 1f;
	public float healingOthersPeriod = 4f;
	private float timer = 0f;

    [Tooltip("unit: second.")]
    public float fastHealWaitingTime = 2f;
    [Tooltip("unit: hp / s.")]
    public float fastHealRate = 40f;
    private float stayTimer = 0f;

	public LayerMask ignoreTheseObjects = (1 << 8);
	public LayerMask ignoreGiver = (1 << 9);
	public bool updateIfExists = true;
	public int maxHealth = 100;
	private List<OnTouchingPlayer> onTouchingList = new List<OnTouchingPlayer>();
    public Color showingColor = new Color32(233, 63, 200, 255);

    // Use this for initialization
    protected override void Start()
	{
		base.Start();
        rb2d = GetComponent<Rigidbody2D>();
        player.Circle.GetComponent<SpriteRenderer>().color = showingColor;
    }

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (!enabled) return;
		if (ignoreTheseObjects == (ignoreTheseObjects | (1 << col.gameObject.layer)) || ignoreGiver ==(ignoreGiver | (1 << col.gameObject.layer))) return;
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
		if (player && timer >= healingPeriod && player.healthPoint < maxHealth) {
            player.UpdateHealthPoint(++player.healthPoint);
			Debug.Log("Player " + player.name + " is healing." + "HP is now " + player.healthPoint);
			timer -= healingPeriod;
		}

		foreach (OnTouchingPlayer p in onTouchingList){
			p.touchTime += Time.deltaTime;
			if (p.touchTime >= healingOthersPeriod && p.player.healthPoint < maxHealth && !p.player.isDead)
			{
                p.player.UpdateHealthPoint(++p.player.healthPoint);
				Debug.Log ("Player " + p.player.name + " is healing." + "HP is now " + p.player.healthPoint);
				p.touchTime = 0;
			}
		}

        if (rb2d.velocity.magnitude <= 0.01)
            stayTimer += Time.deltaTime;
        else
            stayTimer = 0;

        if (stayTimer >= fastHealWaitingTime && player.healthPoint < maxHealth && player.isDead == false)
        {
            //start fast healing
            player.UpdateHealthPoint(player.healthPoint + fastHealRate * Time.deltaTime);
        }
        if (player.healthPoint > maxHealth)
        {
            player.UpdateHealthPoint(maxHealth);
        }

	}

}


