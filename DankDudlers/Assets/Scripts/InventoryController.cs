using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public Transform selectedSlot;
    public GameObject slotPrefab, itemPrefab;
    int pageSize = 8;
    public float Slot_xPos;
    public float slotSize;
    public Vector2 windowSize;

    public static GameObject[] slotArray;
    int currentSlot;
    int currentItem;

    public const string path = "items";

    ItemContainer ic;

    void Awake()
    {
        ic = ItemContainer.Load(path);
        slotArray = new GameObject[pageSize];
        for (int i = 1; i <= pageSize; i++)
        {
            GameObject slot = Instantiate(slotPrefab) as GameObject;
            slot.transform.SetParent(this.transform);
            slot.name = "Slot_" + i.ToString();
            slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(Slot_xPos, -i * (windowSize.y / (pageSize + 1)), 0);
            slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            slot.transform.GetChild(0).gameObject.SetActive(false);
            slotArray[i - 1] = slot;
            if (i <= ic.items.Count)
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.name = "item_" + i;
                item.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + ic.items[i - 1].spriteString);
                item.transform.GetChild(0).GetComponent<Text>().text = ic.items[i - 1].itemName;
                item.transform.GetChild(1).GetComponent<Text>().text = ic.items[i - 1].amount.ToString();
                item.transform.SetParent(slot.transform);
                item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }

        }

        
        currentSlot = 0;
        currentItem = 0;
        selectedSlot = this.transform.GetChild(1);
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = ic.items[currentItem].description;
        transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + ic.items[currentItem].spriteString);
        transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = ic.items[currentItem].rarity;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("360_down"))
        {
            selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
            if (currentSlot < slotArray.Length - 1)
            {
                currentSlot++;
                currentItem++;
            }
            else
            {
                currentSlot = 0;
                currentItem = 0;
            }
            selectedSlot = slotArray[currentSlot].transform;
            selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
            if (currentItem < ic.items.Count)
            {
                transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = ic.items[currentItem].description;
                transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + ic.items[currentItem].spriteString);
                transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = ic.items[currentItem].rarity;
            }
            else
            {
                transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "";
                transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = "";
            }
        }

        if (Input.GetButtonDown("360_up"))
        {
            selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
            if (currentSlot > 0)
            {
                currentSlot--;
                currentItem--;
            }
            else
            {
                currentSlot = slotArray.Length - 1;
                currentItem = slotArray.Length - 1;
            }
            selectedSlot = slotArray[currentSlot].transform;
            selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
            if (currentItem < ic.items.Count)
            {
                transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = ic.items[currentItem].description;
                transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + ic.items[currentItem].spriteString);
                transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = ic.items[currentItem].rarity;
            }
            else
            {
                transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "";
                transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
                transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = "";
            }
        }
    }
}
