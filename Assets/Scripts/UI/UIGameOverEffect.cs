//UI GameOverEffect made by STC
//Contact:          stc.ntu@gmail.com
//Last maintained:  2017/10/15
//Usage:            Do the gameover effect. Work with UI Manager.

using UnityEngine;
using System.Collections;

public class UIGameOverEffect : MonoBehaviour
{

    public GameObject gameoverImageToShow;
    [Tooltip("It will apply to Unity TimeScale when doing effect. Set value < 1 to get slow motion, = 1 for no time effect, > 1 for game speedup.")]
    public float timeScaleWhenGameOver = 0.5f;

    private void Start()
    {
        if (timeScaleWhenGameOver <= 0)
        {
            Debug.LogWarning(GetType().Name + " warning: you have set timeScaleWhenGameOver to 0 / under 0, which might cause error / bug in many occasion. For your safety, script has changed its value to 0.1f. Apply it again if you really wanna see what will happen.");
            //in script debug.
            timeScaleWhenGameOver = 0.1f;
        }
    }

    public void DoGameOverEffect()
    {
        GameOverEffect(true);
    }

    public void CancelGameOverEffect()
    {
        GameOverEffect(false);
    }

    private void GameOverEffect(bool doIt)
    {
        //write down game over effect here. notice to do gameover effect when doIt = true, and cancel it when doIt = false.

        if (gameoverImageToShow != null)
        {
            gameoverImageToShow.SetActive(doIt);
        }

        if (doIt == true)
        {
            Time.timeScale = timeScaleWhenGameOver;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


}
