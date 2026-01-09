using UnityEngine;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour
{
    private Transform UIItem;
    private Transform UIBorder;
    private Transform UISelect;
    private Transform UIStars;

    private PackageLocalItem packageLocalData;
    private PackageItem packageItem;
    private PackagePanel uiParent;

    private void Awake()
    {
        InitUIName();
    }

    private void InitUIName()
    {
        UIItem = transform.Find("Img/Item");
        UIBorder = transform.Find("Img/Border");
        UISelect = transform.Find("Img/Selected");
        UIStars = transform.Find("Img/Stars");
    }

    public void Refresh(PackageLocalItem packageLocalData, PackagePanel uiParent)
    {
        this.packageLocalData = packageLocalData;
        this.packageItem = GameManager.Instance.GetPackageItemById(packageLocalData.id);
        this.uiParent = uiParent;
    }
}
