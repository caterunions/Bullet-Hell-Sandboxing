using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public event Action<WeaponHolder> OnBeginFiring;
    public event Action<WeaponHolder> OnStopFiring;

    private Weapon _curWeapon;

    [SerializeField]
    private BulletLauncher _launcher;

    private bool _firing;
    public bool Firing => _firing;

    private Coroutine _fireRoutine = null;

    private PlayerStats _stats;
    private PlayerStats stats
    {
        get
        {
            if (_stats == null) _stats = GetComponentInParent<PlayerStats>();
            return _stats;
        }
    }

    public void StartFiring()
    {
        if(_fireRoutine == null)
        {
            _firing = true;
            _fireRoutine = StartCoroutine(FireRoutine());
            OnBeginFiring?.Invoke(this);
        }
    }

    public void StopFiring()
    {
        _firing = false;
        OnStopFiring?.Invoke(this);
    }

    public void ChangeWeapon(Weapon weapon)
    {
        _curWeapon = weapon;
        StopFiring();
    }

    private void Start()
    {
        ChangeWeapon(_curWeapon);
    }

    private IEnumerator FireRoutine()
    {
        if(_curWeapon is MeleeWeapon)
        {
            MeleeWeapon weapon = (MeleeWeapon) _curWeapon;

            while(_firing)
            {
                foreach (Attack attack in weapon.Attacks)
                {
                    int curCount = attack.Count;
                    float curSpread = attack.Spread;
                    float curAngleOffset = 0f;
                    for (int i = 0; i < attack.Repetitions; i++)
                    {
                        _launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset + UnityEngine.Random.Range(-attack.RandomAngleOffset, attack.RandomAngleOffset), DamageTeam.Player, weapon.DamageType, false, new List<ItemEffect>(), null, attack.StartAtFixedAngle ? attack.FixedAngle : null), stats.MeleeDamageMultipler);

                        curCount += attack.CountModifier;
                        curSpread += attack.SpreadModifier;
                        curAngleOffset += attack.AngleOffsetIncrease;

                        yield return new WaitForSeconds(attack.RepeatDelay / stats.AttackSpeed);

                        if (!_firing) break;
                    }

                    if (!_firing) break;

                    yield return new WaitForSeconds(attack.EndDelay / stats.AttackSpeed);
                }
            }

            _fireRoutine = null;
        }
        else if(_curWeapon is RangedWeapon)
        {
            RangedWeapon weapon = (RangedWeapon)_curWeapon;

            while (_firing)
            {
                foreach (Attack attack in weapon.Attacks)
                {
                    int curCount = attack.Count;
                    float curSpread = attack.Spread;
                    float curAngleOffset = 0f;
                    for (int i = 0; i < attack.Repetitions; i++)
                    {
                        if(stats.CurrentAmmo <= 0)
                        {
                            yield return new WaitUntil(() => stats.CurrentAmmo > 0);
                        } 
                        else
                        {
                            _launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset, DamageTeam.Player, weapon.DamageType, false, new List<ItemEffect>(), null, attack.StartAtFixedAngle ? attack.FixedAngle : null), stats.RangedDamageMultiplier);

                            curCount += attack.CountModifier;
                            curSpread += attack.SpreadModifier;
                            curAngleOffset += attack.AngleOffsetIncrease;

                            stats.ExpendAmmo(1);

                            yield return new WaitForSeconds(attack.RepeatDelay / stats.AttackSpeed);
                        }

                        if (!_firing) break;
                    }

                    if (!_firing) break;

                    yield return new WaitForSeconds(attack.EndDelay / stats.AttackSpeed);
                }
            }

            _fireRoutine = null;
        }
        else if(_curWeapon is MagicWeapon)
        {
            MagicWeapon weapon = (MagicWeapon)_curWeapon;

            while (_firing)
            {
                foreach (Attack attack in weapon.Attacks)
                {
                    int curCount = attack.Count;
                    float curSpread = attack.Spread;
                    float curAngleOffset = 0f;
                    for (int i = 0; i < attack.Repetitions; i++)
                    {
                        if (stats.CurrentMana < weapon.ManaCost)
                        {
                            yield return new WaitUntil(() => stats.CurrentMana >= weapon.ManaCost);
                        }
                        else
                        {
                            _launcher.Launch(new PatternData(attack.Bullet, curCount, curSpread, curAngleOffset, DamageTeam.Player, weapon.DamageType, false, new List<ItemEffect>(), null, attack.StartAtFixedAngle ? attack.FixedAngle : null), stats.MagicDamageMultipler);

                            curCount += attack.CountModifier;
                            curSpread += attack.SpreadModifier;
                            curAngleOffset += attack.AngleOffsetIncrease;

                            stats.DepleteMana(weapon.ManaCost);

                            yield return new WaitForSeconds(attack.RepeatDelay / stats.AttackSpeed);
                        }

                        if (!_firing) break;
                    }

                    if (!_firing) break;

                    yield return new WaitForSeconds(attack.EndDelay / stats.AttackSpeed);
                }
            }

            _fireRoutine = null;
        }
        else if(_curWeapon is SummoningWeapon)
        {

        }
    }
}
