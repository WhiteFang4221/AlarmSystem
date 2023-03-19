using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntruderChecker : MonoBehaviour
{
    public static Action Penetration;
    public static Action IntruderIsGone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Thief>(out Thief thief))
        {
            Penetration?.Invoke();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        IntruderIsGone?.Invoke();
    }
}
