using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityEffect : ScriptableObject
{
    protected GameObject _player;
    protected PlayerInventory _playerInventory;
    protected PlayerStats _playerStats;
    protected BulletLauncher _bulletLauncher;

    public void ProvideContext(GameObject player, PlayerInventory playerInventory, PlayerStats playerStats, BulletLauncher bulletLauncher)
    {
        _player = player;
        _playerInventory = playerInventory;
        _playerStats = playerStats;
        _bulletLauncher = bulletLauncher;
    }

    public abstract void Activate();
}