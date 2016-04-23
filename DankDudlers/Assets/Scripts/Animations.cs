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
    float lookDir;
    bool weapon = false;
    bool attack_vert = false;
    bool attack_hor = false;

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
        HandleWeapon();
        
        
    }

    void FixedUpdate()
    {
        anim.SetFloat("Walk", v);
        anim.SetFloat("Turn", h);
        anim.SetFloat("Sprint", sprint);
        anim.SetBool("attack_vert", attack_vert);
        anim.SetBool("attack_hor", attack_hor);
    }

    void sprinting()
    {
        if (Input.GetButton("360_rb") || Input.GetButton("360_rb"))
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
    void HandleWeapon()
    {
        attack_vert = Input.GetButton("360_Y");
        attack_hor = Input.GetButton("360_B");
        if (attack_vert && !weapon)
        {
            weapon = true;
            //triggert Animationsübergang und dadurch Aufruf von DrawWeapon(); kein manueller Aufruf nötig!
            anim.SetBool("Drawn", true);
        }
        if (Input.GetButtonDown("360_X") && weapon)
        {
            weapon = false;
            //triggert Animationsübergang und dadurch Aufruf von SheatheWeapon(); kein manueller Aufruf nötig!
            anim.SetBool("Drawn", false);
        }
    }

    void DrawWeapon()
    {
        
        m_Weapon.transform.parent = null;
        m_Weapon.transform.SetParent(char_rightHand.transform);
        //kind of a hack, because there is no idel animation with weapon yet
        //anim.SetBool("Drawn", false);
    }
    void SheatheWeapon()
    {
        
        m_Weapon.transform.parent = null;
        m_Weapon.transform.SetParent(char_back.transform);
    }
}
