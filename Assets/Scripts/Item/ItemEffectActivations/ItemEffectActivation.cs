using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffectActivation : ScriptableObject
{
    protected GameObject _player;
    protected GameObject _activator;
    protected Bullet _bullet;
    protected GameObject _target;
    protected DamageEvent _dmgEvent;
    protected PlayerInventory _playerInventory;
    protected PlayerStats _playerStats;
    protected BulletLauncher _bulletLauncher;
    protected List<ItemEffect> _previousEffectsInChain;

    public void ProvideContext(GameObject player, GameObject activator, Bullet bullet, GameObject target, DamageEvent dmgEvent, PlayerInventory playerInv, PlayerStats playerStats, BulletLauncher bulletLauncher, List<ItemEffect> previousEffectsInChain)
    {
        _player = player;
        _activator = activator;
        _bullet = bullet;
        _target = target;
        _dmgEvent = dmgEvent;
        _playerInventory = playerInv;
        _playerStats = playerStats;
        _bulletLauncher = bulletLauncher;
        _previousEffectsInChain = previousEffectsInChain;
    }

    public abstract void Activate();
}
