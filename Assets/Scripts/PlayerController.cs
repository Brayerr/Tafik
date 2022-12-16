using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]float speed;
    Vector2 move;


    private void Start()
    {
        
    }

    private void Update()
    {
     MovePlayer();
    }
    public void OnMove(InputAction.CallbackContext context)
    {   
        move = context.ReadValue<Vector2>();
    }

    void MovePlayer()
    {
            Vector3 movement = new(move.x, 0f, move.y);

            transform.Translate(movement * speed * Time.deltaTime ,Space.World);
    }


}
