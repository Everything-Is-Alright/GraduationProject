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

        //ŒÔ∆∑Õº∆¨
        Texture2D t = (Texture2D)Resources.Load(this.packageItem.imagePath);
        Sprite temp = Sprite.Create(t, new Rect(0, 0, t.width, t.height), new Vector2(0,0));

        RefreshStars();
    }

    public void RefreshStars()
    {
        for (int i = 0; i < UIStars.childCount; i++)
        {
            Transform star = UIStars.GetChild(i);
            if(this.packageItem.star > 1)
            {
                star.gameObject.SetActive(true);
            }
            else
            {
                star.gameObject.SetActive(false);
            }
        }
    }
}
