using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] ToggleGroup _toggleGroup;
    
    private Action<int, Image> _onItemSelected = null;
    
    public event Action<int, Image> OnItemSelected
    {
        add { _onItemSelected += value; }
        remove { _onItemSelected -= value; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        if (null != _toggleGroup)
        {
            var toggles = _toggleGroup.GetComponentsInChildren<Toggle>();
            if (toggles.Length > 0)
            {
                toggles[0].isOn = true;
            }

            for(int i=0; i<toggles.Length; i++)
            {
                int index = i; // Capture index for the closure
                var toggle = toggles[i];
                toggle.onValueChanged.AddListener((isOn) =>
                {
                    if (isOn && _onItemSelected != null)
                    {
                        var img = toggle.transform.Find("Background").GetComponent<Image>();
                        _onItemSelected.Invoke(index, img);
                    }
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
