using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Prop = _02_script.w3.Prop;
using Math = UnityEngine.Mathf;

public class _04_layer : MonoBehaviour
{
    [SerializeField]
    private Texture[] _texture;

    Material[] _mat;
    Mesh[] _mesh;
    
    BitArray _show;
    // Start is called before the first frame update
    void Start()
    {
        int nLayer = _texture.Length;
        _mat = new Material[nLayer];
        for(int i=0; i<nLayer; ++i)
            _mat[i] = W3Util.CreateMaterial(_texture[i], i);
        _show = new BitArray(nLayer, true);
        
        ResetMesh();
    }


    void ResetMesh()
    {
        int nLayer = _texture.Length;

        byte[,] map = Gen(128, nLayer);
        _mesh = GetMeshs(map, nLayer);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ResetMesh();

        if (_show != null)
        {
            if (_show.Count > 0 && Input.GetKeyDown(KeyCode.Alpha0)) _show.Set(0, !_show[0]);
            if (_show.Count > 1 && Input.GetKeyDown(KeyCode.Alpha1)) _show.Set(1, !_show[1]);
            if (_show.Count > 2 && Input.GetKeyDown(KeyCode.Alpha2)) _show.Set(2, !_show[2]);
            if (_show.Count > 3 && Input.GetKeyDown(KeyCode.Alpha3)) _show.Set(3, !_show[3]);
            if (_show.Count > 4 && Input.GetKeyDown(KeyCode.Alpha4)) _show.Set(4, !_show[4]);
            if (_show.Count > 5 && Input.GetKeyDown(KeyCode.Alpha5)) _show.Set(5, !_show[5]);
        }

        int nLayer = _texture.Length;
        Matrix4x4 m = transform.localToWorldMatrix;
        for (int i = 0; i < nLayer; ++i)
        {
            if (_show[i])
                Graphics.DrawMesh(_mesh[i], m, _mat[i], nLayer - i);
        }
    }
    
    static byte[,] Gen(int sizeInTiles, int nLayer)
    {
        int size = sizeInTiles - 1;
        byte[,] rt = new byte[size, size];
        int num = size * size;

        float fillRate = 0.8F;
        int fillCnt;

        for (int i = 1; i < nLayer; ++i)
        {
            fillCnt = (int)(num * fillRate);
            fillRate *= 0.5F;

            int r, c;
            while (fillCnt > 0)
            {
                int tryCnt = 0;
                do
                {
                    r = UnityEngine.Random.Range(0, size);
                    c = UnityEngine.Random.Range(0, size);
                    if (tryCnt++ > 10)
                        break;
                } while (rt[r, c] != 0);

                --fillCnt;
                rt[r, c] = (byte)i;
            }
        }

        return rt;
    }
    
    static List<Vector3> meshVtx;
    static List<Vector2> meshUvs;
    static List<int> meshTri;

    static _04_layer()
    {
        const int SIZE = 128;
        meshVtx = new List<Vector3>(SIZE * SIZE * 4);
        meshUvs = new List<Vector2>(SIZE * SIZE * 4);
        meshTri = new List<int>(SIZE * SIZE * 6);
    }

    //  +v
    //   |
    // 0 *---* 1
    //   | / |
    // 2 *---* 3 -- +u
    static void PushUv(Prop p)
    {
        Span<Vector2> uvs = stackalloc Vector2[4];
        W3Util.GetedgeUvs(uvs, p);

        meshUvs.Add(uvs[0]);
        meshUvs.Add(uvs[1]);
        meshUvs.Add(uvs[2]);
        meshUvs.Add(uvs[3]);
    }

    // +z
    //  |
    //  *-----*-----*-----*-----*
    //  |     |     |     |     |
    //  |3,0  |0,1  |0,2  |0,3  |
    //  *-----*-----*-----*-----*
    //  |     |     |     |     |
    //  |2,0  |1,1  |1,2  |1,3  |
    //  *-----*-----*-----*-----*
    //  |     |     |     |     |
    //  |1,0  |2,1  |2,2  |2,3  |
    //  *-----*-----*-----*-----*
    //  |r,c  |     |     |     |
    //  |0,0  |3,1  |3,2  |3,3  |
    //  *-----*-----*-----*-----*-- +x
    static void PushVtx(int row, int col, float tileSize)
    {
        Vector3 pt = Vector3.zero;

        int ix = col;
        int iz = row;

        pt.x = (ix + 0) * tileSize;
        pt.z = (iz + 0) * tileSize;
        meshVtx.Add(pt);

        pt.x = (ix + 1) * tileSize;
        pt.z = (iz + 0) * tileSize;
        meshVtx.Add(pt);

        pt.x = (ix + 0) * tileSize;
        pt.z = (iz - 1) * tileSize;
        meshVtx.Add(pt);

        pt.x = (ix + 1) * tileSize;
        pt.z = (iz - 1) * tileSize;
        meshVtx.Add(pt);
    }

    static void PushTri(int i)
    {
        meshTri.Add(i + 0);
        meshTri.Add(i + 1);
        meshTri.Add(i + 2);

        meshTri.Add(i + 1);
        meshTri.Add(i + 3);
        meshTri.Add(i + 2);
    }
    
    static Mesh GetMesh(Prop[,] pmap, int mapSizeInTiles, float tileSize = 1F)
    {
        int ROW = pmap.GetLength(0);
        int COL = pmap.GetLength(1);

        meshVtx.Clear(); meshVtx.Capacity = Math.Max(ROW * COL * 4, meshVtx.Capacity);
        meshUvs.Clear(); meshUvs.Capacity = Math.Max(ROW * COL * 4, meshUvs.Capacity);
        meshTri.Clear(); meshTri.Capacity = Math.Max(ROW * COL * 6, meshTri.Capacity);

        for (int r = 0; r < ROW; ++r)
        {
            for (int c = 0; c < COL; ++c)
            {
                Prop p = pmap[r, c];
                if (p.IsAny)
                {
                    int index = meshVtx.Count;

                    PushVtx(r, c, tileSize);
                    PushUv(p);
                    PushTri(index);
                }
            }
        }

        Mesh mesh = new Mesh();

        mesh.vertices = meshVtx.ToArray();
        mesh.uv = meshUvs.ToArray();
        mesh.triangles = meshTri.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        return mesh;
    }
    static Mesh[] GetMeshs(byte[,] map, int n)
        {
            Mesh[] rt = new Mesh[n];

            int size = map.GetLength(0);
            int mapSizeInTiles = size + 1;

            Prop[][,] pmaps = new Prop[n][,];

            Prop[,] pmap = new Prop[mapSizeInTiles, mapSizeInTiles];
            Prop full = new Prop(true, true, true, true);

            for (int i = 0; i < mapSizeInTiles; ++i)
                for (int j = 0; j < mapSizeInTiles; ++j)
                    pmap[i, j] = full;

            pmaps[0] = pmap;

            for (int l = 1; l < n; ++l)
            {
                pmaps[l] = new Prop[mapSizeInTiles, mapSizeInTiles];
            }

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    byte b = map[i, j];
                    if (b == 0)
                        continue;

                    pmap = pmaps[b];
                    pmap[i + 1, j + 0].RB = true;   pmap[i + 1, j + 1].LB = true;
                    pmap[i + 0, j + 0].RT = true;   pmap[i + 0, j + 1].LT = true;
                }
            }

            Prop[,] btm = pmaps[0];
            for (int r = 0; r < mapSizeInTiles; ++r)
            {
                for (int c = 0; c < mapSizeInTiles; ++c)
                {
                    for (int l = 1; l < n; ++l)
                    {
                        Prop p = pmaps[l][r, c];
                        if (p.IsFull)
                        {
                            btm[r, c].Clear();
                            break;
                        }
                    }
                }
            }

            for (int i = n - 1; i > 0; --i)
            {
                pmap = pmaps[i];
            
                for (int r = 0; r < mapSizeInTiles; ++r)
                {
                    for (int c = 0; c < mapSizeInTiles; ++c)
                    {
                        Prop p = pmap[r, c];
                        if (p.IsFull)
                            btm[r, c].Clear();
                    }
                }
            }

            for (int i = 0; i < n; ++i)
                rt[i] = GetMesh(pmaps[i], mapSizeInTiles);

            return rt;
        }
}
