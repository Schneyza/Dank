using UnityEngine;
using System.Collections;



public class basicCharController : MonoBehaviour {
    Animator anim;
    GameObject m_Camera;
    public GameObject m_Weapon;
    CapsuleCollider m_Capsule;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    GameObject char_back;       //Point where weapon is attached to character's back
    GameObject char_rightHand;  //Point where weapon is attached to character's hand

    
    float v;                     //Vertical Movement
    float h;                     //Horizontal Movement
    float sprint;
    float lookDir;
    bool weapon = false;        //isWeapon drawn?
    bool attack_vert = false;
    bool attack_hor = false;
    bool use_item = false;
    bool roll = false;
    bool rightBumper;           //blocking or sprinting
    bool moveable = true;       //can character move and rotate?

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
        m_Camera = GameObject.FindGameObjectWithTag("MainCamera");
        char_rightHand = GameObject.FindGameObjectWithTag("Weaponhand");
        char_back = GameObject.FindGameObjectWithTag("Weaponback");
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;
        lookDir = 0;
	}

    void Update()
    {
        if (moveable)
        {
            HandleMovAndRot();
        }
        HandleWeapon(); 
    }

    void FixedUpdate()
    {
        //set different variables in the AnimationController, that trigger animation transitions
        anim.SetFloat("Walk", v);
        anim.SetFloat("Turn", h);
        anim.SetBool("rightBumper", rightBumper);
        anim.SetBool("attack_vert", attack_vert);
        anim.SetBool("attack_hor", attack_hor);
        anim.SetBool("Roll", roll);
    }

    

    void HandleMovAndRot()
    {
        //get directional Input
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        //check if character is sprinting
        //determine rotation that the camera is facing, when camera angel is changing
        if (Input.GetKey(KeyCode.Mouse1))
        {
            lookDir = m_Camera.transform.eulerAngles.y;
        }
        else if(Input.GetJoystickNames() != null)
        {
            lookDir = m_Camera.transform.eulerAngles.y;
        }
        //rotate character relative to camera
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
        use_item = Input.GetButton("360_X");
        roll = Input.GetButton("360_A");
        //NOTE: next line handles sprinting & blocking+
        rightBumper = Input.GetButton("360_rb");
        //check if Draw Weapon
        if (attack_vert && !weapon)
        {
            weapon = true;
            //triggert Animationsübergang und dadurch Aufruf von DrawWeapon(); kein manueller Aufruf nötig!
            anim.SetBool("Drawn", true);
        }
        //check if Use Item
        if (use_item && !weapon)
        {
            //TODO implement according item categories and set "item" to equipped item category
            anim.SetInteger("item", 1);
            StartCoroutine(resetItem());
        }
        //Check if Sheathe Weapon
        if (use_item && weapon)
        {
            weapon = false;
            //triggert Animationsübergang und dadurch Aufruf von SheatheWeapon(); kein manueller Aufruf nötig!
            anim.SetBool("Drawn", false);
        }
        //ATTENTION: Use Item wird beim wegstecken der waffe momentan auch getriggert, was aber im moment nichts ausmacht, da die sheathe animation lange genug dauert
        //sollte keine probleme machen, solange alle item effekte durch animationen getriggert werden, ansonsten muss hier wahrscheinlich was gemacht werden (zusätzliche bool flag z.B.)
       

    }

    //parents weapon to character's hand; called by Draw Animation
    void DrawWeapon()
    {
        m_Weapon.transform.parent = null;
        m_Weapon.transform.SetParent(char_rightHand.transform);
    }
    //parents weapon to character's back; called by Sheathe Animation
    void SheatheWeapon()
    {
        m_Weapon.transform.parent = null;
        m_Weapon.transform.SetParent(char_back.transform);
    }
    
    //resets item categorie, so that it is not constantly used
    IEnumerator resetItem()
    {
        yield return new WaitForSeconds(0.2f);
        anim.SetInteger("item", 0);
    }

    //scales down the size of the collider will rolling, triggered by rolling animation
    IEnumerator scaleCollider(float animDur)
    {
        m_Capsule.height = m_Capsule.height /2f;
        m_Capsule.center = m_Capsule.center / 2f;
        yield return new WaitForSeconds(animDur);
        m_Capsule.height = m_CapsuleHeight;
        m_Capsule.center = m_CapsuleCenter;
    }

    //makes character unable to rotate during animation, called by animations
    //NOTE: not a nice way of doing, would be nicer if done with "while anim.getcurrentanimatorstate(0).isName" or something like that
    IEnumerator freezeMovement(float animDur)
    {
        moveable = false;
        yield return new WaitForSeconds(animDur);
        moveable = true;
    }
}
