using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSTBG : MonoBehaviour
{
    private int sw, sh;
    [SerializeField] GlobalEnvironment env;
    // Start is called before the first frame update
    void Start()
    {
        sw = Screen.width;
        sh = Screen.height;

        BgScale bgScale = GetComponent<BgScale>();
        bgScale.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (sw != Screen.width || sh != Screen.height) 
        {
            env.SendMessage("InitWorld");
            BgScale bgScale = GetComponent<BgScale>();
            bgScale.Init();
        }
    }
}
