using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBrain : MonoBehaviour
{
    public event Action<EnemyBrain, Collision2D> OnCollisionEnter;
    public event Action<EnemyBrain, Vector3, Rigidbody2D> OnVelocityStart;
    public event Action<EnemyBrain, Vector3, Rigidbody2D> OnVelocityEnd;

    [System.Serializable]
    private struct PhaseObj
    {
        [SerializeField]
        private EnemyPhase _phase;
        public EnemyPhase Phase => _phase;

        [SerializeField]
        private float _healthTrigger;
        public float HealthTrigger => _healthTrigger;
    }

    private Rigidbody2D _rb;
    public Rigidbody2D Rb
    {
        get
        {
            if(_rb == null) _rb = GetComponent<Rigidbody2D>();
            return _rb;
        }
    }

    private HealthPool _healthPool;
    protected HealthPool HealthPool
    {
        get
        {
            if(_healthPool == null) _healthPool = GetComponent<HealthPool>();
            return _healthPool;
        }
    }

    [SerializeField]
    private List<PhaseObj> _phases = new List<PhaseObj>();

    private EnemyPhase _curPhase;

    [SerializeField]
    private GameObject _player;
    public GameObject Player => _player;

    [SerializeField]
    private EnemyAim _aimer;
    public EnemyAim Aimer => _aimer;

    private Vector3 _lastVelocity;

    private void Awake()
    {
        Aimer.Player = Player.transform;
    }

    private void OnEnable()
    {
        _phases.ForEach(p => p.Phase.Performed = false);
        ChangePhase(_phases.First().Phase);

        HealthPool.OnHPChange += HandlePhaseChange;
    }

    private void OnDisable()
    {
        HealthPool.OnHPChange -= HandlePhaseChange;
    }

    private void ChangePhase(EnemyPhase phase)
    {
        if (_phases.FirstOrDefault(p => p.Phase == phase).Equals(null)) return;
        
        phase.Performed = true;

        _phases.ForEach(p => p.Phase.enabled = false);
        _curPhase = phase;
        _curPhase.enabled = true;
    }

    private void HandlePhaseChange(HealthPool pool, float prev)
    {
        foreach(PhaseObj p in _phases)
        {
            if(!p.Phase.Performed && p.HealthTrigger >= HealthPool.Health)
            {
                ChangePhase(p.Phase);
                break;
            }
        }
    }

    public void LockAimer()
    {
        Aimer.Locked = true;
    }

    public void UnlockAimer()
    {
        Aimer.Locked = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnCollisionEnter?.Invoke(this, collision);
    }

    private void Update()
    {
        Vector3 velocity = Rb.velocity;
        
        if(_lastVelocity == Vector3.zero && velocity.sqrMagnitude > 0)
        {
            OnVelocityStart?.Invoke(this, _lastVelocity, Rb);
        }
        else if(_lastVelocity.sqrMagnitude > 0 && velocity == Vector3.zero)
        {
            OnVelocityEnd?.Invoke(this, _lastVelocity, Rb);
        }

        _lastVelocity = velocity;
    }
}
