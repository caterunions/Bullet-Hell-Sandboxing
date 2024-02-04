using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Summoning Weapon", menuName = "Item/Summoning Weapon")]
public class SummoningWeapon : Weapon
{
    private DamageType _damageType = DamageType.Summoning;
    public DamageType DamageType => _damageType;

    [SerializeField]
    private float _manaCost;
    public float ManaCost => _manaCost;

    [SerializeField]
    private List<Minion> _minions = new List<Minion>();
}