//PF (Platformer) Manager made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/10/16
//Usage:            add it to anywhere on scene, it will work with "PF-" scripts.

using System.Collections;
using UnityEngine;

public class PFManager : MonoBehaviour{

    public PF2DFinish[] finish = new PF2DFinish[0];

    #region Security Check.
    
    private void Awake()
    {
        //debug
        //find all objects that tagged "Player", and if doesn't exist Manager should be get a warning?

        if (finish.Length == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: didn't assign finish. The level will never end.");
            return;
        }


    }
    #endregion
    
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

    public void Finish(GameObject player, PF2DFinish finishPlace)
    {
        //this part can be rewritten on propose.

        //a player is finished, do PF-level effect here
        Debug.Log(GetType().Name + ": " + player.name + " has finished the PF level!");
        //freeze
        player.GetComponent<PF2DController>().FreezeControl();

        //judge if all finish is finished...

        foreach (PF2DFinish f in finish)
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
