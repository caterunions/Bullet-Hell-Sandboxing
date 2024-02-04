using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic Weapon", menuName = "Item/Magic Weapon")]
public class MagicWeapon : Weapon
{
    private DamageType _damageType = DamageType.Magic;
    public DamageType DamageType => _damageType;

    [SerializeField]
    private float _manaCost;
    public float ManaCost => _manaCost;

    [SerializeField]
    private List<Attack> _attacks = new List<Attack>();
    public List<Attack> Attacks => _attacks;
}