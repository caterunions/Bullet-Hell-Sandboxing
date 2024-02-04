using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public event Action<ItemSlot, PointerEventData> OnBeginDragEvent;
    public event Action<ItemSlot, PointerEventData> OnEndDragEvent;
    public event Action<ItemSlot, PointerEventData> OnDropEvent;
    public event Action<ItemSlot, PointerEventData> OnPointerEnterEvent;
    public event Action<ItemSlot, PointerEventData> OnPointerExitEvent;

    [SerializeField]
    private Sprite _emptySlot;

    [SerializeField]
    private Sprite _fullSlot;

    [SerializeField]
    private Image _itemDisplay;

    [SerializeField]
    private Image _itemSlotImage;

    [SerializeField]
    private StarDisplay _starDisplay;

    [SerializeField]
    private Type _itemSlotType;

    private Item _item;
    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
            if (_item == null)
            {
                _itemDisplay.enabled = false;
                _itemSlotImage.sprite = _emptySlot;
                _starDisplay.Item = null;
            }
            else
            {
                _itemDisplay.enabled = true;
                _itemDisplay.sprite = _item.Sprite;
                _itemSlotImage.sprite = _fullSlot;
                _starDisplay.Item = _item as EquippableItem;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnBeginDragEvent?.Invoke(this, eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnEndDragEvent?.Invoke(this, eventData);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnDropEvent?.Invoke(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke(this, eventData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke(this, eventData);
    }
}