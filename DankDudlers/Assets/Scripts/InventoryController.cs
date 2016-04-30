using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    //Variables that need to be set in editor
    public GameObject slotPrefab, itemPrefab;
    public Image detailSprite;
    public Text detailDescription;
    public Text detailRarity;
    public float Slot_xPos;
    public float slotSize;
    public Vector2 windowSize;

    //Other Variables
    GameObject[] slotArray;
    Transform selectedSlot;
    Transform originalSlot;
    Transform originalItem;
    Color originalColor;
    int pageSize = 8;
    int pages = 3;
    int currentSlot;
    int currentPage;
    bool swapping;

    //Item DB
    public const string path = "items";
    ItemContainer ic;
    Dictionary<string, int> dict = new Dictionary<string, int>();
    Dictionary<int, Xml_Item> items = new Dictionary<int, Xml_Item>();

    void Awake()
    {
        //Initialize important variables
        initializeVariables();

        //create slots
        createSlots();
        
    }
    // Use this for initialization
    void Start()
    {
        //fill inventory with dummy items
        //for (int i = 100; i < 124; i++)
        //{
        //    addItem(i);
        //}
        addItem("Potion");
        addItem("Mega Potion", 5);
        addItem(2);
        addItem(100, 4);

        updateDetails();

    }

    // Update is called once per frame
    void Update()
    {
        handleSelection();
        handleActions();
    }

    void handleActions()
    {
        if (Input.GetButtonDown("360_A"))
        {
            swapItems();
            updateDetails();
        }
        if (Input.GetButtonDown("360_X") && !swapping)
        {
            dropItem();
            updateDetails();
        }
        if(Input.GetButtonDown("360_Y") && !swapping)
        {
            sortInventory();
            updateDetails();
        }
    }

    void handleSelection()
    {
        if (Input.GetButtonDown("360_down"))
        {
            moveSelectionBy(1);
        }

        if (Input.GetButtonDown("360_up"))
        {
            moveSelectionBy(-1);
        }
        if (Input.GetButtonDown("360_right"))
        {
            moveActivePageBy(1);
        }
        if (Input.GetButtonDown("360_left"))
        {
            moveActivePageBy(-1);
        }
    }

    void swapItems()
    {
        if (selectedSlot.childCount >= 2 || originalItem!=null)
        {
            if (originalItem != null)
            {
                if (selectedSlot != originalSlot)
                {
                    if (selectedSlot.childCount < 2)
                    {
                        moveItemToEmptySlot(originalSlot, selectedSlot);
                        originalSlot.GetComponent<Image>().color = originalColor;
                        originalSlot = null;
                        originalItem = null;
                        swapping = false;
                    }
                    else {
                        swapItems(originalSlot, selectedSlot);
                        originalSlot.GetComponent<Image>().color = originalColor;
                        originalSlot = null;
                        originalItem = null;
                        swapping = false;
                    }
                }
                else
                {
                    originalSlot.GetComponent<Image>().color = originalColor;
                    originalSlot = null;
                    originalItem = null;
                    swapping = false;
                }

            }
            else
            {
                originalSlot = selectedSlot;
                originalItem = originalSlot.GetChild(1);
                originalSlot.GetComponent<Image>().color = Color.red;
                swapping = true;
            }
        }
    }

    void swapItems(Transform slot1, Transform slot2)
    {
        Transform i1 = slot1.GetChild(1);
        Transform i2 = slot2.GetChild(1);
        i1.SetParent(null); ;
        i2.SetParent(slot1);
        i1.SetParent(slot2);
        i1.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        i1.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        i2.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        i2.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
    }

    void moveItemToEmptySlot(Transform fromSlot, Transform toSlot)
    {
        if (toSlot.transform.childCount < 2)
        {
            Transform item = fromSlot.GetChild(1);
            item.SetParent(toSlot);
            item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    void dropItem()
    {
        if(selectedSlot.childCount >= 2)
        {
            Item item = selectedSlot.GetChild(1).GetComponent<Item>();
            if(item.amount <= 1)
            {
                Destroy(selectedSlot.GetChild(1).gameObject);
            }
            else
            {
                item.amount -= 1;
                item.transform.GetChild(1).GetComponent<Text>().text = item.amount.ToString();

            }
        }
    }

    void sortInventory()
    {
        moveItemsToBeginning();
        for(int i=slotArray.Length; i>1; i--)
        {
            for(int j=0; j<i-1; j++)
            {
                Transform cur = slotArray[j].transform;
                Transform next = slotArray[j + 1].transform;
                if(cur.childCount >= 2 && next.childCount >= 2)
                {
                    if(cur.GetChild(1).GetComponent<Item>().id > next.GetChild(1).GetComponent<Item>().id)
                    {
                        swapItems(cur, next);
                    }
                }

            }
        }
    }

    void moveItemsToBeginning()
    {
        int emptySlot = firstEmptySlot(); 
        if (emptySlot != -1) 
        {
            for (int i = slotArray.Length-1; i >= 0; i--)
            {
                if (slotArray[i].transform.childCount >= 2)
                {
                    if (slotArray[i].GetComponent<SlotController>().position > slotArray[emptySlot].GetComponent<SlotController>().position)
                    {
                        moveItemToEmptySlot(slotArray[i].transform, slotArray[emptySlot].transform);
                        emptySlot = firstEmptySlot();
                    }
                }
            }
        }

    }

    void moveSelectionBy(int by)
    {
        selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
        currentSlot = ((currentSlot + by + pageSize) % pageSize) + (pageSize * currentPage);
        selectedSlot = slotArray[currentSlot].transform;
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        updateDetails();
    }

    void moveActivePageBy(int by)
    {
        selectedSlot.transform.GetChild(0).gameObject.SetActive(false);
        for (int i = 0; i < pageSize; i++)
        {
            slotArray[i + pageSize * currentPage].SetActive(false);
            slotArray[i + pageSize * ((currentPage + by + pages) % pages)].SetActive(true);
        }
        currentPage = (currentPage + by + pages) % pages;
        currentSlot = (currentSlot + by * pageSize + pageSize * pages) % (pageSize * pages);
        selectedSlot = slotArray[currentSlot].transform;
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Inventory " + (currentPage+1) + "/" + pages;
        updateDetails();
    }

    void updateDetails()
    {
        if (slotArray[currentSlot].transform.childCount >= 2)
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

    GameObject addSlot(int slotPos, int pagePos)
    {
        int position = (slotPos + pageSize * pagePos);
        GameObject slot = Instantiate(slotPrefab) as GameObject;
        slot.transform.SetParent(this.transform);
        slot.name = "Slot_" + position.ToString();
        slot.GetComponent<SlotController>().position = position;
        slot.GetComponent<RectTransform>().anchoredPosition = new Vector3(Slot_xPos, -(slotPos + 1) * (windowSize.y / (pageSize + 1)), 0);
        slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        slot.transform.GetChild(0).gameObject.SetActive(false);
        return slot;
    }

    public void addItem(int id)
    {
        int amount = 1;
        addItem(id, amount);

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
                Debug.Log("Inventory Full! Could not add '" + items[id].itemName + "' to Inventory!");
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
        int amount = 1;
        addItem(name, amount);
    }

    public void addItem(string name, int amount)
    {
        if (dict.ContainsKey(name))
        {
            int id = dict[name];
            addItem(id, amount);
        }
        else
        {
            Debug.Log("Item '" + name + "' does not exist!");
        }
    }

    int containsItem(int id)
    {
        foreach (GameObject slot in slotArray)
        {
            if (slot.transform.childCount >= 2)
            {
                if (slot.transform.GetChild(1).GetComponent<Item>().id == id)
                {
                    return slot.GetComponent<SlotController>().position;
                }
            }
        }
        return -1;
    }

    int firstEmptySlot()
    {
        for (int i = 0; i < slotArray.Length; i++)
        {
            if (slotArray[i].transform.childCount < 2)
            {
                return slotArray[i].GetComponent<SlotController>().position;
            }
        }
        return -1;
    }

    void initializeVariables()
    {
        ic = ItemContainer.Load(path);
        foreach (Xml_Item item in ic.items)
        {
            items.Add(item.id, item);
        }
        foreach (Xml_Item item in ic.items)
        {
            dict.Add(item.itemName, item.id);
        }
        slotArray = new GameObject[pageSize * pages];
        originalColor = new Color(38/255f, 35/255f, 35/255f, 143/255f);
    }

    void createSlots()
    {
        for (int page = 0; page < pages; page++)
        {
            for (int slot = 0; slot < pageSize; slot++)
            {
                slotArray[slot + pageSize * page] = addSlot(slot, page);
                if (page > 0)
                {
                    slotArray[slot + pageSize * page].SetActive(false);
                }
            }
        }
        currentPage = 0;
        currentSlot = 0;
        selectedSlot = slotArray[currentSlot].transform;
        selectedSlot.transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(3).GetComponent<Text>().text = "Inventory " + (currentPage + 1) + "/" + pages;
        updateDetails();
    }

}
