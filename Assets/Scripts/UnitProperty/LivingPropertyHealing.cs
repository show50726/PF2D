//Living Property Healing by STC
//contact:          stc.ntu@gmail.com
//last maintain:    2018/06/27
//usage:            put it along with unit stater, this will do the healing effect.

using System.Collections.Generic;
using UnityEngine;
using CMSR;

public class LivingPropertyHealing : LivingProperty2D {

    [Header("Basic Setting.")]
    private Rigidbody2D rb2d;
    public Color showingColor = new Color32(233, 63, 200, 255);

    [Header("Healing Setting")]
    private bool allowHealing = true; //used by fast healing.
    [Tooltip("Unit: HP per Second. Will be smoothing and not guarantee that healing is always integer.")]
	public float healingRate = 1f;

    [Header("Fast Healing Setting")]
    [Tooltip("If you want to heal faster when not disturbed in a fine time, enable fast-healing.")]
    public bool enableFastHealing = false;
    [Tooltip("If true, normal healing effect will be closed while fast-healing.")]
    public bool TurnOffNormalHealing = false;
    [Tooltip("Unit: second.")]
    public float fastHealWarmUp = 2f;
    [Tooltip("Consider vel's \"magnitude.\" If velocity go over this, timer will reset.")]
    public float allowVelocity = 0.1f;
    [Tooltip("Unit: HP per Second. Will be smoothing and not guarantee that healing is always integer.")]
    public float fastHealRate = 40f;
    private float stayTimer = 0;

    public bool JudgeFastHealing()
    {
        if (!enableFastHealing) return false;
        if (rb2d.velocity.magnitude > allowVelocity)
        {
            stayTimer = 0;
            return false;
        }
        if (stayTimer < fastHealWarmUp)
        {
            return false;
        }
        return true;
    }


    [Header("Spread Healing")]
    [Tooltip("If set to true, it can heal other nearby objects.")]
    public bool enableHealingNearby = false;
    public LayerMask ignoreTheseObjects = (1 << 8);
	[Tooltip("Primary than layer check.")]
    public string onlyHealThisTag = "";
    [Tooltip("Unit second. Only heal objects when they stay nearby over this time.")]
    public float healingNearbyWarmUp = 2;
    [Tooltip("Unit: hp per Second. Will be smoothing and not guarantee that healing is always integer.")]
    public float healingNearbyRate = 4f;
    private List<OnTouchingLiving> onTouchingList = new List<OnTouchingLiving>();
    
    /// <summary>
    /// Used to determine whether to heal (nearby object).
    /// </summary>
    /// <param name="targetObj">The one to determine whether to heal.</param>
    /// <returns></returns>
    private bool DetectHealingThisAvailable(GameObject targetObj)
    {
        if (!enableHealingNearby) return false;
        if (onlyHealThisTag != "" && targetObj.tag == onlyHealThisTag) return true;
        //check if layer is correct.
        if (ignoreTheseObjects == (ignoreTheseObjects | (1 << targetObj.layer))) return false;
        //only heal when there's living stater.
        return targetObj.GetComponent<SLivingStater>() != null;
    }

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (!enabled) return;
		if(DetectHealingThisAvailable(col.gameObject)){
			onTouchingList.Add (new OnTouchingLiving (col.gameObject.GetComponent<SLivingStater> ()));
		}
	}

	private void OnCollisionExit2D(Collision2D col){
		if (!enabled) return;
        if (DetectHealingThisAvailable(col.gameObject))
        {
            foreach (OnTouchingLiving p in onTouchingList)
            {
                if (p.targetLiving == col.gameObject.GetComponent<SLivingStater>())
                {
                    onTouchingList.Remove(p);
                    break;
                }
            }
        }
	}

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame

    public void Update(){

        //do the healing
        if(allowHealing) stater.Heal(healingRate * Time.deltaTime);
        if(debugMessage) Debug.Log("Unit " + stater.name + " is healing." + "HP is now " + stater.healthPoint);

        //fast healing
        stayTimer += Time.deltaTime;
        if (JudgeFastHealing())
        {
            stater.Heal(fastHealRate * Time.deltaTime);
            if (debugMessage) Debug.Log("Unit " + stater.name + " is fast healing." + "HP is now " + stater.healthPoint);
        }
        allowHealing = !(JudgeFastHealing() && TurnOffNormalHealing);

        //do the nearby healing
        if (enableHealingNearby)
        {
            foreach (OnTouchingLiving p in onTouchingList)
            {
                p.touchTime += Time.deltaTime;
                if (p.touchTime >= healingNearbyWarmUp)
                {
                    p.targetLiving.Heal(healingNearbyRate * Time.deltaTime);
                    if (debugMessage) Debug.Log(p.targetLiving.gameObject.name + " is healed by " + name + " in a rate of " + healingNearbyRate);
                }
            }
        }



	}

}


