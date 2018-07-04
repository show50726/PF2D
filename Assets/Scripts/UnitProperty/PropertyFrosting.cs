//Property Frosting made by STC, designed by Katian Stoner and WXM.
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//Usage:            This is a specified property, which makes player "cool" and freeze other objects.

using UnityEngine;

public class PropertyFrosting : PropertyNegative
{
    public PropertyFrosting()
    {
        damagePeriod = 3f;
        showingColor = new Color32(94, 228, 240, 255);
    }
    public static System.Collections.Generic.List<GameObject> frozenWater = new System.Collections.Generic.List<GameObject>();

    protected override void Start()
    {
        base.Start();
        player.Circle.GetComponent<SpriteRenderer>().color = showingColor;
    }
    
    [Header("Freezing setting.")]
    [Tooltip("If not set, will apply as default.")]
    public PropertyFrozen freezePropertySample;
    public bool updateIfExists = true;
    [Tooltip("Objects in these layers won't be diffused.")]
    public LayerMask ignoreTheseObjects = (1 << 8) | (1 << 9); //this format means the Layer 8. 9 are selected.

    [Header("Make water Frozen")]
    public LayerMask water = (1 << 4);
    public GameObject icePrefab;
    [Tooltip("This prevents the repeat production of ice.")]
    public float heightTuning = 0.1f;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        //creat frosted Ice when walking on water
        if (CheckLayerIsInTheLayerMask(col.gameObject.layer, water))
        {
            if (frozenWater.Contains(col.gameObject)) return; //already did the ice instantiation.
            if (icePrefab == null)
            {
                Debug.LogError(GetType().Name + " error: trying to instantiate ice while icePrefab not selected.");
                return;
            }
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb)
            {
                rb.velocity -= new Vector2(0, rb.velocity.y);
            }
            transform.position += new Vector3(0, heightTuning * 1.3f); //this help ice effect work normally

            //create an ice
            Vector3 p = new Vector3(
                col.transform.position.x,
                col.bounds.ClosestPoint(transform.position).y + heightTuning);
            GameObject ice = Instantiate(icePrefab, p, Quaternion.Euler(0, 0, 0)) as GameObject;
            float iceLocalLength = col.bounds.size.x / ice.transform.lossyScale.x;
            ice.transform.localScale = new Vector3(iceLocalLength, ice.transform.localScale.y);
            ice.transform.SetParent(col.transform);
            //Debug.Log("An ice " + ice.name + " has been instantiated.");
            //create finished.

            frozenWater.Add(col.gameObject);
        }

        PropertyFrozen f = col.gameObject.GetComponent<PropertyFrozen>();
        if (f != null)
        {
            //Debug.Log("Ready to set stay frozen");
            f.StayFrozen(true);
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
    private void OnCollisionExit2D(Collision2D col)
    {

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        PropertyFrozen f = col.gameObject.GetComponent<PropertyFrozen>();
        if (f != null)
        {
            f.StayFrozen(false);
        }
    }

    private bool CheckLayerIsInTheLayerMask(int layer, LayerMask layerMask)
    {
        return layerMask == (layerMask | (1 << layer));
    }


}
