using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuickInventoryController : MonoBehaviour
{
    public GameObject quickSlotPrefab, quickItemPrefab;
    public InventoryController inventory;

    int slotcount = 7;
    public Vector3[] slotPositions = new Vector3[7];
    public Vector2[] slotSizes = new Vector2[7];
    RectTransform[] slots = new RectTransform[7];

    bool lerped = false;
    int rightBorder;
    int leftBorder;

    void Awake()
    {
        createSlots();
    }
    // Use this for initialization
    void Start()
    {
        createItems();
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
            slots = rotateSlots(-1);
        }
        if (Input.GetButtonDown("360_left"))
        {
            slots = rotateSlots(1);
        }
    }
    RectTransform[] rotateSlots(int dir)
    {
        RectTransform[] result = new RectTransform[slotcount];
        for (int i = 0; i < slots.Length; i++)
        {
            StartCoroutine(waitForLerp(slots[i], slotPositions[(i + dir + slotcount) % slotcount]));
            slots[i].sizeDelta = slotSizes[(i + dir + slotcount) % slotcount];
            result[(i + dir + slotcount) % slotcount] = slots[i];
        }
        orderSlotHierarchy(result);
        
        return result;
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
            slot.transform.SetParent(this.transform);
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
        slots[0].SetSiblingIndex(6);
        slots[1].SetSiblingIndex(5);
        slots[2].SetSiblingIndex(3);
        slots[3].SetSiblingIndex(1);
        slots[4].SetSiblingIndex(0);
        slots[5].SetSiblingIndex(2);
        slots[6].SetSiblingIndex(4);
    }

    void createItems()
    {
        Item it;
        rightBorder = 0;
        leftBorder = 23;
        for (int i = 0; i < slotcount; i++)
        {
            if (i <= slotcount / 2)
            {
                rightBorder = inventory.nextItem(rightBorder, 1);
                it = inventory.slotArray[rightBorder].transform.GetChild(1).GetComponent<Item>();
                rightBorder = (rightBorder + 1) % inventory.slotArray.Length;
                GameObject item = Instantiate(quickItemPrefab) as GameObject;
                item.transform.SetParent(slots[i].transform);
                item.name = "item_" + i;
                item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSizes[i].x - 10, slotSizes[i].y - 10);
                item.GetComponent<Image>().sprite = it.GetComponent<Image>().sprite;
                item.transform.GetChild(0).GetComponent<Text>().text = it.amount.ToString();
            }
            else
            {
                leftBorder = inventory.nextItem(leftBorder, -1);
                it = inventory.slotArray[leftBorder].transform.GetChild(1).GetComponent<Item>();
                leftBorder = (leftBorder - 1 + inventory.slotArray.Length) % inventory.slotArray.Length;
                int tmp = 10 - i;
                GameObject item = Instantiate(quickItemPrefab) as GameObject;
                item.transform.SetParent(slots[tmp].transform);
                item.name = "item_" + tmp;
                item.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                item.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                item.GetComponent<RectTransform>().sizeDelta = new Vector2(slotSizes[tmp].x - 10, slotSizes[tmp].y - 10);
                item.GetComponent<Image>().sprite = it.GetComponent<Image>().sprite;
                item.transform.GetChild(0).GetComponent<Text>().text = it.amount.ToString();
            }

        }
    }

}
