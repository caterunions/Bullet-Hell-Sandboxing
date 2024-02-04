using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyPhase : MonoBehaviour
{
    [SerializeField]
    private EnemyAction _phaseStartAction;

    [SerializeField]
    private List<EnemyAction> _actions = new List<EnemyAction>();

    private EnemyAction _curAction;
    private int _actionIndex = 0;

    private bool _performed = false;
    public bool Performed
    {
        get { return _performed; }
        set { _performed = value; }
    }

    private void OnEnable()
    {
        _actionIndex = 0;
        if (_phaseStartAction != null)
        {
            _curAction = _phaseStartAction;
            _curAction.Act();
        }
        else _curAction = _actions[_actionIndex];
    }

    private void OnDisable()
    {
        _curAction.Stop();
    }

    private void Update()
    {
        if (_curAction.InProgress == false)
        {
            if (_curAction != _actions[_actionIndex] && !_actions[_actionIndex].InProgress)
            {
                _curAction = _actions[_actionIndex];
            }

            _curAction.Act();

            if (_actionIndex < _actions.Count - 1) _actionIndex++;
            else _actionIndex = 0;
        }
    }
}
