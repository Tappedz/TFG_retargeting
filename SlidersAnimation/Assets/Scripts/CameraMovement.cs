using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject cam;
    
    public float movSpeed = 0.5f;
    public float mouseSensitivity = 15f;
    public float xRot = 0f;
    public float yRot = 0f;
    public bool camLocked;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        camLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector2 cameraVelocity = getMouseInput() * mouseSensitivity;
        camRotation += cameraVelocity * Time.deltaTime;
        cam.transform.LookAt() = new Vector3(-camRotation.y, -camRotation.x, 0);
        */
        if (!camLocked)
        {
            Vector2 mouseInput = getMouseInput();
            yRot += mouseSensitivity * mouseInput.x;
            xRot -= mouseSensitivity * mouseInput.y;
            transform.localEulerAngles = new Vector3(xRot, yRot, 0);
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            cam.transform.position += (cam.transform.TransformDirection(Vector3.forward)*vertical + cam.transform.TransformDirection(Vector3.right)*horizontal)*movSpeed*Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (camLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                camLocked = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                camLocked = true;
            }
        }
        
    }

    private Vector2 getMouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
