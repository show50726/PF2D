//GameSystemManager made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//usage:            this script provides basic feature, such as pause & continue, exit game, etc.
//Suggestion:       put it on an empty gameobject called "System" or "GameSystem".
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameSystemManager : STCMonoBehaviour
{
    #region Game and Scene Data (Should be updated to new one)

    public bool theSceneIsInGame = true;
    public float afterThisLevelDoneWaitTime = 3f;

    #endregion

    //others, like all variables of below, will ALWAYS inherit from old one.
    
    public int haveCompletedLevels = 0;

    internal float oringinalTimeScale;

    public Animator animatorSM;
    
    #region Player Data Inherit/Store
    public static List<Player> playerList = new List<Player>();
    
    public Player[] GetPlayerList()
    {
        Player[] pL = new Player[playerList.Count];
        for (int i = 0; i < playerList.Count; i++) pL[i] = playerList[i];
        return pL;
    }

    public static List<GameObject> playerStaticCopySeries = new List<GameObject>();
    public void SavePlayerDataExp(GameObject playerObj)
    {
        GameObject playerStaticCopy = CheckPlayerDataStoraged(playerObj);
        if (playerStaticCopy == null)
        {
            //didn't save before. Create a static copy.
            //Debug.Log(GetType().Name + ": first time to save " + playerObj.name + ".");
            playerStaticCopy = new GameObject(PlayerDataStorageName(playerObj.name));
            DontDestroyOnLoad(playerStaticCopy);
            playerStaticCopy.SetActive(false);
            playerStaticCopy.transform.SetParent(transform);
            playerStaticCopySeries.Add(playerStaticCopy);
        }
        //else Debug.Log(GetType().Name + ": not the first time to save " + playerObj.name + ".");
        //Data Copy. DEV NOTE: could use interface and function to make a better / simpler code?
        Player d_player = playerStaticCopy.GetComponent<Player>();
        if (d_player == null) d_player = playerStaticCopy.AddComponent<Player>();
        
        Player o_player = playerObj.GetComponent<Player>();
        d_player.CopyData(o_player);
        
        PropertyManager d_propertyManager = playerStaticCopy.GetComponent<PropertyManager>();
        if (d_propertyManager == null) d_propertyManager = playerStaticCopy.AddComponent<PropertyManager>();
        PropertyManager o_propertyManager = playerObj.GetComponent<PropertyManager>();
        UnitProperty[] o_propertyList = o_propertyManager.GetPropertyList();
        if (o_propertyList != null)
        {
            foreach (UnitProperty p in o_propertyList)
            {
                d_propertyManager.ApplyProperty(p);
            }
        }
        else d_propertyManager.ClearProperty();
    }
    private string PlayerDataStorageName(string playerName)
    {
        //Used to save / load player data.
        return playerName + " Static Copy";
    }
    public GameObject CheckPlayerDataStoraged(GameObject playerObj)
    {
        foreach (GameObject pSC in playerStaticCopySeries)
        {
            if (pSC.name == PlayerDataStorageName(playerObj.name))
            {
                return pSC;
            }
        }
        return null;
    }
    public void LoadPlayerDataExp(GameObject playerObj)
    {
        LoadPlayerDataExp(playerObj, playerObj.name);
    }
    public void LoadPlayerDataExp(GameObject playerObj, string loadTargetName)
    {
        GameObject playerStaticCopy = CheckPlayerDataStoraged(playerObj);
        if (playerStaticCopy == null)
        {
            DebugMessage(LogType.Warning, "cannot find saved data called " + loadTargetName + ", " +
                "thus the data of " + playerObj.name + " will remain the same.");
            return;
        }
        DebugMessage(LogType.Normal, playerObj.name + " is going to load data from " + playerStaticCopy.name + ".");

        //Data Inherition. DEV NOTE: could use interface and function to make a better / simpler code?
        Player d_player = playerStaticCopy.GetComponent<Player>();
        Player o_player = playerObj.GetComponent<Player>();
        o_player.CopyData(d_player);

        PropertyManager d_propertyManager = playerStaticCopy.GetComponent<PropertyManager>();
        PropertyManager o_propertyManager = playerObj.GetComponent<PropertyManager>();
        UnitProperty[] d_propertyList = d_propertyManager.GetPropertyList();
        if (d_propertyList !=null)
        {
            foreach (UnitProperty p in d_propertyList)
            {
                o_propertyManager.ApplyProperty(p);
            }
        }
        
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
        Player originalPlayerData = playerList[playerListIndex];
        PropertyManager newPPM = newPlayerData.GetComponent<PropertyManager>();

        //upload the data. 
        //DEV NOTE: can write in a better way? such as player.sync(player);
        newPlayerData.UpdateHealthPoint(originalPlayerData.healthPoint);
        
        //upload the property
        if (newPPM == null)
        {
            Debug.LogWarning(GetType().Name + " warning: a player without PropertyManager want to be registered, which is very strange in this game. Script will automatically add one.");
            newPPM = newPlayerData.gameObject.AddComponent<PropertyManager>();
        }
        UnitProperty[] originalProperty = originalPlayerData.GetComponent<PropertyManager>().GetPropertyList();
        if (originalProperty != null)
        {
            foreach (UnitProperty p in originalProperty) newPPM.ApplyProperty(p);
        }

        //replace the stored one due to system design. Ask your programmer.
        playerList[playerListIndex] = newPlayerData;
        playerList[playerListIndex].ResetState();
        Destroy(originalPlayerData.gameObject);
    }
    public void AddPlayerData(Player player)
    {
        playerList.Add(player);
    }
    public void RemovePlayerData(int playerListIndex)
    {
        //Debug.LogWarning(GetType().Name + ": trying to remove player data. Note that once removed some bugs might occur. Use this function carefully...");
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
    
    protected override void Awake()
    {
        base.Awake();
        //here, do the "only-one" process.
        if(exist == null)
        {
            exist = this;
            Debug.Log(name + "/" + GetType().Name + ": This object will NOT be destroyed on level finished.");
            DontDestroyOnLoad(gameObject);
        }
        else if(exist != this)
        {
            //update Game and Scene Data
            exist.theSceneIsInGame = theSceneIsInGame;
            exist.afterThisLevelDoneWaitTime = afterThisLevelDoneWaitTime;
            //update Game and Scene Data finished. Delete the later one (this)
            Debug.Log(GetType().Name + ": removing from " + name + ". This object might be destroyed on level finished.");
            Destroy(this);
            return;
        }
        oringinalTimeScale = Time.timeScale;
        animatorSM = GetComponent<Animator>();
        if (animatorSM == null)
        {
            //Debug.LogWarning(GetType().Name +" warning: animatorSM + not assigned. ");
        }
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

    public KeyCode keyOfShowPList = KeyCode.P;
    private void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (theSceneIsInGame) PauseOrResume();
            //PauseOrResume();
        }
        //DEV NOTE: debug usage.
        /*
        DetectPressedKeyOrButton();
        */
        if (Input.GetKeyDown(KeyCode.T))
        {
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.R) && isPaused == false && theSceneIsInGame == true)
        {
            if (LevelManager.exist)
            {
                LevelManager.exist.RestartLevel();
            }
            else
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(keyOfShowPList))
        {
            Debug.Log(GetType().Name + ": there's " + playerList.Count + " player(s) assigned in playerList.");
            if (playerList.Count > 0)
            {
                foreach (Player p in playerList)
                {
                    Debug.Log(p.gameObject.name + " is assigned.");
                }
            }
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

    public void ResetData()
    {
        while (playerList.Count != 0)
        {
            Destroy(playerList[0].gameObject);
            playerList.RemoveAt(0);
        }
        while (playerStaticCopySeries.Count != 0)
        {
            Destroy(playerStaticCopySeries[0].gameObject);
            playerStaticCopySeries.RemoveAt(0);
        }
        for (int i = 0; i < WorldStarScore.Length; i++)
        {
            WorldStarScore[i] = 0;
        }

    }

    public void QuitGame()
    {
        //here do the things before game ends.
        Debug.Log("Exiting the game...");
        Application.Quit();
    }
    public void LevelIsFinished(string turnOnThisParameter)
    {
        if (animatorSM!=null)
        {
            animatorSM.SetBool(turnOnThisParameter, true);
        }
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
                LoadScene("Level-2");
                haveCompletedLevels = 1;
                break;
            /*
            case "Level-2":
                LoadScene("Level-ex");
                haveCompletedLevels = 2;
                break;
                */
            case "STC-Test-Area":
                LoadScene("STC-Test-Area-2");
                break;
            case "STC-Test-Area-2":
                LoadScene("STC-Test-Area");
                break;
            default:
                LoadScene("Title");
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
    public void UpdateAnimatorControllerParameterBool(string nameOfControllerParameter)
    {
        AnimatorControllerParameter cp = GameSystemStateMachineState.GetParameter(animatorSM, nameOfControllerParameter);
        if (cp != null) animatorSM.SetBool(cp.name, true);
    }
    
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        oringinalTimeScale = 1;
        TogglePauseAndResume(false);
    }
    public void LoadScene(string sceneName, LoadSceneMode mode)
    {
        SceneManager.LoadScene(sceneName,mode);
        
    }

    [ReadOnly]
    public int[] WorldStarScore = new int[3];

    public void AddWorldStarScore(int numOfWorld)
    {
        //here, World 1 is "1" (in array is 0)
        if (numOfWorld > WorldStarScore.Length)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to add Star Score do a non-existent world array. (Will not work)");
        }
        else if (numOfWorld == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: AddWorldStarScore num counts from 1, not from 0. You might want to score the first world?" );
            numOfWorld++;
        }
        numOfWorld--;
        WorldStarScore[numOfWorld]++;
    }
    public int GetWorldStarScore(int numOfWorld)
    {
        //here, World 1 is "1" (in array is 0)
        if (numOfWorld > WorldStarScore.Length)
        {
            Debug.LogWarning(GetType().Name + " warning: trying to add Star Score do a non-existent world array. (Will not work)");
            return -1000;
        }
        else if (numOfWorld == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: AddWorldStarScore num counts from 1, not from 0. You might want to score the first world?");
            numOfWorld++;
        }
        numOfWorld--;
        return WorldStarScore[numOfWorld];
    }
    
}

