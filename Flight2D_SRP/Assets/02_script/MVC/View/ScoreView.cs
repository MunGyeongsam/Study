
using _02_script.MVC.Controller;
using TMPro;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText = null;
    [SerializeField] TextMeshProUGUI _leaderBoardText = null;
    
    private float _currScore = 0F;
    private float _targScore = 0F;
    private int _scoreUi = 0;

    public void SetScore(float score)
    {
        _targScore = score;
    }
    
    public void Reset()
    {
        _currScore = 0F;
        _targScore = 0F;
        _scoreUi = 0;
        UpdateUI();
    }

    void Start()
    {
        ScoreController.Instance.Model.OnScoreChanged += SetScore;
        
        _scoreUi = (int)_currScore;
        UpdateUI();
    }

    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        if (_currScore < _targScore)
        {
            _currScore += (_targScore - _currScore) * 10F * Time.deltaTime;
            if (_currScore > _targScore)
            {
                _currScore = _targScore;
            }
            if (_scoreUi != (int)_currScore)
            {
                _scoreUi = (int)_currScore;
                UpdateUI();
            }
        }
    }
    
    private void UpdateUI()
    {
        if (_scoreText!= null)
        {
            _scoreText.text = _scoreUi.ToString();
        }
        if (_leaderBoardText != null)
        {
            _leaderBoardText.text = "Leader Board\n" +
                                    "1st: " + _scoreUi * 1.5f + "\n" +
                                    "2nd: " + _scoreUi * 1.2f + "\n" +
                                    "3rd: " + _scoreUi * 1.1f + "\n";
        }
    }
}