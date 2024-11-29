using UnityEngine;

public class Player : MonoBehaviour
{
    const float EPSILON = 0.001F;

    [SerializeField]
    GameObject world;

    [SerializeField]
    float speed = 5F;

    WorldPosition _wtarg;
    WorldPosition _wcurr;

    Vector3 _targ;
    Vector3 _curr;
    Vector3 _dir;

    World _world;
    CameraCtrl _cam;

    // Start is called before the first frame update
    void Start()
    {
        _targ = transform.position;
        _curr = _targ;
        _dir = Vector3.zero;

        _wtarg = WorldPosition.FromVector3(transform.position);
        _wcurr = _wtarg;

        _world = world.GetComponent<World>();
        _cam = Camera.main.GetComponent<CameraCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldPosition.ENABLED)
        {
            W_Update();

            _curr = _wcurr.ApproximateVector;
            _targ = _wtarg.ApproximateVector;
        }
        else
        {
            UpdateOrg();

            _wcurr = WorldPosition.FromVector3(_curr);
            _wtarg = WorldPosition.FromVector3(_targ);
        }

        UI.AppendDbgInfo("@{ Player");
        UI.AppendDbgInfo(string.Format("  - _curr : {0}", _curr.ToString()));
        UI.AppendDbgInfo(string.Format("  - _targ : {0}", _targ.ToString()));
        UI.AppendDbgInfo(string.Format("  - _dir : {0}", _dir.ToString()));
        
        UI.AppendDbgInfo(string.Format("  - _wcurr : {0}", _wcurr.ToString()));
        UI.AppendDbgInfo(string.Format("  - _wtarg : {0}", _wtarg.ToString()));
        UI.AppendDbgInfo(string.Format("  - Base : {0}", WorldPosition.Base.ToString()));
        UI.AppendDbgInfo("@}");
    }

    //----------------------------------------------------
    void W_Update()
    {
        W_Pick();
        W_UpdateCurrentPosition(Time.deltaTime);
        W_UpdateTransform();
    }

    void W_Pick()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.R))
        {
            float s = ZoneConfig.WORLD_SIZE * 0.5F;
            float x = Random.Range(-s, s);
            float z = Random.Range(-s, s);

            W_JumpTo(x, z);
            //W_SetTarget(WorldPosition.AddToBase(new Vector3(x, 0F, z)));
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            W_JumpTo(0F, 0F);
        }
        else if (Input.GetMouseButtonUp(0) && _world.Pick())
        {
            W_SetTarget(WorldPosition.AddToBase(_world.PickedPoint()));
        }
    }
    void W_UpdateCurrentPosition(float dt)
    {
        if (_dir == Vector3.zero)
            return;

        Vector3 dist = WorldPosition.FromTo(_wcurr, _wtarg);
        float delta = speed * dt;

        if (Vector3.Dot(_dir, dist) <= delta)
        {
            _wcurr = _wtarg;
            _dir = Vector3.zero;
        }
        else
            _wcurr.Add(_dir * delta);
    }

    void W_UpdateTransform()
    {
        Vector3 newPos = WorldPosition.FromBase(_wcurr);
        newPos.y = transform.position.y;
        transform.position = newPos;

        if (_cam != null)
            _cam.W_Target = _wcurr;
    }

    void W_SetTarget(WorldPosition targ)
    {
        _wtarg = targ;

        _dir = WorldPosition.FromTo(_wcurr, _wtarg);
        _dir.y = 0F;

        if (_dir.sqrMagnitude <= EPSILON)
        {
            _wcurr = targ;
            _dir = Vector3.zero;
        }
        else
            _dir.Normalize();
    }

    void W_JumpTo(float x, float z)
    {
        _wcurr = WorldPosition.FromVector3(new Vector3(x,0F,z));
        _wtarg = _wcurr;
        _dir = Vector3.zero;

        if (_cam != null)
        {
            //_cam.W_Lookat = _wcurr;
            _cam.W_Target = _wcurr;
        }
    }


    //----------------------------------------------------
    void UpdateOrg()
    {
        Pick();
        UpdateCurrentPosition(Time.deltaTime);
        UpdateTransform();
    }

    void Pick()
    {
        if (Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.R))
        {
            float s = ZoneConfig.WORLD_SIZE * 0.5F;
            float x = Random.Range(-s, s);
            float z = Random.Range(-s, s);

            JumpTo(x, z);
            //SetTarget(new Vector3(x, 0F, z));
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            JumpTo(0F, 0F);
        }
        else if (Input.GetMouseButtonUp(0) && _world.Pick())
        {
            SetTarget(_world.PickedPoint());
        }
    }

    void JumpTo(float x, float z)
    {
        _curr.x = x;
        _curr.z = z;

        _targ = _curr;
        _dir = Vector3.zero;
    }

    void SetTarget(Vector3 targ)
    {
        _targ = targ;

        _dir = _targ - _curr;
        _dir.y = 0F;

        if (_dir.sqrMagnitude <= EPSILON)
        {
            _curr = targ;
            _dir = Vector3.zero;
        }
        else
            _dir.Normalize();
    }

    void UpdateTransform()
    {
        _curr.y = transform.position.y;
        transform.position = _curr;

        if (_cam != null)
            _cam.Target = _curr;
    }

    void UpdateCurrentPosition(float dt)
    {
        if (_dir == Vector3.zero)
            return;

        Vector3 dist = _targ - _curr;
        float delta = speed * dt;

        if (Vector3.Dot(_dir, dist) <= delta)
        {
            _curr = _targ;
            _dir = Vector3.zero;
        }
        else
            _curr += _dir * delta;
    }
}
