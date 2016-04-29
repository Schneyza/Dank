using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;

public class Xml_Item  {

    [XmlAttribute("id")]
    public int id;

    [XmlElement("ItemName")]
    public string itemName;

    [XmlElement("Description")]
    public string description;

    [XmlElement("Rarity")]
    public string rarity;

    [XmlElement("Amount")]
    public int amount;

    [XmlElement("TypeString")]
    public string typeString;

    [XmlElement("SpriteString")]
    public string spriteString;

}
