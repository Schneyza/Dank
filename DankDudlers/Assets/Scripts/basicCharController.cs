using UnityEngine;
using System.Collections;



public class basicCharController : MonoBehaviour {
    Animator anim;
    GameObject m_Camera;
    public GameObject m_Weapon;
    //Point where weapon is attached to character's back
    GameObject char_back;
    //Point where weapon is attached to character's hand
    GameObject char_rightHand;
    //Vertical Movement
    float v;
    //Horizontal Movement
    float h;
    float sprint;
    float lookDir;
    //isWeapon drawn?
    bool weapon = false;
    //use vertical Attack? (could probably use refactring)
    bool attack_vert = false;
    //use horizontal Attack? (could probably use refactoring)
    bool attack_hor = false;
    //shall character use currently equipped item? NOTE: no info about the item contained here
    bool use_item = false;

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
        //set different variables in the AnimationController, that trigger animation transitions
        anim.SetFloat("Walk", v);
        anim.SetFloat("Turn", h);
        anim.SetFloat("Sprint", sprint);
        anim.SetBool("attack_vert", attack_vert);
        anim.SetBool("attack_hor", attack_hor);
    }

    //check if the character is printing
    void sprinting()
    {
        if (Input.GetButton("360_rb"))
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
        //get directional Input
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        //check if character is sprinting
        sprinting();
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
}
