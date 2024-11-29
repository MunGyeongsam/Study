using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script.geo_pattern
{
    internal class GeoCircle : GeoBase
    {
        private int numOfAngle;

        protected override int NumOfVertices => numOfAngle + 1;

        public GeoCircle(int numOfAngle)
        {
            this.numOfAngle = numOfAngle;
        }


        protected override void StepVertex(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;

            var vtx = new Vector3[numOfVtx];
            float stepInRadians = Mathf.PI * 2F / numOfAngle;

            for (int i = 0; i < numOfAngle; i++)
            {
                float c = Mathf.Cos((i + 1) * stepInRadians);
                float s = Mathf.Sin((i + 1) * stepInRadians);
                vtx[1 + i].x = c;
                vtx[1 + i].z = s;
            }

            mesh.vertices = vtx;
        }
        protected override void StepUv(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;
            var uv = new Vector2[numOfVtx];
            float stepInRadians = Mathf.PI * 2F / numOfAngle;

            uv[0].x = 0.5F;
            uv[0].y = 0.5F;

            for (int i = 0; i < numOfAngle; i++)
            {
                float c = Mathf.Cos((i + 1) * stepInRadians);
                float s = Mathf.Sin((i + 1) * stepInRadians);

                uv[1 + i].x = c * 0.5F + 0.5F;
                uv[1 + i].y = s * 0.5F + 0.5F;
            }
            mesh.uv = uv;
        }
        protected override void StepTriangle(Mesh mesh)
        {
            var tri = new int[numOfAngle * 3];
            for (int i = 0, j = 2; i < numOfAngle * 3 - 1; i += 3, ++j)
            {
                tri[i + 0] = 0;
                tri[i + 1] = j;
                tri[i + 2] = j - 1;
            }

            tri[^3] = 0;
            tri[^2] = 1;
            tri[^1] = numOfAngle;

            mesh.triangles = tri;
        }

        public override void InitTransform(Transform t, string name)
        {
            var words = name.Split();

            if (words.Length == 3 )
            {
                if (float.TryParse(words[2], out float radius))
                {
                    t.localScale = new Vector3 (radius, radius, radius);
                }
            }
            else if (words.Length == 4 )
            {
                if (float.TryParse(words[2], out float w) && float.TryParse(words[3], out float h))
                {
                    t.localScale = new Vector3(w, 1F, h);
                }
            }

        }
    }
}
