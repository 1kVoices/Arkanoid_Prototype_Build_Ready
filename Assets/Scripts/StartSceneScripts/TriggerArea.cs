using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private bool _stop;
    void OnTriggerEnter(Collider other)
    {
        if (_stop) return;

        _stop = true;

        GameEvents.singleton.TriggerEnter();
    }
}
