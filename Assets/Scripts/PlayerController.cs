using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 move;
    [SerializeField] Vector2 newDirection;
    Vector3 direction;
    [SerializeField] bool buildMode = false;
    [SerializeField] bool canGetInput = true;

    static public event Action BuildModeToggle;



    private void Start()
    {
        PlayerPosition.AreaFilled += BuildEnd;
        PlayerPosition.onTrailStart2 += BuildStart;
        Tile.OnPlayerTileChanged += InputToggler;
    }

    private void Update()
    {
        if (!buildMode) MovePlayer();        
        else AutoMovePlayer();       
    }

    public void OnMove(InputAction.CallbackContext context) => move = context.ReadValue<Vector2>();
    public void MovePlayer()
    {
        Vector3 movement = new(move.x, 0f, move.y);

        transform.Translate(movement.normalized * speed * Time.deltaTime, Space.World);
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

    public void AutoMovePlayer() => transform.Translate(new Vector3(direction.x, 0, direction.y) * speed * Time.deltaTime, Space.World);

    public void BuildModeToggler()
    {
        if (buildMode) buildMode = false;
        else if (!buildMode) buildMode = true;
    }

    public void BuildStart() => buildMode = true;

    public void BuildEnd() => buildMode = false;

    public void InputToggler(Tile t) => canGetInput = true;

}
