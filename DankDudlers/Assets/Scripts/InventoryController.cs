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

    Text detailDescription;
    Image detailSprite;
    Text detailRarity;

    public const string path = "items";

    ItemContainer ic;
    Dictionary<string, int> dict = new Dictionary<string, int>();
    Dictionary<int, Xml_Item> items = new Dictionary<int, Xml_Item>();

    void Awake()
    {
        //Initialize important variables
        ic = ItemContainer.Load(path);
        foreach (Xml_Item item in ic.items)
        {
            items.Add(item.id, item);
        }
        foreach (Xml_Item item in ic.items)
        {
            dict.Add(item.itemName, item.id);
        }
        slotArray = new GameObject[pageSize];
        detailDescription = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();
        detailSprite = transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>();
        detailRarity = transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();

        //create slots
        for (int i = 1; i <= pageSize; i++)
        {
            slotArray[i - 1] = addSlot(i);
        }

        //set selected slot to first slot
        currentSlot = 0;
        selectedSlot = this.transform.GetChild(1);
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        updateDetails();
    }
    // Use this for initialization
    void Start()
    {
        //fill inventory with dummy items
        //for(int i=100; i<124; i++)
        //{
        //    addItem(i);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("360_down"))
        {
            moveSelectionDown();
        }

        if (Input.GetButtonDown("360_up"))
        {
            moveSelectionUp();
        }
    }

    void moveSelectionDown()
    {
        selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
        if (currentSlot < slotArray.Length - 1)
        {
            currentSlot++;
        }
        else
        {
            currentSlot = 0;
        }
        selectedSlot = slotArray[currentSlot].transform;
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        updateDetails();
    }
    void moveSelectionUp()
    {
        selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
        if (currentSlot > 0)
        {
            currentSlot--;
        }
        else
        {
            currentSlot = slotArray.Length - 1;
        }
        selectedSlot = slotArray[currentSlot].transform;
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        updateDetails();
    }
    void updateDetails()
    {
        if(slotArray[currentSlot].transform.childCount >= 2)
        {
            detailDescription.text = slotArray[currentSlot].transform.GetChild(1).GetComponent<Item>().description;
            detailSprite.sprite = slotArray[currentSlot].transform.GetChild(1).GetComponent<Item>().sprite;
            detailRarity.text = slotArray[currentSlot].transform.GetChild(1).GetComponent<Item>().rarity;
        }
        else
        {
            transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>().text = "";
            transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().sprite = null;
            transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>().text = "";
        }
    }

    GameObject addSlot(int position)
    {
        GameObject slot = Instantiate(slotPrefab) as GameObject;
        slot.transform.SetParent(this.transform);
        slot.name = "Slot_" + (position-1).ToString();
        slot.GetComponent<SlotController>().position = position-1;
        slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(Slot_xPos, -position * (windowSize.y / (pageSize + 1)), 0);
        slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        slot.transform.GetChild(0).gameObject.SetActive(false);
        return slot;
    }

    public void addItem(int id) { 
        int newPos = this.firstEmptySlot();
        int curPos = this.containsItem(id);
        if (curPos == -1)
        {
            if (newPos!=-1)
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.SetParent(slotArray[newPos].transform);
                item.name = "item_" + item.transform.parent.GetComponent<SlotController>().position;
                item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Item.xmlToItem(item.GetComponent<Item>(), items[id]);
                Item i = item.GetComponent<Item>();
                item.GetComponent<Image>().sprite = i.sprite;
                item.transform.GetChild(0).GetComponent<Text>().text = i.itemName;
                item.transform.GetChild(1).GetComponent<Text>().text = i.amount.ToString();
            
            }
            else
            {
                Debug.Log("Inventory Full!!!");
            }
        }
        else
        {
            slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount += 1;
            slotArray[curPos].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount.ToString();
        }
        
    }

    public void addItem(int id, int amount)
    {
        int newPos = this.firstEmptySlot();
        int curPos = this.containsItem(id);
        if (curPos == -1)
        {
            if (newPos != -1)
            {
                GameObject item = Instantiate(itemPrefab) as GameObject;
                item.transform.SetParent(slotArray[newPos].transform);
                item.name = "item_" + item.transform.parent.GetComponent<SlotController>().position;
                item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Item.xmlToItem(item.GetComponent<Item>(), items[id]);
                item.GetComponent<Item>().amount = amount;
                Item i = item.GetComponent<Item>();
                item.GetComponent<Image>().sprite = i.sprite;
                item.transform.GetChild(0).GetComponent<Text>().text = i.itemName;
                item.transform.GetChild(1).GetComponent<Text>().text = i.amount.ToString();

            }
            else
            {
                Debug.Log("Inventory Full!!!");
            }
        }
        else
        {
            slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount += amount;
            slotArray[curPos].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount.ToString();
        }
    }

    public void addItem(string name)
    {
        if (dict.ContainsKey(name))
        {
            int id = dict[name];
            int newPos = this.firstEmptySlot();
            int curPos = this.containsItem(id);
            if (curPos == -1)
            {
                if (newPos != -1)
                {
                    GameObject item = Instantiate(itemPrefab) as GameObject;
                    item.transform.SetParent(slotArray[newPos].transform);
                    item.name = "item_" + item.transform.parent.GetComponent<SlotController>().position;
                    item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    Item.xmlToItem(item.GetComponent<Item>(), items[id]);
                    Item i = item.GetComponent<Item>();
                    item.GetComponent<Image>().sprite = i.sprite;
                    item.transform.GetChild(0).GetComponent<Text>().text = i.itemName;
                    item.transform.GetChild(1).GetComponent<Text>().text = i.amount.ToString();

                }
                else
                {
                    Debug.Log("Inventory Full!!!");
                }
            }
            else
            {
                slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount += 1;
                slotArray[curPos].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount.ToString();
            }
        }
        else
        {
            Debug.Log("Item '" + name + "' does not exist!");
        }
        updateDetails(); //Only für debug input field.. could probably be removed
    }

    public void addItem(string name, int amount)
    {
        if (dict.ContainsKey(name))
        {
            int id = dict[name];
            int newPos = this.firstEmptySlot();
            int curPos = this.containsItem(id);
            if (curPos == -1)
            {
                if (newPos != -1)
                {
                    GameObject item = Instantiate(itemPrefab) as GameObject;
                    item.transform.SetParent(slotArray[newPos].transform);
                    item.name = "item_" + item.transform.parent.GetComponent<SlotController>().position;
                    item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                    Item.xmlToItem(item.GetComponent<Item>(), items[id]);
                    item.GetComponent<Item>().amount = amount;
                    Item i = item.GetComponent<Item>();
                    item.GetComponent<Image>().sprite = i.sprite;
                    item.transform.GetChild(0).GetComponent<Text>().text = i.itemName;
                    item.transform.GetChild(1).GetComponent<Text>().text = i.amount.ToString();

                }
                else
                {
                    Debug.Log("Inventory Full!!!");
                }
            }
            else
            {
                slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount += amount;
                slotArray[curPos].transform.GetChild(1).GetChild(1).GetComponent<Text>().text = slotArray[curPos].transform.GetChild(1).GetComponent<Item>().amount.ToString();
            }
        }
        else
        {
            Debug.Log("Item '" + name + "' does not exist!");
        }
    }

    int containsItem(int id)
    {
        foreach(GameObject slot in slotArray)
        {
            if (slot.transform.childCount >= 2)
            {
                if(slot.transform.GetChild(1).GetComponent<Item>().id == id)
                {
                    return slot.GetComponent<SlotController>().position;
                }
            }
        }
        return -1;
    }

    int firstEmptySlot()
    {
        for(int i=0; i<slotArray.Length; i++)
        {
            if (slotArray[i].transform.childCount < 2)
            {
                return slotArray[i].GetComponent<SlotController>().position;
            }
        }
        return -1;
    }
}
