using UnityEngine;
using System.Collections;



public class Animations : MonoBehaviour {
    Animator anim;
    float v;
    float h;
    float sprint;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");
        sprinting();
        float angle = Mathf.Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * Mathf.Rad2Deg;
        if (Mathf.Abs(v) > 0.01 || Mathf.Abs(h) > 0.01)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, angle, transform.rotation.z), 20f);
            //transform.rotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0, transform.rotation.y, 0), new Vector3(0, angle, 0), 0.7f));
            //transform.rotation = Quaternion.Euler(new Vector3(0, angle, 0));
        }
        

        //transform.Translate(Vector3.forward * (Time.deltaTime * v));

        //transform.Rotate(Vector3.up * Time.deltaTime * 200 *h);
        /*if (Input.GetKey("a"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime *-100);
        }
        if (Input.GetKey("d"))
        {
            transform.Rotate(Vector3.up * Time.deltaTime * (100));
        }*/
    }

    void FixedUpdate()
    {
        anim.SetFloat("Walk", v);
        anim.SetFloat("Turn", h);
        anim.SetFloat("Sprint", sprint);
    }

    void sprinting()
    {
        if (Input.GetButton("Fire1"))
        {
            sprint = 0.2f;
        }
        else
        {
            sprint = 0.0f;
        }
    }
}
