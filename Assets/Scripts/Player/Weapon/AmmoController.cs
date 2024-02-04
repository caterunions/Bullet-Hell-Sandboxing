using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoController : MonoBehaviour
{
    public event Action<AmmoController, RangedWeapon> OnReload;
    
    [SerializeField]
    private PlayerInventory _playerInventory;

    private PlayerStats _stats;
    private PlayerStats stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<PlayerStats>();
            return _stats;
        }
    }

    private Coroutine _refillRoutine;

    private RangedWeapon _curRangedWeapon;

    private void OnEnable()
    {
        _playerInventory.OnWeaponChanged += HandleWeaponChange;
        stats.OnAmmoChange += HandleAmmoChange;

        HandleWeaponChange(_playerInventory, null);
    }

    private void OnDisable()
    {
        _playerInventory.OnWeaponChanged -= HandleWeaponChange;
        stats.OnAmmoChange -= HandleAmmoChange;
    }

    private void HandleWeaponChange(PlayerInventory playerInv, Weapon old)
    {
        if (_playerInventory.Weapon == null) return;

        if(_playerInventory.Weapon is RangedWeapon)
        {
            _curRangedWeapon = (RangedWeapon)_playerInventory.Weapon;

            stats.SetMaxAmmo(_curRangedWeapon.Ammo);
            stats.SetAmmoImage(_curRangedWeapon.AmmoImage);
            HandleAmmoChange(stats, 0);
            Refill();
        } 
        else
        {
            stats.SetMaxAmmo(0);
            stats.SetAmmoImage(null);
        }
    }

    private void HandleAmmoChange(PlayerStats stats, int change)
    {
        if (_curRangedWeapon == null) return;
        if (stats.MaxAmmo == 0) return;
        if (stats.CurrentAmmo > 0) return;
        if (_refillRoutine != null) return;

        OnReload?.Invoke(this, _curRangedWeapon);
        _refillRoutine = StartCoroutine(RefillRoutine());
    }

    private void Refill()
    {
        if (_refillRoutine != null) return;

        _refillRoutine = StartCoroutine(RefillRoutine());
    }

    private IEnumerator RefillRoutine()
    {
        yield return new WaitForSeconds(_curRangedWeapon.ReloadTime);

        stats.RefillAmmo();

        _refillRoutine = null;
    }
}
