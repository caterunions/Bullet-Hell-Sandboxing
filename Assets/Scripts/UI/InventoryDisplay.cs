using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour
{
    [SerializeField]
    private RectTransform _inventory;
    [SerializeField]
    private float _inventorySlideTime;

    private Coroutine _inventorySlideRoutine;

    private float _goal = 250;

    private bool _inventoryDisplayed = false;

    private void OnEnable()
    {
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
            _goal = -250;

            while(_inventory.anchoredPosition.x > _goal)
            {
                _inventory.anchoredPosition = new Vector2(_inventory.anchoredPosition.x - (500 * (Time.deltaTime / _inventorySlideTime)), _inventory.anchoredPosition.y);
                yield return null;
            }
            _inventory.anchoredPosition = new Vector2(_goal, _inventory.anchoredPosition.y);
        }
        else
        {
            _goal = 250;

            while (_inventory.anchoredPosition.x < _goal)
            {
                _inventory.anchoredPosition = new Vector2(_inventory.anchoredPosition.x + (500 * (Time.deltaTime / _inventorySlideTime)), _inventory.anchoredPosition.y);
                yield return null;
            }
            _inventory.anchoredPosition = new Vector2(_goal, _inventory.localPosition.y);
        }
    }
}
