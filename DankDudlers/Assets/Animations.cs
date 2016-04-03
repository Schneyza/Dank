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
        sprinting();
        transform.Translate(Vector3.forward * (Time.deltaTime * v));
        h = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up * Time.deltaTime * 200 *h);
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
        //anim.SetFloat("Turn", h);
        anim.SetFloat("Sprint", sprint);
    }

    void sprinting()
    {
        if (Input.GetButton("Fire1"))
        {
            sprint = 0.2f;
            v *= 4;
        }
        else
        {
            sprint = 0.0f;
        }
    }
}
