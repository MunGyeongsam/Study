using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    static UI instance = null;
    Text _dbgText = null;
    

    List<string> _dbgInfo = null;
    StringBuilder _strBuilder = null;

    // Start is called before the first frame update
    void Start()
    {
        _dbgText = transform.Find("DbgText").GetComponent<Text>();
        Debug.Assert(_dbgText != null, "We need DbgText to show debug-info");
        _dbgInfo = new List<string>(12);
        _strBuilder = new StringBuilder(2048);

        instance = this;
    }

    private void LateUpdate()
    {
        foreach (var s in _dbgInfo)
            _strBuilder.AppendLine(s);
        _dbgText.text = _strBuilder.ToString();

        _dbgInfo.Clear();
        _strBuilder.Clear();
    }

    void AppendDbgInfoImpl(string s)
    {
        _dbgInfo.Add(s);
    }

    public static void AppendDbgInfo(string s)
    {
        if (null == instance)
            return;

        instance.AppendDbgInfoImpl(s);
    }
}