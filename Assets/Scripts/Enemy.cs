using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public Vector2 position { get; protected set; }

    public abstract void Move();

    virtual protected void Start()
    {
        EnemyManager.AddEnemy(this);
        position = new Vector2(transform.position.x, transform.position.z);
    }

    virtual protected void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Move();
        //}
        Move();
    }
    
}
