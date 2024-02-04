using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceleratingForwardVelocity : BulletBehaviour
{
    [SerializeField]
    private float _startVelocity;

    [SerializeField] 
    private float _endVelocity;

    [SerializeField]
    private float _rampTime;

    private float _endTime;

    private void OnEnable()
    {
        _startVelocity *= bullet.VelocityMultiplier; 
        _endVelocity *= bullet.VelocityMultiplier;
        _endTime = Time.time + _rampTime;
    }

    private void Update()
    {
        rb.velocity = transform.up * Mathf.SmoothStep(_endVelocity, _startVelocity, (_endTime - Time.time) / _rampTime);
    }
}
