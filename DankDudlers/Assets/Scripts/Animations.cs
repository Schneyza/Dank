using UnityEngine;
using System.Collections;



public class Animations : MonoBehaviour {
    Animator anim;
    GameObject m_Camera;
    float v;
    float h;
    float sprint;
    public float lookDir;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera");
        lookDir = 0;
	}
	
	
	

    void Update()
    {
        HandleMovAndRot();
        
    }

    void FixedUpdate()
    {
        anim.SetFloat("Walk", v);
        anim.SetFloat("Turn", h);
        anim.SetFloat("Sprint", sprint);
    }

    void sprinting()
    {
        if (Input.GetButton("Sprint") || Input.GetButton("360_rb"))
        {
            sprint = 0.2f;
        }
        else
        {
            sprint = 0.0f;
        }
    }

    void HandleMovAndRot()
    {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        sprinting();
        if (Input.GetKey(KeyCode.Mouse1))
        {
            lookDir = m_Camera.transform.eulerAngles.y;
        }
        else if(Input.GetJoystickNames() != null)
        {
            lookDir = m_Camera.transform.eulerAngles.y;
        }
        float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg + lookDir;
        if (Mathf.Abs(v) > 0.01 || Mathf.Abs(h) > 0.01)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z), 20f);

        }
    }
}
