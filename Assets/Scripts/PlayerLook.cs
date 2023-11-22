using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;   // calculates camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0); // camera transform
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);   // rotates player when looking left and right


    }

}

