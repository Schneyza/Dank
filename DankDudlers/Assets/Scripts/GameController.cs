using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public GameObject inventory;
    public GameObject quickInventory;
    bool isActive;
	// Use this for initialization
	void Start () {
        isActive = false;
        inventory.SetActive(isActive);
        quickInventory.SetActive(!isActive);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("360_start"))
        {
            isActive = !isActive;
            inventory.SetActive(isActive);
            quickInventory.SetActive(!isActive);
            if (!isActive)
            {
                quickInventory.GetComponent<QuickInventoryController>().createItems(quickInventory.GetComponent<QuickInventoryController>().selectedSlot.GetComponent<SlotController>().position);
            }
        }
	}
}
