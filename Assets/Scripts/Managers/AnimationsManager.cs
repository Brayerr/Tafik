using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    [SerializeField] Animator whiskersAnimator;

    private void Start()
    {
        PlayerLogic.OnWalk += SetWalkOn;
        PlayerLogic.OnStopWalk += SetWalkOff;
        PlayerLogic.OnDig += SetDigOn;
        PlayerLogic.OnStopDig += SetDigOff;
        PlayerLogic.OnShootGrapple += SetGrappleTrigger;
    }



    void SetWalkOn()
    {
        whiskersAnimator.SetBool("isWalking", true);
    }

    void SetWalkOff()
    {
        whiskersAnimator.SetBool("isWalking", false);
    }

    void SetDigOn()
    {
        whiskersAnimator.SetBool("isDigging", true);
    }

    void SetDigOff()
    {
        whiskersAnimator.SetBool("isDigging", false);
    }

    void SetGrappleTrigger()
    {
        whiskersAnimator.SetTrigger("ShootGrapple");
    }

}
