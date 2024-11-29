using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.script.geo_pattern
{
    internal class GeoSphere : GeoBase
    {
        int numOfAngle;

        protected override int NumOfVertices {
            get
            {
                int numOfSlice = numOfAngle / 2;
                int numOfRect = numOfAngle * numOfSlice;
                int numOfVtx = numOfRect * 4;

                return numOfVtx;
            }
        }

        public GeoSphere(int numOfAngle)
        {
            this.numOfAngle = numOfAngle;
        }

        protected override void StepVertex(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;
            int numOfSlice = numOfAngle / 2;

            var vtx = new Vector3[numOfVtx];
            float stepInRadians = Mathf.PI * 2F / numOfAngle;

            Vector3 pt = Vector3.zero;
            int index = 0;

            for (int i = 0; i < numOfSlice; ++i)
            {
                for (int i2 = 0; i2 < 2; ++i2)
                {
                    float y = Mathf.Cos((i + i2) * Mathf.PI / numOfSlice);
                    float r = Mathf.Sin((i + i2) * Mathf.PI / numOfSlice);

                    pt.y = y;
                    pt.x = r;
                    pt.z = 0F;
                    vtx[index++] = pt;

                    for (int j = 1; j < numOfAngle; j++)
                    {
                        pt.x = r * Mathf.Cos(j * stepInRadians);
                        pt.y = y;
                        pt.z = r * Mathf.Sin(j * stepInRadians);
                        vtx[index++] = pt;
                        vtx[index++] = pt;
                    }

                    pt.x = r;
                    pt.z = 0F;
                    vtx[index++] = pt;
                }
            }
            Debug.Assert(index == numOfVtx);
            mesh.vertices = vtx;
        }
        protected override void StepUv(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;
            int numOfSlice = numOfAngle / 2;

            var uvs = new Vector2[numOfVtx];
            Vector2 uv = Vector2.zero;
            int index = 0;

            for (int i = 0; i < numOfSlice; i++)
            {
                for (int i2 = 0; i2 < 2; ++i2)
                {
                    float y = 0.5F + 0.5F * Mathf.Cos((i + i2) * Mathf.PI / numOfSlice);


                    //uv.y = 1F - (i+i2) / (float)numOfSlice;
                    uv.y = y;
                    uv.x = 0F;
                    uvs[index++] = uv;

                    for (int j = 1; j < numOfAngle; j++)
                    {
                        uv.x = j / (float)numOfAngle;
                        uvs[index++] = uv;
                        uvs[index++] = uv;
                    }

                    uv.x = 1F;
                    uvs[index++] = uv;
                }
            }
            Debug.Assert(index == numOfVtx);
            mesh.uv = uvs;
        }
        protected override void StepTriangle(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;
            int numOfSlice = numOfAngle / 2;
            int numOfRect = numOfAngle * numOfSlice;

            var tri = new int[numOfRect * 6];
            int numOfVtxPerSlice = numOfAngle * 2;
            int index = 0;
            for (int i = 0; i < numOfSlice; i++)
            {
                int baseIndex = i * numOfVtxPerSlice * 2;
                for (int j = 0; j < numOfAngle; j++)
                {
                    tri[index++] = baseIndex + j * 2;
                    tri[index++] = baseIndex + j * 2 + 1;
                    tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;

                    tri[index++] = baseIndex + j * 2 + 1;
                    tri[index++] = baseIndex + j * 2 + 1 + numOfVtxPerSlice;
                    tri[index++] = baseIndex + j * 2 + numOfVtxPerSlice;
                }
            }
            Debug.Assert(index == numOfAngle * 6 * numOfSlice);
            mesh.triangles = tri;
        }

        public override void InitTransform(Transform t, string name)
        {
            var words = name.Split();

            if (words.Length == 3)
            {
                if (float.TryParse(words[2], out float radius))
                {
                    t.localScale = new Vector3(radius, radius, radius);
                }
            }
            else if (words.Length == 5)
            {
                if (float.TryParse(words[2], out float w) && float.TryParse(words[3], out float h) && float.TryParse(words[4], out float d))
                {
                    t.localScale = new Vector3(w, h, d);
                }
            }

        }
    }
}
