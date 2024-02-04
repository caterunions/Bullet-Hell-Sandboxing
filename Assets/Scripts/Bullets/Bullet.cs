using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public event Action<Bullet, DamageReceiver, DamageEvent> OnHit;

    [SerializeField]
    private List<BulletBehaviour> _behaviours = new List<BulletBehaviour>();
    public List<BulletBehaviour> Behaviours => _behaviours;

    [SerializeField]
    private int _solidObjectPierceCount = 0;

    private GameObject _spawner;
    public GameObject Spawner => _spawner;

    [SerializeField]
    private float _baseDamage = 0;
    public float BaseDamage => _baseDamage;

    [SerializeField]
    private bool _armorPiercing = false;
    public bool ArmorPiercing => _armorPiercing;

    [SerializeField]
    private float _procCoefficient = 1;
    public float ProcCoefficient => _procCoefficient;

    [SerializeField]
    private float _range = 10;
    public float Range => _range;

    [SerializeField]
    private Collider2D _collider;

    public float DistanceTravelled
    {
        get
        {
            if (_origin == null) return 0;
            return Vector2.Distance(_origin, transform.position);
        }
    }

    private float _damage = 0;
    public float Damage => _damage;

    private float _critChance = 0;
    public float CritChance => _critChance;

    private float _critPower = 1;
    public float CritPower => _critPower;

    private bool _cameFromEffect;
    public bool CameFromEffect => _cameFromEffect;

    private List<ItemEffect> _previousEffectsInChain = new List<ItemEffect>();
    public List<ItemEffect> PreviousEffectsInChain => _previousEffectsInChain;

    private DamageTeam _team;
    public DamageTeam Team => _team;

    private DamageType _damageType;
    public DamageType DamageType => _damageType;

    private float _velocityMultiplier = 1f;
    public float VelocityMultiplier => _velocityMultiplier;

    private Vector2 _origin;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("wall"))
        {
            Destroy(gameObject);
        }

        DamageReceiver dr = collision.gameObject.GetComponentInParent<DamageReceiver>();
        if (dr != null)
        {
            // looks ugly but trust the process
            if (dr.Team != Team)
            {
                float damage = Damage;
                bool crit = false;

                if(UnityEngine.Random.Range(0, 100) <= CritChance)
                {
                    damage *= (1 + CritPower);
                    crit = true;
                }

                DamageEvent dmgEvent = new DamageEvent(damage, _spawner, gameObject, _armorPiercing, crit);
                dr.ReceiveDamage(dmgEvent);
                OnHit?.Invoke(this, dr, dmgEvent);

                if (!collision.isTrigger)
                {
                    if (_solidObjectPierceCount <= 0) Destroy(gameObject);
                    _solidObjectPierceCount--;
                }
            }
        }
        else if (!collision.isTrigger)
        {
            _solidObjectPierceCount--;
            if (_solidObjectPierceCount <= 0) Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        OnHit = null;
    }

    protected virtual void Awake()
    {
        _collider.enabled = false;
        Behaviours.ForEach(b => b.enabled = false);
        _origin = transform.position;
    }

    public virtual void Initialize(GameObject spawner, DamageTeam team, DamageType type, float damageMultiplier, float velocityMultiplier, float rangeMultiplier, float critChance, float critPower, bool cameFromEffect, List<ItemEffect> previousEffectsInChain)
    {
        _spawner = spawner;
        _team = team;
        _damageType = type;
        _damage = _baseDamage * damageMultiplier * UnityEngine.Random.Range(0.8f, 1.2f);
        _velocityMultiplier = velocityMultiplier;
        _range *= rangeMultiplier;
        _critChance = critChance;
        _critPower = critPower;
        _cameFromEffect = cameFromEffect;
        _previousEffectsInChain = previousEffectsInChain;

        _collider.enabled = true;
        Behaviours.ForEach(b => b.enabled = true);
    }

    private void Update()
    {
        if(DistanceTravelled >= _range) Destroy(gameObject);
    }
}