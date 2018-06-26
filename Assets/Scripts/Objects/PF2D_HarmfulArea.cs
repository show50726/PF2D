//PF (Platformer) 2D Harmful Area by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/06/26
//NOTE: 2D only. Need colliders (to trigger).
//Usage: add it to "Harmful areas", such as a poisoned water, and set it to trigger. This will damage any player inside once a period.
using System.Collections.Generic;
using UnityEngine;
using CMSR;

[RequireComponent(typeof(Collider2D))]
public class PF2D_HarmfulArea : MonoBehaviour {

    public bool debugMessage = false;
    public PFManager platformerManager;

    [Header("Damage Setting")]
    public float damage = 1;
    public float period = 3;
    [Tooltip("Show up in system with combination of Player.cs. Can be null.")]
    public string damageReason = null;
    public bool onlyHurtCriticalPosition = false;

    private List<OnTouchingLiving> onTouchingList = new List<OnTouchingLiving>();
    
    void Start()
    {
        if (platformerManager == null)
        {
            Debug.Log(GetType().Name + " reporting: looks like " + name + " didn't assign PFManager. Script will try to find one.");
            platformerManager = FindObjectOfType<PFManager>();
            if (platformerManager == null)
            {
                Debug.LogWarning(GetType().Name + " warning: failed to find PFManager in Scene. Unable to work.");
                enabled = false;
            }
        }
    }

    private void Update()
    {
        foreach (OnTouchingLiving targetData in onTouchingList)
        {
            if (targetData.targetLiving.GetType() == typeof(Player))
            {
                Player target = (Player)targetData.targetLiving;
                if (onlyHurtCriticalPosition)
                {
                    if (target.criticalPosition == null)
                    {
                        Debug.LogWarning(GetType().Name + " of " + name + " warning: onlyHurtCriticalPosition but the " + target.gameObject.name + " didn't assign one. It will never get hurt.");
                        continue;
                    }
                    if (GetComponent<Collider2D>().bounds.Contains(target.criticalPosition.position) == false)
                    {
                        //critical position isn't inside. don't hurt it.
                        Debug.Log("Position is " + target.criticalPosition.position + "and NO HURT!");
                        continue;
                    }
                }
                targetData.touchTime += Time.deltaTime;
                if (targetData.touchTime >= period)
                {
                    if (target.LivingJudge() == false)
                    {
                        continue; //already dead.
                    }
                    target.TakeDamage(damage, damageReason);
                    targetData.touchTime = 0;
                }
            }
            else
            {
                targetData.touchTime += Time.deltaTime;
                if (targetData.touchTime >= period)
                {
                    //if (targetData.LivingJudge() == false)
                    //{
                    //    continue; //already dead.
                    //}
                    targetData.targetLiving.Damage(damage);
                    targetData.touchTime = 0;
                }
            }
            
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enabled == false) return;
        //if (platformerManager.JudgePlayer(col.gameObject) == true)
        //{
        //    onTouchingList.Add(new OnTouchingLiving(col.gameObject.GetComponent<Player>()));
        //}
        SLivingStater target = col.gameObject.GetComponent<SLivingStater>();
        if (target != null)
        {
            onTouchingList.Add(new OnTouchingLiving(target));
            if (debugMessage) Debug.Log(col.gameObject.name + "has been added.");
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (enabled == false) return;
        if (platformerManager.JudgePlayer(col.gameObject) == true)
        {
            foreach (OnTouchingLiving p in onTouchingList)
            {
                if (p.targetLiving == col.gameObject.GetComponent<Player>())
                {
                    onTouchingList.Remove(p);
                    return;
                }
            }
        }
    }

}
