using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BulletLauncher : MonoBehaviour
{
    public event Action<BulletLauncher, Bullet, DamageReceiver, DamageEvent> OnSpawnedBulletHit;
    public event Action<BulletLauncher, Bullet> OnLaunch;

    private EntityStats _stats;
    private EntityStats stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<EntityStats>();
            return _stats;
        }
    }

    public void Launch(PatternData pattern, float damageMultiplier)
    {
        float angleStep = pattern.Spread / pattern.Count;
        float aimAngle = pattern.FixedAngle == null ? transform.rotation.eulerAngles.z + pattern.AngleOffset : (float)pattern.FixedAngle + pattern.AngleOffset;
        float centeringOffset = (pattern.Spread / 2) - (angleStep / 2);

        for(int i = 0; i < pattern.Count; i++)
        {
            float currentBulletAngle = angleStep * i;

            aimAngle += UnityEngine.Random.Range(pattern.RandomAngleOffset * -1, pattern.RandomAngleOffset);

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, aimAngle + currentBulletAngle - centeringOffset));

            Vector3 position = transform.position;
            if (pattern.Position != null) position = (Vector3)pattern.Position;

            Bullet b = Instantiate(pattern.Bullet, position, rotation);

            float critChance = 0;
            float critPower = 1;

            PlayerStats pstats = stats as PlayerStats;
            if(pstats != null)
            {
                if(pattern.DamageType == DamageType.Neutral)
                {
                    critChance = pstats.CritChance;
                    critPower = pstats.CritPower;
                }
                else if (pattern.DamageType == DamageType.Melee)
                {
                    critChance = pstats.MeleeCritChance;
                    critPower = pstats.MeleeCritPower;
                }
                else if (pattern.DamageType == DamageType.Ranged)
                {
                    critChance = pstats.RangedCritChance;
                    critPower = pstats.RangedCritPower;
                }
                else if (pattern.DamageType == DamageType.Magic)
                {
                    critChance = pstats.MagicCritChance;
                    critPower = pstats.MagicCritPower;
                }
                else if (pattern.DamageType == DamageType.Summoning)
                {
                    critChance = pstats.SummonCritChance;
                    critPower = pstats.SummonCritPower;
                }
            }

            b.OnHit += TriggerSpawnedHitEvent;
            b.Initialize(transform.root.gameObject, this, pattern.Team, pattern.DamageType, damageMultiplier, stats.VelocityMultiplier, stats.RangeMultiplier, critChance, critPower, pattern.CameFromEffect, pattern.PreviousEffectsInChain);
            OnLaunch?.Invoke(this, b);
        }
    }

    private void TriggerSpawnedHitEvent(Bullet bullet, DamageReceiver dr, DamageEvent dmgEvent)
    {
        OnSpawnedBulletHit?.Invoke(this, bullet, dr, dmgEvent);
    }
}

public class PatternData
{
    public PatternData(Bullet bullet, int count, float spread, float angleOffset, float randomAngleOffset, DamageTeam team, DamageType type, bool cameFromEffect, List<ItemEffect> previousEffectsInChain, Vector3? position, float? fixedAngle)
    {
        _bullet = bullet;
        _count = count;
        _spread = spread;
        _angleOffset = angleOffset;
        _randomAngleOffset = randomAngleOffset;
        _team = team;
        _damageType = type;
        _position = position;
        _fixedAngle = fixedAngle;
        _cameFromEffect = cameFromEffect;
        _previousEffectsInChain = previousEffectsInChain;
    }

    private Bullet _bullet;
    public Bullet Bullet => _bullet;

    private int _count;
    public int Count => _count;

    private float _spread;
    public float Spread => _spread;

    private float _angleOffset;
    public float AngleOffset => _angleOffset;

    private float _randomAngleOffset;
    public float RandomAngleOffset => _randomAngleOffset;

    private DamageTeam _team;
    public DamageTeam Team => _team;

    private DamageType _damageType;
    public DamageType DamageType => _damageType;

    private bool _cameFromEffect;
    public bool CameFromEffect => _cameFromEffect;

    private List<ItemEffect> _previousEffectsInChain = new List<ItemEffect>();
    public List<ItemEffect> PreviousEffectsInChain => _previousEffectsInChain;

    private Vector3? _position;
    public Vector3? Position => _position;

    private float? _fixedAngle;
    public float? FixedAngle => _fixedAngle;
}
