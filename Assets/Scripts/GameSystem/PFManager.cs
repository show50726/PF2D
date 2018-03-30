//PF (Platformer) Manager made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/03/30
//Usage:            add it to anywhere on scene, it will work with "PF-" scripts.

using System.Collections.Generic;
using UnityEngine;

public class PFManager : MonoBehaviour{

    public static PFManager exist;

    public Finish[] finish = new Finish[0];
    [Tooltip("These objects should be in specified state(in animator), otherwise won't be seen as finished. Remain nothing if you don't want to check.")]
    public List<GameObject> checkTheseState = new List<GameObject>();
    [Tooltip("Won't check if left empty.")]
    public string assignedState = "InFinish";
    [Tooltip("Don't modifiy this until you know what you're doing.")]
    public int checkLayer = 0;
    private int assignedStateHash;

    private void Start()
    {
        exist = this;
        if (checkTheseState.Count == 0)
        {
            Debug.Log(GetType().Name +": checkTheseState not assigned. Will inherit ones from players.");
            Player[] l = GameSystemManager.exist.GetPlayerList();
            
            if(l!=null)
                foreach (Player p in l)
                    if (p!= null && p.gameObject != null)
                        checkTheseState.Add(p.gameObject);
            
        }
        foreach (GameObject obj in checkTheseState)
            if(obj == null)
                Debug.LogError(GetType().Name +" error: didn't assign all of checkTheseState!");
    }

    #region PF function

    //judge if it's the player
    public bool JudgePlayer(GameObject obj)
    {
        //due to program-friendly, only check if it's tagged player.
        if (obj.tag == "Player")
        {
            if (obj.GetComponent<Player>() == null)
            {
                Debug.LogWarning(GetType().Name + " warning: " + obj.name + " didn't assign a Player script but tagged Player. It might cause some problem. However, system will try seeing it as player." );
            }
            return true;
        }
        else return false;

    }
    public bool JudgePlayerData(Player pfPlayerData)
    {
        //use this to judge if the pfPlayerData exists.
        if (pfPlayerData == null)
        {
            Debug.LogWarning(GetType().Name + " warning: " + pfPlayerData.name + " doesn't have a Player script but still being required to have one. This might be a bug.");
            return false;
        }
        return true;
    }

    public void APlayerIsFinished(GameObject player)
    {
        if (checkTheseState.Count == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: didn't assign checkTheseState so part of functions won't work.");
            return;
        }
        if (checkTheseState.Contains(player) == false)
        {
            Debug.LogWarning(GetType().Name + " warning: a non-assigned object declines that it has finished.");
            return;
        }
        if (assignedState == "")
        {
            Debug.LogWarning(GetType().Name + " warning: checking state with assignedState not-assigned. Did you forget to key in that?");
            return;
        }
        foreach (GameObject obj in checkTheseState)
        {
            Animator anim = obj.GetComponent<Animator>();
            if (anim == null)
            {
                Debug.LogWarning(GetType().Name + " warning: checkTheseState list contains one that doesn't has animator. Did you forget to assign it?");
                return;
            }
            assignedStateHash = Animator.StringToHash(anim.GetLayerName(checkLayer) + "." + assignedState);
            if (anim.GetCurrentAnimatorStateInfo(checkLayer).fullPathHash != assignedStateHash)
            {
                Debug.Log(GetType().Name + ": " + obj.name +" isn't in assignedState so the level still not finished.");
                return;
            }
        }
        //if code can come here, that means all of the check has passed.
        if (LevelManager.exist)
        {
            LevelManager.exist.LevelIsFinished();
        }
        else
        {
            Debug.Log(GetType().Name + ": level is finished. But due to LevelManager isn't assigned, the game don't know what to do next.");
        }
    }

    public void Finish(GameObject player, Finish finishPlace)
    {
        //this part can be rewritten on propose.
        if (finish.Length == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: didn't assign finish so part of functions won't work.");
            return;
        }
        //a player is finished, do PF-level effect here
        Debug.Log(GetType().Name + ": " + player.name + " has finished the PF level!");
        //freeze
        player.GetComponent<PF2DController>().FreezeControl();

        //judge if all finish is finished...

        foreach (Finish f in finish)
        {
            if (f.isFinished == false)
            {
                //some finish isn't finished, return.
                return;
            }

        }

        //if all finish is finished....

        if (LevelManager.exist)
        {
            LevelManager.exist.LevelIsFinished(player);
        }
    }
    

    #endregion //PF function

}
