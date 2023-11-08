using easyar;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryImageTarget : MonoBehaviour
{
    private GameController GC;
    private ImageTargetController imageTracker;

    private void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        imageTracker = GetComponent<ImageTargetController>();
        imageTracker.TargetLost += OnTargetLost;
        imageTracker.TargetFound += OnTargetFound;
    }
    private void OnTargetLost()
    {

        GC.moveToPicturePosition = false;
    }

    private void OnTargetFound()
    {
        GC.moveToPicturePosition = true;
    }
}
