using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBossOctopus : Enemy
{
    [SerializeField] Danger SlamTelegraph;
    [SerializeField] float cooldown;
    [SerializeField] float postCooldown;
    float sequencer;

    protected override void Update()
    {
        base.Update();
        sequencer += Time.deltaTime;

        if (sequencer > cooldown)
        {
            Slam();
            sequencer = -postCooldown;
        }

    }

    void Slam()
    {
        Danger attack = Instantiate(SlamTelegraph, transform.position + Vector3.back * 14 + new Vector3(0, 0.1f, 0), transform.rotation);
        attack.GetAreaSize(new(3, 10));
    }

    public override void Move()
    {

    }

    public override void RotateEnemy()
    {

    }
}
