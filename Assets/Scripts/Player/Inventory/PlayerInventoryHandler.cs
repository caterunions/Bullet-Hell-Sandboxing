using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventoryHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory _inventory;

    [SerializeField]
    private WeaponHolder _weaponHolder;

    [SerializeField]
    private AbilityHolder _abilityHolder;

    private void OnEnable()
    {
        _inventory.OnWeaponChanged += HandleWeaponChange;
        _inventory.OnAbilityChanged += HandleAbilityChange;
        HandleWeaponChange(_inventory, null);
        HandleAbilityChange(_inventory, null);
    }

    private void OnDisable()
    {
        _inventory.OnWeaponChanged -= HandleWeaponChange;
        _inventory.OnAbilityChanged -= HandleAbilityChange;
    }

    private void HandleWeaponChange(PlayerInventory playerInv, Weapon oldWeapon)
    {
        _weaponHolder.ChangeWeapon(_inventory.Weapon);
    }

    private void HandleAbilityChange(PlayerInventory playerInv, Ability old)
    {
        _abilityHolder.ChangeAbility(_inventory.Ability);
    }
}
