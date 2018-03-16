//UIStarScoreShow   made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/03/16
//usage:            use this to show GameSystemManager - WorldStarScore.

using UnityEngine;
using UnityEngine.UI;

public class UIStarScoreShow : MonoBehaviour
{
    [Header("Info Setting")]
    [Tooltip("This must count from 1"), Range(1, 99)]
    public int theWorld = 1;

    [Header("UI Show Setting")]
    public Text scoreText;
    public string formerDisplay = "World Score: ";

    private int score = 0;

    private void Start()
    {
        if (GameSystemManager.exist == null)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to access star score while no GSM exists. Will be unable to use.");
            enabled = false;
            return;
        }
        score = GameSystemManager.exist.GetWorldStarScore(theWorld);
        UpdateScoreShow();

    }

    public void UpdateScoreShow()
    {
        scoreText.text = formerDisplay + score;
    }

}
