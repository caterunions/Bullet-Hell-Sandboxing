using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpawnDamageNumbersOnHit : MonoBehaviour
{
    private DamageReceiver _dr;
    protected DamageReceiver Dr
    {
        get
        {
            if(_dr == null) _dr = GetComponent<DamageReceiver>();
            return _dr;
        }
    }

    [SerializeField]
    private DamageNumber _damageNumberObj;

    private void OnEnable()
    {
        Dr.OnDamage += SpawnDamageNumber;
    }

    private void OnDisable()
    {
        Dr.OnDamage -= SpawnDamageNumber;
    }

    private void SpawnDamageNumber(DamageReceiver dr, DamageEvent dmgEvent)
    {
        Vector3 spawnPos = dmgEvent.SpecificSource.transform.position;

        DamageNumber dmgNum = Instantiate(_damageNumberObj, spawnPos, Quaternion.identity);

        if(dmgEvent.ArmorPiercing) dmgNum.Text.text = $"<color=#942dc4>-{Mathf.CeilToInt(dmgEvent.AppliedDamage)}";
        else dmgNum.Text.text = $"<color=#FF0000>-{Mathf.CeilToInt(dmgEvent.AppliedDamage)}";

        if (dmgEvent.Crit)
        {
            dmgNum.Text.fontSize = 10;
            dmgNum.Text.fontStyle = FontStyles.Bold | FontStyles.Italic;
            dmgNum.Text.text += "!";
        }
    }
}
