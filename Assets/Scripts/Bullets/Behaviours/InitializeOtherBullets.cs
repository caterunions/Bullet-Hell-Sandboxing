using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class InitializeOtherBullets : BulletBehaviour
{
    [SerializeField]
    private List<Bullet> _bullets = new List<Bullet>();

    private void OnEnable()
    {
        foreach (Bullet b in _bullets)
        {
            b.gameObject.SetActive(true);

            b.Initialize(bullet.Spawner, bullet.Launcher, bullet.Team, bullet.DamageType, bullet.DamageMultiplier, bullet.VelocityMultiplier, 1, bullet.CritChance, bullet.CritPower, bullet.CameFromEffect, bullet.PreviousEffectsInChain);
        }
    }
}
