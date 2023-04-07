using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationsManager : MonoBehaviour
{
    public static event Action OnGaugeFilled;
    public static event Action OnGaugeUnfilled;
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
        if (value >= 200)
        {
            abilityButtonAnimator.SetBool("isFilled", true);
            OnGaugeFilled.Invoke();
        }
        else
        {
            abilityButtonAnimator.SetBool("isFilled", false);
            OnGaugeUnfilled.Invoke();
        }
    }
}
