using UnityEngine;
using TMPro;
using System.Data;
using UnityEngine.UIElements;

public class PackageManager : MonoBehaviour
{
    static PackageManager instance;

    public Package myBag;
    public Slot slotPrefab;
    public GameObject slotGrid;
    public TextMeshProUGUI itemInfromation;
    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        instance = this;
    }

    public static void CreateNewItem(Item item)
    {
        Slot newItem = Instantiate(instance.slotPrefab,instance.slotGrid.transform.position,Quaternion.identity);
        newItem.gameObject.transform.SetParent(instance.slotGrid.transform);
        newItem.slotItem = item;
        newItem.slotImage.sprite = item.itemImage;
        newItem.slotNum.text = item.itemHeld.ToString();
    }

    private void OnEnable()
    {
        RefreshItem();
    }

    public static void RefreshItem()
    {
        for(int i = 0; i < instance.slotGrid.transform.childCount;  i++)
        {
            if(instance.slotGrid.transform.childCount == 0)
            {
                break;
            }
            Destroy(instance.slotGrid.transform.GetChild(i).gameObject);
        }

        for(int i =0; i<instance.myBag.itemList.Count; i++)
        {
            CreateNewItem(instance.myBag.itemList[i]);
        }
    }
}
