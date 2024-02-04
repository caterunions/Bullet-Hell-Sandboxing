using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Inventory", menuName = "Data/Player Inventory")]
public class PlayerInventory : ScriptableObject
{
    public event Action<PlayerInventory, Item[]> OnMiscInventoryChanged;
    public event Action<PlayerInventory, Weapon> OnWeaponChanged;
    public event Action<PlayerInventory, Ability> OnAbilityChanged;
    public event Action<PlayerInventory, Accessory[]> OnAccessoriesChanged;
    public event Action<PlayerInventory, Helmet> OnHelmetChanged;
    public event Action<PlayerInventory, ChestArmor> OnChestArmorChanged;
    public event Action<PlayerInventory, LegsArmor> OnLegsArmorChanged;
    public event Action<PlayerInventory, EquippableItem, EquippableItem> OnEquipmentChanged;

    [SerializeField]
    private Item[] _miscInventory = new Item[8];
    public Item[] MiscInventory => _miscInventory;

    [SerializeField]
    private Weapon _weapon;
    public Weapon Weapon => _weapon;

    [SerializeField]
    private Ability _ability;
    public Ability Ability => _ability;

    [SerializeField]
    private Accessory[] _accessories = new Accessory[4];
    public Accessory[] Accessories => _accessories;

    [SerializeField]
    private Helmet _helmet;
    public Helmet Helmet => _helmet;

    [SerializeField]
    private ChestArmor _chestArmor;
    public ChestArmor ChestArmor => _chestArmor;

    [SerializeField]
    private LegsArmor _legsArmor;
    public LegsArmor LegsArmor => _legsArmor;

    public void SetWeapon(Weapon weapon)
    {
        Weapon old = _weapon;
        _weapon = weapon;
        OnWeaponChanged?.Invoke(this, old);
        OnEquipmentChanged?.Invoke(this, weapon, old);
    }

    public void SetAbility(Ability ability)
    {
        Ability old = _ability;
        _ability = ability;
        OnAbilityChanged?.Invoke(this, old);
        OnEquipmentChanged?.Invoke(this, ability, old);
    }

    public void SetHelmet(Helmet helmet)
    {
        Helmet old = _helmet;
        _helmet = helmet;
        OnHelmetChanged?.Invoke(this, old);
        OnEquipmentChanged?.Invoke(this, helmet, old);
    }

    public void SetChestArmor(ChestArmor chestArmor)
    {
        ChestArmor old = _chestArmor;
        _chestArmor = chestArmor;
        OnChestArmorChanged?.Invoke(this, old);
        OnEquipmentChanged?.Invoke(this, chestArmor, old);
    }

    public void SetLegsArmor(LegsArmor legsArmor)
    {
        LegsArmor old = _legsArmor;
        _legsArmor = legsArmor;
        OnLegsArmorChanged?.Invoke(this, old);
        OnEquipmentChanged?.Invoke(this, legsArmor, old);
    }

    public void SetAccessorySlot(int index, Accessory accessory)
    {
        Accessory[] old = new Accessory[4];
        Array.Copy(_accessories, old, 4);
        _accessories[index] = accessory;
        OnAccessoriesChanged?.Invoke(this, old);
    }

    public void SetMiscItemSlot(int index, Item item)
    {
        Item[] old = _miscInventory;
        _miscInventory[index] = item;
        OnMiscInventoryChanged?.Invoke(this, old);
    }
}
