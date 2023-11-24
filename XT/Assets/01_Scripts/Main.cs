using System;
using System.Collections;
using System.Collections.Generic;
using _01_Scripts;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Map map;

    [SerializeField] private GameObject prefabAgent;
    [SerializeField] private GameObject prefabClosedTile;
    [SerializeField] private GameObject prefabOpenedTile;
    [SerializeField] private Transform hideClosedTile;
    [SerializeField] private Transform hideOpenedTile;
    
    private Transform _transform;
    private Mesh _mesh;

    private List<Transform> _closedOlverays = new();
    private List<Transform> _openedOlverays = new();
    private List<Node> _closedNodes = new();
    private List<Node> _openedNodes = new();
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!prefabAgent || !map)
            return;
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            //var agent = Instantiate(prefabAgent, Vector3.zero, Quaternion.identity, _transform);
            //agent.transform.localScale = Vector3.one * map.Scale;

            var (fi, fj, ti, tj) = map.GetFromTo();
            var xy = map.ToXy(fi, fj);

            _closedNodes = PathFinder.Find(new Node(fi, fj), new Node(ti, tj));

            MakeOverays(_closedOlverays, _closedNodes, prefabClosedTile, hideClosedTile);

            //agent.transform.position = new Vector3(xy.x, xy.y, 0F);
        }
    }

    void MakeOverays(List<Transform> overlays, List<Node> nodes, GameObject prefab, Transform pool)
    {
        foreach (var t in (overlays))
        {
            t.SetParent(pool);
        }
        overlays.Clear();

        float scale = map.Scale * 0.8F;
        foreach (var n in nodes)
        {
            var child = GetInstance(prefab, pool, scale);
            child.position = PathFinder.ToVector2(n);
            overlays.Add(child);
        }
    }

    Transform GetInstance(GameObject prefab, Transform pool, float scale)
    {
        if (pool.childCount > 0)
        {
            Transform child = pool.GetChild(pool.childCount - 1);
            child.SetParent(_transform);
            return child;
        }
        else
        {
            var child = Instantiate(prefab, Vector3.zero, Quaternion.identity, _transform).transform;
            child.localScale = Vector3.one * scale;
            return child;
        }
    }
}
