using UnityEngine;
using UnityEngine.UI;

namespace _02_script.MVC.View
{
    public class PlayerInventory : MonoBehaviour
    {
        ToggleGroup _toggleGroup;
        
        private void Start()
        {
            _toggleGroup = GetComponent<ToggleGroup>();
        }
        
        
    }
}