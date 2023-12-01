using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class Algorithm
{
      public delegate float Heuristic(int dx, int dy);
      
      public static void Find(List<Node> path, Node from, Node to, Grid _grid, Heuristic heuristic)
      {
            from.G = 0;
            from.H = 0;

            float SQRT2 = Mathf.Sqrt(2F);
            
            Queue<Node> queue = new();
            queue.Enqueue(from);

            int TR = to.Row;
            int TC = to.Col;

            Node endNode = from;
            
            while (queue.Count > 0)
            {
                  Node node = queue.Dequeue();
                  node.Closed = true;
                  endNode = node;

                  if (Node.IsEqual(node, to))
                        break;

                  int R = node.Row;
                  int C = node.Col;
                  float G = node.G;

                  Node[] neighbors = _grid.Neighbors(node);
                  bool updated = false;
                  foreach (var neighbor in neighbors)
                  {
                        if(neighbor.Closed)
                              continue;

                        int r = neighbor.Row;
                        int c = neighbor.Col;
                        float g = G + ((R == r || C == c) ? 1F : SQRT2);

                        if (!neighbor.Opened || g < neighbor.G)
                        {
                              neighbor.G = g;
                              neighbor.H = heuristic(Math.Abs(TC - c), Math.Abs(TR - r));
                              neighbor.Parent = node;
                              if (!neighbor.Opened)
                              {
                                    queue.Enqueue(neighbor);
                                    neighbor.Opened = true;
                              }

                              updated = true;
                        }
                  }

                  if (updated)
                        queue = new(queue.OrderBy(n => n.F));
            }
            
            GetPath(path, endNode);
      }

      static void GetPath(List<Node> path, Node endNode)
      {
            Debug.LogFormat("cost : {0}", endNode.G);
            path.Clear();

            do
            {
                  path.Add(endNode);
                  endNode = endNode.Parent;
            } while (endNode != null);

            path.Reverse();
      }
}

static class AStar
{
      static float Heuristic(int dx, int dy)
      {
            return (float)(dx + dy);
      }

      public static void Find(List<Node> path, Node from, Node to, Grid grid)
      {
            Algorithm.Find(path, from, to, grid, Heuristic);
      }
}

static class Dijkstra
{
      static float Heuristic(int dx, int dy)
      {
            return 0F;
      }

      public static void Find(List<Node> path, Node from, Node to, Grid grid)
      {
            Algorithm.Find(path, from, to, grid, Heuristic);
      }
}

static class BestFirstSearch
{
      static float Heuristic(int dx, int dy)
      {
            return (float)(dx + dy) * 1000F;
      }

      public static void Find(List<Node> path, Node from, Node to, Grid grid)
      {
            Algorithm.Find(path, from, to, grid, Heuristic);
      }
}