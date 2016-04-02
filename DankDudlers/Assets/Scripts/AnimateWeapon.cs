using UnityEngine;
using System.Collections;

public class AnimateWeapon : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Attack"))
        {
            GetComponent<Animation>().Play("AttackAnimation");
        }
	}
}
