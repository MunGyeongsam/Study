using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    //  +z
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +x
    
    
    public static Material CreateMaterial(Texture texture)
    {
        Material material = new Material(Shader.Find("Standard"));

        material.mainTexture = texture;
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        material.EnableKeyword("_ALPHATEST_ON");
        material.SetPass(0);

        return material;
    }

    public static Material CreateCustomMaterial(Texture texture)
    {
        Material material = new Material(Shader.Find("Custom/TexWithColor"));

        material.SetTexture("_MainTex", texture);
        return material;
    }
}
