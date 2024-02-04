using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAtPlayer : EnemyAction
{
    [SerializeField]
    private float _velocity;
    public float Velocity => _velocity;

    [SerializeField]
    private List<float> _durations = new List<float>();
    public List<float> Durations => _durations;

    [SerializeField]
    private int _repetitions = 1;
    public int Repetitions => _repetitions;

    [SerializeField]
    private float _repeatDelay;
    public float RepeatDelay => _repeatDelay;

    [SerializeField]
    private bool _canAimDuringCharge = false;
    public bool CanAimDuringCharge => _canAimDuringCharge;

    [SerializeField]
    private SpriteRenderer _chargeWarning;

    private bool _wallHit = false;

    private void OnEnable()
    {
        EnemyBrain.OnCollisionEnter += HandleWallHit;
    }

    private void OnDisable()
    {
        EnemyBrain.OnCollisionEnter -= HandleWallHit;
    }

    protected override IEnumerator ActionInstructions()
    {
        for (int i = 0; i < Repetitions; i++)
        {
            if (_chargeWarning != null) _chargeWarning.enabled = true;

            yield return new WaitForSeconds(RepeatDelay);

            if (_chargeWarning != null) _chargeWarning.enabled = false;

            _wallHit = false;
            if(!CanAimDuringCharge) EnemyBrain.LockAimer();
            
            float endTime = Time.time + Durations[i];
            
            while(Time.time < endTime && !_wallHit)
            {
                EnemyBrain.Rb.velocity = EnemyBrain.Aimer.transform.up * Velocity;
                yield return null;
            }

            EnemyBrain.Rb.velocity = Vector2.zero;
            if(!CanAimDuringCharge) EnemyBrain.UnlockAimer();
        }
    }

    protected override void ExtraStopInstructions()
    {
        EnemyBrain.Rb.velocity = Vector2.zero;
        if (!CanAimDuringCharge) EnemyBrain.UnlockAimer();
        if (_chargeWarning != null) _chargeWarning.enabled = false;
    }

    private void HandleWallHit(EnemyBrain brain, Collision2D collision)
    {
        if(collision.collider.CompareTag("wall"))
        {
            _wallHit = true;
        }
    }
}