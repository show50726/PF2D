//UI Manager        made by STC
//Contact:          stc.ntu@gmail.com
//Last maintained:  2018/07/04
//Usage:            UI Manager is used to 'connect' UI functions (like buttons) to 'Game System' (like GameSystemManager). Assign it to object that contain all UI-things.
//Notice:           Unlike other manager, Multiple UI Manager is ALLOWED.

using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    public UIPauseEffect pauseEffect;
    public UIGameOverEffect gameoverEffect;
    public Text systemMessageContainer;
    public Text levelScoreShower;

    //[ReadOnly]
    [Tooltip("Will be auto updated if GameSystemManager exists. Delete the ReadOnly attribute to allow manually assign.")]
    public Player[] players = new Player[0];

    public Text[] playerScoreShower;

    public UIPointShow2D[] healthPointDisplay = new UIPointShow2D[0];
    public UIPointShow2D[] manaPointDisplay = new UIPointShow2D[0];
    public UIPropertyDisplayer[] propertyDisplay = new UIPropertyDisplayer[0];



    private string systemMessage;

    public int CheckPlayerIndex(Player aPlayer)
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (aPlayer == players[i])
            {
                return i;
            }
        }
        return -1;
    }
    
    #region Security check.
    
    private void Start()
    {
        if(GameSystemManager.exist == null)
        {
            Debug.LogWarning(GetType().Name + " warning: some functions won't work because there's no GameSystemManager.");
            //enabled = false;
            return;
        }
        if (players.Length == 0)
        {
            players = GameSystemManager.exist.GetPlayerList();
        }

        //if (pauseEffect == null) pauseEffect = FindObjectOfType<UIPauseEffect>();
        if(pauseEffect != null) pauseEffect.DoPauseEffect(GameSystemManager.exist.isPaused);
        //do the effect once when UI create (think about the moment calling pause menu)

        if (healthPointDisplay.Length != players.Length)
        {
            Debug.LogWarning(GetType().Name + " warning: the amounts of health point display and players is not the same.");
        }
        if (manaPointDisplay.Length != players.Length)
        {
            //Debug.LogWarning(GetType().Name + " warning: the amounts of mana point display and players is not the same.");
        }
        if (propertyDisplay.Length != players.Length)
        {
            //Debug.LogWarning(GetType().Name + " warning: the amounts of propertyDisplay and players is not the same.");
        }

        if(healthPointDisplay.Length != 0)
        {
            for (int i = 0; i < healthPointDisplay.Length; i++)
            {
                healthPointDisplay[i].SetFullPoint(players[i].healthPointLimit);
                healthPointDisplay[i].UpdatePoint(players[i].healthPoint); //this helps to initialize
            }
        }
        if (manaPointDisplay.Length != 0)
        {
            for (int i = 0; i < manaPointDisplay.Length; i++)
            {
                manaPointDisplay[i].SetFullPoint(players[i].initialMP);
                manaPointDisplay[i].UpdatePoint(players[i].manaPoint); //this helps to initialize
            }
        }
        if (propertyDisplay.Length != 0)
        {
            for (int i = 0; i < propertyDisplay.Length; i++)
            {
                RefreshPropertyDisplay(i);
            }
        }

    }
    #endregion
    
    #region Functions Calling Game System Manager

    private bool CheckGameSystemManager(string action)
    {
        if(GameSystemManager.exist == null)
        {
            Debug.LogError(GetType().Name + " error: unable to " + action + " because there's no GameSystemManager!");
            return false;
        }
        return true;
    }

    public void QuitGameFromUI()
    {
        if(CheckGameSystemManager("quit game from UI")) GameSystemManager.exist.QuitGame();
    }

    public void PauseOrResumeGameFromUI()
    {
        if (CheckGameSystemManager("pause or resume game from UI")) GameSystemManager.exist.PauseOrResume();
    }

    public void RestartLevelFromUI()
    {
        if (CheckGameSystemManager("restart level from UI"))
        {
            GameSystemManager.exist.PauseOrResume();
            LevelManager.exist.RestartLevel();
        }
    }

    public void GoBackToTitleFromUI()
    {
        if (CheckGameSystemManager("go back to title from UI"))
        {
            GameSystemManager.exist.ResetData();
            GameSystemManager.exist.LoadScene("Title");
        }
    }

    public void LoadSceneFromUI(string sceneName)
    {
        if (CheckGameSystemManager("load level from UI") == false) return;
        if (GameSystemManager.exist.JudgeIfAllowedToPlayThisLevel(sceneName)) GameSystemManager.exist.LoadScene(sceneName);
        else ShowSystemMessage("Cannot load " + sceneName + " because you didn't finish the last level!");
    }
    #endregion
    
    public void ShowSystemMessage(string messageWantToShow)
    {
        systemMessage = messageWantToShow;
        if (systemMessageContainer) systemMessageContainer.text = systemMessage;
        else Debug.Log(GetType().Name + ": " + messageWantToShow);
    }

    public void UpdateScore(double score)
    {
        if (levelScoreShower) levelScoreShower.text = "Score: " + score;
        else Debug.Log(GetType().Name + ": score-- " + score);

    }

    public void UpdateHealthPoint(int playerIndex, double healthPoint)
    {
        if (healthPointDisplay.Length == 0)
        {
            Debug.LogWarning(GetType().Name + " warning: cannot update health point because there's no health point display assigned!");
            return;
        }
        if (healthPointDisplay[playerIndex]) healthPointDisplay[playerIndex].UpdatePoint(healthPoint);
        else Debug.LogWarning(GetType().Name + " reporting: the health point display of " + players[playerIndex].name + " is not assigned, thus it can't be displayed.");
    }

    public void RefreshPropertyDisplay(int playerIndex)
    {
        if (propertyDisplay[playerIndex] && players[playerIndex])
        {
            PropertyManager pM = players[playerIndex].GetComponent<PropertyManager>();
            if (pM ==null)
            {
                Debug.LogWarning(GetType().Name + " warning: the assigned player " + players[playerIndex].gameObject.name + " doesn't have PropertyManager, and thus cannot display property. This should not happen.");
                return;
            }
            propertyDisplay[playerIndex].DisplayProperty(pM);
        }
        else
        {
            Debug.LogWarning(GetType().Name + " warnig: cannot find the assigned " + playerIndex + "th player, or the corresponding propertyDisplay. Please check your variable assign.");
        }

    }

    public void DoGameOverEffect()
    {
        if (gameoverEffect) gameoverEffect.DoGameOverEffect();
        else Debug.Log(GetType().Name + ": Gameover!");
    }
    
    public float GetGameOverEffectTimeScale()
    {
        if (gameoverEffect == null) return 1;
        return gameoverEffect.timeScaleWhenGameOver;
    }

    public void CancelGameOverEffect()
    {
        if (gameoverEffect) gameoverEffect.CancelGameOverEffect();
    }

    public void UpdatePlayerScore(double score, int playerIndex)
    {
        if (playerIndex > players.Length - 1)
        {
            Debug.LogWarning(GetType().Name + " warning: someone called UpdatePlayerScore with overhigh playerIndex. Please check UIManager playerIndex setting, or check if any script / button calls with a wrong input.");
            return;
        }
        else if(playerIndex < 0)
        {
            Debug.LogWarning(GetType().Name + " warning: someone called UpdatePlayerScore with negative playerIndex. Please check if any script / button calls with a wrong input.");
            return;
        }
        if (playerScoreShower[playerIndex] != null) playerScoreShower[playerIndex].text = "Score: " + score;
        else Debug.Log(GetType().Name + ": the playerScoreShower[" + playerIndex +  "] is not assigned, thus the score shown cannot be updated.");
    }


}
