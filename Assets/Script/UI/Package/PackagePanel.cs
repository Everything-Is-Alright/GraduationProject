using System;
using UnityEngine;
using UnityEngine.UI;

public class PackagePanel : BasePanel
{
    private Transform UIMenu;
    private Transform UIMenuWeapon;
    private Transform UIMenuArmor;
    private Transform UIMenuAccessories;
    private Transform UIMenuPotion;
    private Transform UIMenuTask;

    private Transform UICenter;
    private Transform UICenterMenu;
    private Transform UICenterScrollView;
    private Transform UICenterDetailPanel;

    private Transform UILeftBag;

    private Transform UIBottomClose;


    protected override void Awake()
    {
        base.Awake();
        InitUI();
    }

    private void InitUI()
    {
        InitUIName();
        InitUIClick();
    }

    private void InitUIName()
    {
        UIMenu = transform.Find("TopCenter/Menus");
        UIMenuWeapon = transform.Find("TopCenter/Menus/Weapon");
        UIMenuArmor = transform.Find("TopCenter/Menus/Armor");
        UIMenuAccessories = transform.Find("TopCenter/Menus/Accessories");
        UIMenuPotion = transform.Find("TopCenter/Menus/Potion");
        UIMenuTask = transform.Find("TopCenter/Menus/Task");

        UICenter = transform.Find("Center");
        UICenterMenu = transform.Find("Center/Menus");
        UICenterScrollView = transform.Find("Center/Scroll View");
        UICenterDetailPanel = transform.Find("Center/DetailPanel");

        UILeftBag = transform.Find("Left/Bag");

        UIBottomClose = transform.Find("Bottom/Close");
    }

    private void InitUIClick()
    {
        UIMenuWeapon.GetComponent<Button>().onClick.AddListener(OnClickWeapon);
        UIMenuArmor.GetComponent<Button>().onClick.AddListener(OnClickArmor);
        UIMenuAccessories.GetComponent<Button>().onClick.AddListener(OnClickAccessories);
        UIMenuPotion.GetComponent<Button>().onClick.AddListener(OnClickPotion);
        UIMenuTask.GetComponent<Button>().onClick.AddListener(OnClickTask);

        UILeftBag.GetComponent<Button>().onClick.AddListener(OnClickLeftBag);
        UIBottomClose.GetComponent<Button>().onClick.AddListener(OnClickBottomClose);
    }

    private void OnClickWeapon()
    {
        Debug.Log("点击武器！");
    }
    private void OnClickArmor()
    {
        Debug.Log("点击护甲！");
    }
    private void OnClickAccessories()
    {
        Debug.Log("点击饰品！");
    }
    private void OnClickPotion()
    {
        Debug.Log("点击药物！");
    }
    private void OnClickTask()
    {
        Debug.Log("点击任务！");
    }
    private void OnClickBottomClose()
    {
        Debug.Log("点击退出！");
    }
    private void OnClickLeftBag()
    {
        Debug.Log("点击背包栏！");
    }


}
