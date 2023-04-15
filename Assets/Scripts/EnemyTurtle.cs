using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurtle : Enemy
{
    //for designer
    [SerializeField] List<MovementPattern> movements = new List<MovementPattern>();

    //internal
    float speed;
    float actionDuration = -1;
    int _rotationIndex = -1;

    //rotation cycle
    Vector2 direction;
    float _sequencer = 0;



    public override void Move()
    {
        //the movement enemy is about to perform
        Vector3 dMovement = new Vector3(direction.x, 0, direction.y) * Time.deltaTime * speed;

        //check if moving horizontally hits a wall
        if (TileBoardManager.Board.Tiles[(int)(position.x + dMovement.x), (int)position.y].State == 1)
        {
            direction = new(-direction.x, direction.y);
            dMovement.x = -dMovement.x;
            RotateEnemy();
        }
        //check if moving vertically hits a wall
        if (TileBoardManager.Board.Tiles[(int)position.x, (int)(position.y + dMovement.z)].State == 1)
        {
            direction = new(direction.x, -direction.y);
            dMovement.z = -dMovement.z;
            RotateEnemy();
        }
        //move in data
        position = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //update position in unity
        transform.position = new(position.x, 1, position.y);
    }

    protected override void Start()
    {
        base.Start();
        RotateActions();
    }

    protected override void Update()
    {
        base.Update();
        _sequencer += Time.deltaTime;

        if (_sequencer > actionDuration)
        {
            RotateActions();
            _sequencer = 0;
        }
        if(_rotationIndex == 2)
        {
            transform.Rotate(0, 360*Time.deltaTime, 0);
        }
    }

    // a basic rotation of algorithms
    // actions should be made a new data structure so they could be made easily by designers
    // speed, action index and duration
    void RotateActions()
    {
        _rotationIndex++;
        _rotationIndex %= movements.Count;
        MovementPattern currentMovement = movements[_rotationIndex];
        speed = currentMovement.GetSpeed();
        actionDuration = currentMovement.GetDuration();
        if (currentMovement.isTurning)
        {
            direction = currentMovement.GetDirection();
            RotateEnemy();
        }
    }

    public override void RotateEnemy()
    {
        if (direction == Vector2.up) transform.rotation = Quaternion.Euler(new(0, 0, 0));
        else if (direction == new Vector2(1, 1)) transform.rotation = Quaternion.Euler(new(0, 45, 0));
        else if (direction == Vector2.right) transform.rotation = Quaternion.Euler(new(0, 90, 0));
        else if (direction == new Vector2(1, -1)) transform.rotation = Quaternion.Euler(new(0, 135, 0));
        else if (direction == Vector2.down) transform.rotation = Quaternion.Euler(new(0, 180, 0));
        else if (direction == new Vector2(-1, -1)) transform.rotation = Quaternion.Euler(new(0, -135, 0));
        else if (direction == Vector2.left) transform.rotation = Quaternion.Euler(new(0, -90, 0));
        else if (direction == new Vector2(-1, 1)) transform.rotation = Quaternion.Euler(new(0, -45, 0));

    }
}
