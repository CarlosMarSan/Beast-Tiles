using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraManager : MonoBehaviour
{
    [SerializeField]
    private Camera cam;

    private Vector2 lastMousePosition;
    bool dragPanMoveActive = false;

    private void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragPanMoveActive = true;
            lastMousePosition = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragPanMoveActive = false;
        }

        if (dragPanMoveActive)
        {
            Vector2 difference = (Vector2)Input.mousePosition - lastMousePosition;
            Vector3 mouseMovementDelta;

            float dragPanSpeed = 0.1f;
            mouseMovementDelta.x = difference.x * dragPanSpeed;
            mouseMovementDelta.y = 0.0f;
            mouseMovementDelta.z = difference.y * dragPanSpeed;

            cam.transform.position -= mouseMovementDelta;

            lastMousePosition = Input.mousePosition;
        }
    }
    
    public void RotateLeft()
    {
        transform.Rotate(Vector3.up, 90, Space.Self);
    }

    public void RotateRight()
    {
        transform.Rotate(Vector3.up, -90, Space.Self);
    }
}
