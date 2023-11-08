using easyar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Note: The child object of the Image Target, the first GC
// should implement the ITargetEvents interface.

public class ImageTargetEventsHandler : MonoBehaviour
{
    [SerializeField]
    private MonoBehaviour targetScript;
    private ImageTargetController imageTracker;

    private void Start()
    {
        if (targetScript == null)
        {
            // If targetScript is not assigned manually, try to get the first child
            Transform firstChild = transform.GetChild(0);
            if (firstChild != null)
            {
                targetScript = firstChild.GetComponent<MonoBehaviour>();
            }
        }


        if (targetScript == null)
        {
            Debug.LogError("Target GC is not assigned in OnFoundLostTarget..");
            return;
        }

        imageTracker = GetComponent<ImageTargetController>();
        if (imageTracker == null)
        {
            Debug.LogError("ImageTargetController not found on this object.");
            return;
        }


        imageTracker.TargetLost += OnTargetLost;
        imageTracker.TargetFound += OnTargetFound;
    }

    private void OnTargetLost()
    {
        ITargetEvents targetEvents = targetScript as ITargetEvents;
        if (targetEvents != null)
        {
            targetEvents.OnTargetLost();
        }
    }

    private void OnTargetFound()
    {
        ITargetEvents targetEvents = targetScript as ITargetEvents;
        if (targetEvents != null)
        {
            targetEvents.OnTargetFound();
        }
    }
}
