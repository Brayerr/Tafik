using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHCrabShell : Enemy
{
    EnemyHCrab2 crabParent;

    protected override void Start()
    {
        base.Start();
        crabParent = GetComponentInParent<EnemyHCrab2>();
    }
    // Update is called once per frame
    protected override void Update()
    {
        if (transform.parent != null)
            position = crabParent.position;
    }

    public void Drop()
    {
        transform.SetParent(null);
    }

    public bool PickUp(Transform parent, EnemyHCrab2 crab)
    {
        crabParent = crab;
        transform.position = new Vector3(crabParent.transform.position.x, crabParent.transform.position.y + 1, crabParent.transform.position.z);
        transform.SetParent(parent);
        //transform.localPosition = Vector3.up;
        return true;
    }

    public override void Move()
    {

    }

    public override void RotateEnemy()
    {

    }
}
