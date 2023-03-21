using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHCrab2 : Enemy
{
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
            dMovement.x = -dMovement.x;
        }
        //check if moving vertically hits a wall
        if (TileBoardManager.Board.Tiles[(int)position.x, (int)(position.y + dMovement.z)].State == 1)
        {
            direction = new(direction.x, -direction.y);
            dMovement.z = -dMovement.z;
        }
        //move in data
        position = new Vector2(position.x + dMovement.x, position.y + dMovement.z);
        //update position in unity
        transform.position = new(position.x, 1, position.y);

    }

    protected override void Start()
    {
        base.Start();
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
                Debug.Log("searching");
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
    }

    void PickShell()
    {
        ChildShell?.PickUp(transform, this);
        if (ChildShell != null) hasShell = true;
    }


}
