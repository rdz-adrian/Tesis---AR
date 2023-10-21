using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float offset = 0;

    private Vector3 initialPosition;
    private float distance;
    private PrimaryImageTarget primaryImageTarget;

    private void Start()
    {
        initialPosition = transform.position;
        primaryImageTarget = GameObject.FindGameObjectWithTag("PrimaryImageTarget").GetComponent<PrimaryImageTarget>();

        Debug.Log(primaryImageTarget);
    }

    private void Update()
    {
        checkMovement();
    }

    public void checkMovement()
    {
        distance = Vector3.Distance(transform.position, target.position);
        if (primaryImageTarget.moveToSecondaryPosition && distance > offset)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            //transform.position += transform.parent.up * speed * Time.deltaTime;
        }
        if (!primaryImageTarget.moveToSecondaryPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
        }
    }
}
