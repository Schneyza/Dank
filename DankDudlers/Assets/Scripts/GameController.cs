using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public GameObject inventory;
    public GameObject quickInventory;
    public GameObject player;
    bool inventoryActive;
	// Use this for initialization
	void Start () {
        inventoryActive = false;
        inventory.SetActive(inventoryActive);
        quickInventory.SetActive(!inventoryActive);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("360_start"))
        {
            inventoryActive = !inventoryActive;
            inventory.SetActive(inventoryActive);
            quickInventory.SetActive(!inventoryActive);
            player.GetComponent<basicCharController>().canAttack = !inventoryActive;
            if (!inventoryActive)
            {
                quickInventory.GetComponent<QuickInventoryController>().createItems(quickInventory.GetComponent<QuickInventoryController>().selectedSlot.GetComponent<SlotController>().position);
            }
        }
	}
}
