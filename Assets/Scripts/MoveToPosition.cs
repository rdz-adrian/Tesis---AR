using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float smoothRotationSpeed = 0.5f;

    [Header("Animation Parameters Name")]
    [SerializeField] private string IdlePP = "Idle PP";
    [SerializeField] private string IdleSP = "Idle SP";
    [SerializeField] private string Movement = "Movement";

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float distance;
    private float threshold = 0.05f;
    private PrimaryImageTarget primaryImageTarget;
    private Animator animator;

    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        primaryImageTarget = GameObject.FindGameObjectWithTag("PrimaryImageTarget").GetComponent<PrimaryImageTarget>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        checkMovement();
    }

    public void checkMovement()
    {
        distance = Vector3.Distance(transform.position, target.position);

        if (distance <= threshold)
        {
            SetBooleanParameter(IdleSP);
            RotateTo(primaryImageTarget.transform.position);


        }

        if (primaryImageTarget.moveToSecondaryPosition && distance > threshold)
        {
            RotateTo(target.position);
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            SetBooleanParameter(Movement);
        }
        if (!primaryImageTarget.moveToSecondaryPosition)
        {
            if (Vector3.Distance(transform.position, initialPosition) <= threshold)
            {
                SetBooleanParameter(IdlePP);
                RotateTo(initialRotation);
                RotateTo(initialRotation);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, initialPosition, speed * Time.deltaTime);
                RotateTo(initialPosition);
                SetBooleanParameter(Movement);
            }

        }
    }

    void SetBooleanParameter(string parameterName)
    {
        // Verificar si el parámetro ya está establecido en true
        if (animator.GetBool(parameterName))
            return;

        Debug.Log("Enbtra");

        int boolParameterCount = animator.parameterCount;

        for (int i = 0; i < boolParameterCount; i++)
        {
            AnimatorControllerParameter parameter = animator.GetParameter(i);
            if (parameter.type == AnimatorControllerParameterType.Bool)
            {
                animator.SetBool(parameter.nameHash, parameter.name.Equals(parameterName));
            }
        }
    }

    void RotateTo(Vector3 target)
    {
        //Rotation
        Vector3 direction = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothRotationSpeed);
    }

    void RotateTo(Quaternion rotation)
    {
        //Rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * (smoothRotationSpeed - smoothRotationSpeed * 0.3f));
    }
}
