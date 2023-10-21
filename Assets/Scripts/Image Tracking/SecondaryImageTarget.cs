using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryImageTarget : MonoBehaviour, ITargetEvents
{
    [SerializeField] private PrimaryImageTarget script;

    void ITargetEvents.OnTargetFound()
    {
        script.moveToSecondaryPosition = true;
    }

    void ITargetEvents.OnTargetLost()
    {
        script.moveToSecondaryPosition = false;
    }
}
