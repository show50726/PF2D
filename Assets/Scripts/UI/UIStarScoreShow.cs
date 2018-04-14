//UIStarScoreShow   made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/03/22
//usage:            use this to show GameSystemManager - WorldStarScore.

using UnityEngine;
using UnityEngine.UI;

public class UIStarScoreShow : MonoBehaviour
{
    [Header("Info Setting")]
    [Tooltip("This must count from 1"), Range(1, 99)]
    public int theWorld = 1;
    [Tooltip("At least 1"), Range(1, 99)]
	public int coinAmount = 1;

    [Header("UI Show Setting")]
    public Text scoreText;
    [Tooltip("Score Text & Score Text Mesh don't need to exist at the same time.")]
    public TextMesh scoreTextMesh;
    [Tooltip("The text will be \"World n \" + formerDisplay + score. Modifiy script if you want.")]
    public string formerDisplay = "score: ";

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
        //string showText = "World " + theWorld + " " + formerDisplay + score;
		string showText = new string('○', (score>0)? 0: 1) + new System.String('●', (score>0)? 1: 0);
        if (scoreText != null) scoreText.text = showText;
        if (scoreTextMesh != null) scoreTextMesh.text = showText;
        if (scoreText == null && scoreTextMesh == null)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to update star score while nothing text assigned. You know this will do nothing, right?");
            return;
        }

    }

}
