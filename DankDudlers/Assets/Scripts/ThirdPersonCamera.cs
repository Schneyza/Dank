using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
    public float smooth = 1.5f;
    public float rotateSpeed = 50f;
    public Transform player;
    public float test;
    private Vector3 relCamPos;
    private Vector3 newPos;
    

    void Awake()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
        relCamPos = transform.position - player.position;
    }

    void FixedUpdate()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        newPos = player.position + relCamPos;
        //transform.position = Vector3.Lerp(transform.position, newPos, smooth * Time.deltaTime);
        transform.position = newPos;
    }

    void HandleRotation()
    {
        float right_x;
        float right_y;
        if (Input.GetKey(KeyCode.Mouse0))
        {
            transform.RotateAround(player.position, Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed);
            transform.RotateAround(player.position, transform.right, Input.GetAxis("Mouse Y") * Time.deltaTime * -rotateSpeed);
            relCamPos = transform.position - player.position;
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            transform.RotateAround(player.position, Vector3.up, Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed);
            player.Rotate(Vector3.up * Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed);
            transform.RotateAround(player.position, transform.right, Input.GetAxis("Mouse Y") * Time.deltaTime * -rotateSpeed);
            relCamPos = transform.position - player.position;
        }

        if (Input.GetJoystickNames() != null)
        {
            right_x = Input.GetAxis("360_right_x");
            right_y = Input.GetAxis("360_right_y");
            if (Mathf.Abs(right_x) > 0.2)
            {
                transform.RotateAround(player.position, Vector3.up, right_x * Time.deltaTime * -rotateSpeed);
            }
            if (Mathf.Abs(right_y) > 0.2)
            {
                transform.RotateAround(player.position, transform.right, right_y * Time.deltaTime * -rotateSpeed);
            }

            relCamPos = transform.position - player.position;
        }
    }
}
