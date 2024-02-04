using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerStats : EntityStats
{
    [SerializeField]
    private AbilityHolder _abilityHolder;

    public event Action<PlayerStats, int> OnAmmoChange;
    public event Action<PlayerStats, int> OnMaxAmmoChange;
    public event Action<PlayerStats, Sprite> OnAmmoIconChange;

    public event Action<PlayerStats, float> OnManaChange;

    private float _meleeDamageMultipler = 1;
    public float MeleeDamageMultipler
    {
        get { return ApplyStatBoosts(_meleeDamageMultipler, StatTypes.MeleeDamageMult) + (DamageMultiplier - 1); }
    }

    private float _rangedDamageMultiplier = 1;
    public float RangedDamageMultiplier
    {
        get { return ApplyStatBoosts(_rangedDamageMultiplier, StatTypes.RangedDamageMult) + (DamageMultiplier - 1); }
    }

    private float _magicDamageMultipler = 1;
    public float MagicDamageMultipler
    {
        get { return ApplyStatBoosts(_magicDamageMultipler, StatTypes.MagicDamageMult) + (DamageMultiplier - 1); }
    }

    private float _summoningDamageMultiplier = 1;
    public float SummoningDamageMultiplier
    {
        get { return ApplyStatBoosts(_summoningDamageMultiplier, StatTypes.SummoningDamageMult) + (DamageMultiplier - 1); }
    }

    private int _currentAmmo;
    public int CurrentAmmo
    {
        get
        {
            if(_currentAmmo > MaxAmmo) _currentAmmo = MaxAmmo;
            return _currentAmmo;
        }
    }

    private int _maxAmmo;
    public int MaxAmmo
    {
        get { return Math.Clamp(Mathf.FloorToInt(ApplyStatBoosts(_maxAmmo, StatTypes.MaxAmmo)), 1, int.MaxValue); }
    }

    private Sprite _ammoImage;
    public Sprite AmmoImage => _ammoImage;

    private float _currentMana;
    public float CurrentMana => _currentMana;

    [SerializeField]
    private float _maxMana;
    public float MaxMana
    {
        get { return ApplyStatBoosts(_maxMana, StatTypes.MaxMana); }
    }

    [SerializeField]
    private float _manaRegen;
    public float ManaRegen
    {
        get 
        {
            if (_abilityHolder.UsingAbility) return 0f;
            return ApplyStatBoosts(_manaRegen, StatTypes.ManaRegen); 
        }
    }

    [SerializeField]
    private float _maxManaRegenMultiplier;
    public float MaxManaRegenMultiplier
    {
        get { return ApplyStatBoosts(_maxManaRegenMultiplier, StatTypes.MaxManaRegenMult); }
    }

    [SerializeField]
    private float _timeToMaxManaRegen;
    public float TimeToMaxManaRegen
    {
        get { return ApplyStatBoosts(_timeToMaxManaRegen, StatTypes.TimeToMaxManaRegen); }
    }

    [SerializeField]
    private float _critChance;
    public float CritChance
    {
        get { return ApplyStatBoosts(_critChance, StatTypes.CritChance); }
    }

    private float _critPower = 1;
    public float CritPower
    {
        get { return ApplyStatBoosts(_critPower, StatTypes.CritPower); }
    }

    private float _meleeCritChance;
    public float MeleeCritChance
    {
        get { return ApplyStatBoosts(_meleeCritChance, StatTypes.MeleeCritChance) + CritChance; }
    }

    private float _meleeCritPower = 1;
    public float MeleeCritPower
    {
        get { return ApplyStatBoosts(_meleeCritPower, StatTypes.MeleeCritPower) + (CritPower - 1); }
    }

    private float _rangedCritChance;
    public float RangedCritChance
    {
        get { return ApplyStatBoosts(_rangedCritChance, StatTypes.RangedCritChance) + CritChance; }
    }

    private float _rangedCritPower = 1;
    public float RangedCritPower
    {
        get { return ApplyStatBoosts(_rangedCritPower, StatTypes.RangedCritPower) + (CritPower - 1); }
    }

    private float _magicCritChance;
    public float MagicCritChance
    {
        get { return ApplyStatBoosts(_magicCritChance, StatTypes.MagicCritChance) + CritChance; }
    }

    private float _magicCritPower = 1;
    public float MagicCritPower
    {
        get { return ApplyStatBoosts(_magicCritPower, StatTypes.MagicCritPower) + (CritPower - 1); }
    }

    private float _summonCritChance;
    public float SummonCritChance
    {
        get { return ApplyStatBoosts(_summonCritChance, StatTypes.SummonCritChance) + CritChance; }
    }

    private float _summonCritPower = 1;
    public float SummonCritPower
    {
        get { return ApplyStatBoosts(_summonCritPower, StatTypes.SummonCritPower) + (CritPower - 1); }
    }

    [SerializeField]
    private PlayerInventory _playerInventory;

    private void OnEnable()
    {
        _playerInventory.OnWeaponChanged += ResetAmmo;
    }

    private void OnDisable()
    {
        _playerInventory.OnWeaponChanged -= ResetAmmo;
    }

    #region ammo stuff
    public void SetMaxAmmo(int max)
    {
        int old = _maxAmmo;
        _maxAmmo = max;
        OnMaxAmmoChange?.Invoke(this, old);
    }

    public void SetAmmoImage(Sprite ammoImage)
    {
        Sprite old = _ammoImage;
        _ammoImage = ammoImage;
        OnAmmoIconChange?.Invoke(this, old);
    }

    public void RefillAmmo()
    {
        _currentAmmo = MaxAmmo;

        OnAmmoChange?.Invoke(this, 0);
    }

    public void ExpendAmmo(int ammo)
    {
        _currentAmmo -= ammo;

        if(_currentAmmo < 0)
        {
            _currentAmmo = 0;
        }

        OnAmmoChange?.Invoke(this, ammo);
    }

    private void ResetAmmo(PlayerInventory playerInv, Weapon old)
    {
        ExpendAmmo(MaxAmmo);
    }
    #endregion

    #region mana stuff
    public void RegainMana(float mana)
    {
        _currentMana += mana;
        if(_currentMana > _maxMana) _currentMana = _maxMana;

        OnManaChange?.Invoke(this, mana);
    }

    public void DepleteMana(float mana)
    {
        _currentMana -= mana;
        if(_currentMana < 0) _currentMana = 0;

        OnManaChange?.Invoke(this, -mana);
    }
    #endregion
}

public enum StatTypes
{
    MaxHealth,
    Regen,
    GlobalDamageMult,
    AttackSpeed,
    RangeMult,
    VelocityMult,
    Armor,
    MeleeDamageMult,
    RangedDamageMult,
    MagicDamageMult,
    SummoningDamageMult,
    MaxAmmo,
    MaxMana,
    ManaRegen,
    MaxManaRegenMult,
    TimeToMaxManaRegen,
    CritChance,
    CritPower,
    MeleeCritChance,
    MeleeCritPower,
    RangedCritChance,
    RangedCritPower,
    MagicCritChance,
    MagicCritPower,
    SummonCritChance,
    SummonCritPower
}

public enum StatBoostType
{
    Additive,
    Multiplicative
}

public enum StatBoostSource
{
    Equipment,
    StatusEffect
}

[Serializable]
public class StatBoost
{
    [SerializeField]
    private StatTypes _statIncrease;
    public StatTypes StatIncrease => _statIncrease;

    [SerializeField]
    private StatBoostType _type;
    public StatBoostType Type => _type;

    [SerializeField]
    private StatBoostSource _source;
    public StatBoostSource Source => _source;

    [SerializeField]
    private float _statIncreaseAmount;
    public float StatIncreaseAmount => _statIncreaseAmount;

    public override bool Equals(System.Object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            StatBoost sb = (StatBoost)obj;
            return (_statIncrease == sb.StatIncrease) && (_type == sb.Type) && (_source == sb.Source) && (_statIncreaseAmount == sb.StatIncreaseAmount);
        }
    }
}
