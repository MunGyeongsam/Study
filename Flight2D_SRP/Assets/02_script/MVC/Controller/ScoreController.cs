using _02_script.MVC.Model;

namespace _02_script.MVC.Controller
{
    public class ScoreController
    {
        public static ScoreController Instance { get; } = new ScoreController();
        
        private ScoreModel _model = new ScoreModel();
        
        public ScoreModel Model => _model;
        
        public void AddScore(float v)
        {
            _model.AddScore(v);
        }
        
        public void Reset()
        {
            _model.Reset();
        }
    }
}