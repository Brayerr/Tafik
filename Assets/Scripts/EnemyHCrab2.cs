using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHCrab2 : Enemy
{
    float _cooldown;


    [SerializeField] float throwRadius;
    [SerializeField] float pickRadius;
    [SerializeField] float walkSpeed;
    [SerializeField] float skillCooldown;

    public override void Move()
    {
        throw new System.NotImplementedException();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    void DropShell()
    {

    }
}
