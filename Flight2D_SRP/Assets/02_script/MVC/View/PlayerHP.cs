using System;
using _02_script.MVC.Controller;
using UnityEngine;
using UnityEngine.UI;

namespace _02_script.MVC.View
{
    public class PlayerHP : MonoBehaviour
    {
        [SerializeField] private Slider _slider;
        private void Start()
        {
            PlayerController.Instance.Model.OnHealthChanged += OnChangedHP;
        }
        
        void OnChangedHP(float hp, float maxHp)
        {
            _slider.value = hp / maxHp;
        }
    }
}