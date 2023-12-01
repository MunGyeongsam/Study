using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Map map;

    [SerializeField] private GameObject prefabAgent;
    [SerializeField] private GameObject prefabClosed;
    [SerializeField] private GameObject prefabOpened;

    [SerializeField] private Transform hideClosed;
    [SerializeField] private Transform hideOpened;

    Transform _transform;

    List<Transform> closedList = new List<Transform>();
    List<Transform> openedList = new List<Transform>();

    List<Node> _closedNodes = new List<Node>();
    List<Node> _openedNodes = new List<Node>();

    Agent _agent;


    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _agent = Instantiate(prefabAgent, Vector3.zero, Quaternion.identity, _transform).GetComponent<Agent>();

        _agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            var (f, t) = PathFinder.FromTo();
            PathFinder.Find(_closedNodes, f, t);

            var list = closedList;
            var pool = hideClosed;
            foreach (var child in list)
            {
                child.SetParent(pool);
            }
            list.Clear();


            foreach (var n in _closedNodes)
            {
                var child = GetTile(hideClosed, prefabClosed);
                var pos = PathFinder.ToPos(n);

                child.name = string.Format("{0},{1}", n.Row, n.Col);
                closedList.Add(child);

                child.position = pos;
            }

            _agent.SetPath(_closedNodes);
        }
    }

    Transform GetTile(Transform pool, GameObject prefab)
    {
        if (pool.childCount > 0)
        {
            var rt = pool.GetChild(pool.childCount - 1);
            rt.SetParent(_transform);

            return rt;
        }
        else
        {
            var rt = Instantiate(prefab, Vector3.zero, Quaternion.identity, _transform).transform;
            rt.localScale = Vector3.one * map.Scale * 0.8F;

            return rt;
        }
    }
}
