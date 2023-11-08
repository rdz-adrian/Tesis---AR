using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutOfView : MonoBehaviour
{
    public Camera camera;
    public GameObject objectToCheck;
    public Transform objectToFollow;
    public float visibilityDelay = 1.0f; // Tiempo de retraso en segundos antes de considerar que el objeto está fuera de la vista.

    private bool isVisible = true;
    private float visibilityTimer = 0.0f;
    public float edgeOffset = 1.0f;

    void Start()
    {
        if (camera == null)
        {
            camera = Camera.main; // Usa la cámara principal si no se asigna ninguna.
        }
    }

    void Update()
    {
        if (objectToCheck != null)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);

            if (GeometryUtility.TestPlanesAABB(planes, objectToCheck.GetComponent<Renderer>().bounds))
            {
                if (!isVisible)
                {
                    visibilityTimer = Time.time + visibilityDelay;
                    isVisible = true;
                    Debug.Log("The object is inside the camera's view.");
                }
            }
            else
            {
                if (isVisible && Time.time >= visibilityTimer)
                {
                    isVisible = false;
                    //objectToCheck.SetActive(false);
                    Debug.Log("The object is now considered outside of the camera's view with a delay.");
                }
            }
        }

        if (objectToFollow != null)
        {
            Vector3 objectViewportPosition = camera.WorldToViewportPoint(objectToFollow.position);

            float xPosition = Mathf.Clamp(objectViewportPosition.x, edgeOffset, 1.0f - edgeOffset);
            float yPosition = objectToFollow.position.y; // Mantén la posición en Y del objeto sin cambios.

            Vector3 newPosition = camera.ViewportToWorldPoint(new Vector3(xPosition, objectViewportPosition.y, objectViewportPosition.z));
            objectToFollow.position = newPosition;
        }
    }
}
