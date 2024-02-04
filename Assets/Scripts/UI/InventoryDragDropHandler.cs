using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryDragDropHandler : MonoBehaviour
{
    [SerializeField]
    private PlayerInventory _playerInventory;

    [SerializeField]
    private List<ItemSlot> _slots = new List<ItemSlot>();

    [SerializeField]
    private Image _dragIcon;

    [SerializeField]
    private TooltipDisplay _tooltipDisplay;

    private void OnEnable()
    {
        foreach(ItemSlot slot in _slots)
        {
            slot.OnBeginDragEvent += HandleBeginDrag;
            slot.OnEndDragEvent += HandleEndDrag;
            slot.OnDropEvent += HandleDrop;
            slot.OnPointerEnterEvent += HandlePointerEnter;
            slot.OnPointerExitEvent += HandlePointerExit;
        }
    }

    private void OnDisable()
    {
        foreach (ItemSlot slot in _slots)
        {
            slot.OnBeginDragEvent -= HandleBeginDrag;
            slot.OnEndDragEvent -= HandleEndDrag;
            slot.OnDropEvent -= HandleDrop;
            slot.OnPointerEnterEvent -= HandlePointerEnter;
            slot.OnPointerExitEvent -= HandlePointerExit;
        }
    }

    private bool _dragging = false;

    private ItemSlot _draggingFromSlot = null;

    private void HandleBeginDrag(ItemSlot slot, PointerEventData eventData)
    {
        if (slot.Item == null) return;
        if (_dragging) return;

        _dragging = true;
        _tooltipDisplay.Disable();
        _draggingFromSlot = slot;

        _dragIcon.sprite = _draggingFromSlot.Item.Sprite;
        _dragIcon.color = new Color(1, 1, 1, 0.5f);
    }

    private void HandleEndDrag(ItemSlot slot, PointerEventData eventData)
    {
        _dragging = false;

        _dragIcon.color = new Color(1, 1, 1, 0);
    }

    private void HandleDrop(ItemSlot slot, PointerEventData eventData)
    {
        if (_draggingFromSlot == null) return;
        
        if(slot.Item == null)
        {
            if (!CheckCompatibility(slot, _draggingFromSlot.Item)) return;

            SetItemInSlot(slot, _draggingFromSlot.Item);
            SetItemInSlot(_draggingFromSlot, null);

            _tooltipDisplay.transform.position = new Vector2(slot.transform.position.x - 7, slot.transform.position.y);
            _tooltipDisplay.Enable();
        }
        else
        {
            Item fromItem = _draggingFromSlot.Item;
            Item toItem = slot.Item;

            if (!CheckCompatibility(_draggingFromSlot, toItem)) return;
            if (!CheckCompatibility(slot, fromItem)) return;

            SetItemInSlot(slot, fromItem);
            SetItemInSlot(_draggingFromSlot, toItem);

            _tooltipDisplay.transform.position = new Vector2(slot.transform.position.x - 7, slot.transform.position.y);
            _tooltipDisplay.Enable();
        }
    }

    private void HandlePointerEnter(ItemSlot slot, PointerEventData eventData)
    {
        if (_dragging) return;
        if (slot.Item == null) return;

        _tooltipDisplay.transform.position = new Vector2(slot.transform.position.x - 7, slot.transform.position.y);
        _tooltipDisplay.SetValues(slot.Item);
        _tooltipDisplay.Enable();
    }

    private void HandlePointerExit(ItemSlot slot, PointerEventData eventData)
    {
        _tooltipDisplay.Disable();
    }

    private bool CheckCompatibility(ItemSlot slot, Item item)
    {
        if (slot is MiscInventoryItemSlot) return true;
        if (slot is WeaponItemSlot && item is Weapon) return true;
        if (slot is AbilityItemSlot && item is Ability) return true;
        if (slot is AccessoryItemSlot && item is Accessory && !_playerInventory.Accessories.Contains(item)) return true;
        if (slot is HelmetItemSlot && item is Helmet) return true;
        if (slot is ChestArmorItemSlot && item is ChestArmor) return true;
        if (slot is LegsArmorItemSlot && item is LegsArmor) return true;

        return false;
    }

    private void SetItemInSlot(ItemSlot slot, Item item)
    {
        slot.Item = item;

        if (slot is WeaponItemSlot) _playerInventory.SetWeapon(item as Weapon);
        else if (slot is AbilityItemSlot) _playerInventory.SetAbility(item as Ability);
        else if (slot is HelmetItemSlot) _playerInventory.SetHelmet(item as Helmet);
        else if (slot is ChestArmorItemSlot) _playerInventory.SetChestArmor(item as ChestArmor);
        else if (slot is LegsArmorItemSlot) _playerInventory.SetLegsArmor(item as LegsArmor);
        else if (slot is AccessoryItemSlot)
        {
            AccessoryItemSlot aslot = slot as AccessoryItemSlot;
            _playerInventory.SetAccessorySlot(aslot.Index, item as Accessory);
        }
        else if (slot is MiscInventoryItemSlot)
        {
            MiscInventoryItemSlot mslot = slot as MiscInventoryItemSlot;
            _playerInventory.SetMiscItemSlot(mslot.Index, item);
        }
    }
}
