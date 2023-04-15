using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyHCrab : Enemy
{
    int _rotationIndex;
    float _sequencer = 0;
    float _skillLatestUse;
    int actionDuration = -1;
    Vector2Int direction;
    float speed;

    //for designer
    [SerializeField] float transformHeight;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float skillCooldown;
    [SerializeField] Vector2 movementDurationRange;
    [SerializeField] Vector2 stopDurationRange;

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
        transform.position = new(position.x, transformHeight, position.y);

    }

    protected override void Start()
    {
        base.Start();
        RotateActions();
        speed = walkSpeed;
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

    }

    // a basic rotation of algorithms
    // actions should be made a new data structure so they could be made easily by designers
    // speed, action index and duration
    void RotateActions()
    {
        //_rotationIndex++;
        //if (_rotationIndex > 1) _rotationIndex = 0;
        //switch (_rotationIndex)
        //{
        //    //move
        //    case 0:
        //        speed = walkSpeed;
        //        actionDuration = Random.Range((int)(movementDurationRange.x * 10), (int)(movementDurationRange.y * 10)) / 10;
        //        if (Time.time - _skillLatestUse > skillCooldown)
        //        {
        //            _skillLatestUse = Time.time;
        //            speed = runSpeed;
        //            actionDuration /= 2;
        //        }
        //        break;

        //    //stop
        //    case 1:
        //        speed = 0;
        //        actionDuration = Random.Range((int)(stopDurationRange.x * 10), (int)(stopDurationRange.y * 10)) / 10;
        //        break;
        //    default:
        //        break;
        //}
        //PickDirection();

        PickDirection();
        actionDuration = Random.Range((int)(movementDurationRange.x * 10), (int)(movementDurationRange.y * 10)) / 10;

    }

    void PickDirection()
    {
        switch (Random.Range(0, 4))
        {
            case 0:
                direction = Vector2Int.up;
                break;
            case 1:
                direction = Vector2Int.right;
                break;
            case 2:
                direction = Vector2Int.down;
                break;
            case 3:
                direction = Vector2Int.left;
                break;
            default:
                break;
        }
        RotateEnemy();
    }

    public override void RotateEnemy()
    {
        if (direction == Vector2.up) transform.rotation = Quaternion.Euler(new(0, 0, 0));
        else if (direction == Vector2.left) transform.rotation = Quaternion.Euler(new(0, -90, 0));
        else if (direction == Vector2.down) transform.rotation = Quaternion.Euler(new(0, 180, 0));
        else if (direction == Vector2.right) transform.rotation = Quaternion.Euler(new(0, 90, 0));
    }
}
