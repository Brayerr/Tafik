using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SimpleEnemyBehavior : Enemy
{
    Vector2 direction;
    float speed = 3;

    protected override void Start()
    {
        base.Start();
        position = new(transform.position.x, transform.position.z);
        switch (Random.Range(0, 4))
        {
            case 0:
                direction = Vector2.up + Vector2.right;
                break;
            case 1:
                direction = Vector2.down + Vector2.right;
                break;
            case 2:
                direction = Vector2.down + Vector2.left;
                break;
            case 3:
                direction = Vector2.up + Vector2.left;
                break;
            default:
                break;
        }
        RotateEnemy();
    }

    //a simple movement pattern that is similar to a pingpong/screensaver
    public override void Move()
    {
        //the movement enemy is about to perform
        Vector3 dMovement = new Vector3(direction.x, 0, direction.y) * Time.deltaTime * speed;

        //check if moving horizontally hits a wall
        if (TileBoardManager.Board.Tiles[(int)(position.x + dMovement.x), (int)position.y].State == 1)
        {
            direction = new(-direction.x, direction.y);
            RotateEnemy();
            dMovement.x = -dMovement.x;
        }
        //check if moving vertically hits a wall
        if (TileBoardManager.Board.Tiles[(int)position.x, (int)(position.y + dMovement.z)].State == 1)
        {
            direction = new(direction.x, -direction.y);
            RotateEnemy();
            dMovement.z = -dMovement.z;
        }
        //move in data
        position = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //update position in unity
        transform.position = new(position.x, 1.5f, position.y);
    }

    public override void RotateEnemy()
    {
        if (direction == new Vector2(1, 1)) transform.rotation = Quaternion.Euler(new(0, 45, 0));
        else if (direction == new Vector2(1, -1)) transform.rotation = Quaternion.Euler(new(0, 135, 0));
        else if (direction == new Vector2(-1, 1)) transform.rotation = Quaternion.Euler(new(0, -45, 0));
        else if (direction == new Vector2(-1, -1)) transform.rotation = Quaternion.Euler(new(0, -135, 0));
    }
}
