using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class EquipmentManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private BulletLauncher _bulletLauncher;

    [SerializeField]
    private PlayerStats _playerStats;

    [SerializeField]
    private PlayerInventory _playerInventory;

    private List<ItemEffect> _activeItemEffects = new List<ItemEffect>();

    private void OnEnable()
    {
        _playerInventory.OnEquipmentChanged += HandleEquipmentChange;
        _playerInventory.OnAccessoriesChanged += HandleAccessoryChange;

        _bulletLauncher.OnSpawnedBulletHit += HandleOnHitEffects;
        _bulletLauncher.OnLaunch += HandleOnShootEffects;

        HandleEquipmentChange(_playerInventory, _playerInventory.Weapon, null);
        HandleEquipmentChange(_playerInventory, _playerInventory.Ability, null);
        HandleEquipmentChange(_playerInventory, _playerInventory.Helmet, null);
        HandleEquipmentChange(_playerInventory, _playerInventory.ChestArmor, null);
        HandleEquipmentChange(_playerInventory, _playerInventory.LegsArmor, null);
        HandleAccessoryChange(_playerInventory, new Accessory[4]);
    }

    private void OnDisable()
    {
        _playerInventory.OnEquipmentChanged -= HandleEquipmentChange;
        _playerInventory.OnAccessoriesChanged -= HandleAccessoryChange;

        _bulletLauncher.OnSpawnedBulletHit -= HandleOnHitEffects;
        _bulletLauncher.OnLaunch -= HandleOnShootEffects;

        foreach (StatBoost statBoost in _playerStats.ActiveStatBoosts.ToList())
        {
            if(statBoost.Source == StatBoostSource.Equipment) _playerStats.ActiveStatBoosts.Remove(statBoost);
        }
    }

    private void HandleEquipmentChange(PlayerInventory inv, EquippableItem newItem, EquippableItem oldItem)
    {
        if(oldItem != null)
        {
            foreach(StatBoost statBoost in oldItem.StatBoosts)
            {
                if(_playerStats.ActiveStatBoosts.Any(sb => sb.Equals(statBoost))) _playerStats.ActiveStatBoosts.Remove(_playerStats.ActiveStatBoosts.First(sb => sb.Equals(statBoost)));
            }
            foreach(ItemEffect itemEffect in oldItem.ItemEffects)
            {
                if (_activeItemEffects.Any(ie => ie.Equals(itemEffect))) _activeItemEffects.Remove(_activeItemEffects.First(ie => ie.Equals(itemEffect)));
            }
        }
        if(newItem != null)
        {
            foreach(StatBoost statBoost in newItem.StatBoosts)
            {
                _playerStats.ActiveStatBoosts.Add(statBoost);
            }
            foreach(ItemEffect itemEffect in newItem.ItemEffects)
            {
                _activeItemEffects.Add(itemEffect);
            }
        }
    }

    private void HandleAccessoryChange(PlayerInventory inv, Accessory[] oldAccessories)
    {
        foreach(Accessory accessory in oldAccessories)
        {
            if (accessory != null)
            {
                foreach (StatBoost statBoost in accessory.StatBoosts)
                {
                    if (_playerStats.ActiveStatBoosts.Any(sb => sb.Equals(statBoost))) _playerStats.ActiveStatBoosts.Remove(_playerStats.ActiveStatBoosts.First(sb => sb.Equals(statBoost)));
                }
                foreach (ItemEffect itemEffect in accessory.ItemEffects)
                {
                    if (_activeItemEffects.Any(ie => ie.Equals(itemEffect))) _activeItemEffects.Remove(_activeItemEffects.First(ie => ie.Equals(itemEffect)));
                }
            }
        }
        foreach(Accessory accessory in _playerInventory.Accessories)
        {
            if (accessory != null)
            {
                foreach (StatBoost statBoost in accessory.StatBoosts)
                {
                    _playerStats.ActiveStatBoosts.Add(statBoost);
                }
                foreach (ItemEffect itemEffect in accessory.ItemEffects)
                {
                    _activeItemEffects.Add(itemEffect);
                }
            }
        }
    }

    private void ProcItemEffects(ItemEffectTriggers trigger, GameObject activator, Bullet bullet, GameObject target, DamageEvent dmgEvent)
    {
        foreach (ItemEffect itemEffect in _activeItemEffects.Where(ie => ie.Trigger == trigger))
        {
            float chance = itemEffect.Chance;
            List<ItemEffect> itemEffectsInChain = new List<ItemEffect>();

            if (bullet != null)
            {
                if (bullet.PreviousEffectsInChain.Any(ie => ie.Equals(itemEffect))) break;

                chance *= bullet.ProcCoefficient;
                itemEffectsInChain = bullet.PreviousEffectsInChain;
            }

            if (UnityEngine.Random.Range(0, 100) <= itemEffect.Chance)
            {
                itemEffect.Proc(_player, activator, bullet, target, dmgEvent, _playerInventory, _playerStats, _bulletLauncher, itemEffectsInChain);
            }
        }
    }

    private void HandleOnHitEffects(BulletLauncher launcher, Bullet bullet, DamageReceiver dr, DamageEvent dmgEvent)
    {
        ProcItemEffects(ItemEffectTriggers.OnHit, _player, bullet, dr.gameObject, dmgEvent);

        if(dmgEvent.Crit) ProcItemEffects(ItemEffectTriggers.OnCrit, _player, bullet, dr.gameObject, dmgEvent);

        HealthDamageReceiver hdr = dr as HealthDamageReceiver;
        if(hdr != null && hdr.HealthPools.All(pool => pool.Health <= 0)) ProcItemEffects(ItemEffectTriggers.OnKill, _player, bullet, dr.gameObject, dmgEvent);

        if(bullet.DamageType == DamageType.Melee)
        {
            ProcItemEffects(ItemEffectTriggers.OnMeleeHit, _player, bullet, dr.gameObject, dmgEvent);

            if (dmgEvent.Crit) ProcItemEffects(ItemEffectTriggers.OnMeleeCrit, _player, bullet, dr.gameObject, dmgEvent);

            if (hdr != null && hdr.HealthPools.All(pool => pool.Health <= 0)) ProcItemEffects(ItemEffectTriggers.OnMeleeKill, _player, bullet, dr.gameObject, dmgEvent);
        }
        else if(bullet.DamageType == DamageType.Ranged)
        {
            ProcItemEffects(ItemEffectTriggers.OnRangedHit, _player, bullet, dr.gameObject, dmgEvent);

            if (dmgEvent.Crit) ProcItemEffects(ItemEffectTriggers.OnRangedCrit, _player, bullet, dr.gameObject, dmgEvent);

            if (hdr != null && hdr.HealthPools.All(pool => pool.Health <= 0)) ProcItemEffects(ItemEffectTriggers.OnRangedKill, _player, bullet, dr.gameObject, dmgEvent);
        }
        else if(bullet.DamageType == DamageType.Magic)
        {
            ProcItemEffects(ItemEffectTriggers.OnMagicHit, _player, bullet, dr.gameObject, dmgEvent);

            if (dmgEvent.Crit) ProcItemEffects(ItemEffectTriggers.OnMagicCrit, _player, bullet, dr.gameObject, dmgEvent);

            if (hdr != null && hdr.HealthPools.All(pool => pool.Health <= 0)) ProcItemEffects(ItemEffectTriggers.OnMagicKill, _player, bullet, dr.gameObject, dmgEvent);
        }
        else if(bullet.DamageType == DamageType.Summoning)
        {
            ProcItemEffects(ItemEffectTriggers.OnSummonHit, _player, bullet, dr.gameObject, dmgEvent);

            if (dmgEvent.Crit) ProcItemEffects(ItemEffectTriggers.OnSummonCrit, _player, bullet, dr.gameObject, dmgEvent);

            if (hdr != null && hdr.HealthPools.All(pool => pool.Health <= 0)) ProcItemEffects(ItemEffectTriggers.OnSummonKill, _player, bullet, dr.gameObject, dmgEvent);
        }
    }

    private void HandleOnShootEffects(BulletLauncher launcher, Bullet bullet)
    {
        if (bullet.CameFromEffect) return;

        ProcItemEffects(ItemEffectTriggers.OnShoot, _player, bullet, null, null);

        if(bullet.DamageType == DamageType.Melee) ProcItemEffects(ItemEffectTriggers.OnMeleeShoot, _player, bullet, null, null);
        else if (bullet.DamageType == DamageType.Ranged) ProcItemEffects(ItemEffectTriggers.OnRangedShoot, _player, bullet, null, null);
        else if (bullet.DamageType == DamageType.Magic) ProcItemEffects(ItemEffectTriggers.OnMagicShoot, _player, bullet, null, null);
        else if (bullet.DamageType == DamageType.Summoning) ProcItemEffects(ItemEffectTriggers.OnSummonShoot, _player, bullet, null, null);
    }
}
