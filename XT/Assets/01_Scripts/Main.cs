using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField] private Map map;

    [SerializeField] private GameObject prefabAgent;

    private Transform _transform;
    
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
            var agent = Instantiate(prefabAgent, Vector3.zero, Quaternion.identity, _transform);
            agent.transform.localScale = Vector3.one * map.Scale;

            var (fi, fj, ti, tj) = map.GetFromTo();
            var xy = map.ToXy(fi, fj);
            
            //Debug.Log($"----{fi.ToString()}, {fj.ToString()}, ({xy.ToString()})");

            agent.transform.position = new Vector3(xy.x, xy.y, 0F);
        }
    }
}
