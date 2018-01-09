//PF (Platformer) 2D Finish made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/11/22
//NOTICE:           2D only.
//Usage:            add it to the "finish point" objects. It'll work with other "PF-" scripts.
//NOTICE:           must set to be a trigger.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class PF2DFinish : MonoBehaviour {

    //in this example script, the "ending-special-effect" is to make the player floating into air.
    [Tooltip("in this edition,the ending special effect is to make the player floating into air.")]
    public float floatingDist = 3.0f;
    private GameObject playerCatched;
    private bool catched = false;
    private Rigidbody2D playerRb2D;
    private Vector3 floatingPos;
    private int step = 20;

    public PFManager platformerManager;

    internal bool isFinished = false;
    [Tooltip("If it's assigned, the finish can only be seen as finished if specific player comes here.")]
    public GameObject onlyAllowThisPlayer;

    private void Start()
    {
        floatingPos = transform.position + new Vector3(0, floatingDist, 0);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enabled) return;
        if (catched) return; // not allow a second player to finish it.
        GameObject objTouched = collision.gameObject;
        if (!platformerManager.JudgePlayer(objTouched)) return;

        //status/overall change will happen in PFManager.Finish function.
        if (onlyAllowThisPlayer)
        {
            if (objTouched != onlyAllowThisPlayer)
            {
                //not the player allowed, don't do things.
                Debug.Log(name + ": not this player...");
                return;
            }
        }
        isFinished = true;
        platformerManager.Finish(objTouched, this);


        //below, do the special effect.
        if (objTouched.GetComponent<Rigidbody2D>() != null)
        {
            playerCatched = objTouched;
            playerRb2D = playerCatched.GetComponent<Rigidbody2D>();
            playerRb2D.bodyType = RigidbodyType2D.Kinematic;
            catched = true;
        }
    }
    
    private void Update()
    {
        if (!enabled) return;
        #region Floating effect
        //this part do the floating-effect. Can be deleted when don't need.
        if (catched)
        {
            if (step < 20)
            {
                step ++;
            }
            else
            {
                if ((floatingPos - playerCatched.transform.position).magnitude <=0.2)
                {
                    playerCatched.transform.position = floatingPos;
                    playerCatched.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                    catched = false;
                    return;
                }
                playerRb2D.velocity = (float)5.0 * (floatingPos - playerCatched.transform.position);
                step = 0;
            }
        }
        #endregion
        
    }

}
