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
    }

    public List<TogglePanelPair> togglePanelPairs;
    private Dictionary<Toggle, GameObject> panelMap;
    private Toggle currentSelectedToggle;

    void Awake()
    {
        // ³õÊ¼»¯×Öµä
        panelMap = new Dictionary<Toggle, GameObject>();
        foreach (var pair in togglePanelPairs)
        {
            if (pair.toggle != null && pair.panel != null)
            {
                panelMap.Add(pair.toggle, pair.panel);
                pair.toggle.onValueChanged.AddListener((isOn) => OnToggleChanged(pair.toggle, isOn));
            }
        }
    }

    void Start()
    {
        if (togglePanelPairs.Count > 0 && togglePanelPairs[0].toggle != null)
        {
            togglePanelPairs[0].toggle.isOn = true;
            currentSelectedToggle = togglePanelPairs[0].toggle;
        }
    }

    private void OnToggleChanged(Toggle toggle, bool isOn)
    {
        if (isOn)
        {
            foreach (var pair in panelMap)
            {
                if (pair.Key == toggle)
                {
                    pair.Value.SetActive(true);
                    currentSelectedToggle = toggle;
                }
                else
                {
                    pair.Value.SetActive(false);
                }
            }
        }
    }
}