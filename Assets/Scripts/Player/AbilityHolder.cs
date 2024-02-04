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

    [SerializeField]
    private PlayerInventory _playerInventory;

    [SerializeField]
    private GameObject _player;

    private bool _usingAbility = false;
    public bool UsingAbility => _usingAbility;

    private float _curAbilityCooldown = 0;

    public void StartAbility()
    {
        if (_curAbility == null) return;
        if (_usingAbility) return;
        if (_curAbilityCooldown > 0) return;
        if (_stats.CurrentMana < _curAbility.ManaCost) return;
        
        _usingAbility = true;
        _stats.DepleteMana(_curAbility.ManaCost);

        foreach(AbilityEffect effect in _curAbility.AbilityStartEffects)
        {
            effect.ProvideContext(_player, _playerInventory, _stats, _launcher);
            effect.Activate();
        }
        
        if(_curAbility.CanBeHeldDown)
        {
            foreach (AbilitySustainEffect effect in _curAbility.AbilitySustainEffects)
            {
                effect.ProvideContext(_player, _playerInventory, _stats, _launcher);
                effect.Activate();
            }
        }
    }

    public void StopAbility()
    {
        if (_curAbility == null) return;
        if (!_usingAbility) return;

        _usingAbility = false;
        _curAbilityCooldown = _curAbility.Cooldown;

        if(_curAbility.CanBeHeldDown)
        {
            foreach (AbilitySustainEffect effect in _curAbility.AbilitySustainEffects)
            {
                effect.Cancel();
            }
            foreach (AbilityEffect effect in _curAbility.AbilityEndEffects)
            {
                effect.ProvideContext(_player, _playerInventory, _stats, _launcher);
                effect.Activate();
            }
        }
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
        if(_usingAbility && _curAbility.CanBeHeldDown)
        {
            if (_curAbility.ManaDrain * Time.deltaTime > _stats.CurrentMana) StopAbility();
            else _stats.DepleteMana(_curAbility.ManaDrain * Time.deltaTime);
        }
    }
}
