using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [Serializable]
    public class TogglePanelPair
    {
        public Toggle toggle;
        public GameObject panel;
        // 新增：该 Toggle 对应显示的物品类型
        public ItemType pageType = ItemType.Weapon;
    }

    public List<TogglePanelPair> togglePanelPairs;
    // 从 Toggle 到 配对数据 的映射，便于在回调中查找 pageType/panel
    private Dictionary<Toggle, TogglePanelPair> pairMap;
    private Toggle currentSelectedToggle;

    void Awake()
    {
        // 初始化字典
        pairMap = new Dictionary<Toggle, TogglePanelPair>();
        foreach (var pair in togglePanelPairs)
        {
            if (pair.toggle != null && pair.panel != null)
            {
                pairMap.Add(pair.toggle, pair);
                // 为每个 Toggle 添加回调（使用闭包安全通过 pair 访问 pageType/panel）
                pair.toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(pair.toggle, isOn));
            }
        }
    }

    void Start()
    {
        if (togglePanelPairs.Count > 0 && togglePanelPairs[0].toggle != null)
        {
            // 触发回调以保证首次显示的面板会刷新
            togglePanelPairs[0].toggle.isOn = true;
            currentSelectedToggle = togglePanelPairs[0].toggle;
        }
    }

    private void OnToggleChanged(Toggle toggle, bool isOn)
    {
        if (!pairMap.ContainsKey(toggle)) return;

        if (isOn)
        {
            foreach (var kv in pairMap)
            {
                var pair = kv.Value;
                if (pair == null || pair.panel == null) continue;

                if (kv.Key == toggle)
                {
                    pair.panel.SetActive(true);
                    currentSelectedToggle = toggle;

                    // 找到面板上的 PackageUIView（包含未激活子对象）
                    var view = pair.panel.GetComponentInChildren<PackageUIView>(true);
                    if (view != null)
                    {
                        // 告知 View 切换到对应页面并刷新
                        view.SwitchPage(pair.pageType);
                        // SwitchPage 内部已刷新，但为保险再次调用 RefreshView 可被省略
                        // view.RefreshView();
                    }
                }
                else
                {
                    pair.panel.SetActive(false);
                }
            }
        }
    }
}