//PF (Platformer) 2D Harmful Area by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/01/07
//NOTE: 2D only.
//Usage: add it to "Harmful areas", such as a poisoned water, and set it to trigger. This will damage any player inside once a period.
using System.Collections.Generic;
using UnityEngine;

public class PF2D_HarmfulArea : MonoBehaviour {

    public PFManager platformerManager;

    [Header("Damage Setting")]
    public double damage = 1;
    public float period = 3;
    [Tooltip("Show up in system with combination of Player.cs. Can be null.")]
    public string damageReason = null;

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
            p.touchTime += Time.deltaTime;
            if (p.touchTime >= period)
            {
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
            OnTouchingPlayer p = new OnTouchingPlayer(col.gameObject.GetComponent<Player>());
            onTouchingList.Add(p);
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
