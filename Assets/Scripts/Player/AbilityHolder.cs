using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    private Ability _curAbility;

    [SerializeField]
    private BulletLauncher _launcher;

    [SerializeField]
    private PlayerStats _stats;

    private bool _usingAbility = false;

    private float _curAbilityCooldown = 0;

    public void StartAbility()
    {
        if (_curAbility == null) return;
        if (_usingAbility) return;
        if (_curAbilityCooldown > 0) return;
        if (_stats.CurrentMana < _curAbility.ManaCost) return;
        
        _usingAbility = true;
        _stats.DepleteMana(_curAbility.ManaCost);
    }

    public void StopAbility()
    {
        if (_curAbility == null) return;
        if (!_usingAbility) return;

        _usingAbility = false;
        _curAbilityCooldown = _curAbility.Cooldown;
    }

    public void ChangeAbility(Ability ability)
    {
        _curAbility = ability;
        StopAbility();
    }

    private void Update()
    {
        if(_curAbilityCooldown > 0)
        {
            _curAbilityCooldown -= Time.deltaTime;
            if( _curAbilityCooldown < 0) _curAbilityCooldown = 0;
        }
    }
}
