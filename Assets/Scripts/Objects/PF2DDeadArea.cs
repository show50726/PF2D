//PF (Platformer) 2D Dead Area made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2017/10/16
//NOTE: 2D only.
//Usage: add it to the "Dead Area" and set it to trigger, it will call funct. from manager once there's a thing droped in. 

using UnityEngine;
using System.Collections;

public class PF2DDeadArea : MonoBehaviour {

    [Tooltip("Show up in system as format \" The player (name) is dead due to 'dead reason'.\". Only tells dead if no dead reason.")] //this is decided by Player.cs
    public string deadReason = null;

    public PFManager platformerManager;

    void Start () {

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

    //if something touches the dead area
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enabled == false) return;

        GameObject objTouched = collision.gameObject;
        //judge if it is a player
        if (platformerManager.JudgePlayer(objTouched) == true)
        {
            Player player = objTouched.GetComponent<Player>();
            player.Death(deadReason);

            //To avoid other players 'survive' by standing on the corpse, change collider to trigger.
            player.SetToTrigger2D();
            //To avoid corpse fly / move to somewhere else, immobilize it.
            player.SetToBeImmobilized2D();

            return;
        }
        
        //else.... clean up?
        Destroy(objTouched);
        
    }

}
