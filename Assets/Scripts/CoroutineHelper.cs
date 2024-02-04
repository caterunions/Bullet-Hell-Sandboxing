using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    [Header("Object to help ScriptableObjects run coroutines.")]
    public static CoroutineHelper Instance;

    private void OnEnable()
    {
        Instance = this;
    }
}