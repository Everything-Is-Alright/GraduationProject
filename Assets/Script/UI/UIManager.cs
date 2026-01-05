using System.Collections.Generic;
using UnityEngine;

public class UIManager
{
    private static UIManager instance;

    //路径缓存字典
    private Dictionary<string, string> pathDict;
    //预制件缓存字典
    private Dictionary<string, GameObject> prefabDict;
    //已打开界面
    public Dictionary<string, BasePanel> panelDict;

    private Transform uiRoot;

    public Transform UIRoot
    {
        get
        {
            if(uiRoot == null)
            {
                if(GameObject.Find("Canvas"))
                {
                    uiRoot = GameObject.Find("Canvas").transform;
                }
                else
                {
                    uiRoot = GameObject.Find("UISystem").transform;
                }
            }
            return uiRoot; 
        }
    }

    public static UIManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    private UIManager()
    {
        InitDicts();
    }

    private void InitDicts()
    {
        pathDict = new Dictionary<string, string>()
        {
            {UIConst.PackagePanel, "Prefab/PackagePanel" }
        };
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
    }

    public BasePanel OpenPanel(string name)
    {
        //检查是否已打开
        BasePanel panel = null;
        if(panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面已打开！");
            return null;
        }

        //检查路径是否正确
        string path = "";
        if(!pathDict.TryGetValue(name, out path))
        {
            Debug.LogError("界面名称错误或未配置路径");
            return null;
        }

        //使用缓存的预制件
        GameObject panelPrefab = null;
        if(!prefabDict.TryGetValue(name, out panelPrefab))
        {
            string realPath = path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab);
        }

        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel);
        return panel;
    }

    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        if(!panelDict.TryGetValue(name, out panel))
        {
            Debug.LogError("界面未打开");
            return false;
        }

        panel.ClosePanel();
        return true;
    }
}
