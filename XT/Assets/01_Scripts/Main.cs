using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Map map;

    [SerializeField] private GameObject prefabAgent;
    [SerializeField] private GameObject prefabClosed;
    [SerializeField] private GameObject prefabOpened;
    [SerializeField] private GameObject prefabPath;

    [SerializeField] private Transform hideClosed;
    [SerializeField] private Transform hideOpened;
    [SerializeField] private Transform hidePath;

    Transform _transform;

    List<Transform> closedList = new List<Transform>();
    List<Transform> openedList = new List<Transform>();
    List<Transform> pathList = new List<Transform>();

    List<Node> _closedNodes = new List<Node>();
    List<Node> _openedNodes = new List<Node>();
    List<Node> _pathNodes = new List<Node>();
    private Node _from;
    private Node _to;

    Agent _agent;


    // Start is called before the first frame update
    void Start()
    {
        _transform = transform;
        _agent = Instantiate(prefabAgent, Vector3.zero, Quaternion.identity, _transform).GetComponent<Agent>();

        _agent.enabled = false;

        /*
        Queue<int> a = new();
        
        a.Enqueue(0);
        a.Enqueue(1);
        a.Enqueue(0);
        a.Enqueue(9);
        a.Enqueue(8);
        a.Enqueue(5);
        a.Enqueue(0);
        a.Enqueue(8);
        a.Enqueue(0);
        a.Enqueue(5);
        a.Enqueue(5);
        
        Debug.Log("--------1");
        foreach (var n in a)
        {
            Debug.Log($"{n.ToString()}");
        }
        Debug.Log("--------2");
        a = new(a.OrderBy((v) => v));
        foreach (var n in a)
        {
            Debug.Log($"{n.ToString()}");
        }
        Debug.Log("--------3");
        
        //*/
        
    }

    // Update is called once per frame
    void Update()
    {
        bool chk = false;
        if (_from == null)
        {
            (_from, _to) = PathFinder.FromTo();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            chk = true;
            PathFinder.Algorithm = 1;
            Debug.Log("Dijkstra");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            chk = true;
            PathFinder.Algorithm = 2;
            Debug.Log("Astar");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            chk = true;
            PathFinder.Algorithm = 3;
            Debug.Log("BestFirst");
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            chk = true;
            (_from, _to) = PathFinder.FromTo();
        }

        if (chk)
        {
            PathFinder.Find(_pathNodes, _from, _to);
            PathFinder.GetOpenAndClosedList(_openedNodes, _closedNodes);

            ClearList(openedList, hideOpened);
            ClearList(closedList, hideClosed);
            ClearList(pathList, hidePath);

            ShowNodes(_openedNodes, openedList, hideOpened, prefabOpened, "opened");
            ShowNodes(_closedNodes, closedList, hideClosed, prefabClosed, "closed");
            ShowNodes(_pathNodes, pathList, hidePath, prefabPath, "path");

            _agent.SetPath(_pathNodes);
        }
    }

    void ShowNodes(List<Node> nodes, List<Transform> list, Transform pool, GameObject prefab, string prefix)
    {
        
        foreach (var n in nodes)
        {
            var child = GetTile(pool, prefab);
            var pos = PathFinder.ToPos(n);

            child.name = string.Format("{0} {1},{2}", prefix, n.Row, n.Col);
            list.Add(child);

            child.position = pos;
        }
    }

    void ClearList(List<Transform> list, Transform hideNode)
    {
        foreach (var child in list)
        {
            child.SetParent(hideNode);
        }
        list.Clear();
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
