using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantForwardVelocity : BulletBehaviour
{
    [SerializeField]
    private float _velocity = 1f;

    private void OnEnable()
    {
        _velocity *= bullet.VelocityMultiplier;
    }

    private void Update()
    {
        rb.velocity = transform.up * _velocity;
    }
}
