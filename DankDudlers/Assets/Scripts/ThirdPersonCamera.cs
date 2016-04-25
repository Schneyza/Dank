using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {
    public float smooth = 1.5f;
    public float rotateSpeed = 50f;
    public Transform player;
    private Vector3 relCamPos;
    private Vector3 newPos;
    float lb_dur;                   //how long has left bumper been pressed
    public float angleY;
    public float angleX;
    bool block_cam = false;
    public float test;
    

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
        test = relCamPos.magnitude;
        
        if (!block_cam)  
        {
            HandleMovement();
            HandleRotation();
        }
    }

    void HandleMovement()
    {
        newPos = player.position + relCamPos;
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

        if (Input.GetButton("360_lb"))
        {
            lb_dur += Time.deltaTime;
        }
        if (Input.GetButtonUp("360_lb"))
        {
            if(lb_dur < 0.4)
            {
                angleY = transform.rotation.eulerAngles.y - player.rotation.eulerAngles.y;
                angleX = transform.rotation.eulerAngles.x - player.rotation.eulerAngles.x;
                StartCoroutine(focusCamera());
            }
            lb_dur = 0;
        }
    }
    IEnumerator focusCamera()
    {
        block_cam = true;
        for(int i = 0; i < 5; i++)
        {
            newPos = player.position + relCamPos;
            transform.position = newPos;
            transform.RotateAround(player.position, Vector3.up, (-angleY) / 5);
            transform.RotateAround(player.position, transform.right, (24 - angleX)/5);
            relCamPos = transform.position - player.position;
            yield return null;
        }
        block_cam = false;
    }
}
