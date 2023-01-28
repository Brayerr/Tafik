using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    static public event Action PlayerHit;
    static public event Action PlayerDead;

    public Vector3 startPos = new Vector3(15, 1, 0);

    public int MaxHP { get; protected set; } = 5;
    public int HP { get; protected set; }

    private void Start()
    {
        PlayerHit += TakeDamage;
        PlayerHit += RepositionPlayer;
        HP = MaxHP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")
        || other.gameObject.CompareTag("Trail"))
        {
            PlayerHit.Invoke();
        }

        if (HP <= 0) PlayerDead.Invoke();
    }

    public void TakeDamage() => HP--;

    public void RepositionPlayer() => transform.position = startPos;

}
