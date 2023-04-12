using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHCrab2 : Enemy
{
    public static event Action<Vector3> OnDropShell;
    public static event Action<Vector3> OnPickShell;

    [SerializeField] float _cooldown;
    [SerializeField] Vector2 direction;
    [SerializeField] float offset;
    [SerializeField] EnemyHCrabShell ChildShell;
    bool hasShell = true;
    float timer = 0;
    float freq = 5;

    [SerializeField] float throwRadius;
    [SerializeField] float pickRadius = 5;
    [SerializeField] float walkSpeed;
    [SerializeField] float skillCooldown;

    public override void Move()
    {
        //the movement enemy is about to perform
        Vector3 dMovement = new Vector3(direction.x, 0, direction.y) * Time.deltaTime * walkSpeed;

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
        transform.position = new(position.x, 2, position.y);

    }

    protected override void Start()
    {
        base.Start();
        switch (UnityEngine.Random.Range(0, 4))
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

    protected override void Update()
    {
        base.Update();
        if (hasShell && timer > skillCooldown)
        {
            DropShell();
            timer = 0;
        }
        if (!hasShell)
        {
            if (timer > _cooldown)
            {
                ChildShell = EnemyManager.FindEnemy<EnemyHCrabShell>(position, pickRadius);
                PickShell();
                timer = 0;
            }
        }
        timer += Time.deltaTime;
    }

    void DropShell()
    {
        ChildShell.Drop();
        ChildShell = null;
        hasShell = false;
        OnDropShell.Invoke(transform.position);
    }

    void PickShell()
    {
        ChildShell?.PickUp(transform, this);
        if (ChildShell != null)
        {
            hasShell = true;
            ChildShell.transform.rotation=transform.rotation;
            OnPickShell.Invoke(transform.position);
        }
    }

    public override void RotateEnemy()
    {
        if (direction == new Vector2(1, 1)) transform.rotation = Quaternion.Euler(new(0, 45, 0));
        else if (direction == new Vector2(1, -1)) transform.rotation = Quaternion.Euler(new(0, 135, 0));
        else if (direction == new Vector2(-1, 1)) transform.rotation = Quaternion.Euler(new(0, -45, 0));
        else if (direction == new Vector2(-1, -1)) transform.rotation = Quaternion.Euler(new(0, -135, 0));
    }
}
