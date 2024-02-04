using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossBarDisplay : MonoBehaviour
{
    [SerializeField]
    private EnemyInfo _bossInfo;
    [SerializeField]
    private HealthPool _healthPool;
    [SerializeField]
    private Image _healthBar;
    [SerializeField]
    private TextMeshProUGUI _healthText;
    [SerializeField]
    private TextMeshProUGUI _bossNameText;
    [SerializeField]
    private Gradient _colorOverHealth;

    private void OnEnable()
    {
        _bossNameText.text = _bossInfo.Name;

        _healthPool.OnHPChange += UpdateHealthBar;

        UpdateHealthBar(_healthPool, 0);
    }

    private void OnDisable()
    {
        _healthPool.OnHPChange -= UpdateHealthBar;
    }

    private void UpdateHealthBar(HealthPool pool, float damage)
    {
        float hpPercentage = pool.Health / pool.MaxHealth;

        _healthText.text = $"{Mathf.Ceil(pool.Health)}/{Mathf.Ceil(pool.MaxHealth)}";

        _healthBar.fillAmount = hpPercentage;

        _healthBar.color = _colorOverHealth.Evaluate(hpPercentage);
    }
}
