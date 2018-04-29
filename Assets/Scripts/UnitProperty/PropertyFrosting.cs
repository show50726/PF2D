//Property Frosting made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2018/03/21
//Usage:            This is a specified property, which makes player "cool" and freeze other objects.

using UnityEngine;
using System.Collections;

public class PropertyFrosting : PropertyNegative
{
    public Color showingColor = new Color32(94, 228, 240, 255);

    protected override void Start()
	{
		base.Start();
        player.Circle.GetComponent<SpriteRenderer>().color = showingColor;
    }

    public PropertyFrosting()
    {
        damagePeriod = 3f;
    }
    [Header("Freezing setting.")]
    [Tooltip("If not set, will apply as default.")]
    public PropertyFrozen freezePropertySample;
    public bool updateIfExists = true;
    [Tooltip("Objects in these layers won't be diffused.")]
    public LayerMask ignoreTheseObjects = (1 << 8)|(1 << 9); //this format means the Layer 8. 9 are selected.

    [Header("Make water Frozen")]
    public LayerMask water = (1 << 4);
    public GameObject icePrefab;

    private void OnTriggerEnter2D(Collider2D col)
    {

        //creat frosted Ice when walking on water
        if (CheckLayerIsInTheLayerMask(col.gameObject.layer, water))
        {
            if (icePrefab == null)
            {
                Debug.LogError(GetType().Name + " error: trying to instantiate ice while icePrefab not selected.");
                return;
            }
            GameObject ice = Instantiate(icePrefab) as GameObject;
            float iceLocalLength = col.bounds.size.x / ice.transform.lossyScale.x;
            ice.transform.localScale = new Vector3(iceLocalLength, ice.transform.localScale.y);
            ice.transform.position = new Vector3(
                col.transform.position.x,
                col.bounds.ClosestPoint(transform.position).y);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!enabled) return;
		if (CheckLayerIsInTheLayerMask(col.gameObject.layer, ignoreTheseObjects)) return;
        //notice that PropertyBurn has scripted to destroy each other on touch already.
        if (GetProperty<PropertyWooden>(col.gameObject) != null)
        {
            if (freezePropertySample) GivePropertyTo(col.gameObject, freezePropertySample, updateIfExists);
            else GivePropertyTo(col.gameObject, new PropertyFrozen(), updateIfExists);
        }


    }

    private bool CheckLayerIsInTheLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }


}
