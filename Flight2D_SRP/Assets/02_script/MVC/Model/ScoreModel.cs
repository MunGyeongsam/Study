using System;

namespace _02_script.MVC.Model
{
    public class ScoreModel
    {
        private float _score = 0F;
        
        private event Action<float> _onScoreChanged;
        
        public event Action<float> OnScoreChanged
        {
            add
            {
                _onScoreChanged += value;
                value?.Invoke(_score);
            }
            remove
            {
                _onScoreChanged -= value;
            }
        }
        
        public void AddScore(float v)
        {
            _score += v;
            _onScoreChanged?.Invoke(_score);
        }

        public void Reset()
        {
            _score = 0F;
            _onScoreChanged?.Invoke(_score);
        }
    }
}