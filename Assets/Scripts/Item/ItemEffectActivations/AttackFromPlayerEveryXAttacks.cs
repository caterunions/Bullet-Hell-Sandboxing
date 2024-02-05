using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack From Player Every X Attacks", menuName = "Item/Effect Activations/Attack From Player Every X Attacks")]
public class AttackFromPlayerEveryXAttacks : ItemEffectActivation
{
    [SerializeField]
    private Attack _attack;

    [SerializeField]
    private DamageType _damageType;

    [SerializeField]
    private int _attacksRequired;

    private int _counter = 0;

    private void Awake()
    {
        _counter = 0;
    }

    public override void Activate()
    {
        _counter++;
        if(_counter >= _attacksRequired)
        {
            CoroutineHelper.Instance.StartCoroutine(AttackRoutine());
            _counter = 0;
        }
    }

    private IEnumerator AttackRoutine()
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

            _bulletLauncher.Launch(new PatternData(_attack.Bullet, curCount, curSpread, curAngleOffset + Random.Range(-_attack.RandomAngleOffset, _attack.RandomAngleOffset), DamageTeam.Player, _damageType, true, _previousEffectsInChain, null, _attack.StartAtFixedAngle ? _attack.FixedAngle : null), damageMultiplier);

            curCount += _attack.CountModifier;
            curSpread += _attack.SpreadModifier;
            curAngleOffset += _attack.AngleOffsetIncrease;

            yield return new WaitForSeconds(_attack.RepeatDelay / _playerStats.AttackSpeed);
        }
    }
}
