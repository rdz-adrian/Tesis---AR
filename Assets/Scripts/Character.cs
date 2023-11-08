using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Transform picturePosition;
    public Transform initialPosition;

    [SerializeField] private float speed = 1f;
    [SerializeField] private float smoothRotationSpeed = 0.5f;
    [SerializeField] private int posesCount = 0;

    [Header("Animation Parameters Name")]
    [SerializeField] private string IdlePP = "Idle PP";
    [SerializeField] private string IdleSP = "Idle SP";
    [SerializeField] private string Movement = "Movement";
    [SerializeField] private string Pose = "Pose";

    private GameController GC;

    [Header("Movement Parameters")]
    public bool moveToPicturePosition = false;
    public bool moveToInitialPosition = false;
    public bool moveToOutOfViewPosition = false;

    private float distance;
    private float threshold = 0.05f; // este valor es para ajustar la posición al llegar al punto
    private Animator animator;
    private int poseAnim = 0;

    private void Start()
    {
        GC = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        Debug.Log("move to initial position " + moveToInitialPosition);
        Debug.Log("move to out position " + moveToOutOfViewPosition);
        Debug.Log("move to picture position " + moveToPicturePosition);

        checkMovement();
    }

    public void checkMovement()
    {
        if (moveToOutOfViewPosition)
        {
            Vector3 outOfViewPosition = picturePosition.GetChild(0).position;
            distance = Vector3.Distance(transform.position, outOfViewPosition);

            //Llegando a posición fuera de la pantalla
            if (distance <= threshold)
            {
                moveToOutOfViewPosition = false;
                //todo: resetear parametros para cuando vuelva a salir salga en el 
                //spawn position con todo funcionando bien
                GC.onReachedOutScreenPosition();
            }
            else WalkingTo(outOfViewPosition);
        }

        if (moveToInitialPosition)
        {
            distance = Vector3.Distance(transform.position, initialPosition.position);

            //Llegando a posición inicial
            if (distance <= threshold)
            {
                SetBooleanParameter(IdlePP);
                RotateTo(initialPosition.parent.transform.position, ref moveToInitialPosition);
            }
            else WalkingTo(initialPosition.position);
        }

        if (moveToPicturePosition)
        {
            distance = Vector3.Distance(transform.position, picturePosition.position);

            //Llegando a posición para la foto
            if (distance <= threshold)
            {
                SetBooleanParameter(IdleSP);
                if (poseAnim == 0)
                {
                    poseAnim = Mathf.RoundToInt(UnityEngine.Random.Range(1, posesCount + 1));
                    animator.SetFloat(Pose, poseAnim);
                }
                RotateTo(initialPosition.position, ref moveToPicturePosition);
            }
            else WalkingTo(picturePosition.position);
        }

    }

    void SetBooleanParameter(string parameterName)
    {
        // Verificar si el parámetro ya está establecido en true
        if (animator.GetBool(parameterName))
            return;

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

    public void RotateTo(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothRotationSpeed);
    }

    public void RotateTo(Vector3 target, ref bool param)
    {
        Vector3 direction = target - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothRotationSpeed);

        float angleDiff = Quaternion.Angle(transform.rotation, lookRotation);

        if (angleDiff <= threshold) param = false;
    }

    public void WalkingTo(Vector3 target)
    {

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        RotateTo(target);
        SetBooleanParameter(Movement);
        poseAnim = 0;
    }
}
