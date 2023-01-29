using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    [SerializeField] public float speed { get; protected set; } = 5;
    [SerializeField] public float buildSpeed { get; protected set; } = 8;
    Vector2 move;
    public Vector3 direction;
    [SerializeField] public Vector2 newDirection;
    [SerializeField] public bool buildMode { get; protected set; } = false;
    [SerializeField] public bool canGetInput { get; protected set; } = true;
    [SerializeField] public bool usingSpecialAbility { get; protected set; } = false;




    private void Start()
    {
        PlayerPosition.AreaFilled += BuildEnd;
        PlayerPosition.onTrailStart2 += BuildStart;
        Tile.OnPlayerTileChanged += InputToggler;
        Tile.OnGrappleTileChanged += InputToggler;
        Player.PlayerHit += BuildEnd;
        GrappleHook.OnShoot += UsingSpecialAbilityOn;
        Tile.OnShootFinished += UsingSpecialAbilityOff;
    }

    private void Update()
    {
        if (!buildMode && !usingSpecialAbility) MovePlayer();        
        else if (buildMode && !usingSpecialAbility) AutoMovePlayer();       
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

    public void AutoMovePlayer() => transform.Translate(new Vector3(direction.x, 0, direction.y) * buildSpeed * Time.deltaTime, Space.World);

    public void BuildStart() => buildMode = true;

    public void BuildEnd() => buildMode = false;

    public void InputToggler(Tile t) => canGetInput = true;

    public void UsingSpecialAbilityOn() => usingSpecialAbility = true;

    public void UsingSpecialAbilityOff() => usingSpecialAbility = false;

}
