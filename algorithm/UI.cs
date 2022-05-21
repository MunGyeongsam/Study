using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject floor;
    Floor _floor;
    Button _btnReset;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(floor);
        _floor = floor.GetComponent<Floor>();
        _btnReset = transform.Find("ButtonReset").GetComponent<Button>();
        Debug.Assert(_btnReset);

        _btnReset.onClick.AddListener(() => { _floor.Reset(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
