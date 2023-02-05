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
    float actionTimer = 0;
    private bool executingAction = false;

    private Vector2 moveVec;

    public void OnMove(InputAction.CallbackContext ctx) => moveVec = ctx.ReadValue<Vector2>();

    private Cell occupiedCell;

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
        Direction inputDir = DisambiguateInputVector(moveVec);

        if (inputDir != Direction.NONE)
        {
            // Input is being received.
            if (!executingAction || inputDir != prevDir)
            {
                // Current input is a new action, reset the timer.

                // ## DETERMINE ACTION HERE ##

                actionTimer = 1;

                executingAction = true;
            }

            // Tick the timer.
            actionTimer -= Time.deltaTime;

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
            actionTimer = 0;
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

    private void Move(Direction moveDir)
    {
        switch (moveDir)
        {
            case Direction.LEFT:
                {
                    if (SetOccupiedCell(occupiedCell.BondLeft.Cell1).Equals(occupiedCell)) ;
                    {

                    }
                    SetOccupiedCell(occupiedCell.BondLeft.Cell1);
                    break;
                }
            case Direction.RIGHT:
                {
                    break;
                }
            case Direction.UP:
                {
                    break;
                }
            case Direction.DOWN:
                {
                    break;
                }
        }
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
}
