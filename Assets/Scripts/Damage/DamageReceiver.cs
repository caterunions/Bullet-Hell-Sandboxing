using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTeam
{
    Enemy,
    Player,
    Neutral
}

public class DamageReceiver : MonoBehaviour
{
    public event Action<DamageReceiver, DamageEvent> OnDamage;

    [SerializeField]
    private DamageTeam _team;
    public DamageTeam Team => _team;

    public void ReceiveDamage(DamageEvent dmgEvent)
    {
        HandleDamage(dmgEvent);

        OnDamage?.Invoke(this, dmgEvent);
    }

    protected virtual void HandleDamage(DamageEvent dmgEvent)
    {
        //logic for child damagereceivers overrides this method
    }
}

public class DamageEvent
{
    public DamageEvent(float damage, GameObject mainSource, GameObject specificSource, bool armorPiercing, bool crit)
    {
        Damage = damage;
        MainSource = mainSource;
        SpecificSource = specificSource;
        ArmorPiercing = armorPiercing;
        Crit = crit;
    }

    public float Damage { get; }
    public GameObject MainSource { get; }
    public GameObject SpecificSource { get; }
    public bool ArmorPiercing { get; }
    public bool Crit { get; }
    public float AppliedDamage { get; set; }
}
