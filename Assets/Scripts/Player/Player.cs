//Player            proudly made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2017/10/16
//Usage:            add it to objects that represent players (remember to set their tags to "Player"), it will provide basic data and function which a 'player' should have.

using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour {

    [Header("Basic Player Data")]
    public double healthPoint = 100;
    public double manaPoint = 100;
    public int lives = 0;
    public double score = 0;

    //inner player data.
    internal Vector3 respawnPos;
    internal double initialHP;
    internal double initialMP;
    internal bool isDead = false;
    internal bool isRespawning = false;
    //QUESTION: making it into ASM?

    private Rigidbody rb;
    private Rigidbody2D rb2D;
    private Collider cld;
    private Collider2D cld2D;
    
    private void OnEnable()
    {
        respawnPos = transform.position;
        initialHP = healthPoint;
        initialMP = manaPoint;
        rb = GetComponent<Rigidbody>();
        rb2D = GetComponent<Rigidbody2D>();
        cld = GetComponent<Collider>();
        cld2D = GetComponent<Collider2D>();

        //debug
        if (tag != "Player")
        {
            Debug.LogWarning("Warning: the object " + name + " have script " + GetType().Name + ".cs" 
                + " (and thus should represents player?) but doesn't have tagged \"Player\". "
                + "It might cause some unpredictable errors.");
            
        }
    }

    private void SystemMessage(string message)
    {
        if (LevelManager.exist)
            LevelManager.exist.UpdateSystemMessage(message);
        else
            Debug.Log(GetType().Name + " reporting: " + message);

    }

    //LivingJudge is here to prevent a "second-death" ex. moving in & out the dead area.
    //return true for still living; return false to be dead.
    //NOTE that die in action (etc. hp goes under 0) is also written here.
    public bool LivingJudge()
    {
        if (isDead)
        {
            //this guy has been dead already.
            return false;
        }
        if (healthPoint <= 0.0)
        {
            //this guy die in action. return that he should be dead now.
            return false;
        }

        //it is alive.
        return true;
    }

    
    #region Death
    //Death funct. should be called when player is dead. Modify this to produce death message / gameover / etc
    //NOTE: the gameover funct. should be written in LevelManager.cs, NOT HERE.
    public void Death()
    {
        Death("");
    }
    public void Death(string deadReason)
    {
        if (isDead == true)
        {
            //player has been dead already, skip the whole procedure (to avoid second-death).
            Debug.Log("This guy " + name + " is already dead. Don't let him die twice.");
            return;
        }
        AfterDeathProcess(deadReason);
    }
    private void AfterDeathProcess(string deadReason)
    {
        //NOTE: AfterDeathProcess should be called ONLY after Death (to avoid bugs).
        //write the death process here, like "life-1", some animation, etc.
        //Suggestion: for status that will change between alive and dead, write them in ToggleAliveOrDeadStatus remind yourself.

        lives--;
        ToggleAliveOrDeadStatus(false);

        //below is LevelManager setting.
        if (LevelManager.exist == null)
        {
            Debug.Log(GetType().Name + " reporting: cannot find LevelManager...maybe you're running a simple test? Due to that guess, script will help you respawn dead player after 3 seconds.");
            StartCoroutine(RespawnAfterTime(3f));
            return;
        }
        LevelManager.exist.APlayerIsDead(this);
        
    }
    #endregion //Death

    private void ToggleAliveOrDeadStatus(bool alive)
    {
        //NOTE: ToggleAliveOrDeadStatus is called in AfterDeathProcess and Respawn. Do NOT use it anywhere else.
        //NOTE: you may add / decrease status vars due to situation.
        //Suggestion: do NOT write "changes" here, such as live-- or update position / velocity. In my logic they are not seen as status, so write them inside AfterDeathProcess and Respawn.

        PF2DController controllerPF2D = GetComponent<PF2DController>();

        isDead = !alive;

        if (alive)
        {
            //the player has come back to live.
            isRespawning = false;
            healthPoint = initialHP;
            manaPoint = initialMP;
            
            if (controllerPF2D) controllerPF2D.Reset();
        }
        else
        {
            //the player should be dead, totally.
            healthPoint = 0;

            if (controllerPF2D) controllerPF2D.Dead();

        }
        UpdateHealthPoint(healthPoint);
    }
    


    #region OTWIR: Operations That Will Involve Respawn / reset.

    //DEV NOTE: this area might be rewritten as PType. 

    private bool haveSetToTrigger2D = false;
    public void SetToTrigger2D()
    {
        if (cld2D && cld2D.isTrigger == false)
        {
            cld2D.isTrigger = true;
            haveSetToTrigger2D = true;
        }
    }

    private bool haveSetToTrigger = false;
    public void SetToTrigger()
    {
        if (cld && cld.isTrigger == false)
        {
            cld.isTrigger = true;
            haveSetToTrigger = true;
        }
    }

    private bool haveSetToBeImmobilized = false;
    public void SetToBeImmobilized()
    {
        rb.velocity = Vector3.zero;
        if (rb.isKinematic == false)
        {
            rb.isKinematic = true;
            haveSetToBeImmobilized = true;
        }
    }

    private bool haveSetToBeImmobilized2D = false;
    public void SetToBeImmobilized2D()
    {
        rb2D.velocity = Vector2.zero;
        if (rb2D.isKinematic == false)
        {
            rb2D.isKinematic = true;
            haveSetToBeImmobilized2D = true;
        }
    }

    private void OTWIRResetToAlive()
    {
        //here, write down how to deal with OTWIR changes if player comes back to live.
        //NOTE: to avoid some bugs, it should be called ONLY by Respawn.
        if (haveSetToTrigger)
        {
            cld.isTrigger = false;
            haveSetToTrigger = false;
        }
        if (haveSetToTrigger2D)
        {
            cld2D.isTrigger = false;
            haveSetToTrigger2D = false;
        }
        if (haveSetToBeImmobilized)
        {
            rb.isKinematic = false;
            haveSetToBeImmobilized = false;
        }
        if (haveSetToBeImmobilized2D)
        {
            rb2D.isKinematic = false;
            haveSetToBeImmobilized2D = false;
        }
    }

    #endregion

    //Respawn should be called when you want to "respawn" a player.
    //NOTE: Respawn is worked as "setting back" the player obj. data, NOT re-spawning a new one.
    public void Respawn()
    {
        //do the respawn action below, like reset position, reset state, start game, etc.
        //NOTE: it respawns player ON INSTANT. For a delayed-respawn, use RespawnAfterTime.
        //NOTE: Do NOT write the death process (like "life-1") here. Write them in AfterDeathProcess.
        //NOTE: keep in mind that you might make different appearance of death (fall out the map / stay on map as corpse...), but there's only ONE appearance to be alive (...usually). So Respawn code should be much longer than AfterDeathProcess, considering EVERY appearance set back to alive one.

        transform.position = respawnPos;
        SystemMessage("Player " + name + " has respawn at position " + transform.position + ". " + lives + " lives left.");

        //Reset for OTWIR functions.
        OTWIRResetToAlive();
        
        //reset velocity to zero in case some bugs occur.
        if (rb) rb.velocity = Vector3.zero;
        if (rb2D) rb2D.velocity = Vector2.zero;

        ToggleAliveOrDeadStatus(true);

    }

    public IEnumerator RespawnAfterTime(float timeWaiting)
    {
        //RespawnAfterTime can be only called by writting "StartCoroutine(RespawnAfterTime(time));" due to the code attribute.
        //NOTE: this will ONLY respawn player instead of resetting whole level. Please consider the situation; and if you want to reset the level, I suggest to write this in LevelManager.
        
        //to avoid overcalled, judge if player is marked as respawning.
        if (!isRespawning)
        {
            isRespawning = true;
            yield return new WaitForSeconds(timeWaiting);
            Respawn();
        }

    }
    
    public void UpdateHealthPoint(double hp)
    {
        healthPoint = hp;
        if (LevelManager.exist)
        {
            LevelManager.exist.RefreshPlayerHPDisplay(this);
        }
        else
        {
            Debug.Log(GetType().Name + ": " + name + " is now " + healthPoint + " HP.");
        }
    }

    public void TakeDamage(double damageTaken, string reason)
    {
        UpdateHealthPoint(healthPoint - damageTaken);
        if (reason== "")    SystemMessage("Player " + name + " is hurt." + "HP is now " + healthPoint);
        else                SystemMessage("Player " + name + " is hurt by " + reason + "." + "HP is now " + healthPoint);
        if (LivingJudge() == false)
        {
            Death();
        }
    }
    public void TakeDamage(double damageTaken)
    {
        TakeDamage(damageTaken, "");
    }

    public void AddScore(double scoreAdded)
    {
        score += scoreAdded;
    }

}
