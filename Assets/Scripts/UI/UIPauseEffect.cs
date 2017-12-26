//UI Pause Effect   made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/10/14
//usage:            this shall be called when pause/resume happens. Write pause effects here (ex. text change between "pause" and "continue", show up a transparent black screen).

using UnityEngine;
using System.Collections;

public class UIPauseEffect : MonoBehaviour
{

    public GameObject buttonPauseAndResume;

    private bool workable = true;
    
    private void OnEnable()
    {
        if(!buttonPauseAndResume)
        {
            buttonPauseAndResume = GameObject.Find("Button_pause");
            if (!buttonPauseAndResume)
            {
                Debug.LogWarning(GetType().Name + " cannot find " + " the \"Button_Pause\" to work on.");
            }
        }
        
    }


    public void DoPauseEffect(bool isPaused)
    {
        //will be called BEFORE GameSystemManager.isPaused(bool) changed. (i.e. do pause effect when isPaused == false )
        if (!workable) return;
        UnityEngine.UI.Text uiText = buttonPauseAndResume.GetComponentInChildren<UnityEngine.UI.Text>();
        bool onPausing = !isPaused;
        if(uiText != null)
        {
            ChangeText(uiText, onPausing);
        }
        
    }

    public void ChangeText(UnityEngine.UI.Text uiText , bool isPaused)
    {
        //this will change text if there's a text component.
        uiText.text = isPaused ? "Resume" : "Pause";
        
    }



}
