using System;
using _02_script.MVC.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _02_script.MVC.Controller
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _speed = 5F;
        [SerializeField] private GlobalEnvironment _ge;
        
        
        Camera _mainCamera;
        Transform _transform;
        Rigidbody2D _rigidbody2D;
        Vector2 _velocity;
        Vector2 _min;
        Vector2 _max;
        bool _uiHold;
        
        static PlayerController _instance = null;
        public static PlayerController Instance => _instance;

        PlayerModel _model = new PlayerModel();
        public PlayerModel Model => _model;
        private void Awake()
        {
            _instance = this;
        }
        
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();

            _mainCamera = Camera.main;
            _transform = transform;
            _max = _ge.WorldMax;
            _min = -_max;
        }
        
        void Update()
        {
            // UI 클릭 상태 갱신
            if (Input.GetMouseButtonDown(0) && IsPointerOverUI())
                _uiHold = true;
            if (Input.GetMouseButtonUp(0))
                _uiHold = false;

            // 홀드 중이면 조작 차단
            if (_uiHold)
            {
                _velocity = Vector2.zero;
                Clamp();
                return;
            }

            if (!ControllByMouse())
                ControllByKeyboard();

            Clamp();
        }

    void FixedUpdate()
    {
        var vel = _rigidbody2D.velocity;
        _rigidbody2D.velocity = vel + (_velocity - vel) * Time.fixedDeltaTime * 10F;
    }

    void Clamp()
    {
        var pos = _transform.position;
        pos.x = Mathf.Clamp(pos.x, _min.x, _max.x);
        pos.y = Mathf.Clamp(pos.y, _min.y, _max.y);
        _transform.position = pos;
    }

    bool ControllByMouse()
    {
        if (!Input.GetMouseButton(0))
        {
            _velocity = Vector2.zero;
            return false;
        }
        // 클릭 중인데 이미 UI에서 시작했다면 차단
        if (_uiHold)
        {
            _velocity = Vector2.zero;
            return false;
        }

        var cursor = Input.mousePosition;
        var ptInWorld = _mainCamera.ScreenToWorldPoint(cursor);
        _velocity = (ptInWorld - _transform.position) * _speed;
        return true;
    }

    bool ControllByKeyboard()
    {
        bool l = Input.GetKey(KeyCode.LeftArrow);
        bool r = Input.GetKey(KeyCode.RightArrow);
        bool u = Input.GetKey(KeyCode.UpArrow);
        bool d = Input.GetKey(KeyCode.DownArrow);

        var dir = new Vector2(l ? -1F : (r ? 1F : 0F),
                              d ? -1F : (u ? 1F : 0F));
        _velocity = dir.normalized * _speed;
        return (l || r || u || d);
    }

    bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        return EventSystem.current.IsPointerOverGameObject();
#else
        if (Input.touchCount > 0)
            return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);
        return false;
#endif
    }
        
        public void TakeDamage(float v)
        {
            _model.Health -= v;
        }
    }
}