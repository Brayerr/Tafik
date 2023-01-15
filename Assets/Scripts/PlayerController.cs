using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 move;
    Vector3 direction;
    [SerializeField] bool buildMode = false;


    public delegate void BuildModeToggle();



    private void Start()
    {
        BuildModeToggle buildModeToggle = new(BuildModeToggler);
    }

    private void Update()
    {
        if (!buildMode) MovePlayer();
        else
        {
            AutoMoveDirectionSetter();
            AutoMovePlayer();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!buildMode) move = context.ReadValue<Vector2>();
    }

    public void MovePlayer()
    {
        Vector3 movement = new(move.x, 0f, move.y);

        transform.Translate(movement * speed * Time.deltaTime, Space.World);
    }

    void AutoMoveDirectionSetter()
    {
        if (Input.GetKeyDown(KeyCode.W)) direction = new(0, 0, 1);
        if (Input.GetKeyDown(KeyCode.S)) direction = new(0, 0, -1);
        if (Input.GetKeyDown(KeyCode.A)) direction = new(-1, 0, 0);
        if (Input.GetKeyDown(KeyCode.D)) direction = new(1, 0, 0);
    }

    public void AutoMovePlayer()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    public void BuildModeToggler()
    {
        if (buildMode) buildMode = false;
        else if (!buildMode) buildMode = true;
    }


}
