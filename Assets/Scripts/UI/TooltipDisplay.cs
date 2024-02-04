using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject _background;

    [SerializeField]
    private TextMeshProUGUI _itemName;

    [SerializeField]
    private TextMeshProUGUI _rarityType;

    [SerializeField]
    private StarDisplay _starDisplay;

    [SerializeField]
    private Image _itemDisplay;

    [SerializeField]
    private TextMeshProUGUI _description;

    public void Enable()
    {
        _background.SetActive(true);
    }

    public void Disable()
    {
        _background.SetActive(false);
    }

    public void SetValues(Item item)
    {
        _itemName.text = item.DisplayName;

        _rarityType.text = "";
        EquippableItem equippableItem = item as EquippableItem;
        if(equippableItem != null)
        {
            switch(equippableItem.Rarity)
            {
                case ItemRarity.Common:
                    _rarityType.text += "<color=#d9b57c>Common ";
                    break;
                case ItemRarity.Uncommon:
                    _rarityType.text += "<color=#b7c1cc>Uncommon ";
                    break;
                case ItemRarity.Rare:
                    _rarityType.text += "<color=#ffeb57>Rare ";
                    break;
                case ItemRarity.Epic:
                    _rarityType.text += "<color=#ff6bee>Epic ";
                    break;
                case ItemRarity.Legendary:
                    _rarityType.text += "<color=#f5555d>Legendary ";
                    break;
                default:
                    _rarityType.text += "";
                    break;
            }
            if (equippableItem as MeleeWeapon != null) _rarityType.text += "<color=#bbbbbb>Melee Weapon";
            else if (equippableItem as RangedWeapon != null) _rarityType.text += "<color=#bbbbbb>Ranged Weapon";
            else if (equippableItem as MagicWeapon != null) _rarityType.text += "<color=#bbbbbb>Magic Weapon";
            else if (equippableItem as SummoningWeapon != null) _rarityType.text += "<color=#bbbbbb>Summoning Weapon";
            else if (equippableItem as Ability != null) _rarityType.text += "<color=#bbbbbb>Ability";
            else if (equippableItem as Accessory != null) _rarityType.text += "<color=#bbbbbb>Accessory";
            else if (equippableItem as Helmet != null) _rarityType.text += "<color=#bbbbbb>Helmet";
            else if (equippableItem as ChestArmor != null) _rarityType.text += "<color=#bbbbbb>Chest Armor";
            else if (equippableItem as LegsArmor != null) _rarityType.text += "<color=#bbbbbb>Leggings";
        }

        _starDisplay.Item = item as EquippableItem;

        _itemDisplay.sprite = item.Sprite;

        _description.text = item.Description;
    }
}
