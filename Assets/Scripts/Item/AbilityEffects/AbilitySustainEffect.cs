using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilitySustainEffect : AbilityEffect
{
    [SerializeField]
    protected float _repeatDelay;

    public abstract void Cancel();
}
