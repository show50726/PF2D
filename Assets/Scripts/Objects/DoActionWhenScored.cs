using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoActionWhenScored : MonoBehaviour {

    [Tooltip("This must count from 1"), Range(1, 99)]
    public int theWorld = 1;
    public int starScoreRequired = 1;

    private int starScoreNow = 0;

    private void Update()
    {
        if (GameSystemManager.exist)
        {
            starScoreNow = GameSystemManager.exist.GetWorldStarScore(theWorld);
            if (starScoreNow >= starScoreRequired)
            {
                DoAction();
            }
        }
    }
    private void DoAction()
    {
        Destroy(gameObject);
    }
    
}
