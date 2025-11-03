using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalEnvironment : MonoBehaviour
{
    public Vector2 WorldMin { get { return -WorldMax; } }
    public Vector2 WorldMax { get; private set; }

    [SerializeField] private SpriteRenderer _bg;
    private bool _isInMatrixTime = false;

    static GlobalEnvironment _instance;

    public static GlobalEnvironment Instance { get { return _instance; } }
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
