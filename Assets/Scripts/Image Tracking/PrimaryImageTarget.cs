using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryImageTarget : MonoBehaviour
{
    public bool moveToSecondaryPosition;
    void Start()
    {
        moveToSecondaryPosition = false;
    }

   
    void Update()
    {
        //TODO: Quitar esto
        if (Input.GetKeyDown(KeyCode.M))
        {
            moveToSecondaryPosition = !moveToSecondaryPosition;
        }
    }

}
