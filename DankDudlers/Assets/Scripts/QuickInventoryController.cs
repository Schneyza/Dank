using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuickInventoryController : MonoBehaviour
{
    public GameObject quickSlotPrefab, quickItemPrefab;
    public InventoryController inventory;
    public bool created = false;

    int slotcount = 7;
    public Vector3[] slotPositions = new Vector3[7];
    public Vector2[] slotSizes = new Vector2[7];
    RectTransform[] slots = new RectTransform[7];
    Text itemName;
    public Transform selectedSlot;
    int currentSlot;
    bool lerped = false;
    int rightBorder;
    int leftBorder;

    void Awake()
    {
        createSlots();
        itemName = this.transform.GetChild(0).GetComponent<Text>();
    }
    // Use this for initialization
    void Start()
    {
        createItems(24);
        created = true;
    }

    // Update is called once per frame
    void Update()
    {
        handleSelection();
    }

    void handleSelection()
    {
        if (Input.GetButtonDown("360_right"))
        {
            rotateSlotsLeft();
        }
        if (Input.GetButtonDown("360_left"))
        {
            rotateSlotsRight();

        }
    }

    void rotateSlotsRight()
    {
        RectTransform[] result = new RectTransform[slotcount];
        for (int i = 0; i < slots.Length; i++)
        {
            int newPos = (i + 1 + slotcount) % slotcount;
            StartCoroutine(waitForLerp(slots[i], slotPositions[newPos]));
            slots[i].sizeDelta = slotSizes[newPos];
            if (slots[i].childCount >= 1)
            {
                slots[i].GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSizes[newPos].x - 10, slotSizes[newPos].y - 10);
            }
            result[newPos] = slots[i];
        }
        orderSlotHierarchy(result);
        slots = result;

        if (slots[0].childCount >= 1)
        {
            Destroy(slots[0].GetChild(0).gameObject);
        }
        if (leftBorder != 24)
        {
            addItem(0, leftBorder);
        }
        else
        {
            slots[0].GetComponent<SlotController>().position = 24;
        }

        leftBorder = (leftBorder - 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        leftBorder = inventory.nextItem(leftBorder, -1);
        rightBorder = (rightBorder - 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        rightBorder = inventory.nextItem(rightBorder, -1);
        selectedSlot = slots[currentSlot];
        updateQuickItemName();

    }

    void rotateSlotsLeft()
    {
        RectTransform[] result = new RectTransform[slotcount];
        for (int i = 0; i < slots.Length; i++)
        {
            int newPos = (i + -1 + slotcount) % slotcount;
            StartCoroutine(waitForLerp(slots[i], slotPositions[newPos]));
            slots[i].sizeDelta = slotSizes[newPos];
            if(slots[i].childCount >= 1)
            {
                slots[i].GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(slotSizes[newPos].x - 10, slotSizes[newPos].y - 10);
            }
            result[newPos] = slots[i];
        }
        orderSlotHierarchy(result);
        slots = result;

        if (slots[slots.Length - 1].childCount >= 1)
        {
            Destroy(slots[slots.Length - 1].GetChild(0).gameObject);
        }
        if (rightBorder != 24)
        {
            addItem(slots.Length - 1, rightBorder);
        }
        else
        {
            slots[slots.Length - 1].GetComponent<SlotController>().position = 24;
        }
        leftBorder = (leftBorder + 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        leftBorder = inventory.nextItem(leftBorder, 1);
        rightBorder = (rightBorder + 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        rightBorder = inventory.nextItem(rightBorder, 1);
        selectedSlot = slots[currentSlot];
        updateQuickItemName();

    }


    IEnumerator waitForLerp(RectTransform slot, Vector3 targetPos)
    {
        StartCoroutine(lerpSlot(slot, targetPos));
        while (!lerped)
        {
            yield return null;
        }

    }

    IEnumerator lerpSlot(RectTransform slot, Vector3 targetPos)
    {
        float duration = 5.0f;
        Vector3 origin = slot.localPosition;
        for (int i = 0; i < duration; i++)
        {
            slot.localPosition = Vector3.Lerp(origin, targetPos, (i + 1) / duration);
            yield return null;
        }
        lerped = true;
    }

    void createSlots()
    {
        for (int i = 0; i < slotcount; i++)
        {
            GameObject slot = Instantiate(quickSlotPrefab) as GameObject;
            slot.transform.SetParent(this.transform.GetChild(1));
            slot.name = "Slot_" + i;
            slot.GetComponent<RectTransform>().localPosition = slotPositions[i];
            slot.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            slot.GetComponent<RectTransform>().sizeDelta = slotSizes[i];
            slots[i] = slot.GetComponent<RectTransform>();
        }
        orderSlotHierarchy(slots);
    }

    void orderSlotHierarchy(RectTransform[] slots)
    {
        slots[0].SetSiblingIndex(1);
        slots[1].SetSiblingIndex(3);
        slots[2].SetSiblingIndex(5);
        slots[3].SetSiblingIndex(6);
        slots[4].SetSiblingIndex(4);
        slots[5].SetSiblingIndex(2);
        slots[6].SetSiblingIndex(0);
    }

    public void createItems(int middlePos)
    {
        if (created)
        {
            clearQuickItems();
        }
        leftBorder = inventory.nextItem(middlePos-1, -1);
        rightBorder = inventory.nextItem(middlePos, 1);
        addItem(2, leftBorder);
        leftBorder = (leftBorder - 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        leftBorder = inventory.nextItem(leftBorder, -1);
        addItem(1, leftBorder);
        leftBorder = (leftBorder - 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        leftBorder = inventory.nextItem(leftBorder, -1);
        addItem(0, leftBorder);
        leftBorder = (leftBorder - 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        leftBorder = inventory.nextItem(leftBorder, -1);
        addItem(3, rightBorder);
        rightBorder = (rightBorder + 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        rightBorder = inventory.nextItem(rightBorder, 1);
        addItem(4, rightBorder);
        rightBorder = (rightBorder + 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        rightBorder = inventory.nextItem(rightBorder, 1);
        addItem(5, rightBorder);
        rightBorder = (rightBorder + 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        rightBorder = inventory.nextItem(rightBorder, 1);
        addItem(6, rightBorder);
        rightBorder = (rightBorder + 1 + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
        rightBorder = inventory.nextItem(rightBorder, 1);


        currentSlot = 3;
        selectedSlot = slots[currentSlot];
        updateQuickItemName();
    }

    void clearQuickItems()
    {
        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].childCount >= 1)
            {
                Destroy(slots[i].GetChild(0).gameObject);
            }
        }
    }

    void updateQuickItemName()
    {
        if (selectedSlot.GetComponent<SlotController>().position != 24 && !inventory.isEmpty())
        {
            itemName.text = inventory.slotArray[selectedSlot.GetComponent<SlotController>().position].transform.GetChild(1).GetComponent<Item>().itemName;
        }
        else
        {
            itemName.text = "No Item";
        }
    }

    public void updateQuickAmounts()
    {
        for(int i=0; i<slots.Length; i++)
        {
            if(slots[i].GetComponent<SlotController>().position != 24 && !inventory.isEmpty())
            {
                slots[i].GetChild(0).GetChild(0).GetComponent<Text>().text = inventory.slotArray[slots[i].GetComponent<SlotController>().position].transform.GetChild(1).GetComponent<Item>().amount.ToString();
            }
        }
    }

    void addItem(int slotPos, int inventoryPos)
    {
        if ((inventoryPos) != 24 && !inventory.isEmpty())
        {
            Item it = inventory.slotArray[(inventoryPos + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1)].transform.GetChild(1).GetComponent<Item>();
            GameObject item = Instantiate(quickItemPrefab) as GameObject;
            item.transform.SetParent(slots[slotPos].transform);
            slots[slotPos].GetComponent<SlotController>().position = (inventoryPos + inventory.slotArray.Length + 1) % (inventory.slotArray.Length + 1);
            item.name = "item_" + (inventoryPos);
            item.GetComponent<RectTransform>().localPosition = Vector3.zero;
            item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            item.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSizes[slotPos].x - 10, slotSizes[slotPos].y - 10);
            item.GetComponent<Image>().sprite = it.GetComponent<Image>().sprite;
            item.transform.GetChild(0).GetComponent<Text>().text = it.amount.ToString();
        }
        else
        {
            slots[slotPos].GetComponent<SlotController>().position = inventoryPos;
        }
    }
}
