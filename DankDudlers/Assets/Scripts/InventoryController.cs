using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour {
    public Transform selectedSlot;
    public GameObject slotPrefab, itemPrefab;
    int pageSize = 8;
    public float Slot_xPos;
    public float slotSize;
    public Vector2 windowSize;

    public static GameObject[] slotArray;
    int currentSlot;

    void Awake()
    {
        slotArray = new GameObject[pageSize];
        for(int i=1; i<= pageSize; i++)
        {
            GameObject slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.SetParent(this.transform);
            slot.name = "Slot_" + i.ToString();
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(Slot_xPos, -i * (windowSize.y / (pageSize + 1)), 0);
            slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            slot.transform.GetChild(1).gameObject.SetActive(false);
            slotArray[i-1] = slot;
            currentSlot = 0;
            selectedSlot = this.transform.GetChild(2);
            selectedSlot.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("360_down"))
        {
            selectedSlot.transform.GetChild(1).gameObject.SetActive(false);
            if (currentSlot < slotArray.Length-1) {
                currentSlot++;
            }
            else
            {
                currentSlot = 0;
            }
            selectedSlot = slotArray[currentSlot].transform;
            selectedSlot.transform.GetChild(1).gameObject.SetActive(true);
        }

        if (Input.GetButtonDown("360_up"))
        {
            selectedSlot.transform.GetChild(1).gameObject.SetActive(false);
            if (currentSlot > 0)
            {
                currentSlot--;
            }
            else
            {
                currentSlot = slotArray.Length-1;
            }
            selectedSlot = slotArray[currentSlot].transform;
            selectedSlot.transform.GetChild(1).gameObject.SetActive(true);
        }
    }
}
