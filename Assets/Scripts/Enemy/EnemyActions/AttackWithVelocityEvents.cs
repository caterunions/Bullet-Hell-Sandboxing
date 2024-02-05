using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackWithVelocityEvents : EnemyAction
{
    [Header("Must be part of a grouped action")]
    [Header("All attacks must only have one repitition")]

    [SerializeField]
    private Attack _velocityStartAttack;

    [SerializeField]
    private Attack _velocitySustainAttack;

    [SerializeField]
    private float _velocityAttackRepeatDelay = 1;

    [SerializeField]
    private Attack _velocityEndAttack;

    [SerializeField]
    private BulletLauncher _launcher;

    [SerializeField]
    private EnemyAction _actionDurationToMatch;

    private Coroutine _sustainAttackRoutine;

    protected override IEnumerator ActionInstructions()
    {
        EnemyBrain.OnVelocityStart += HandleVelocityStart;
        EnemyBrain.OnVelocityEnd += HandleVelocityEnd;

        yield return new WaitUntil(() => !_actionDurationToMatch.InProgress);

        EnemyBrain.OnVelocityStart -= HandleVelocityStart;
        EnemyBrain.OnVelocityEnd -= HandleVelocityEnd;
    }

    protected override void ExtraStopInstructions()
    {
        EnemyBrain.OnVelocityStart -= HandleVelocityStart;
        EnemyBrain.OnVelocityEnd -= HandleVelocityEnd;
    }

    private void LaunchAttack(Attack attack)
    {
        _launcher.Launch(new PatternData(attack.Bullet, attack.Count, attack.Spread, Random.Range(attack.RandomAngleOffset * -1, attack.RandomAngleOffset), DamageTeam.Enemy, DamageType.Neutral, false, new List<ItemEffect>(), null, attack.StartAtFixedAngle ? attack.FixedAngle : null), Stats.DamageMultiplier);
    }

    private void HandleVelocityStart(EnemyBrain brain, Vector3 lastVelocity, Rigidbody2D rb)
    {
        _sustainAttackRoutine = StartCoroutine(SustainAttackRoutine());
        if (_velocityStartAttack != null) LaunchAttack(_velocityStartAttack);
    }

    private void HandleVelocityEnd(EnemyBrain brain, Vector3 lastVelocity, Rigidbody2D rb)
    {
        if(_sustainAttackRoutine != null) StopCoroutine(_sustainAttackRoutine);
        _sustainAttackRoutine = null;
        if (_velocityEndAttack != null) LaunchAttack(_velocityEndAttack);
    }

    private IEnumerator SustainAttackRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_velocityAttackRepeatDelay);

            if (_velocitySustainAttack != null) LaunchAttack(_velocitySustainAttack);
        }
    }
}
