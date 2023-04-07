using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    [SerializeField] Animator abilityButtonAnimator;


    private void Start()
    {      
        GrappleHook2.onActivatedAbility += SetAbilityButtonTrigger;
        UIManager.OnAbilityFillUpdated += UpdateAbilityValue;
    }


    void SetAbilityButtonTrigger()
    {
        abilityButtonAnimator.SetTrigger("onClick");
    }

    void UpdateAbilityValue(float value)
    {
        if (value >= 200) abilityButtonAnimator.SetBool("isFilled", true);
        else abilityButtonAnimator.SetBool("isFilled", false);
    }
}
