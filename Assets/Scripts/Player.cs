using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // The ID number for this player. Player 1, Player 2, etc.
    private int playerNum = 0;
    private Uprooter playerController;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject actIndicFill, actIndicOutline;

    private enum Direction
    {
        NONE,
        LEFT,
        RIGHT,
        UP,
        DOWN
    }
    private float dirThreshold = 0.70f;
    private Direction prevDir = Direction.NONE;
    private float actionTimer = 0;
    private float actionTimeMax = 0;
    private bool executingAction = false;

    private Vector2 moveVec;

    public void OnMove(InputAction.CallbackContext ctx) => moveVec = ctx.ReadValue<Vector2>();

    private Cell occupiedCell;
    [SerializeField] private float actRootToRoot, actRootConnectRoot, actRootToEmpty, actRootToEnemy, actRootToRock;

    private void Awake()
    {
        playerController = new Uprooter();
    }

    private void Start()
    {
        GameManager.Instance.PlayerJoined(this);
    }

    private void OnEnable()
    {
        playerController.Enable();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        if (!GameManager.Instance.GetIsGameInProgress())
        {
            // Can't do anything until the game starts!
            return;
        }

        Direction inputDir = DisambiguateInputVector(moveVec);

        if (inputDir != Direction.NONE)
        {
            // Input is being received.
            OrientPlayer(inputDir);

            if (!executingAction || inputDir != prevDir)
            {
                // Current input is a new action, reset the timer.

                DetermineAction(inputDir);
            }

            // Tick the timer.
            actionTimer -= Time.deltaTime;
            RefreshIndicatorFill();

            if (actionTimer <= 0)
            {
                // Timer has expired, current action is complete! Move to the cell indicated by currentMove.
                Move(inputDir);

                executingAction = false;
            }
        }
        else
        {
            // No input is being received, cancel any current action.
            ToggleIndicator(false);
            actionTimer = 0;
            actionTimeMax = 0;
            playerAnimator.SetTrigger("Idle");
            executingAction = false;
        }

        prevDir = inputDir;
    }

    private Direction DisambiguateInputVector(Vector2 moveVec)
    {
        //                      (LEFT, RIGHT, UP, DOWN), in order of priority
        Direction[] dirs = { Direction.LEFT, Direction.RIGHT, Direction.UP, Direction.DOWN };
        bool[] moveVecDirs = { false, false, false, false };
        bool goingRight = moveVec.x > 0;
        bool goingDown = moveVec.y < 0;

        if (moveVec.x < -dirThreshold && Mathf.Abs(moveVec.x) > Mathf.Abs(moveVec.y) )
        {
            return Direction.LEFT;
        }
        if (moveVec.x > dirThreshold && Mathf.Abs(moveVec.x) > Mathf.Abs(moveVec.y))
        {
            return Direction.RIGHT;
        }
        if (moveVec.y > dirThreshold && Mathf.Abs(moveVec.y) > Mathf.Abs(moveVec.x))
        {
            return Direction.UP;
        }
        if (moveVec.y < -dirThreshold && Mathf.Abs(moveVec.y) > Mathf.Abs(moveVec.x))
        {
            return Direction.DOWN;
        }

        return Direction.NONE;
    }

    private void DetermineAction(Direction actDir)
    {
        // This needs to gather this information:
        // Is the player ON a root?
        // Is the player traveling ALONG a root?
        // Is the player traveling ONTO a root?
        // Is the player traveling onto a ROCK?
        // Is the player traveling onto an ENEMY root?

        Bond targetBond = BondFromDir(actDir);
        
        if (targetBond == null)
        {
            // Wall. No action
            actionTimer = 0;
            actionTimeMax = 0;
            executingAction = false;
            ToggleIndicator(false);
            playerAnimator.SetTrigger("Idle");
            return;
        }

        Cell targetCell = targetBond.OtherCell(occupiedCell);

        if (targetBond.Player == playerNum)
        {
            // The player is moving along a Bond they own. FASTEST action.
            actionTimer = actRootToRoot;
            actionTimeMax = actRootToRoot;
            executingAction = true;
            ToggleIndicator(true);
            playerAnimator.SetTrigger("Walk");
            return;
        }

        // The player is moving along an unowned Bond.
        int targetCellOwner = targetCell.Occupancy();

        if (targetCellOwner == playerNum)
        {
            // The player wants to connect to a Cell they already own. FAST action.
            actionTimer = actRootConnectRoot;
            actionTimeMax = actRootConnectRoot;
            executingAction = true;
            ToggleIndicator(true);
            playerAnimator.SetTrigger("Action");
            return;
        }
        else if (targetCellOwner > 0)
        {
            // The player wants to move onto an enemy Cell. SLOW action.
            actionTimer = actRootToEnemy;
            actionTimeMax = actRootToEnemy;
            executingAction = true;
            ToggleIndicator(true);
            playerAnimator.SetTrigger("Action");
            return;
        }

        // The player is moving into unclaimed territory; check terrain.

        if (targetCell.HasRock())
        {
            // The player wants to move onto difficult terrain. SLOWEST action.
            actionTimer = actRootToRock;
            actionTimeMax = actRootToRock;
            executingAction = true;
            ToggleIndicator(true);
            playerAnimator.SetTrigger("Action");
            return;
        }

        // The player is making a basic movement onto normal terrain. MODERATE action.
        actionTimer = actRootToEmpty;
        actionTimeMax = actRootToEmpty;
        executingAction = true;
        ToggleIndicator(true);
        playerAnimator.SetTrigger("Action");
        return;
    }

    private void Move(Direction moveDir)
    {
        // We use the assumption that in any given bond, Cell1 will be RIGHT or UP based on the verticality

        Bond targetBond = BondFromDir(moveDir);
        if (targetBond != null)
        {
            SetOccupiedCell(targetBond.OtherCell(occupiedCell));
        }
    }

    private Bond BondFromDir(Direction dir)
    {
        Bond[] bonds = { occupiedCell.BondLeft, occupiedCell.BondRight, occupiedCell.BondUp, occupiedCell.BondDown };

        if (dir != Direction.NONE)
        {
            int dirNum = (int)dir - 1;

            return bonds[dirNum];
        }

        return null;
    }

    private void OrientPlayer(Direction dir)
    {
        int[] rots = { 0, 90, 270, 0, 180 };
        Vector3[] offs = { Vector3.zero, Vector3.left, Vector3.right, Vector3.up, Vector3.down };
        int dirNum = (int)dir;

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, rots[dirNum]);
        //actIndicFill.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -rots[dirNum]);
        //actIndicOutline.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -rots[dirNum]);
        //Debug.Log(-rots[dirNum]);

        actIndicFill.transform.position = transform.position + offs[dirNum];
        actIndicOutline.transform.position = transform.position + offs[dirNum];
    }

    public void SetOccupiedCell(Cell newCell)
    {
        occupiedCell = newCell;
        transform.position = newCell.transform.position;
    }

    public void SetPlayerNum(int newNum)
    {
        playerNum = newNum;
        playerAnimator.SetInteger("Color", newNum);
    }

    private void ToggleIndicator(bool doEnable)
    {
        actIndicFill.gameObject.SetActive(doEnable);
        actIndicOutline.gameObject.SetActive(doEnable);
    }

    private void RefreshIndicatorFill()
    {
        float actProgress = 1 - (actionTimer / actionTimeMax);
        actProgress = Mathf.Clamp(actProgress, 0, 1);
        actIndicFill.transform.localScale = new Vector3(1, actProgress, 1);
        actIndicFill.transform.position = new Vector3(actIndicFill.transform.position.x, actIndicFill.transform.position.y - 0.5f + (actProgress / 2), actIndicFill.transform.position.z);
    }
}
