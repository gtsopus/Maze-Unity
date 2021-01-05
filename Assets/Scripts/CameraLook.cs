using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{

    public float cameraSensitivity = 100f;

    public Transform playerLoc;

    float xRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //lock cursor in the screen center
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        bool paused = GameObject.Find("Player(Clone)").GetComponent<Player>().paused;

        //mouse input
        if (!paused)
        {
            float mouseX = Input.GetAxis("Mouse X") * cameraSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * cameraSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            //limit rotation to maximum -50 to 50 degrees to avoid camera clipping through walls
            xRotation = Mathf.Clamp(xRotation, -47f, 47f);
            //rotations of each axis
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerLoc.Rotate(Vector3.up * mouseX);
        }
    }
}
