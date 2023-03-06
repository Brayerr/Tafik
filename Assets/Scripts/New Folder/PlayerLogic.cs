using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    Vector2 move;
    TileLogic tile;
    int _state = 1;
    [SerializeField] Ability ability;

    [SerializeField] float speed;
    [SerializeField] float buildSpeedMultiplier = 1;
    [SerializeField] Vector2 newDirection;
    [SerializeField] bool buildMode = false;
    [SerializeField] bool canGetInput = true;
    [SerializeField] bool canMove = true; //override

    [SerializeField] public Vector2 position;// { get; protected set; }

    static public event Action BuildModeToggle;
    static public event Action<TileLogic> OnPlayerTileChanged;

    static public event Action OnTrailStart;
    static public event Action OnTrailEnd;

    public Vector3 Direction { get; private set; }


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
            if (!buildMode) MovePlayer();
            else AutoMovePlayer();
        }

        UpdateGridPosition();
    }

    public void OnMove(InputAction.CallbackContext context) => move = context.ReadValue<Vector2>();
    public void MovePlayer()
    {
        Vector3 dMovement = new Vector3(move.x, 0f, move.y) * speed * Time.deltaTime;
        //dMovement = dMovement.normalized * Time.deltaTime * speed;
        Vector2 dPosition = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        if (dPosition.x < 0 || dPosition.x > TileBoardManager.Board.Dimen.x || dPosition.y < 0 || dPosition.y > TileBoardManager.Board.Dimen.y)
            return;
        position = dPosition;
        //transform.position = new(position.x, 1, position.y);
    }

    public void AutoMoveDirectionSetter(InputAction.CallbackContext context)
    {
        if (canGetInput) newDirection = context.ReadValue<Vector2>();

        if (Math.Abs(newDirection.x) > Math.Abs(newDirection.y)) newDirection.y = 0;
        else newDirection.x = 0;

        if (newDirection == Vector2.up && Direction == Vector3.down
            || newDirection == Vector2.down && Direction == Vector3.up
            || newDirection == Vector2.right && Direction == Vector3.left
            || newDirection == Vector2.left && Direction == Vector3.right) newDirection = Vector2.zero;

        if (newDirection != Vector2.zero)
        {
            Direction = newDirection.normalized;
            canGetInput = false;
        }

    }

    public void AutoMovePlayer()
    {
        Vector3 dMovement = new Vector3(Direction.x, 0, Direction.y) * speed * buildSpeedMultiplier * Time.deltaTime;
        position = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //transform.position = new(position.x, 1, position.y);

        //transform.Translate(new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime, Space.World);
    }
    public void BuildModeToggler()
    {
        if (buildMode) buildMode = false;
        else if (!buildMode) buildMode = true;
    }

    public void BuildStart() => buildMode = true;

    public void BuildEnd()
    {
        buildMode = false;
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
            ability.Activate(this);
    }
}
