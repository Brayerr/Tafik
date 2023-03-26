using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    #region Animation Events
    static public event Action OnWalk;
    static public event Action OnStopWalk;
    static public event Action OnDig;
    static public event Action OnStopDig;
    static public event Action OnShootGrapple;
    #endregion


    public bool Alive { get; private set; } = true;
    Vector2 move;
    TileLogic tile;
    TileLogic missedTile;
    int _state = 1;
    [SerializeField] Ability ability;

    [SerializeField] public float speed;
    [SerializeField] float buildSpeedMultiplier = 1;
    [SerializeField] Vector2 newDirection;
    public bool buildMode = false;
    [SerializeField] bool canGetInput = true;
    [SerializeField] bool canMove = true; //override

    [SerializeField] public Vector2 position;// { get; protected set; }
    [SerializeField] PlayerTouchMovement touchMovement;

    static public event Action BuildModeToggle;
    static public event Action<TileLogic> OnPlayerTileChanged;

    static public event Action OnTrailStart;
    static public event Action OnTrailEnd;

    public Vector2 Direction { get; private set; }


    private void Start()
    {
        position = new Vector2(transform.position.x, transform.position.z);

        //tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];

        //PlayerPosition.AreaFilled += BuildEnd;
        //PlayerPosition.onTrailStart2 += BuildStart;
        //Tile.OnPlayerTileChanged += InputToggler;
    }

    private void Update()
    {
        if (canMove)
        {
            if (!buildMode)
            {
                NewMovePlayer(touchMovement.MovementAmount);
                //MovePlayer();
            }
            else /*AutoMovePlayer()*/ NewAutoMoveDirectionSetter(touchMovement.MovementAmount);
        }
        RotatePlayer();
        UpdateGridPosition();
    }

    public void OnMove(InputAction.CallbackContext context) => move = context.ReadValue<Vector2>();
    public void MovePlayer()
    {
        if (move != Vector2.zero) OnWalk.Invoke();
        else OnStopWalk.Invoke();

        //calculate movement for this frame
        Vector3 dMovement = new Vector3(move.x, 0f, move.y) * speed * Time.deltaTime;
        //find the position after movement
        Vector2 dPosition = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //leave the method if the player would leave the board
        if (dPosition.x < 0 || dPosition.x > TileBoardManager.Board.Dimen.x || dPosition.y < 0 || dPosition.y > TileBoardManager.Board.Dimen.y)
            return;
        //update position
        position = dPosition;
    }

    public void AutoMoveDirectionSetter(InputAction.CallbackContext context)
    {
        if (canGetInput) newDirection = context.ReadValue<Vector2>();

        //cancel out diagonal movement
        if (Math.Abs(newDirection.x) > Math.Abs(newDirection.y)) newDirection.y = 0;
        else newDirection.x = 0;


        if (newDirection == Vector2.up && Direction == Vector2.down
            || newDirection == Vector2.down && Direction == Vector2.up
            || newDirection == Vector2.right && Direction == Vector2.left
            || newDirection == Vector2.left && Direction == Vector2.right) newDirection = Vector2.zero;



        if (newDirection != Vector2.zero)
        {
            Direction = newDirection.normalized;
            canGetInput = false;
        }
    }

    public void AutoMovePlayer()
    {
        //calculate movement for this frame
        Vector3 dMovement = new Vector3(Direction.x, 0, Direction.y) * speed * buildSpeedMultiplier * Time.deltaTime;
        //update position
        position = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
    }

    public void RotatePlayer()
    {
        if (Direction == Vector2.up) transform.rotation = Quaternion.Euler(new(0, 180, 0));
        else if (Direction == Vector2.right) transform.rotation = Quaternion.Euler(new(0, -90, 0));
        else if (Direction == Vector2.left) transform.rotation = Quaternion.Euler(new(0, 90, 0));
        else if (Direction == Vector2.down) transform.rotation = Quaternion.Euler(new(0, 0, 0));
        else if (!buildMode)
        {
            if (Direction == new Vector2(1, 1).normalized) transform.rotation = Quaternion.Euler(new(0, 225, 0));
            else if (Direction == new Vector2(1, -1).normalized) transform.rotation = Quaternion.Euler(new(0, -45, 0));
            else if (Direction == new Vector2(-1, 1).normalized) transform.rotation = Quaternion.Euler(new(0, 135, 0));
            else if (Direction == new Vector2(-1, -1).normalized) transform.rotation = Quaternion.Euler(new(0, 45, 0));
        }
    }

    public void BuildModeToggler()
    {
        if (buildMode) buildMode = false;
        else if (!buildMode) buildMode = true;
    }

    public void BuildStart()
    {
        buildMode = true;
        OnDig.Invoke();
    }

    public void BuildEnd()
    {
        buildMode = false;
        OnStopDig.Invoke();
        Direction = Vector3.zero;
    }
    public void AllowInput() => canGetInput = true;

    public void AdjustSpeed(float newSpeed)
    {
        float prevSpeed = speed;
        speed = newSpeed;
    }

    public void EnableMove() => canMove = true;
    public void DisableMove() => canMove = false;
    public void KillPlayer()
    {
        Alive = false;
        Debug.Log($"Player Died {Time.realtimeSinceStartup}");
    }

    //logic stuff

    public void UpdateGridPosition()
    {
        transform.position = new(position.x, 1, position.y);
        //enters if tile changed
        if (tile != TileBoardManager.Board.Tiles[(int)position.x, (int)position.y])
        {
            if (tile == null)
            {
                tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
                return;
            }
            tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
            if (tile.State == 2)
                KillPlayer();
            AllowInput();
            if (tile.State != _state)
            {
                _state = tile.State;
                ToggleTrail();

            }

            OnPlayerTileChanged?.Invoke(tile);
        }
    }

    void ToggleTrail()
    {
        switch (_state)
        {
            case 0:
                OnTrailStart?.Invoke();
                BuildStart();
                break;
            case 1:
                OnTrailEnd?.Invoke();
                BuildEnd();
                break;
            default:
                break;
        }
    }

    public void UseSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ability.Activate(this);
            OnShootGrapple.Invoke();
        }
    }

    public void NewMovePlayer(Vector3 movement)
    {
        if (touchMovement.MovementAmount != Vector2.zero) OnWalk.Invoke();
        else OnStopWalk.Invoke();

        movement = movement.normalized;
        //calculate movement for this frame
        Vector3 dMovement = new Vector3(movement.x, 0f, movement.y) * speed * Time.deltaTime;
        //find the position after movement
        Vector2 dPosition = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //leave the method if the player would leave the board
        if (dPosition.x < 0 || dPosition.x > TileBoardManager.Board.Dimen.x || dPosition.y < 0 || dPosition.y > TileBoardManager.Board.Dimen.y)
            return;
        //update position
        position = dPosition;
        Direction = movement;
        if (dMovement.sqrMagnitude > 1)
        {
            missedTile = TileBoardManager.Board.Tiles[(int)(position.x - Direction.x), (int)(position.y - Direction.y)];
        }
    }

    public void NewAutoMoveDirectionSetter(Vector2 movement)
    {
        if (canGetInput) newDirection = movement;

        //cancel out diagonal movement
        if (Math.Abs(newDirection.x) > Math.Abs(newDirection.y))
        {
            newDirection.y = 0;
        }
        else
        {
            newDirection.x = 0;
        }


        if (newDirection == Vector2.up && Direction == Vector2.down
            || newDirection == Vector2.down && Direction == Vector2.up
            || newDirection == Vector2.right && Direction == Vector2.left
            || newDirection == Vector2.left && Direction == Vector2.right) newDirection = Vector2.zero;



        if (newDirection != Vector2.zero)
        {
            Direction = newDirection.normalized;
            canGetInput = false;
        }
        AutoMovePlayer();
    }
}
