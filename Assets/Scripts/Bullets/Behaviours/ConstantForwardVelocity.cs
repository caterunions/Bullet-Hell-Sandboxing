using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForwardVelocity : BulletBehaviour
{
    [SerializeField]
    private float _velocity = 1f;

    private Vector2 _dir;

    private void OnEnable()
    {
        _velocity *= bullet.VelocityMultiplier;

        _dir = transform.up;
    }

    private void Update()
    {
        rb.velocity = _dir * _velocity;
    }
}
