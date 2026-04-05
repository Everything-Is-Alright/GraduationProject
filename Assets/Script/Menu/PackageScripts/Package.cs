
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Package", menuName = "Package/New Package")]
public class Package : ScriptableObject 
{
    public List<Item> itemList = new List<Item>();
}
