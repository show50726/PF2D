//GameSystemManager made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/10/14
//usage:            this script provides basic feature, such as pause & continue, exit game, etc.
//Suggestion:       put it on an empty gameobject called "System" or "GameSystem".
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameSystemManager : MonoBehaviour
{
    #region Game and Scene Data (Should be updated to new one)

    public bool theSceneIsInGame = true;
    public float afterThisLevelDoneWaitTime = 3f;

    #endregion

    //others, like all variables of below, will ALWAYS inherit from old one.
    
    public int haveCompletedLevels = 0;

    internal float oringinalTimeScale;

    #region "Only-One" Process
    public static GameSystemManager exist;
    //this is used to check whether there's already one manager.
    
    private void Awake()
    {
        //here, do the "only-one" process.
        if(exist == null)
        {
            exist = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(exist != this)
        {
            //update Game and Scene Data
            exist.theSceneIsInGame = theSceneIsInGame;
            exist.afterThisLevelDoneWaitTime = afterThisLevelDoneWaitTime;
            //update Game and Scene Data finished.
            Destroy(gameObject);
        }
        oringinalTimeScale = Time.timeScale;
    }

    #endregion

    private void DetectPressedKeyOrButton()
    {
        //only for debug use
        foreach (KeyCode kcode in Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(kcode))
                Debug.Log("KeyCode down: " + kcode);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (theSceneIsInGame) PauseOrResume();
        }
        //DEV NOTE: debug usage.
        /*
        DetectPressedKeyOrButton();
        */
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 1;
        }
        
    }

    #region Pause Thing

    internal bool isPaused = false;
    internal bool haveOpenedPauseMenu = false;
    public KeyCode pauseKey = KeyCode.Escape;
    
    public void PauseOrResume()
    {
        //this is used by buttons / events.
        TogglePauseAndResume(!isPaused);
    }

    public void TogglePauseAndResume(bool wantPause)
    {

        //DEV NOTE: basically, the pause effect will be done by activating UIManager (which exist in pause menu).

        if (wantPause)
        {
            //pausing
            Debug.Log("Paused.");

            LoadScene("PauseMenu", LoadSceneMode.Additive);
            haveOpenedPauseMenu = true;

            oringinalTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            //resuming
            Debug.Log("Continue...");
            if (haveOpenedPauseMenu)
            {
                SceneManager.UnloadSceneAsync("PauseMenu");
                haveOpenedPauseMenu = false;
            }

            Time.timeScale = oringinalTimeScale;
        }

        isPaused = wantPause;
        
    }

    #endregion
    
    #region In-Script Function
    private void FindOrReply<T>(ref T something, string objName)
    {
        if(GameObject.Find(objName)) something = GameObject.Find(objName).GetComponent<T>();
        if(something == null)
        {
            Debug.LogWarning("Cannot find " + something.GetType().Name + ". This might cause some problem.");
        }
        
    }//DEV NOTE: this function is still in testing
    #endregion
    
    
    public void QuitGame()
    {
        //here do the things before game ends.
        Debug.Log("Exiting the game...");
        Application.Quit();
    }

    public void LevelIsFinished (Scene theLevelFinished)
    {
        StartCoroutine(LoadNewLevelAfterTime(afterThisLevelDoneWaitTime,theLevelFinished));
    }
    private System.Collections.IEnumerator LoadNewLevelAfterTime(float timeWaiting, Scene afterThisLevel)
    {
        //IEnumerator funct. can be only called by writing "StartCoroutine(function);" due to the code attribute.
        yield return new WaitForSeconds(timeWaiting);

        //below do the things
        LoadANewLevelByPurpose(afterThisLevel);
    }

    private void LoadANewLevelByPurpose(Scene afterThisLevel)
    {
        //here, write the "procedure", such as " if level-1 -> go level-2".
        //DEV NOTE: maybe can redesign to a graphical flow chart, such as animator SM?

        switch (afterThisLevel.name)
        {
            case "Level-1":
                LoadLevel("Level-2");
                haveCompletedLevels = 1;
                break;
            /*
            case "Level-2":
                LoadLevel("Level-ex");
                haveCompletedLevels = 2;
                break;
                */
            default:
                LoadLevel("Title");
                break;
        }

    }
    public bool JudgeIfAllowedToPlayThisLevel(string nameOfLevelWantedToLoad)
    {
        //here, write the "judgement", such as " if don't get archivement, not allowed to play this level".
        //DEV NOTE: maybe can redesign to a graphical flow chart, such as animator SM?

        if (haveCompletedLevels < 1 && nameOfLevelWantedToLoad == "Level-2")
        {
            return false;
        }
        return true;
    }
    
    public void LoadLevel(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
        oringinalTimeScale = 1;
        TogglePauseAndResume(false);
    }
    public void LoadScene(string sceneName, LoadSceneMode mode)
    {
        SceneManager.LoadSceneAsync(sceneName,mode);
        
    }


    
}
