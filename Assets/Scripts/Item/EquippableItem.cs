using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippableItem : Item
{
    [SerializeField]
    protected int _starReq;
    public int StarReq => _starReq;

    [SerializeField]
    protected ItemRarity _rarity;
    public ItemRarity Rarity => _rarity;

    [SerializeField]
    protected List<StatBoost> _statBoosts = new List<StatBoost>();
    public List<StatBoost> StatBoosts => _statBoosts;

    [SerializeField]
    protected List<ItemEffect> _itemEffects = new List<ItemEffect>();
    public List<ItemEffect> ItemEffects => _itemEffects;
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

[System.Serializable]
public class ItemEffect
{
    [Header("Use the format [ItemName]/[Activation] for unique identifiers")]
    [SerializeField]
    private string _uniqueIdentifier;
    public string UniqueIdentifier => _uniqueIdentifier;

    [SerializeField]
    private ItemEffectTriggers _trigger;
    public ItemEffectTriggers Trigger => _trigger;

    [SerializeField]
    private float _chance;
    public float Chance => _chance;

    [SerializeField]
    private List<ItemEffectActivation> _activations = new List<ItemEffectActivation>();
    public List<ItemEffectActivation> Activations => _activations;

    public void Proc(GameObject player, GameObject activator, Bullet bullet, GameObject target, DamageEvent dmgEvent, PlayerInventory playerInv, PlayerStats playerStats, BulletLauncher bulletLauncher, List<ItemEffect> previousEffectsInChain)
    {
        List<ItemEffect> effectsIncludingThis = previousEffectsInChain;
        effectsIncludingThis.Add(this);

        foreach(ItemEffectActivation activation in _activations)
        {
            activation.ProvideContext(player, activator, bullet, target, dmgEvent, playerInv, playerStats, bulletLauncher, effectsIncludingThis);
            activation.Activate();
        }
    }

    public override bool Equals(System.Object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            ItemEffect ie = (ItemEffect)obj;
            return (_uniqueIdentifier == ie.UniqueIdentifier);
        }
    }
}

public enum ItemEffectTriggers
{
    OnHit,
    OnKill,
    OnCrit,
    OnShoot,
    OnMeleeHit,
    OnMeleeKill,
    OnMeleeCrit,
    OnMeleeShoot,
    OnRangedHit,
    OnRangedKill,
    OnRangedCrit,
    OnRangedShoot,
    OnReload,
    OnMagicHit,
    OnMagicKill,
    OnMagicCrit,
    OnMagicShoot,
    OnSummonHit,
    OnSummonKill,
    OnSummonCrit,
    OnSummonShoot,
    OnSummon,
    OnAbilityUse,
    OnBuff,
    OnDebuff,
    OnTakeDamage,
    OnHeal
}