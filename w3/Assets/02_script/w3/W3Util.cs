using System;
using System.Collections;
using System.Collections.Generic;
using _02_script.w3;
using UnityEngine;
using Random = UnityEngine.Random;

public static class W3Util
{
    //       +v
    //        |
    //        |
    //   1.00 *-----*-----*-----*-----*-----*-----*-----*-----*(1.0, 1.0)
    //        |     |     |     |     |     |     |     |     |       
    //        |15   |0    |1    |2    |16   |17   |18   |19   |       
    //   0.75 *-----*-----*-----*-----*-----*-----*-----*-----*      0.75
    //        |     |     |     |     |     |     |     |     |          
    //        |3    |4    |5    |6    |20   |21   |22   |23   |          
    //   0.50 *-----*-----*-----*-----*-----*-----*-----*-----*      0.50
    //        |     |     |     |     |     |     |     |     |          
    //        |7    |8    |9    |10   |24   |25   |26   |27   |          
    //   0.25 *-----*-----*-----*-----*-----*-----*-----*-----*      0.25
    //        |     |     |     |     |     |     |     |     |          
    //        |11   |12   |13   |14   |28   |29   |30   |31   |          
    //  (0, 0)*-----*-----*-----*-----*-----*-----*-----*-----*      0.00 --- +u
    //       0.0   0.125 0.25  0.375 0.5   0.625 0.75  0.876 1.0
    
    
    //       +v
    //        |
    //        |  0     1     2     3     4     5     6     7                                 
    //   1.00 *-----*-----*-----*-----*-----*-----*-----*-----*(1.0, 1.0)
    //        | 1 1 | 0 0 | 0 0 | 0 0 | 1 1 | 1 1 | 1 1 | 1 1 |       
    //     3  | 1 1 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |       
    //   0.75 *-----*-----*-----*-----*-----*-----*-----*-----*      0.75
    //        | 0 1 | 0 1 | 0 1 | 0 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //     2  | 0 0 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //   0.50 *-----*-----*-----*-----*-----*-----*-----*-----*      0.50
    //        | 1 0 | 1 0 | 1 0 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //     1  | 0 0 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //   0.25 *-----*-----*-----*-----*-----*-----*-----*-----*      0.25
    //        | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //     0  | 0 0 | 0 1 | 1 0 | 1 1 | 1 1 | 1 1 | 1 1 | 1 1 |          
    //  (0, 0)*-----*-----*-----*-----*-----*-----*-----*-----*      0.00 --- +u
    //       0.0   0.125 0.25  0.375 0.5   0.625 0.75  0.876 1.0
    
    
    //  +v
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +u

    private static readonly Vector2[] EdgeUvs;
    private static readonly Vector2[] FullUvs;
    static void GetUv(Span<Vector2> buffer, int r, int c)
    {
        const float vStep = 1F / 4F;
        const float uStep = 1F / 8F;
        
        buffer[0] = new Vector2((c+0)*uStep, (r+1)*vStep);
        buffer[1] = new Vector2((c+1)*uStep, (r+1)*vStep);
        buffer[2] = new Vector2((c+0)*uStep, (r+0)*vStep);
        buffer[3] = new Vector2((c+1)*uStep, (r+0)*vStep);
    }
    
    private static int Row(this Prop prop)
    {
        int row = 0;
        
        if (prop.LT && prop.RT)
            row = 0;
        else if (prop.LT && !prop.RT)
            row = 1;
        else if (!prop.LT && prop.RT)
            row = 2;
        else if (!prop.LT && !prop.RT)
            row = 3;

        
        //if (prop is { LT: true, RT: true })
        //    row = 0;
        //else if (prop is { LT: true, RT: false })
        //    row = 1;
        //else if (prop is { LT: false, RT: true })
        //    row = 2;
        //else if (prop is { LT: false, RT: false })
        //    row = 3;
        
        //row = prop switch
        //{
        //    { LT: true, RT: true } => 0,
        //    { LT: true, RT: false } => 1,
        //    { LT: false, RT: true } => 2,
        //    { LT: false, RT: false } => 3,
        //};

        return row;
    }

    private static int Col(this Prop prop)
    {
        int col = 0;
        
        if (!prop.LB && !prop.RB)
            col = 0;
        else if (!prop.LB && prop.RB)
            col = 1;
        else if (prop.LB && !prop.RB)
            col = 2;
        else if (prop.LB && prop.RB)
            col = 3;

        //col = prop switch
        //{
        //    { LB: false, RB: false } => 0,
        //    { LB: false, RB: true } => 1,
        //    { LB: true, RB: false } => 2,
        //    { LB: true, RB: true } => 3
        //};
        
        //if (prop is { LB: false, RB: false })
        //    col = 0;
        //else if (prop is { LB: false, RB: true })
        //    col = 1;
        //else if (prop is { LB: true, RB: false })
        //    col = 2;
        //else if (prop is { LB: true, RB: true })
        //    col = 3;

        return col;
    }
    
    static W3Util()
    {
        EdgeUvs = new Vector2[16 * 4];
        Array.Fill(EdgeUvs, Vector2.zero);

        var span = EdgeUvs.AsSpan();
        for (int i = 1; i < 16; ++i)
        {
            Prop p = new Prop((byte)i);
            GetUv(span.Slice(i * 4, 4), p.Row(), p.Col());
        }
        
        FullUvs = new Vector2[(4*4 + 2)*4];
        span = FullUvs.AsSpan();
        int offset = 0;
        for(int r=0; r<4; ++r)
        {
            for(int c=4; c<8; ++c)
            {
                GetUv(span[offset..(offset+4)], r, c);
                offset += 4;
            }
        }
        GetUv(span[offset..(offset+4)], 3, 0);
        offset += 4;
        GetUv(span[offset..(offset+4)], 0, 3);
    }
    
    public static void GetedgeUvs(Span<Vector2> buffer, Prop prop)
    {
        if (prop.IsFull)
        {
            int index = Random.Range(0, 4*4+2) * 4;
            buffer[0] = FullUvs[index + 0];
            buffer[1] = FullUvs[index + 1];
            buffer[2] = FullUvs[index + 2];
            buffer[3] = FullUvs[index + 3];
        }
        else
        {
            int index = (int)prop * 4;
            buffer[0] = EdgeUvs[index + 0];
            buffer[1] = EdgeUvs[index + 1];
            buffer[2] = EdgeUvs[index + 2];
            buffer[3] = EdgeUvs[index + 3];
        }
    }
    
    //  +z
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +x
    public static void GetVtx(Span<Vector3> buffer, Vector3 leftBottom, float tileSize)
    {
        buffer[0] = leftBottom + new Vector3(0F, 0F, tileSize);
        buffer[1] = leftBottom + new Vector3(tileSize, 0F, tileSize);
        buffer[2] = leftBottom;
        buffer[3] = leftBottom + new Vector3(tileSize, 0F, 0F);
    }
    
    
    public static Material CreateMaterial(Texture texture, int rq = 0)
    {
        Material material = new Material(Shader.Find("Standard"));

        material.mainTexture = texture;
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.SetInt("_ZWrite", 1);
        material.EnableKeyword("_ALPHATEST_ON");
        material.DisableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = rq;
        material.SetPass(0);

        return material;
    }

    public static Material CreateCustomMaterial(Texture texture)
    {
        Material material = new Material(Shader.Find("Custom/TexWithColor"));

        material.SetTexture("_MainTex", texture);
        return material;
    }

    public static void RecalcMesh(Mesh mesh)
    {
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
