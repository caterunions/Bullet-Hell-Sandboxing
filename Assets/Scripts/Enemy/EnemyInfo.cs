using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInfo : MonoBehaviour
{
    [SerializeField]
    private string _name;
    public string Name => _name;

    [SerializeField]
    private bool _boss = false;
    public bool Boss => _boss;
}
