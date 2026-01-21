using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;
    public Package playerPackage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        if(!playerPackage.itemList.Contains(thisItem))
        {
            playerPackage.itemList.Add(thisItem);
        }
        else 
        {
            thisItem.itemHeld++;
        }
        PackageManager.RefreshItem();
    }
}
