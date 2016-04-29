using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public int id;
    public string itemName;
    public string description;
    public int amount;
    public string rarity;
    public Sprite sprite;
    public Type type;
    public enum Type {consumable, throwable, placeable, misc};
    
    //public Item()
    //{

    //}

    //public Item(Xml_Item xml)
    //{
    //    Item item = new Item();
    //    item.id = xml.id;
    //    item.itemName = xml.itemName;
    //    item.amount = xml.amount;
    //    item.rarity = xml.rarity;
    //    item.sprite = Resources.Load<Sprite>("Sprites/" + xml.spriteString);
    //    item.type = stringToType(xml.typeString);
        
    //}
    public static void xmlToItem(Item target, Xml_Item source)
    {
        target.id = source.id;
        target.itemName = source.itemName;
        target.description = source.description;
        target.amount = source.amount;
        target.rarity = source.rarity;
        target.sprite = Resources.Load<Sprite>("Sprites/" + source.spriteString);
        target.type = stringToType(source.typeString);
    }

    public static Type stringToType(string typeString)
    {
        switch (typeString)
        {
            case "consumable":
                return Type.consumable;
            case "throwable":
                return Type.throwable;
            case "placeable":
                return Type.placeable;
            case "misc":
                return Type.misc;
            default:
                return Type.misc;
        }
    }

}
