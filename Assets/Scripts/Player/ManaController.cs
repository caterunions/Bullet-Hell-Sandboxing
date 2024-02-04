using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaController : MonoBehaviour
{
    private PlayerStats _stats;
    private PlayerStats stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<PlayerStats>();
            return _stats;
        }
    }

    private float _timeSinceManaUse = 0;

    private void OnEnable()
    {
        stats.OnManaChange += HandleManaChange;
    }

    private void OnDisable()
    {
        stats.OnManaChange -= HandleManaChange;
    }

    private void Update()
    {
        if(stats.CurrentMana < stats.MaxMana)
        {
            stats.RegainMana(stats.ManaRegen * Mathf.Lerp(1, stats.MaxManaRegenMultiplier, _timeSinceManaUse / stats.TimeToMaxManaRegen) * Time.deltaTime);
            _timeSinceManaUse += Time.deltaTime;
        }
    }

    private void HandleManaChange(PlayerStats playerStats, float manaChange)
    {
        if(manaChange < 0)
        {
            _timeSinceManaUse = 0;
        }
    }
}
