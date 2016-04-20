using UnityEngine;
using System.Collections;



public class Animations : MonoBehaviour {
    Animator anim;
    GameObject m_Camera;
    public GameObject m_Weapon;
    GameObject char_back;
    GameObject char_rightHand;
    float v;
    float h;
    float sprint;
    public float lookDir;
    bool weapon = false;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera");
        char_rightHand = GameObject.FindGameObjectWithTag("Weaponhand");
        char_back = GameObject.FindGameObjectWithTag("Weaponback");
        lookDir = 0;
	}
	
	
	

    void Update()
    {
        HandleMovAndRot();
        if (Input.GetButtonDown("Draw") && !weapon)
        {
            weapon = true;
        
        }
        
    }

    void FixedUpdate()
    {
        anim.SetFloat("Walk", v);
        anim.SetFloat("Turn", h);
        anim.SetFloat("Sprint", sprint);
        anim.SetBool("Drawn", weapon);
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

    void DrawWeapon()
    {
        m_Weapon.transform.parent = null;
        m_Weapon.transform.SetParent(char_rightHand.transform);
    }
}
