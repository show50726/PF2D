//Item_Collectable made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/03/07
//usage:            make an item collectable by players.
//NOTICE:           2D only. Set the collider to trigger.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Item_Collectable : MonoBehaviour {

    [Header("Setting the collectable player")]
    public bool checkPlayerFromGSM = true;

    private List<GameObject> playerList = new List<GameObject>();

    [Header("Setting of Item")]
    public bool destroyWhenCollected = true;
    [Tooltip("This must count from 1"), Range(1,99)]
    public int theWorld = 1;

    private void Start()
    {
        if (GameSystemManager.exist)
        {
            Player[] player = GameSystemManager.exist.GetPlayerList();
            foreach (Player p in player)
            {
                playerList.Add(p.gameObject);
            }
        }
        else
        {
            Debug.LogWarning(GetType().Name + " warning: cannot find GameSystemManager, thus cannot identify the players (the script will NOT work)");
            enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!enabled) return;
        if (!CheckIfIsPlayer(col.gameObject)) return;
        //It's the player collecting this, do the action
        DoAction(col.gameObject);
    }
    private void DoAction(GameObject byThisObj)
    {
        if (GameSystemManager.exist)
        {
            GameSystemManager.exist.AddWorldStarScore(theWorld);
        }
        Destroy(gameObject);
    }


    private bool CheckIfIsPlayer(GameObject obj)
    {
        foreach (GameObject pObj in playerList)
        {
            if (obj == pObj)
            {
                return true;
            }
        }
        return false;
    }



}
