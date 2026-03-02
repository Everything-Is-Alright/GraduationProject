using UnityEngine;

public class ItemOnWorld : MonoBehaviour
{
    public Item thisItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AddNewItem();
            Destroy(gameObject);
        }
    }

    public void AddNewItem()
    {
        PackageManager.AddItemToPackage(thisItem);
    }
}
