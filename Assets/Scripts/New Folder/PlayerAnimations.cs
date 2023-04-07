using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        PlayerLogic.OnWalk += SetWalkOn;
        PlayerLogic.OnStopWalk += SetWalkOff;
        PlayerLogic.OnDig += SetDigOn;
        PlayerLogic.OnStopDig += SetDigOff;
        PlayerLogic.OnShootGrapple += SetGrappleTrigger;
    }

    void SetWalkOn()
    {
        anim.SetBool("isWalking", true);
    }

    void SetWalkOff()
    {
        anim.SetBool("isWalking", false);
    }

    void SetDigOn()
    {
        anim.SetBool("isDigging", true);
    }

    void SetDigOff()
    {
        anim.SetBool("isDigging", false);
    }

    void SetGrappleTrigger()
    {
        anim.SetTrigger("ShootGrapple");
    }

}
