//Level Manager     made by STC
//Contact:          stc.ntu@gmail.com
//Last maintained:  2018/02/11
//Usage:            Level Manager records eveything happened in level, and will call related system to work (think about completing level). Better assign it to an empty gameobject, which contains 'the whole level objects'.

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    #region Preparation: call Level Manager from public
    public static LevelManager exist;
    private void Awake()
    {
        //always set the newest level manager to be current / working one.
        exist = this;
    }
    #endregion

    public UIManager levelUIManager;
    [ReadOnly, Tooltip("Will be auto updated if GameSystemManager exists. Delete the ReadOnly attribute to allow manually assign.")]
    public Player[] players = new Player[0];

    public double levelScore = 0;
    public float restartWaitingTime = 3f;
    public bool stopTimeAfterGameOver = false;

    #region Security check.

    private void Start()
    {
        if (GameSystemManager.exist)
        {
            players = GameSystemManager.exist.GetPlayerList();
        }
        if (players.Length == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: didn't assign players. This might cause some bugs.");
        }
        foreach (Player p in players)
        {
            if (p == null)
            {
                Debug.LogError(GetType().Name + " error: didn't assign all of players. Please check your set of size and elements.");
            }
        }


        //below, do the "UI judge".
        if (levelUIManager == null)
        {
            Debug.LogWarning(GetType().Name + " warning: the level UIManager doesn't assign or work right, please check it otherwise level-UI related function won't work. ");
        }

    }

    #endregion

    #region Level Function

    public void AddScore(double scoreAdded)
    {
        levelScore += scoreAdded;
        if (levelUIManager) levelUIManager.UpdateScore(levelScore);
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].lives >= 0)
            {
                //only alive player will get score.
                players[i].AddScore(scoreAdded);
                if (levelUIManager)
                {
                    levelUIManager.UpdatePlayerScore(players[i].score, i);
                }
            }
        }
    }
    public void RefreshPlayerHPDisplay(Player player)
    {
        if (levelUIManager != null)
        {
            int playerIndex = levelUIManager.CheckPlayerIndex(player);
            if (playerIndex >= 0)
            {
                levelUIManager.UpdateHealthPoint(playerIndex, player.healthPoint);
            }
            else
            {
                Debug.LogWarning(GetType().Name + " warning: the player " + name + " is not registered in "
                    + levelUIManager.GetType().Name + " of " + levelUIManager.name + ". "
                    + "Thus the UI health point update won't work.");
            }
        }
    }

    public void APlayerIsDead(Player player)
    {
        APlayerIsDead(player, "");
    }
    public void APlayerIsDead(Player player, string deadReason)
    {
        //this can be designed proposely.
        //in PF2D-Test edition, this is called on the VERY LAST of player death (after lives --, hp = 0, ...).

        if (deadReason == "")
        {
            UpdateSystemMessage("The player " + player.name + " is dead.");
        }
        else
        {
            UpdateSystemMessage("The player " + player.name + " is dead due to " + deadReason + ".");
        }

        //Update HP / lives info to UI.
        RefreshPlayerHPDisplay(player);

        if (player.lives >= 0)
        {
            //REVIVE!!!!
            StartCoroutine(player.RespawnAfterTime(3f));
            //...and keep playing.
            return;
        }

        //usually, player death will cause gameover / level restart.
        JudgeIfHaveToRestartOrQuitLevel();
    }
    

    public void JudgeIfHaveToRestartOrQuitLevel()
    {
        //this part can be designed proposely

        #region Competitve/Last-try mode: only when all players died, the game will then restart.
        if (players.Length != 0)
        {
            foreach (Player p in players)
            {
                if (p.lives >= 0)
                {
                    //don't need to restart; keep playing.
                    return;
                }
            }
        }
        if (players.Length <= 1) RestartLevel(restartWaitingTime);
        else
        {
            DoGameOverEffect(true);
            if (stopTimeAfterGameOver)
            {
                Time.timeScale = 0;
            }
        }
        #endregion

    }

    public void DoGameOverEffect(bool doIt)
    {
        if (levelUIManager == null)
        {
            Debug.Log(GetType().Name + " reporting: level UI Manager not assigned, so cannot do/cancel game over effect.");
            return;
        }

        if (doIt == true)
        {
            levelUIManager.DoGameOverEffect();
        }
        else
        {
            levelUIManager.CancelGameOverEffect();
        }
    }

    public void RestartLevel()
    {
        //you may write some "change" here, like life --;
        //Do NOT write judge here (like 'does player have another life?') , that's the job of JudgeIfHaveToRestartOrQuitLevel
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        
    }


    public void RestartLevel(float secsWaiting)
    {
        if(levelUIManager)
        {
            DoGameOverEffect(true);
            StartCoroutine(
                RestartLevelAfterTime(secsWaiting * levelUIManager.GetGameOverEffectTimeScale())
                ); //due to UIGameOverEffect slowMotion effect.
        }
        else
        {
            StartCoroutine(RestartLevelAfterTime(secsWaiting));
        }
    }
    private IEnumerator RestartLevelAfterTime(float timeWaiting)
    {
        //IEnumerator funct. can be only called by writing "StartCoroutine(function);" due to the code attribute.
        yield return new WaitForSeconds(timeWaiting);
        DoGameOverEffect(false);
        RestartLevel();
    }

    public void LevelIsFinished()
    {
        LevelIsFinished(null);
    }
    public void LevelIsFinished(GameObject thePlayerFinishedIt)
    {
        //in order to being program-friendly, assign Gameobject instead of Player script.
        if (thePlayerFinishedIt == null)
        {
            UpdateSystemMessage("The level " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + " is done!");
        }
        else
        {
            UpdateSystemMessage("The player " + thePlayerFinishedIt.name + " has done " + UnityEngine.SceneManagement.SceneManager.GetActiveScene().name + "!");
        }
        if (players.Length == 0)
        {
            Debug.LogWarning(GetType().Name +" warning: no Player assigned. Some feature might not run correctly.");
        }
        else
        {
            //string oneGoingDirection = players[0].levelGoingDirectionConditionName;
            string goingScene = players[0].nextScene;
            foreach (Player p in players)
            {
                //if (p.levelGoingDirectionConditionName != oneGoingDirection)
                if (p.nextScene != goingScene)
                {
                    Debug.LogWarning(GetType().Name +  " warning: not all of players are going to the same scene. " +
                        "This should be a bug, and GameSystemManager might load a wrong level.");
                    break;
                }
            }
        }
        if (GameSystemManager.exist)
        {
            /*
            if (players[0].levelGoingDirectionConditionName != "")
            {
                GameSystemManager.exist.LevelIsFinished(players[0].levelGoingDirectionConditionName);
                Debug.Log("Setting parameter " + players[0].levelGoingDirectionConditionName + " to true");
            }
            */
            if (players[0].nextScene != "")
            {
                GameSystemManager.exist.LoadScene(players[0].nextScene);
            }
            else
                GameSystemManager.exist.LevelIsFinished(UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        }
        else
        {
            Debug.Log(GetType().Name + ": the level is finished, but since there's no GameSystemManager, don't know what to do next.");
        }
    }
    
    #endregion

    #region UI Function
    
    public void UpdateSystemMessage(string message)
    {
        if (levelUIManager)
        {
            levelUIManager.ShowSystemMessage(message);
        }
        else
        {
            Debug.Log(GetType().Name + ": system message \"" + message + "\"");
        }
    }

    #endregion
    
}
