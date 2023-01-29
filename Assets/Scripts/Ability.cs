using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    public abstract string abilityName { get; protected set; }
    public abstract int MPUsage { get; protected set; }
    
    
    public enum Type
    {
        Passive,
        Active,
        Buff
    }


}
