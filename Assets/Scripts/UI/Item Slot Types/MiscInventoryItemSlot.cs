using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscInventoryItemSlot : ItemSlot
{
    [SerializeField]
    private int _index;
    public int Index => _index;
}