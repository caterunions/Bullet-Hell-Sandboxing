using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    [SerializeField]
    private Image _starDisplay;

    [SerializeField]
    private Sprite _zeroStars;

    [SerializeField]
    private Sprite[] _commonStars = new Sprite[5];

    [SerializeField]
    private Sprite[] _uncommonStars = new Sprite[5];

    [SerializeField]
    private Sprite[] _rareStars = new Sprite[5];

    [SerializeField]
    private Sprite[] _epicStars = new Sprite[5];

    [SerializeField]
    private Sprite[] _legendaryStars = new Sprite[5];

    private EquippableItem _item;
    public EquippableItem Item
    {
        set
        {
            _item = value;

            if(_item == null)
            {
                _starDisplay.color = new Color(1, 1, 1, 0);
            }
            else if (_item.StarReq == 0)
            {
                _starDisplay.color = new Color(1, 1, 1, 1);
                _starDisplay.sprite = _zeroStars;
            }
            else if (_item.Rarity == ItemRarity.Common)
            {
                _starDisplay.color = new Color(1, 1, 1, 1);
                _starDisplay.sprite = _commonStars[_item.StarReq - 1];
            }
            else if (_item.Rarity == ItemRarity.Uncommon)
            {
                _starDisplay.color = new Color(1, 1, 1, 1);
                _starDisplay.sprite = _uncommonStars[_item.StarReq - 1];
            }
            else if (_item.Rarity == ItemRarity.Rare)
            {
                _starDisplay.color = new Color(1, 1, 1, 1);
                _starDisplay.sprite = _rareStars[_item.StarReq - 1];
            }
            else if (_item.Rarity == ItemRarity.Epic)
            {
                _starDisplay.color = new Color(1, 1, 1, 1);
                _starDisplay.sprite = _epicStars[_item.StarReq - 1];
            }
            else if (_item.Rarity == ItemRarity.Legendary)
            {
                _starDisplay.color = new Color(1, 1, 1, 1);
                _starDisplay.sprite = _legendaryStars[_item.StarReq - 1];
            }
            else
            {
                _starDisplay.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
