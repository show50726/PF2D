//LevelReporter made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/30
//Usage:            for level-based project debugging, the script will report specified things if wanted.
//NOTE:             to make sure this will be executed after ALL of initialization, I use a strange "later-start" method writting. See below.

using UnityEngine;

public class LevelReporter : MonoBehaviour
{
    private void Update()
    {
        StartChecking();
        enabled = false;
    }
    private void StartChecking()
    {
        Debug.Log(GetType().Name + " start reporting: ");
        bool gsmExists = GameSystemManager.exist != null;
        if (gsmExists == false) Debug.Log("The level didn't assign GSM.");
        bool lmExists = LevelManager.exist != null;
        if (lmExists == false) Debug.Log("The level didn't assign LM");
        if (gsmExists)
        {
            Player[] pl = GameSystemManager.exist.GetPlayerList();
            Debug.Log("GSM has recorded " + pl.Length + " players.");
            foreach (Player p in pl)
            {
                Debug.Log("One player is " + p.gameObject.name +" standing on " + p.transform.position + ", " +
                    "hp: " + p.healthPoint + ", " +
                    "animator state: " + p.animator.GetCurrentAnimatorStateInfo(0));
                PropertyManager pM = p.GetComponent<PropertyManager>();
                if (pM == null)
                {
                    Debug.Log("This player doesn't have PropertyManager, which is very strange.");
                }
                else
                {
                    //Debug.Log("This player is " + pM.prope);
                }
            }
        }
        if (PFManager.exist == null) Debug.Log("The level didn't assign PFM.");
        Finish[] doors = FindObjectsOfType<Finish>();
        foreach (Finish d in doors)
        {
            Debug.Log("The level has a door named " + d.gameObject.name + " directing to " + d.goToThisScene);
        }


    }


}
