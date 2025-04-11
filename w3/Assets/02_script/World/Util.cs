using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prop = _02_script.w3.Prop;
using Random = UnityEngine.Random;

namespace Util 
{
    static class Zone
    {
        static List<Vector3> meshVtx;
        static List<Vector2> meshUvs;
        static List<Color> meshColors;
        static List<int> meshTri;

        static Zone()
        {
            const int SIZE = ZoneConfig.ZONE_SIZE_IN_TILES;
            meshVtx = new List<Vector3>(SIZE * SIZE * 4);
            meshUvs = new List<Vector2>(SIZE * SIZE * 4);
            meshColors = new List<Color>(SIZE * SIZE * 4);
            meshTri = new List<int>(SIZE * SIZE * 6);
        }
        
        public static Mesh[] GetMeshs(int layer)
        {
            byte[,] map = GetByteMap(ZoneConfig.ZONE_SIZE_IN_TILES, layer);
            var pmaps = GetPropMap(map, layer);
            
            Mesh[] rt = new Mesh[layer];
            
            for (int i = 0; i < layer; i++)
            {
                rt[i] = GetMesh(pmaps[i]);
            }

            return rt;
        }

        static Mesh GetMesh(Prop[,] pmap)
        {
            Mesh rt = new Mesh();

            int numOfRow = pmap.GetLength(0);
            int numOfCol = pmap.GetLength(1);
            
            meshVtx.Clear();
            meshUvs.Clear();
            meshColors.Clear();
            meshTri.Clear();
            
            Span<Vector2> uvBuffer = stackalloc Vector2[4];
            Span<Vector3> vtxBuffer = stackalloc Vector3[4];
            
            float tileSize = ZoneConfig.TILE_SIZE;

            Vector3 leftBottom = Vector3.zero;
            for (int r = 0; r < numOfRow; r++)
            {
                leftBottom.x = 0F;
                for (int c = 0; c < numOfCol; c++)
                {
                    Prop prop = pmap[r, c];
                    
                    if (prop.IsEmpty)
                        continue;
                    
                    W3Util.GetVtx(vtxBuffer, leftBottom, tileSize);
                    W3Util.GetedgeUvs(uvBuffer, prop);

                    meshVtx.Add(vtxBuffer[0]);
                    meshVtx.Add(vtxBuffer[1]);
                    meshVtx.Add(vtxBuffer[2]);
                    meshVtx.Add(vtxBuffer[3]);

                    meshUvs.Add(uvBuffer[0]);
                    meshUvs.Add(uvBuffer[1]);
                    meshUvs.Add(uvBuffer[2]);
                    meshUvs.Add(uvBuffer[3]);

                    meshColors.Add(Color.white);
                    meshColors.Add(Color.white);
                    meshColors.Add(Color.white);
                    meshColors.Add(Color.white);

                    int iv = meshVtx.Count - 4;
                    meshTri.Add(iv + 0);
                    meshTri.Add(iv + 1);
                    meshTri.Add(iv + 2);
                    meshTri.Add(iv + 1);
                    meshTri.Add(iv + 3);
                    meshTri.Add(iv + 2);

                    leftBottom.x += tileSize;
                }

                leftBottom.z += tileSize;
            }

            rt.vertices = meshVtx.ToArray();
            rt.uv = meshUvs.ToArray();
            rt.colors = meshColors.ToArray();
            rt.triangles = meshTri.ToArray();

            W3Util.RecalcMesh(rt);

            return rt;
        }
        
        static Prop[][,] GetPropMap(byte[,] map, int nLayer)
        {
            int size = map.GetLength(0);
            int mapSizeInTiles = size + 1;
            
            Debug.Assert(size == map.GetLength(1));

            Prop[][,] pmaps = new Prop[nLayer][,];
            for(int i= 0;i < nLayer; i++)
            {
                pmaps[i] = new Prop[mapSizeInTiles, mapSizeInTiles];
            }

            Prop[,] pmap = pmaps[0];
            
            Prop full = new Prop(true, true, true, true);
            for (int i = 0; i < mapSizeInTiles; ++i)
            {
                for (int j = 0; j < mapSizeInTiles; ++j)
                {
                    pmap[i, j] = full;
                }
            }

            for (int r = 0; r < size; r++)
            {
                for (int c = 0; c < size; c++)
                {
                    int layer = map[r, c];
                    if (layer == 0)
                        continue;

                    pmap = pmaps[layer];

                    pmap[r + 0, c + 0].RT = true;
                    pmap[r + 0, c + 1].LT = true;
                    pmap[r + 1, c + 0].RB = true;
                    pmap[r + 1, c + 1].LB = true;
                }
            }
            
            //ClearPropMap(pmaps);

            return pmaps;
        }

        static void ClearPropMap(Prop[][,] pmaps)
        { 
            int nLayer = pmaps.Length;
            for (int i = nLayer - 1; i >= 1; --i)
            {
                var pmap = pmaps[i];
                for (int r = 0; r < pmap.GetLength(0); ++r)
                {
                    for (int c = 0; c < pmap.GetLength(1); ++c)
                    {
                        if (pmap[r, c].IsFull)
                        {
                            ClearLowerLayerProps(pmaps, i, r, c);
                        }
                    }
                }
            }
        }

        static void ClearLowerLayerProps(Prop[][,] pmaps, int layer, int r, int c)
        {
            for (int i = 0; i < layer; ++i)
            {
                pmaps[i][r, c].Clear();
            }
        }
        
        static byte[,] GetByteMap(int sizeInTiles, int nLayer)
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
                        r = Random.Range(0, size);
                        c = Random.Range(0, size);
                        if (tryCnt++ > 10)
                            break;
                    } while (rt[r, c] != 0);

                    --fillCnt;
                    rt[r, c] = (byte)i;
                }
            }

            return rt;
        }
    }
}
