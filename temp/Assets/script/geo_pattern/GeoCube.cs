using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script.geo_pattern
{
    internal class GeoCube : GeoBase
    {
        protected override int NumOfVertices => 6*4;

        protected override void StepVertex(Mesh mesh)
        {
            float hs = 0.5F;
            mesh.vertices = new Vector3[]{
                //top
                new Vector3(-hs, hs, hs),
                new Vector3( hs, hs, hs),
                new Vector3(-hs, hs,-hs),
                new Vector3( hs, hs,-hs),

                //front
                new Vector3(-hs, hs,-hs),
                new Vector3( hs, hs,-hs),
                new Vector3(-hs,-hs,-hs),
                new Vector3( hs,-hs,-hs),

                //right
                new Vector3( hs, hs,-hs),
                new Vector3( hs, hs, hs),
                new Vector3( hs,-hs,-hs),
                new Vector3( hs,-hs, hs),

                //back
                new Vector3( hs, hs, hs),
                new Vector3(-hs, hs, hs),
                new Vector3( hs,-hs, hs),
                new Vector3(-hs,-hs, hs),

                //left
                new Vector3(-hs, hs, hs),
                new Vector3(-hs, hs,-hs),
                new Vector3(-hs,-hs, hs),
                new Vector3(-hs,-hs,-hs),

                //bottom
                new Vector3(-hs,-hs,-hs),
                new Vector3( hs,-hs,-hs),
                new Vector3(-hs,-hs, hs),
                new Vector3( hs,-hs, hs),
            };

            Debug.Assert(NumOfVertices == mesh.vertices.Length);
        }
        protected override void StepUv(Mesh mesh)
        {
            var uv = new Vector2[NumOfVertices];
            for (int i = 0; i < 6; i++)
            {
                uv[i * 4 + 0] = new Vector2(0, 1);
                uv[i * 4 + 1] = new Vector2(1, 1);
                uv[i * 4 + 2] = new Vector2(0, 0);
                uv[i * 4 + 3] = new Vector2(1, 0);
            }
            mesh.uv = uv;
        }
        protected override void StepTriangle(Mesh mesh)
        {
            var tri = new int[6 * 6];
            for (int i = 0; i < 6; i++)
            {
                tri[i * 6 + 0] = i * 4 + 0;
                tri[i * 6 + 1] = i * 4 + 1;
                tri[i * 6 + 2] = i * 4 + 2;

                tri[i * 6 + 3] = i * 4 + 1;
                tri[i * 6 + 4] = i * 4 + 3;
                tri[i * 6 + 5] = i * 4 + 2;
            }
            mesh.triangles = tri;
        }

        public override void InitTransform(Transform t, string name)
        {
            var words = name.Split();

            if (words.Length == 4)
            {
                if (float.TryParse(words[1], out float w) && float.TryParse(words[2], out float h) && float.TryParse(words[3], out float d))
                {
                    t.localScale = new Vector3(w, h, d);
                }
            }
        }
    }
}
