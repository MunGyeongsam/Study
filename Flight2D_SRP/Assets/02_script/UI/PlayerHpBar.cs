using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBar : MonoBehaviour
{
    private Slider _slider;
    
    public void SetHp(float curr, float max)
    {
        if (_slider == null)
        {
            _slider = GetComponent<Slider>();
        }

        if (_slider != null)
        {
            _slider.value = curr / max;
        }
    }
}
