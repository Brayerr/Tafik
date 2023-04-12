using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class GrappleHook2 : Ability
{
    public static event Action onActivatedAbility;
    public override string abilityName { get; protected set; } = "Grapple Hook";
    public override int MPUsage { get; protected set; }
    public bool isGaugeFull { get; protected set; } = false;

    UnityEngine.GameObject hookObject;
    GrappleHookHook hook;

    private void Start()
    {
        hookObject = Resources.Load<UnityEngine.GameObject>("Prefabs/Hook");
        AnimationsManager.OnGaugeFilled += AllowGaugeBool;
        AnimationsManager.OnGaugeUnfilled += DisAllowGaugeBool;
    }

    public override void Activate(PlayerLogic player)
    {
        if(isGaugeFull && PlayerLogic.buildMode)
        {
            isGaugeFull = false;
            player.DisableMove();
            //player.BuildEnd();

            hook = Instantiate(hookObject/*Resources.Load("Prefabs/Hook", typeof(GameObject)) as GameObject*/, transform.position + (Vector3)player.Direction, player.transform.rotation).GetComponent<GrappleHookHook>();
            //Resources.Load("enemy", typeof(GameObject))
            hook.GetPlayer(player);

            //Vector2Int p = new((int)player.position.x, (int)player.position.y);
            //Vector2Int d = new((int)player.Direction.x, (int)player.Direction.z);
            //while (TileBoardManager.Board.Tiles[p.x, p.y].State != 1)
            //{
            //    p += d;
            //}
            //player.position = p;
            onActivatedAbility?.Invoke();
        }
    }

    void AllowGaugeBool() => isGaugeFull = true;
    void DisAllowGaugeBool() => isGaugeFull = false;
}

