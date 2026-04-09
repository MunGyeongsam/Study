using System;

namespace _02_script.MVC.Model
{
    public class PlayerModel
    {
        public event Action<float, float> OnHealthChanged;
        public event Action<int> OnItemChanged;
        public event Action OnPlayerDied;
        
        public const float MAX_HEALTH = 100F;
        float _health = MAX_HEALTH;
        
        public float Health
        {
            get => _health;
            set
            {
                if (value <= 0F)
                {
                    _health = 0F;
                    OnHealthChanged?.Invoke(_health, MAX_HEALTH);
                    OnPlayerDied?.Invoke();
                    return;
                }
                
                _health = value;
                OnHealthChanged?.Invoke(_health, MAX_HEALTH);
            }
        }

        private int _currentItem = 0;
        public int CurrentItem 
        {
            get => _currentItem;
            set
            {
                _currentItem = value;
                OnItemChanged?.Invoke(_currentItem);
            } 
        }
    }
}