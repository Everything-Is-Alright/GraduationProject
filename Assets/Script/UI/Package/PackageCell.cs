using UnityEngine;
using UnityEngine.UI;

public class PackageCell : MonoBehaviour
{
    private Transform UIItem;
    private Transform UIBorder;
    private Transform UISelect;
    private Transform UIStars;

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
}
