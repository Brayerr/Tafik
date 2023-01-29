using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 move;
    [SerializeField] Vector2 newDirection;
    Vector3 direction;
    [SerializeField] bool buildMode = false;
    [SerializeField] bool canGetInput = true;

    [SerializeField] public Vector2 position;// { get; protected set; }
    TileLogic tile;
    int _state = 1;

    static public event Action BuildModeToggle;
    static public event Action<TileLogic> OnPlayerTileChanged;

    static public event Action OnTrailStart;
    static public event Action OnTrailEnd;



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
        if (!buildMode) MovePlayer();
        else AutoMovePlayer();

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
        transform.position = new(position.x, 1, position.y);
    }

    public void AutoMoveDirectionSetter(InputAction.CallbackContext context)
    {
        if (canGetInput) newDirection = context.ReadValue<Vector2>();

        if (Math.Abs(newDirection.x) > Math.Abs(newDirection.y)) newDirection.y = 0;
        else newDirection.x = 0;

        if (newDirection == Vector2.up && direction == Vector3.down
            || newDirection == Vector2.down && direction == Vector3.up
            || newDirection == Vector2.right && direction == Vector3.left
            || newDirection == Vector2.left && direction == Vector3.right) newDirection = Vector2.zero;

        if (newDirection != Vector2.zero)
        {
            direction = newDirection.normalized;
            canGetInput = false;
        }

    }

    public void AutoMovePlayer()
    {
        Vector3 dMovement = new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime;
        position = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        transform.position = new(position.x, 1, position.y);

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
        direction = Vector3.zero;
    }
    public void InputToggler() => canGetInput = true;


    //logic stuff

    public void UpdateGridPosition()
    {
        //enters if tile changed
        if (tile != TileBoardManager.Board.Tiles[(int)position.x, (int)position.y])
        {
            if (tile == null)
            {
                tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
                return;
            }
            tile = TileBoardManager.Board.Tiles[(int)position.x, (int)position.y];
            InputToggler();
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
}
