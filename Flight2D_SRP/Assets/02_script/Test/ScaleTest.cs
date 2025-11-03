using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleTest : MonoBehaviour
{
    int sw, sh;

    Vector2 w_size_half;
    

    void SetScale()
    {
        var sr = GetComponent<SpriteRenderer>();
        Debug.Log($"---->>> sprite : {sr.size}");

        var mc = Camera.main;
        float sh = mc.orthographicSize * 2F;
        float sw = sh * mc.aspect;

        Debug.Log($"---->>> screen : {sw}, {sh}");

        float sx = sw / sr.size.x;
        float sy = sh / sr.size.y;
        float s = Mathf.Max(sx, sy);
        transform.localScale = new Vector3(s, s, s);

        w_size_half = sr.size * s * 0.5F;
    }
    // Start is called before the first frame update
    void Start()
    {
        sw = Screen.width;
        sh = Screen.height;

        SetScale();
    }

    // Update is called once per frame
    void Update()
    {
        if (sw != Screen.width || sh != Screen.height)
        {
            SetScale();
            sw = Screen.width;
            sh = Screen.height;
        }


        Vector3 lt = new Vector3(-w_size_half.x,  w_size_half.y, 0F);
        Vector3 rt = new Vector3( w_size_half.x,  w_size_half.y, 0F);
        Vector3 rb = new Vector3( w_size_half.x, -w_size_half.y, 0F);
        Vector3 lb = new Vector3(-w_size_half.x, -w_size_half.y, 0F);
        Debug.DrawLine(lt, rt, Color.yellow);
        Debug.DrawLine(rt, rb, Color.yellow);
        Debug.DrawLine(rb, lb, Color.yellow);
        Debug.DrawLine(lb, lt, Color.yellow);
    }
}
