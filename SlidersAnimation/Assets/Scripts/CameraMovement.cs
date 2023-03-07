using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameObject cam;
    
    public float movSpeed = 3.5f;
    public float mouseSensitivity = 2f;
    public float xRot = 0;
    public float yRot = 0;
    private bool camLocked;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        Cursor.lockState = CursorLockMode.Locked;
        camLocked = true;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.mousePosition);
        /*
        Vector2 cameraVelocity = getMouseInput() * mouseSensitivity;
        camRotation += cameraVelocity * Time.deltaTime;
        cam.transform.LookAt() = new Vector3(-camRotation.y, -camRotation.x, 0);
        */
        if (camLocked)
        {
            Vector2 mouseInput = getMouseInput();
            yRot += mouseSensitivity * mouseInput.x;
            xRot -= mouseSensitivity * mouseInput.y;
            transform.eulerAngles = new Vector3(xRot, yRot, 0);
            //cam.transform.LookAt(cam.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.GetComponent<Camera>().nearClipPlane)),Vector3.up * mouseSensitivity);
            if (Input.GetKey(KeyCode.W))
            {
                cam.transform.Translate(Vector3.forward * movSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.S))
            {
                cam.transform.Translate(-Vector3.forward * movSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.A))
            {
                cam.transform.Translate(Vector3.left * movSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.D))
            {
                cam.transform.Translate(Vector3.right * movSpeed * Time.deltaTime, Space.World);
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (!camLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                camLocked = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                camLocked = false;
            }
        }
        
    }

    private Vector2 getMouseInput()
    {
        return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }
}
