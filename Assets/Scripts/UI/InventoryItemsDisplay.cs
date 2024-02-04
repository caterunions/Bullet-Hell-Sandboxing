using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItemsDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory _playerInventory;

    [SerializeField]
    private ItemSlot _weaponSlot;

    [SerializeField]
    private ItemSlot _abilitySlot;

    [SerializeField]
    private ItemSlot _helmetSlot;

    [SerializeField]
    private ItemSlot _chestArmorSlot;

    [SerializeField]
    private ItemSlot _legsArmorSlot;

    [SerializeField]
    private ItemSlot[] _accessorySlots = new ItemSlot[4];

    [SerializeField]
    private ItemSlot[] _miscSlots = new ItemSlot[8];

    private void OnEnable()
    {
        _playerInventory.OnWeaponChanged += HandleWeaponChange;
        HandleWeaponChange(_playerInventory, null);

        _playerInventory.OnAbilityChanged += HandleAbilityChange;
        HandleAbilityChange(_playerInventory, null);

        _playerInventory.OnHelmetChanged += HandleHelmetChange;
        HandleHelmetChange(_playerInventory, null);

        _playerInventory.OnChestArmorChanged += HandleChestArmorChange;
        HandleChestArmorChange(_playerInventory, null);

        _playerInventory.OnLegsArmorChanged += HandleLegsArmorChange;
        HandleLegsArmorChange(_playerInventory, null);

        _playerInventory.OnAccessoriesChanged += HandleAccessoriesChange;
        HandleAccessoriesChange(_playerInventory, new Accessory[4]);

        _playerInventory.OnMiscInventoryChanged += HandleMiscInventoryChange;
        HandleMiscInventoryChange(_playerInventory, new Item[8]);
    }

    private void OnDisable()
    {
        _playerInventory.OnWeaponChanged -= HandleWeaponChange;

        _playerInventory.OnAbilityChanged -= HandleAbilityChange;

        _playerInventory.OnHelmetChanged -= HandleHelmetChange;

        _playerInventory.OnChestArmorChanged -= HandleChestArmorChange;

        _playerInventory.OnLegsArmorChanged -= HandleLegsArmorChange;

        _playerInventory.OnAccessoriesChanged -= HandleAccessoriesChange;

        _playerInventory.OnMiscInventoryChanged -= HandleMiscInventoryChange;
    }

    private void HandleWeaponChange(PlayerInventory playerInv, Weapon oldWeapon)
    {
        _weaponSlot.Item = _playerInventory.Weapon;
    }

    private void HandleAbilityChange(PlayerInventory playerInv, Ability oldAbility)
    {
        _abilitySlot.Item = _playerInventory.Ability;
    }

    private void HandleHelmetChange(PlayerInventory playerInv, Helmet oldHelmet)
    {
        _helmetSlot.Item = _playerInventory.Helmet;
    }

    private void HandleChestArmorChange(PlayerInventory playerInv, ChestArmor oldChestArmor)
    {
        _chestArmorSlot.Item = _playerInventory.ChestArmor;
    }

    private void HandleLegsArmorChange(PlayerInventory playerInv, LegsArmor oldLegsArmor)
    {
        _legsArmorSlot.Item = _playerInventory.LegsArmor;
    }

    private void HandleAccessoriesChange(PlayerInventory playerInv, Accessory[] oldAccessories)
    {
        for(int i = 0; i < _playerInventory.Accessories.Length; i++)
        {
            _accessorySlots[i].Item = _playerInventory.Accessories[i];
        }
    }

    private void HandleMiscInventoryChange(PlayerInventory playerInv, Item[] oldMiscInventory)
    {
        for (int i = 0; i < _playerInventory.MiscInventory.Length; i++)
        {
            _miscSlots[i].Item = _playerInventory.MiscInventory[i];
        }
    }
}
