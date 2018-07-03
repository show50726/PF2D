using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyMetal : PlayerProperty2D {
    public PropertyMetal()
    {
        showingColor = new Color32(170, 170, 170, 255);
    }

    protected override void Start()
	{
		base.Start();
        player.Circle.GetComponent<SpriteRenderer>().color = showingColor;
    }
}
