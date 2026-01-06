using UnityEditor.Tilemaps;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BasePanel : MonoBehaviour
{

    protected bool isRemove = false;

    protected new string name;

    protected virtual void Awake()
    {

    }

    public virtual void OpenPanel(string name)
    {
        this.name = name;
        gameObject.SetActive(true);
    }

    public virtual void ClosePanel()
    {
        isRemove = true;
        gameObject.SetActive(false);
        Destroy(gameObject);

        //移除缓存，表示界面没打开
        if(UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
