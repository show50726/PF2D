//PF (Platformer) 2D Controller made by STC PROUDLY
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/06
//Usage:            Assign it to the "player" object you want to control. It will give you basic control, plus functions working with other "PF-" scripts.
//NOTE:             2D only.
//NOTE(of jump):    Due to the physics of "jump", component rigidbody2D is needed. If no, the script will add one.
//NOTE(of wall jump):To save RAM, wall jump is judged only when entering collider2D, thus a collider2D is needed.

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class PF2DController : MonoBehaviour
{
    #region Control Setting

    #region Move Setting

    [Header("Move Setting")]
    [Tooltip("When disabling this, player will unable to move.")]
    public bool allowMovement = true;
    public KeyCode leftButton = KeyCode.A;
    public KeyCode rightButton = KeyCode.D;
    public float movingSpeed = 10f;
    [Tooltip("Calculated as unit/sec^2. Would be unable to move when set to 0.")]
    public float movingAcceleration = 40f; //DEV NOTE: more smart detection / correction?
    public GameObject SmoothyL;
    public GameObject SmoothyR;

    #endregion
    #region Jump Setting
    [Header("Jump Setting")]

    [Tooltip("When disabling this, player will unable to jump. Notice that you still CAN make jump by script-calling.")]
    public bool allowJump = true;
    public KeyCode jumpButton = KeyCode.W;
    public float initialJumpSpeed = 10f;
    [Tooltip("If set > 0, the jump will take data of it instead of initial jump speed. NOTE: better set a little higher than wanted, ex. want 3 --> set 3.15, due to calculation error and controllability.")]
    public float jumpHeight = 0;
    [Tooltip("Time from jump to falling to same position. If set >0, script will MODIFY the gravity scale to make jump time right.")]
    public float verticalJumpTime = 0;
    public LayerMask cannotJumpOnTheseThing = (1 << 1) | (1 << 2) | (1 << 5); //this format means the Layer 1/2/5 are selected.
    [Tooltip("NOTE: once the Layer are not set to 'Everything', the execution will IGNORE the beyond 'Cannot Jump On These Thing' setting.")]
    public LayerMask onlyJumpOnTheseThing = ~(0);
    [Tooltip("Warning: set this/these OUT OF the player's body. Don't set more than 2, they won't work.")]
    public Transform[] footPosition = new Transform[2]; // 'Foot' is the base of jump action. Only if your player's foot "touches" something, then you can jump.
    [Range(0.001f, 0.1f)]
    [Tooltip("Only when only 1 foot is set, this info will be given to judge.")]
    public float footDepth = 0.05f; // the judgement depth of foot 'touching' thing.

    #endregion
    #region Wall Jump Setting
    [Header("Wall Jump Setting")]

    public bool allowWallJump = true;
    public Transform leftPosition;
    public Transform rightPosition;
    [Range(0.01f, 0.99f)]
    [Tooltip("This is used to detect whether it's in touch with wall. For smaller, the time to jump is")]
    public float sideLength = 0.4f;
    private float sideJudgeRange = 0.01f;
    public LayerMask cannotWallJumpOnTheseThing = (1 << 1) | (1 << 2) | (1 << 5); //this format means the Layer 1/2/5 are selected.
    [Tooltip("NOTE: once the Layer are not set to 'Eveything', the execution will IGNORE the beyond 'Cannot Jump On These Thing' setting.")]
    public LayerMask onlyWallJumpOnTheseThing = ~(0);
    [Range(-89f,89f)]
    [Tooltip("Wall jump works based on jump speed and jump degree. When set to 0, the wall jump will be horizontal.")]
    public float wallJumpDegree = 45f;

    public bool inheritAllSetFromJump = false; //DEV NOTE: still in developing.
    private bool leftIsLeanOnWall = false, rightIsLeanOnWall = false;

    #endregion
    #region Dash Setting
    [Header("Dash Setting")]
    public bool allowDash = true;
    public KeyCode dashButton = KeyCode.LeftShift;
    public float dashSpeedMultiplier = 3f;
    [Tooltip("If this is set > 0, dash speed will use this instead of move speed * multiplier.")]
    public float dashSpeed = 0f;
    [Tooltip("For a spell-like dash action, don't check this; if is expected to use dash along with move, check this.")]
    public bool constantDashing = false;
    [Tooltip("Only works when constant dashing isn't checked. Unit: seconds.")]
    public float dashDuration = 0.1f;
    [Tooltip("Only works when constant dashing isn't checked. Unit: seconds.")]
    public float dashCoolDown = 0.5f;
    public bool isFacingRight = false;
    private float initialMovingSpeed = 0f;
    #endregion
    
    #endregion

    #region Initial Data

    private bool[] initialAllowment = new bool[3]; //DEV NOTE: bad structure. consider enum or specified interface?

    #endregion

    #region State Description

    //for state updating
    //public Animator animStateMachine;
    //public string updateBoolName;
    //private bool animSMConditionChecked = false;

    private bool isFreezed = false; //once isFreezed, the controller will (for player) be unabled to use.
    private bool isDead = false; //only used by "a-player-is-dead" situation. look at Dead / Reset.
    
    private bool IsOnGround
    {
        get
        {
            RaycastHit2D g;
            GameObject hitObj = null;
            Vector3 judgePoint1, judgePoint2;
            if (footPosition.Length == 0)
            {
                Debug.LogWarning(GetType().Name + " warning: didn't set up foot for " + name + ", script will take this as permission to jump.");
                return true;
            }
            else if (footPosition.Length == 1)
            {
                judgePoint1 = footPosition[0].position;
                judgePoint2 = footPosition[0].position + new Vector3(0, -footDepth);
            }
            else
            {
                judgePoint1 = footPosition[0].position;
                judgePoint2 = footPosition[1].position;
            }

            if( ~(onlyJumpOnTheseThing) != 0)
            {
                g = Physics2D.Linecast(judgePoint1, judgePoint2, onlyJumpOnTheseThing);
            }
            else
            {
                g = Physics2D.Linecast(judgePoint1, judgePoint2, ~(cannotJumpOnTheseThing));
            }
            if (g)
            {
                hitObj = g.collider.gameObject;
                //Debug.Log("The foot is detected stand on " + hitObj.name);
                if (hitObj.transform == GetComponentInParent<Transform>())
                {
                    Debug.LogError(GetType().Name + " error on " + name + ": the 'standing judge' find it stand on player itself. Fix it (by moving lower the foot position) otherwise the jump will never work.");
                    return false;
                }
            }
            else
            {
                //Debug.Log("The foot is standing on nothing.");
            }
            return g;
        }
    }
    private bool JudgeIfIsLeanOnWall(bool judgeTheRightSide)
    {
        RaycastHit2D scan;
        GameObject hitObj = null;
        Vector3 judgePoint1, judgePoint2;
        if (leftPosition == null || rightPosition == null)
        {
            return false;
        }

        if (judgeTheRightSide)
        {
            judgePoint1 = rightPosition.position + new Vector3(0, sideJudgeRange);
            judgePoint2 = rightPosition.position + new Vector3(0, -sideJudgeRange);
        }
        else
        {
            judgePoint1 = leftPosition.position + new Vector3(0, sideJudgeRange);
            judgePoint2 = leftPosition.position + new Vector3(0, -sideJudgeRange);
        }

        if (~(onlyJumpOnTheseThing) != 0)
        {
            scan = Physics2D.Linecast(judgePoint1, judgePoint2, onlyWallJumpOnTheseThing);
        }
        else
        {
            scan = Physics2D.Linecast(judgePoint1, judgePoint2, ~(cannotWallJumpOnTheseThing));
        }
        if (scan)
        {
            hitObj = scan.collider.gameObject;
            //Debug.Log("The body is detected lean on " + hitObj.name);
            if (hitObj.transform == GetComponentInParent<Transform>())
            {
                Debug.LogError(GetType().Name + " error on " + name + ": the 'leaning judge' find it lean on itself. Fix it (by moving outside the left/right position) otherwise the wall jump will never work.");
                return false;
            }
        }
        else
        {
            //Debug.Log("The body is leaning on nothing.");
        }
        return scan;
    }

    private bool needToRefreshVelocity = false; //use as ALMOST EVERY action in this script.
    private Rigidbody2D rb;
    private Vector2 forcedSpeed = Vector2.zero;
    #endregion

    //When enabled, accquiring data
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!GetComponent<Player>())
        {
            Debug.Log("Player" + name + "doesn't have Player.cs along with " + GetType().Name + ". "
                + "It seems strange because they're working along, and missing each one might cause errors.");
            return;
        }

        //set of jump.
		if (verticalJumpTime > 0)
        {
			Vector2 v = new Vector2(0f, 0f);
			if (jumpHeight > 0) {
				v.y = 2 * jumpHeight / (-verticalJumpTime * verticalJumpTime / 4);
				Physics2D.gravity = v; //to make jump time correct.
			} else 
			{
				v.y = initialJumpSpeed / ( - verticalJumpTime/2);
				Physics2D.gravity = v;
			} 
        }
        CheckJumpSpeed();

        //set of wall jump.
        if (allowWallJump && (leftPosition == null || rightPosition == null))
        {
            Debug.LogWarning(GetType().Name + " warning: left or right position is not assigned, thus wall jump cannot find judge point and will not work.");
        }
        sideJudgeRange = GetComponent<BoxCollider2D>().size.y / 2.5f; //just a little bit smaller than 1/2, to avoid judging a ground as wall when falling too quickly.

        //Store initial Data
        initialAllowment[0] = allowMovement;
        initialAllowment[1] = allowJump;
        initialAllowment[2] = allowDash;
        initialMovingSpeed = movingSpeed;

        SmoothyR.SetActive(false);
        SmoothyL.SetActive(false);

        //check if animStateMachine have the right bool var.
        /*if (animStateMachine != null)
            foreach (AnimatorControllerParameter para in animStateMachine.parameters)
            {
                if (para.name == updateBoolName) animSMConditionChecked = true;
            }*/

    }

    private Vector2 movingDirection = Vector2.zero;
    private Vector2 lastMovingDirection = Vector2.zero;
    void Update()
    {
        if (isFreezed) return;
        if (isDead) return;
        if (allowDash)
        {
            if (Input.GetKeyDown(dashButton))
            {
                if (constantDashing)
                {
                    needToRefreshVelocity = true;
                    if (dashSpeed > 0) movingSpeed = dashSpeed;
                    else movingSpeed *= dashSpeedMultiplier;
                }
                else DoADash();
            }
            if (Input.GetKeyUp(dashButton) && constantDashing)
            {
                movingSpeed = initialMovingSpeed;
            }
        }
        if (allowMovement)
        {
            lastMovingDirection = movingDirection;
            movingDirection = Vector2.zero;
            CheckMove(rightButton, ref movingDirection, transform.right);
            CheckMove(leftButton, ref movingDirection, -transform.right);
            if (movingDirection != Vector2.zero || lastMovingDirection != Vector2.zero)
            {
                ControlMove(movingDirection);
            }
            /*
            if (rightIsLeanOnWall == false)
            {
                CheckMove(rightButton, ref movingDirection, transform.right);
            }
            if (leftIsLeanOnWall == false)
            {
                CheckMove(leftButton, ref movingDirection, -transform.right);
            }
            */
            //if (movingDirection != lastMovingDirection) needToRefreshVelocity = true;
            //if (movingDirection != Vector2.zero || lastMovingDirection != Vector2.zero)
            //{
            //    needToRefreshVelocity = true;
            //}

        }
        if (needToRefreshVelocity)
        {
            //rb.velocity = new Vector2(movingDirection.x * movingSpeed, rb.velocity.y) + forcedSpeed;
            rb.velocity += forcedSpeed;
            needToRefreshVelocity = false;
        }

        if (allowJump)
        {
            if (Input.GetKeyDown(jumpButton))
            {
                if (IsOnGround)
                {
                    DoJump();
                }
                else if (leftIsLeanOnWall)
                {
                    //false = left
                    DoWallJump(false);
                }
                else if (rightIsLeanOnWall)
                {
                    //true = right
                    DoWallJump(true);
                }
            }
        }
        
    }
    #region For Efficacy
    //Goal: use physics2D.Linecast / getcomponent<>() / ... as less as possible
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        leftIsLeanOnWall = JudgeIfIsLeanOnWall(false);
        rightIsLeanOnWall = JudgeIfIsLeanOnWall(true);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        leftIsLeanOnWall = JudgeIfIsLeanOnWall(false);
        rightIsLeanOnWall = JudgeIfIsLeanOnWall(true);
    }

    #endregion

    private void ControlMove(Vector2 movingDirection)
    {
        //check->change
        //check list:
        //  加速/減速check(同方向尚未滿檔)
        //  自動減速(不動則減速)
        //change:
        //  使用加速度(若超過則修正)

        short accDirection = 0; // 1/-1 go to vel max. 2/-2 go to vel.zero. 0 does nothing.
        //check
        if (movingDirection.x > 0 && rb.velocity.x < movingSpeed)
        {
            accDirection = 1;
        }
        else if (movingDirection.x < 0 && rb.velocity.x > -movingSpeed)
        {
            accDirection = -1;
        }
        else if (movingDirection.x == 0)
        {
            if (rb.velocity.x > 0) accDirection = -2;
            if (rb.velocity.x < 0) accDirection = 2;
        }
        if (movingAcceleration <= 0)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: movingAcceleration set to 0 or negative. This is illegal and will reset movingAcceleration.");
            movingAcceleration = 10f;
        }
        float movingAccPerFrame = movingAcceleration * Time.deltaTime;
        //change
        switch (accDirection)
        {
            case 1:
                rb.velocity += new Vector2 (Mathf.Min(movingSpeed - rb.velocity.x, movingAccPerFrame), 0);
                break;
            case -1:
                rb.velocity -= new Vector2(Mathf.Min(movingSpeed + rb.velocity.x, movingAccPerFrame), 0);
                break;
            case 2:
                rb.velocity -= new Vector2(Mathf.Min(rb.velocity.x, movingAccPerFrame), 0);
                break;
            case -2:
                rb.velocity += new Vector2(Mathf.Min(-rb.velocity.x, movingAccPerFrame), 0);
                break;
            default:
                break;
        }

    }

    public void Reset()
    {
        allowMovement = initialAllowment[0];
        allowJump = initialAllowment[1];
        allowDash = initialAllowment[2];
        isFreezed = false;
        isDead = false;
    }

    public void Dead()
    {
        allowMovement = false;
        allowJump = false;
        allowDash = false;
        isDead = true;
    }
    
    public void DoADash()
    {
        allowMovement = false;
        ToggleGravityScale(false);
        if (dashSpeed > 0) rb.velocity = new Vector2(dashSpeed, 0);
        else rb.velocity = new Vector2(movingSpeed * dashSpeedMultiplier, 0);

        if (isFacingRight == false) rb.velocity = new Vector2(- rb.velocity.x, 0);

        StartCoroutine(StopADash(dashDuration));
    }
    private IEnumerator StopADash(float afterTimeInSeconds)
    {
        yield return new WaitForSeconds(afterTimeInSeconds);
        rb.velocity = new Vector2(0, rb.velocity.y);
        allowMovement = initialAllowment[0];
        ToggleGravityScale(true);
        allowDash = false;
        needToRefreshVelocity = true;
        StartCoroutine(ResumeDashAllowment(dashCoolDown));
    }
    private IEnumerator ResumeDashAllowment(float afterTimeInSeconds)
    {
        yield return new WaitForSeconds(afterTimeInSeconds);
        allowDash = initialAllowment[2];
    }

    private float originalGravityScale = 0;
    public void ToggleGravityScale(bool turnOn)
    {
        if (turnOn)
        {
            rb.gravityScale = originalGravityScale;
        }
        else
        {
            originalGravityScale = rb.gravityScale;
            rb.gravityScale = 0;
        }
    }

    private void CheckMove(KeyCode keyCode, ref Vector2 baseVector, Vector2 directionVector)
    {
        if (Input.GetKey(keyCode))
        {
            baseVector += directionVector;
            isFacingRight = directionVector.x > 0 ? true : false;
            if (isFacingRight)
            {
                SmoothyR.SetActive(true);
                SmoothyL.SetActive(false);
            }
            else {
                SmoothyL.SetActive(true);
                SmoothyR.SetActive(false);
            }
        }

        if (Input.GetKeyUp(keyCode))
        {
            SmoothyR.SetActive(false);
            SmoothyL.SetActive(false);
        }
    }

    private void CheckJumpSpeed()
    {
        if (jumpHeight > 0)
        {
            initialJumpSpeed = Mathf.Sqrt(2 * jumpHeight * -Physics2D.gravity.y * rb.gravityScale); //"physics".
        }
        else if (initialJumpSpeed <= 0)
        {
            Debug.LogWarning(GetType().Name +  " warning: jump speed below than 0. It might cause a bug.");
        }
    }

    public void DoJump()
    {
        CheckJumpSpeed();
        rb.velocity = new Vector2(rb.velocity.x, initialJumpSpeed);
        JumpEffect(true);
    }

    public void DoWallJump(bool isFromRightWall)
    {
        CheckJumpSpeed();
        if (isFromRightWall)
        {
            rb.velocity = VectorOfGivenMagnitudeAndAngle(initialJumpSpeed, 180 - wallJumpDegree);
            isFacingRight = false;
        }
        else
        {
            rb.velocity = VectorOfGivenMagnitudeAndAngle(initialJumpSpeed, wallJumpDegree);
            isFacingRight = true;
        }
        JumpEffect(true); //this means wall jump share the same effect/animation with jump. Can be differed / rewritten if wanted.
    }

    private void JumpEffect(bool turnOn)
    {
        //here, put the effect, like do the animation
        //if (animSMConditionChecked) animStateMachine.SetBool(updateBoolName, true);
    }
    
    public void AddVelocity(Vector2 velocity)
    {
        rb.velocity += velocity;
    }

    public void AddVelocityByForce(Vector2 velocity)
    {
        //this function set the forced speed.
        forcedSpeed  += velocity;
        needToRefreshVelocity = true;
    }
    
    public void FreezeControl()
    {
        isFreezed = true;
    }
    public void UnFreezeControl()
    {
        isFreezed = false;
        needToRefreshVelocity = true;
    }
    public void FreezeControl(float freezeTime)
    {
        isFreezed = true;
        StartCoroutine(UnFreezeControl(freezeTime));
    }
    private IEnumerator UnFreezeControl(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        UnFreezeControl();
    }
    private Vector2 VectorOfGivenMagnitudeAndAngle(float magnitude, float angleInDegree)
    {
        return new Vector2(
            magnitude * Mathf.Cos(angleInDegree * Mathf.Deg2Rad), 
            magnitude * Mathf.Sin(angleInDegree * Mathf.Deg2Rad));
    }

}
