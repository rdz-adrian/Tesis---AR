using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Nombres de los parametros de las animaciones")]
    [SerializeField] string primaryPosition;
    [SerializeField] string secondaryPosition;


    Animator animator;
    PrimaryImageTarget primaryImageTarget;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
