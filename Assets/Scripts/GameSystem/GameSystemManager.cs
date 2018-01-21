﻿//GameSystemManager made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/21
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

    #region Player Data Inherit/Store
    public static System.Collections.Generic.List<Player> playerList = new System.Collections.Generic.List<Player>(); 
    
    public Player[] GetPlayerList()
    {
        Player[] pL = new Player[playerList.Count];
        for (int i = 0; i < playerList.Count; i++) pL[i] = playerList[i];
        return pL;
    }

    public int CheckPlayerIndex(Player player)
    {
        //return -1 when cannot find.
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].gameObject.name == player.gameObject.name)
            {
                return i;
            }
        }
        return -1;
    }
    public void UpdatePlayerData(int playerListIndex, Player newPlayerData)
    {
        //this part can be customized.
        //Updating data in such format: playerList[i].something = newPlayerData.something;
        playerList[playerListIndex].transform.position = newPlayerData.transform.position;

        //NOTE: the useless newPlayerData will be automatically deleted. Check Player.cs
        playerList[playerListIndex].ResetState();
    }
    public void AddPlayerData(Player player)
    {
        playerList.Add(player);
    }
    public void RemovePlayerData(int playerListIndex)
    {
        Debug.LogWarning(GetType().Name + " warning: trying to remove player data. Note that once removed some bugs might occur. Use this function carefully...");
        playerList.RemoveAt(playerListIndex);
    }
    public void RemovePlayerData(Player player)
    {
        int index = CheckPlayerIndex(player);
        if (index < 0)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to remove a player data that has never be saved. This should be a bug and nothing will happen. Check your program flow.");
            return;
        }
        RemovePlayerData(index);
    }
    #endregion



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
            //update Game and Scene Data finished. Delete the later one (this)
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
            case "STC-Test-Area":
                LoadLevel("STC-Test-Area-2");
                break;
            case "STC-Test-Area-2":
                LoadLevel("STC-Test-Area");
                break;
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
