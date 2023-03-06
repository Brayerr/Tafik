using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrappleHook2 : Ability
{
    public override string abilityName { get; protected set; } = "Grapple Hook";
    public override int MPUsage { get; protected set; }

    GameObject hookObject;
    GrappleHookHook hook;

    private void Start()
    {
        hookObject = Resources.Load<GameObject>("Prefabs/Hook");
    }

    public override void Activate(PlayerLogic player)
    {
        player.DisableMove();
        //player.BuildEnd();

        hook = Instantiate(hookObject/*Resources.Load("Prefabs/Hook", typeof(GameObject)) as GameObject*/, transform.position + player.Direction, Quaternion.identity).GetComponent<GrappleHookHook>();
        //Resources.Load("enemy", typeof(GameObject))
        hook.GetPlayer(player);

        //Vector2Int p = new((int)player.position.x, (int)player.position.y);
        //Vector2Int d = new((int)player.Direction.x, (int)player.Direction.z);
        //while (TileBoardManager.Board.Tiles[p.x, p.y].State != 1)
        //{
        //    p += d;
        //}
        //player.position = p;
    }
}

