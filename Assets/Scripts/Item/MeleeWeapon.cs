using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melee Weapon", menuName = "Item/Melee Weapon")]
public class MeleeWeapon : Weapon
{
    private DamageType _damageType = DamageType.Melee;
    public DamageType DamageType => _damageType;

    [SerializeField]
    private List<Attack> _attacks = new List<Attack>();
    public List<Attack> Attacks => _attacks;
}