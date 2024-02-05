using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBundle : Bullet
{
    [SerializeField]
    private List<Bullet> _bullets = new List<Bullet>();

    protected override void Awake()
    {
        foreach(Bullet b in _bullets)
        {
            b.gameObject.SetActive(false);
        }
    }

    public override void Initialize(GameObject spawner, BulletLauncher launcher, DamageTeam team, DamageType type, float damageMultiplier, float velocityMultiplier, float rangeMultiplier, float critChance, float critPower, bool cameFromEffect, List<ItemEffect> previousEffectsInChain)
    {
        foreach(Bullet b in _bullets)
        {
            b.gameObject.SetActive(true);

            b.Initialize(spawner, launcher, team, type, damageMultiplier, velocityMultiplier, rangeMultiplier, critChance, critPower, cameFromEffect, previousEffectsInChain);

            b.transform.parent = null;
        }
        Destroy(gameObject);
    }
}
