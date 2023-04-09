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
    private IEnumerator respawnCoroutine;

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
    static public event Action OnPlayerDied;


    public Vector2 Direction { get; private set; }


    private void Start()
    {
        position = new Vector2(transform.position.x, transform.position.z);

        respawnCoroutine = SpawnPlayerDelayed(2);
        //tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];

        //PlayerPosition.AreaFilled += BuildEnd;
        //PlayerPosition.onTrailStart2 += BuildStart;
        //Tile.OnPlayerTileChanged += InputToggler;
    }

    private void Update()
    {
        if (Alive && canMove)
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

    private void FixedUpdate()
    {
        if (Alive)
        {
            foreach (var enemy in EnemyManager.enemies)
            {
                if (position.InRange(enemy.position, .5f) && _state == enemy.state)
                {
                    KillPlayer();
                    Debug.Log($"{enemy} killed player");
                }
            }
        }
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
        //find the position after movement
        Vector2 dPosition = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //leave the method if the player would leave the board
        if (dPosition.x < 0 || dPosition.x > TileBoardManager.Board.Dimen.x || dPosition.y < 0 || dPosition.y > TileBoardManager.Board.Dimen.y)
            return;
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

    public void SpawnPlayer()
    {
        position = new(16, 0);
        Alive = true;
        if (Alive) Debug.Log("Alive");
    }
    public void KillPlayer()
    {
        if (!Alive) return;
        OnPlayerDied.Invoke();
        Alive = false;
        OnPlayerTileChanged = null;
        BuildEnd();
        Debug.Log($"Player Died {Time.realtimeSinceStartup}");
        StartCoroutine("SpawnPlayerDelayed",2);
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
            //if player has suddenly moved more than 1 tile, dig the tile behind
            if ((tile.Position - position.RoundVector()).sqrMagnitude > 1)
            {
                missedTile = TileBoardManager.Board.Tiles[(int)(position.x - Direction.x), (int)(position.y - Direction.y)];
                if (missedTile.State == 2)
                {
                    KillPlayer();
                    Debug.Log($"tile behind killed player");
                }
                OnPlayerTileChanged?.Invoke(missedTile);
            }

            tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
            //fail if player touches trail
            if (tile.State == 2)
            {
                KillPlayer();
                Debug.Log($"{tile.Position}");
                Debug.Log($"trail killed player");

            }
            //allow direction change
            AllowInput();
            //enters if digging starts or ends
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
            //entered high tile, start digging
            case 0:
                OnTrailStart?.Invoke();
                BuildStart();
                break;
            //entered dug tile, stop digging
            case 1:
                OnTrailEnd?.Invoke();
                BuildEnd();
                break;
            default:
                break;
        }
    }

    public void UseSkill()
    {
        ability.Activate(this);
        OnShootGrapple.Invoke();
    }

    IEnumerator SpawnPlayerDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        SpawnPlayer();
        
    }

    public void NewMovePlayer(Vector3 movement)
    {
        //this is not optimal
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

        //compare pos to active pickup pos ,if true interact with it. 
        foreach (var item in PickupManager.pickupList)
        {
            if (item.isActiveAndEnabled
                && item.position.x == (int)position.x && item.position.y == (int)position.y
                || item.position.x == (int)position.x + 1 && item.position.y == (int)position.y + 1
                || item.position.x == (int)position.x - 1 && item.position.y == (int)position.y - 1
                || item.position.x == (int)position.x - 1 && item.position.y == (int)position.y + 1
                || item.position.x == (int)position.x + 1 && item.position.y == (int)position.y - 1) item.InteractWithPickup();
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
