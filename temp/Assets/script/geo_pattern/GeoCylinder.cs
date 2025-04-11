using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.script.geo_pattern
{
    // todo 03 : 현재 실린더는 윗면과 아랫면이 비어 있습니다.
    //  윗 면과 아랫면이 만들어지도록 수정하세요.
    internal class GeoCylinder : GeoBase
    {
        int numOfAngle;

        protected override int NumOfVertices => numOfAngle * 2 * 2;

        public GeoCylinder(int numOfAngle)
        {
            this.numOfAngle = numOfAngle;
        }

        protected override void StepVertex(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;
            var vtx = new Vector3[numOfVtx];
            float stepInRadians = Mathf.PI * 2F / numOfAngle;
            float stepInHeight = 0.5F;

            Vector3 pt = Vector3.zero;
            int index = 0;
            float r = 1F;

            for (int i = 0; i <= 1; i++)
            {
                pt.y = 0.5F - i * stepInHeight;

                pt.x = r;
                pt.z = 0F;
                vtx[index++] = pt;

                for (int j = 1; j < numOfAngle; j++)
                {
                    pt.x = r * Mathf.Cos(j * stepInRadians);
                    pt.z = r * Mathf.Sin(j * stepInRadians);
                    vtx[index++] = pt;
                    vtx[index++] = pt;
                }

                pt.x = r;
                pt.z = 0F;
                vtx[index++] = pt;
            }
            Debug.Assert(index == numOfVtx);
            mesh.vertices = vtx;
        }
        protected override void StepUv(Mesh mesh)
        {
            int numOfVtx = NumOfVertices;
            var uvs = new Vector2[numOfVtx];
            Vector2 uv = Vector2.zero;
            int index = 0;

            for (int i = 0; i <= 1; i++)
            {
                uv.y = 1F - (float)i;
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
            Debug.Assert(index == numOfVtx);
            mesh.uv = uvs;
        }
        protected override void StepTriangle(Mesh mesh)
        {
            var tri = new int[numOfAngle * 6];
            int numOfVtxPerSlice = numOfAngle * 2;
            int index = 0;
            for (int i = 0; i < 1; i++)
            {
                int baseIndex = i * numOfVtxPerSlice;
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
            Debug.Assert(index == numOfAngle * 6);
            mesh.triangles = tri;
        }

        public override void InitTransform(Transform t, string name)
        {
            var words = name.Split();

            if (words.Length == 4)
            {
                if (float.TryParse(words[2], out float r) && float.TryParse(words[3], out float h))
                {
                    t.localScale = new Vector3(r, h, r);
                }
            }
        }
    }
}
