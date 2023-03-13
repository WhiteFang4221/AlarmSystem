using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntruderChecker : MonoBehaviour
{
    //private Alarm _alarm;
    public bool IsAlarmOn { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            IsAlarmOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IsAlarmOn = false;
        //_alarm.StartCoroutine(ChangeVolume)
    }
}
