using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;
    [SerializeField] private Score _score;
    [SerializeField] private LeaderBoard _leaderboard;
    [SerializeField] private PlayerHpBar _playerHpBar;
    [SerializeField] private Button _currentItem;
    [SerializeField] private Player _player;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_currentItem != null && _inventory != null)
        {
            var inventory = _inventory.GetComponent<Inventory>();
            inventory.OnItemSelected += OnItemSelected;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_player.IsAlive == false && Input.GetKeyUp(KeyCode.Space))
        {
            _player.Revive();
            _score.Reset();
            GlobalEnvironment.Instance.GameState.ChangeState(GameStateType.InGame);
        }
    }
    
    public void AddScore(float v)
    {
        if (_score != null)
            _score.AddScore(v);
    }
    
    public void HpChange(float curr, float max)
    {
        if (_playerHpBar != null)
        {
            _playerHpBar.SetHp(curr, max);
        }
    }
    
    public void OnItemSelected(int index, UnityEngine.UI.Image img)
    {
        if (GlobalEnvironment.Instance.GameState.CurrentState != GameStateType.Pause)
        {
            return;
        }
        
        GlobalEnvironment.Instance.GameState.ChangeState(GameStateType.InGame);
        
        if (_currentItem != null)
        {
            var currentImg = _currentItem.GetComponent<Image>();
            currentImg.sprite = img.sprite;
        }
        
        if (_player != null)
        {
            _player.SetItem(index);
        }
        
        if (_inventory != null)
        {
            _inventory.SetActive(false);
        }
    }
    
    public void OnClickCurrentItem()
    {
        if (_inventory != null)
        {
            var isActive = !_inventory.activeSelf;
            _inventory.SetActive(isActive);
            
            GlobalEnvironment.Instance.GameState.ChangeState(isActive ? GameStateType.Pause : (_player.IsAlive ? GameStateType.InGame : GameStateType.GameOver));
        }
    }
}
