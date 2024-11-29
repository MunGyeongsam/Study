using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script.geo_pattern
{
    internal class GeoRect : GeoBase
    {
        protected override int NumOfVertices => 4;

        protected override void StepVertex(Mesh mesh)
        {
            float px = 1 * 0.5F;    //positive x
            float nx = -px;         //negative x

            float pz = 1 * 0.5F;    //positive z
            float nz = -pz;

            //  +z
            //   |
            // 0 *---* 1
            //   | / |
            // 2 *---* 3 -- +x
            mesh.vertices = new Vector3[4]
            {
                new Vector3 (nx, 0F, pz),
                new Vector3 (px, 0F, pz),
                new Vector3 (nx, 0F, nz),
                new Vector3 (px, 0F, nz),
            };
        }
        protected override void StepUv(Mesh mesh)
        {
            mesh.uv = new Vector2[4]
            {
                new Vector2 (0F, 1F),
                new Vector2 (1F, 1F),
                new Vector2 (0F, 0F),
                new Vector2 (1F, 0F),
            };
        }

        protected override void StepTriangle(Mesh mesh)
        {
            mesh.triangles = new int[2 * 3]
            {
                0, 1, 2,
                2, 1, 3
            };
        }

        public override void InitTransform(Transform t, string name)
        {
            var words = name.Split();


            if (words.Length == 2)
            {
                if (float.TryParse(words[1], out float size))
                {
                    t.localScale = new Vector3(size, size, size);
                }
            }
            else if (words.Length == 3)
            {
                if (float.TryParse(words[1], out float w) && float.TryParse(words[2], out float h))
                {
                    t.localScale = new Vector3(w, 1F, h);
                }
            }
        }
    }
}
