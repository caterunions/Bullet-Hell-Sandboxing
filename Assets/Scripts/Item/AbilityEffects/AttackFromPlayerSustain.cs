using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack From Player Sustain", menuName = "Item/Ability Effects/Attack From Player Sustain")]
public class AttackFromPlayerSustain : AbilitySustainEffect
{
    [SerializeField]
    private Attack _attack;

    [SerializeField]
    private DamageType _damageType;

    private Coroutine _attackRoutine;

    public override void Activate()
    {
        _attackRoutine = CoroutineHelper.Instance.StartCoroutine(AttackRoutine());
    }

    public override void Cancel()
    {
        CoroutineHelper.Instance.StopCoroutine(_attackRoutine);
    }

    private IEnumerator AttackRoutine()
    {
        while(true)
        {
            int curCount = _attack.Count;
            float curSpread = _attack.Spread;
            float curAngleOffset = _attack.AngleOffsetStart;

            for (int i = 0; i < _attack.Repetitions; i++)
            {
                float damageMultiplier = 1f;
                if (_damageType == DamageType.Neutral) damageMultiplier = _playerStats.DamageMultiplier;
                else if (_damageType == DamageType.Melee) damageMultiplier = _playerStats.MeleeDamageMultipler;
                else if (_damageType == DamageType.Ranged) damageMultiplier = _playerStats.RangedDamageMultiplier;
                else if (_damageType == DamageType.Magic) damageMultiplier = _playerStats.MagicDamageMultipler;
                else if (_damageType == DamageType.Summoning) damageMultiplier = _playerStats.SummoningDamageMultiplier;

                _bulletLauncher.Launch(new PatternData(_attack.Bullet, curCount, curSpread, curAngleOffset, _attack.RandomAngleOffset, DamageTeam.Player, _damageType, true, new List<ItemEffect>(), null, _attack.StartAtFixedAngle ? _attack.FixedAngle : null), damageMultiplier);

                curCount += _attack.CountModifier;
                curSpread += _attack.SpreadModifier;
                curAngleOffset += _attack.AngleOffsetIncrease;

                yield return new WaitForSeconds(_attack.RepeatDelay / _playerStats.AttackSpeed);
            }

            yield return new WaitForSeconds(_repeatDelay);
        }
    }
}
