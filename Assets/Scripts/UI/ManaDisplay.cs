using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ManaDisplay : MonoBehaviour
{
    [SerializeField]
    private PlayerStats _playerStats;
    [SerializeField]
    private Image _manaBar;
    [SerializeField]
    private TextMeshProUGUI _manaText;
    [SerializeField]
    private Gradient _colorOverMana;

    private void OnEnable()
    {
        _playerStats.OnManaChange += UpdateManaBar;

        UpdateManaBar(_playerStats, 0);
    }

    private void OnDisable()
    {
        _playerStats.OnManaChange -= UpdateManaBar;
    }

    private void UpdateManaBar(PlayerStats playerStats, float manaChange)
    {
        float manaPercentage = _playerStats.CurrentMana / _playerStats.MaxMana;

        _manaText.text = $"{Mathf.Ceil(_playerStats.CurrentMana)}/{Mathf.Ceil(_playerStats.MaxMana)}";

        _manaBar.fillAmount = manaPercentage;

        _manaBar.color = _colorOverMana.Evaluate(manaPercentage);
    }
}