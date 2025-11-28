using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _scoreText = null;
    
    private float _currScore = 0F;
    private float _targScore = 0F;
    private int _scoreUi = 0;

    public void Reset()
    {
        _currScore = 0F;
        _targScore = 0F;
        _scoreUi = 0;
        UpdateUI();
    }

    public void AddScore(float v)
    {
        _targScore += v;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _scoreUi = (int)_currScore;
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        AddScore(Time.deltaTime);
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
    }
}
