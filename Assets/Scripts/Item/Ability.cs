using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Item/Ability")]
public class Ability : EquippableItem
{
    [SerializeField]
    private float _manaCost;
    public float ManaCost => _manaCost;

    [SerializeField]
    private float _cooldown;
    public float Cooldown => _cooldown;

    [Header("Only AbilityStartEffects is required if not making a held ability.")]
    [SerializeField]
    private bool _canBeHeldDown;
    public bool CanBeHeldDown => _canBeHeldDown;

    [SerializeField]
    private float _manaDrain;
    public float ManaDrain => _manaDrain;

    [SerializeField]
    private List<AbilityEffect> _abilityStartEffects = new List<AbilityEffect>();
    public List<AbilityEffect> AbilityStartEffects => _abilityStartEffects;

    [SerializeField]
    private List<AbilityEffect> _abilitySustainEffects = new List<AbilityEffect>();
    public List<AbilityEffect> AbilitySustainEffects => _abilitySustainEffects;

    [SerializeField]
    private List<AbilityEffect> _abilityEndEffects = new List<AbilityEffect>();
    public List<AbilityEffect> AbilityEndEffects => _abilityEndEffects;
}