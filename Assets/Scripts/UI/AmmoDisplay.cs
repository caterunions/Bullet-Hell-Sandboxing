using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerStats _playerStats;

    [SerializeField]
    private PlayerInventory _playerInventory;

    [SerializeField]
    private AmmoController _ammoController;

    [SerializeField]
    private TextMeshProUGUI _ammoText;

    [SerializeField]
    private Image _ammoIcon;

    [SerializeField]
    private Image _ammoIconBG;

    private Coroutine _refillRoutine;

    private void OnEnable()
    {
        _playerInventory.OnWeaponChanged += UpdateVisibility;
        _playerStats.OnAmmoChange += UpdateAmmoText;
        _playerStats.OnMaxAmmoChange += UpdateAmmoText;
        _playerInventory.OnEquipmentChanged += UpdateAmmoTextEquipment;
        _playerInventory.OnAccessoriesChanged += UpdateAmmoTextAccessories;
        _playerStats.OnAmmoIconChange += UpdateAmmoIcon;
        _ammoController.OnReload += RefillAnimation;

        UpdateVisibility(_playerInventory, null);
        UpdateAmmoIcon(_playerStats, null);
    }

    private void OnDisable()
    {
        _playerInventory.OnWeaponChanged -= UpdateVisibility;
        _playerStats.OnAmmoChange -= UpdateAmmoText;
        _playerStats.OnMaxAmmoChange -= UpdateAmmoText;
        _playerInventory.OnEquipmentChanged -= UpdateAmmoTextEquipment;
        _playerInventory.OnAccessoriesChanged -= UpdateAmmoTextAccessories;
        _playerStats.OnAmmoIconChange -= UpdateAmmoIcon;
        _ammoController.OnReload -= RefillAnimation;
    }

    private void UpdateVisibility(PlayerInventory playerInv, Weapon old)
    {
        if(_playerInventory.Weapon is RangedWeapon)
        {
            _ammoText.gameObject.SetActive(true);
            _ammoIcon.gameObject.SetActive(true);
            _ammoIconBG.gameObject.SetActive(true);
            UpdateAmmoText(_playerStats, 0);
            RefillAnimation(_ammoController, _playerInventory.Weapon as RangedWeapon);
        }
        else
        {
            _ammoText.gameObject.SetActive(false);
            _ammoIcon.gameObject.SetActive(false);
            _ammoIconBG.gameObject.SetActive(false);
        }
    }

    private void UpdateAmmoText(PlayerStats playerStats, int old)
    {
        _ammoText.text = $"{_playerStats.CurrentAmmo}/{_playerStats.MaxAmmo}";
    }

    private void UpdateAmmoTextEquipment(PlayerInventory playerInv, EquippableItem newItem, EquippableItem old)
    {
        _ammoText.text = $"{_playerStats.CurrentAmmo}/{_playerStats.MaxAmmo}";
    }

    private void UpdateAmmoTextAccessories(PlayerInventory playerInv, Accessory[] old)
    {
        _ammoText.text = $"{_playerStats.CurrentAmmo}/{_playerStats.MaxAmmo}";
    }

    private void UpdateAmmoIcon(PlayerStats playerStats, Sprite old)
    {
        if(_playerStats.AmmoImage == null)
        {
            _ammoIcon.sprite = null;
            _ammoIconBG.sprite = null;
            return;
        }

        _ammoIcon.sprite = _playerStats.AmmoImage;
        _ammoIconBG.sprite = _playerStats.AmmoImage;
    }

    private void RefillAnimation(AmmoController ammoController, RangedWeapon weapon)
    {
        if(_refillRoutine == null)
        {
            _refillRoutine = StartCoroutine(RefillRoutine(weapon.ReloadTime));
        }
    }

    private IEnumerator RefillRoutine(float reloadTime)
    {
        float endTime = Time.time + reloadTime;

        while(Time.time < endTime)
        {
            _ammoIcon.fillAmount = Mathf.Lerp(1, 0, (endTime - Time.time) / reloadTime);
            yield return null;
        }
        _ammoIcon.fillAmount = 1;

        _refillRoutine = null;
    }
}
