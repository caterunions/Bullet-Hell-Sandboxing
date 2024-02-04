using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderHealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthPool _healthPool;
    [SerializeField]
    private SpriteRenderer _healthBar;
    [SerializeField]
    private Gradient _colorOverHealth;

    private void OnEnable()
    {
        if(_healthBar.drawMode != SpriteDrawMode.Tiled)
        {
            Debug.LogWarning("A health bar does not have its draw mode set to tiled.");
            _healthBar.drawMode = SpriteDrawMode.Tiled;
        }

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

        _healthBar.size = new Vector2(hpPercentage, 1);

        _healthBar.color = _colorOverHealth.Evaluate(hpPercentage);
    }
}
