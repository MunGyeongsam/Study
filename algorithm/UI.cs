using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    public GameObject floor;
    Button _btnReset;

    Text _dbgText;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(floor);
        _btnReset = transform.Find("ButtonReset").GetComponent<Button>();
        Debug.Assert(_btnReset);

        _dbgText = transform.Find("dbgText").GetComponent<Text>();
        Debug.Assert(_dbgText);

        _btnReset.onClick.AddListener(() => { floor.SendMessage("Reset"); });
    }

    public void SetDbgText(string msg)
    {
        _dbgText.text = msg;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
