using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizedSinWaveMovement : BulletBehaviour
{
    [SerializeField]
    private float _minFrequency;
    [SerializeField]
    private float _maxFrequency;

    private float _frequency;

    [SerializeField]
    private float _minMagnitude;
    [SerializeField]
    private float _maxMagnitude;

    private float _magnitude;

    private float _timeAlive;
    private float _rotMult;
    private float _offset;

    private Vector2 _dir;

    private void OnEnable()
    {
        _magnitude = Random.Range(_minMagnitude, _maxMagnitude);
        _frequency = Random.Range(_minFrequency, _maxFrequency);

        _offset = transform.eulerAngles.z;

        _dir = transform.right;

        if (_magnitude >= 0f) _rotMult = Mathf.Clamp((_magnitude / 2) * 45f, 0f, 45f);
        else _rotMult = -Mathf.Clamp((_magnitude / 2) * 45f, 0f, 45f);
    }

    private void Update()
    {
        _timeAlive += Time.deltaTime;

        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, _offset + (-Mathf.Cos(_timeAlive * _frequency) * _rotMult)));
        transform.position = new Vector2(transform.position.x, transform.position.y) + _dir * Mathf.Cos(_timeAlive * _frequency) * (_magnitude * 0.01f);
    }
}
