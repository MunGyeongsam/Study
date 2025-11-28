using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameStateType
{
    None,
    InGame,
    Pause,
    GameOver,
}

public class GameState
{
    public GameStateType CurrentState { get; private set; } = GameStateType.None;

    public void ChangeState(GameStateType newState)
    {
        CurrentState = newState;
    }
    
}
public class GlobalEnvironment : MonoBehaviour
{
    public Vector2 WorldMin { get { return -WorldMax; } }
    public Vector2 WorldMax { get; private set; }
    
    public GameState GameState { get; private set; } = new GameState();

    [SerializeField] private SpriteRenderer _bg;
    [SerializeField] private UI _ui;
    private bool _isInMatrixTime = false;
    
    public UI UI => _ui;

    static GlobalEnvironment _instance;
    public static GlobalEnvironment Instance { get { return _instance; } }
    
    private Dictionary<Rigidbody2D, (Vector2 velocity, float angularVelocity)> _savedVelocities 
        = new Dictionary<Rigidbody2D, (Vector2, float)>();
    
    public void ControlPhysics(Rigidbody2D rb)
    {
        if (rb == null) return;
    
        if (GameState.CurrentState == GameStateType.Pause)
        {
            if (rb.simulated)
            {
                _savedVelocities[rb] = (rb.velocity, rb.angularVelocity);
                rb.Sleep();
                rb.simulated = false;
            }
        }
        else
        {
            if (!rb.simulated)
            {
                rb.simulated = true;
                rb.WakeUp();
                
                if (_savedVelocities.TryGetValue(rb, out var saved))
                {
                    rb.velocity = saved.velocity;
                    rb.angularVelocity = saved.angularVelocity;
                }
            }
        }
    }
    
    void Awake()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        InitWorld();
    }

    void InitWorld()
    {
        var mc = Camera.main;

        float screenHeight = mc.orthographicSize * 2F;
        float screenWidth = screenHeight * mc.aspect;
        var size = _bg.size;

        float scale = Mathf.Max(screenWidth / size.x
            , screenHeight / size.y);

        var worldSize = size * scale;
        var halfWorldSize = worldSize * 0.5F;
        WorldMax = halfWorldSize;
        
        GameState.ChangeState(GameStateType.InGame);
    }

    public void OnTakeDamage()
    {
        if (!_isInMatrixTime)
        {
            StartCoroutine(MatrixEffect());
        }
    }

    private IEnumerator MatrixEffect()
    {
        _isInMatrixTime = true;

        Time.timeScale = 0.1F;
        yield return new WaitForSecondsRealtime(1F);
        Time.timeScale = 1F;

        _isInMatrixTime = false;
    }
}
