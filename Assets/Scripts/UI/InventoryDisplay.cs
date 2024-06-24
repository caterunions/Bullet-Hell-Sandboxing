using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField]
    private RectTransform _inventory;
    [SerializeField]
    private float _inventorySlideTime;
    [SerializeField]
    private float _inventoryWidth;

    private Coroutine _inventorySlideRoutine;

    private float _goal;

    private bool _inventoryDisplayed = false;

    private void OnEnable()
    {
        _goal = _inventoryWidth / 2;
        _inventory.anchoredPosition = new Vector2(_goal, _inventory.localPosition.y);
    }

    public void ToggleInventory()
    {
        _inventorySlideRoutine = null;
        _inventorySlideRoutine = StartCoroutine(InventorySlideRoutine(!_inventoryDisplayed));
    }

    private IEnumerator InventorySlideRoutine(bool show)
    {
        _inventoryDisplayed = show;
        if(show)
        {
            _goal = -(_inventoryWidth / 2);

            while(_inventory.anchoredPosition.x > _goal)
            {
                _inventory.anchoredPosition = new Vector2(_inventory.anchoredPosition.x - (_inventoryWidth * (Time.deltaTime / _inventorySlideTime)), _inventory.anchoredPosition.y);
                yield return null;
            }
            _inventory.anchoredPosition = new Vector2(_goal, _inventory.anchoredPosition.y);
        }
        else
        {
            _goal = _inventoryWidth / 2;

            while (_inventory.anchoredPosition.x < _goal)
            {
                _inventory.anchoredPosition = new Vector2(_inventory.anchoredPosition.x + (_inventoryWidth * (Time.deltaTime / _inventorySlideTime)), _inventory.anchoredPosition.y);
                yield return null;
            }
            _inventory.anchoredPosition = new Vector2(_goal, _inventory.localPosition.y);
        }
    }
}
