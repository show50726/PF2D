using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyMetal : PlayerProperty2D {
	public GameObject metalGiver;

	protected override void Start()
	{
		base.Start();
		player.Circle.GetComponent<SpriteRenderer> ().color = metalGiver.GetComponent<SpriteRenderer>().color;
	}
}
