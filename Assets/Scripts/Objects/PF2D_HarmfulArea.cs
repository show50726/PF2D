//PF (Platformer) 2D Harmful Area by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/06/26
//NOTE: 2D only. Need colliders (to trigger).
//Usage: add it to "Harmful areas", such as a poisoned water, and set it to trigger. This will damage any player inside once a period.
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PF2D_HarmfulArea : MonoBehaviour {

    public PFManager platformerManager;

    [Header("Damage Setting")]
    public float damage = 1;
    public float period = 3;
    [Tooltip("Show up in system with combination of Player.cs. Can be null.")]
    public string damageReason = null;
    public bool onlyHurtCriticalPosition = false;

    private List<OnTouchingPlayer> onTouchingList = new List<OnTouchingPlayer>();
    
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
        foreach (OnTouchingPlayer p in onTouchingList)
        {
            if (onlyHurtCriticalPosition)
            {
                if (p.player.criticalPosition == null)
                {
                    Debug.LogWarning(GetType().Name + " of " + name + " warning: onlyHurtCriticalPosition but the " + p.player.gameObject.name + " didn't assign one. It will never get hurt.");
                    continue;
                }
                if (GetComponent<Collider2D>().bounds.Contains(p.player.criticalPosition.position) == false)
                {
                    //critical position isn't inside. don't hurt it.
                    Debug.Log("Position is " + p.player.criticalPosition.position + "and NO HURT!");
                    continue;
                }
            }
            p.touchTime += Time.deltaTime;
            if (p.touchTime >= period)
            {
                if (p.player.LivingJudge() == false)
                {
                    continue; //already dead.
                }
                p.player.TakeDamage(damage, damageReason);
                p.touchTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (enabled == false) return;
        if (platformerManager.JudgePlayer(col.gameObject) == true)
        {
            onTouchingList.Add(new OnTouchingPlayer(col.gameObject.GetComponent<Player>()));
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (enabled == false) return;
        if (platformerManager.JudgePlayer(col.gameObject) == true)
        {
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

}
