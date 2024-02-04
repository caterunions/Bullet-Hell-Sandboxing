using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ranged Weapon", menuName = "Item/Ranged Weapon")]
public class RangedWeapon : Weapon
{
    private DamageType _damageType = DamageType.Ranged;
    public DamageType DamageType => _damageType;

    [SerializeField]
    private int _ammo;
    public int Ammo => _ammo;

    [SerializeField]
    private Sprite _ammoImage;
    public Sprite AmmoImage => _ammoImage;

    [SerializeField]
    private float _reloadTime;
    public float ReloadTime => _reloadTime;

    [SerializeField]
    private List<Attack> _attacks = new List<Attack>();
    public List<Attack> Attacks => _attacks;
}
