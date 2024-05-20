using jungol.UT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jungol.Beginner
{
    internal class _08_Recursive
    {
        public static void Run()
        {
            Util.Call(_1309);
            Util.Call(_1169);
            Util.Call(_1175);
            Util.Call(_2817);
            Util.Call(_1490);
            Util.Call(_1161);
            Util.Call(_1459);
            Util.Call(_1021);
            Util.Call(_2567);
        }

        //--------------------------------------------------
        // 1309 팩토리얼
        //--------------------------------------------------
        static ulong Impl_1309_Factorial(ulong n)
        {
            if (n == 1ul)
            {
                Console.WriteLine("1! = 1");
                return 1ul;
            }
            else
            {
                Console.WriteLine("{0}! = {0} * {1}!", n, n - 1);
                return n * Impl_1309_Factorial(n - 1);
            }

            //return (n == 0ul) ? 1ul : n*Impl_1309_Factorial(n-1);
        }
        static void Impl_1309(int n)
        {
            Console.WriteLine(Impl_1309_Factorial((ulong)n));
        }
        static void _1309()
        {
            Impl_1309(4);
        }

        //--------------------------------------------------
        // 1169 주사위 던지기1
        //--------------------------------------------------
        static void Impl_1169_1_rcv(int n, int di, int[] nums)
        {
            if (di >= n)
            {
                for (int i = 0; i < n; ++i)
                    Console.Write("{0} ", nums[i]);
                Console.WriteLine();
                return;
            }

            for (int i = 1; i <= 6; ++i)
            {
                nums[di] = i;
                Impl_1169_1_rcv(n, di + 1, nums);
            }
        }
        static void Impl_1169_1(int n)
        {
            int[] nums = new int[n];
            Impl_1169_1_rcv(n, 0, nums);
        }


        static void Impl_1169_2_rcv(int n, int di, int[] nums, HashSet<int> flags)
        {
            if (di >= n)
            {
                int[] tmp = new int[n];
                Array.Copy(nums, tmp, n);
                Array.Sort(tmp);

                int hash = 0;
                int digit = 1;
                for (int i = 0; i < n; ++i)
                {
                    hash += tmp[i] * digit;
                    digit *= 10;
                }

                if (flags.Contains(hash))
                    return;

                flags.Add(hash);
                for (int i = 0; i < n; ++i)
                    Console.Write("{0} ", tmp[i]);
                Console.WriteLine();
                return;
            }

            for (int i = 1; i <= 6; ++i)
            {
                nums[di] = i;
                Impl_1169_2_rcv(n, di + 1, nums, flags);
            }
        }
        static void Impl_1169_2(int n)
        {
            int[] nums = new int[n];
            HashSet<int> flags = new HashSet<int>();
            Impl_1169_2_rcv(n, 0, nums, flags);
        }


        static void Impl_1169_3_rcv(int n, int di, int[] nums)
        {
            if (di >= n)
            {
                for (int i = 0; i < n; ++i)
                    Console.Write("{0} ", nums[i]);
                Console.WriteLine();
                return;
            }

            for (int i = 1; i <= 6; ++i)
            {
                int index = Array.IndexOf<int>(nums, i);
                if (index < 0 || index >= di)
                {
                    nums[di] = i;
                    Impl_1169_3_rcv(n, di + 1, nums);
                }
            }
        }
        static void Impl_1169_3(int n)
        {
            int[] nums = new int[3];

            Impl_1169_3_rcv(n, 0, nums);
        }
        static void Impl_1169(int n, int t)
        {
            if (t == 1)
                Impl_1169_1(n);
            else if (t == 2)
                Impl_1169_2(n);
            else if (t == 3)
                Impl_1169_3(n);
            Console.WriteLine();
        }
        static void _1169()
        {
            Impl_1169(3, 1);
            Impl_1169(3, 2);
            Impl_1169(3, 3);
        }

        //--------------------------------------------------
        // 1175 주사위 던지기2
        //--------------------------------------------------
        static void Impl_1175_rcv(int n, int sum, int di, int[] nums)
        {
            if (di >= n)
            {
                int tot = 0;
                foreach (int num in nums)
                {
                    tot += num;
                }

                if (tot != sum)
                    return;

                for (int i = 0; i < n; ++i)
                {
                    Console.Write("{0} ", nums[i]);
                }
                Console.WriteLine();
            }
            else
            {
                for (int i = 1; i <= 6; ++i)
                {
                    nums[di] = i;
                    Impl_1175_rcv(n, sum, di + 1, nums);
                }
            }
        }
        static void Impl_1175(int n, int sum)
        {
            int[] nums = new int[n];
            Impl_1175_rcv(n, sum, 0, nums);
        }
        static void _1175()
        {
            Impl_1175(3, 10);
        }

        //--------------------------------------------------
        // 2817 로또(Lotto)
        //--------------------------------------------------
        static void Impl_2817_Lotto(string input)
        {
            var words = input.Split(' ');
            int n = int.Parse(words[0]);
            System.Diagnostics.Debug.Assert(n == words.Length - 1);

            int[] elements = new int[n];
            for (int i = 0; i < n; ++i)
            {
                elements[i] = int.Parse(words[i + 1]);
            }
            Array.Sort(elements);
            int[] dest = new int[6];
            Impl_2817_RecvGetCombination(elements, dest, 0, 0);
        }
        static void Impl_2817_RecvGetCombination(int[] input, int[] dest, int index, int start)
        {
            if (index == 6)
            {
                for (int i = 0; i < 6; ++i)
                {
                    Console.Write($"{dest[i],2} ");
                }
                Console.WriteLine();
                return;
            }

            for (int i = start; i < input.Length; ++i)
            {
                dest[index] = input[i];
                Impl_2817_RecvGetCombination(input, dest, index + 1, i + 1);
            }
        }
        static void _2817()
        {
            Impl_2817_Lotto("7 1 2 3 4 5 6 7");
            Console.WriteLine();
            Impl_2817_Lotto("8 1 2 3 5 8 13 21 34");
            Console.WriteLine();
        }

        //--------------------------------------------------
        // 1490 다음 조합(next combination)
        //--------------------------------------------------
        static void Impl_1490(string input)
        {
            var lines = input.Split('\n');
            var words = lines[0].Split(' ');
            int n = int.Parse(words[0]);
            int k = int.Parse(words[1]);

            int[] dest = new int[k];
            int[] prev = new int[k];
            words = lines[1].Split(' ');
            for (int i = 0; i < k; ++i)
            {
                prev[i] = int.Parse(words[i]);
            }
            int state = 0; //0:not found, 1:found, 2:exit
            Impl_1490_RecsvGetCombination(n, k, dest, prev, 0, 0, ref state);
            if (state != 2)
                Console.WriteLine("NONE");
        }

        static void Impl_1490_RecsvGetCombination(int n, int k, int[] dest, int[] prev, int index, int start, ref int state)
        {
            if (index == k)
            {
                if (state == 0)
                {
                    state = 1;
                    return;
                }

                for (int i = 0; i < k; ++i)
                {
                    Console.Write($"{dest[i],2} ");
                }
                Console.WriteLine();
                state = 2;
                return;
            }

            for (int i = start; i < n; ++i)
            {
                dest[index] = i + 1;
                if (state == 0 && prev[index] != dest[index])
                    continue;

                if (state == 2)
                    return;

                Impl_1490_RecsvGetCombination(n, k, dest, prev, index + 1, i + 1, ref state);
            }
        }

        static void _1490()
        {
            Impl_1490("5 3\n1 4 5");
            Impl_1490("5 3\n3 4 5");
        }

        //--------------------------------------------------
        // 1161 하노이의 탑
        //--------------------------------------------------
        class HanoiTower
        {
            int Layer { get; set; }
            bool ShowGraph { get; set; }

            int[,] columns;


            public HanoiTower(int n)
            {
                ShowGraph = false;
                Layer = n;
                columns = new int[3, n];

                for (int i = 0; i < n; ++i)
                    columns[0, i] = n - i;
            }


            public void Solve(bool showGraph)
            {
                ShowGraph = showGraph;
                Move(0, 2, 0);
            }


            private int Height(int c)
            {
                for (int i = 0; i < Layer; ++i)
                    if (columns[c, i] == 0)
                        return i;

                return Layer;
            }
            private int TopLayer(int c)
            {
                return Height(c) - 1;
            }

            private int FindOtherColumn(int from, int to)
            {
                if (0 != from && 0 != to)
                    return 0;
                else if (1 != from && 1 != to)
                    return 1;
                else
                    return 2;
            }

            private int BottomLayer(int c, int plate)
            {
                int bottom = -1;
                for (int i = TopLayer(c); i >= 0; --i)
                {
                    if (columns[c, i] < plate)
                        bottom = i;
                }

                return bottom;
            }

            private int Pop(int c)
            {
                int h = Height(c);
                System.Diagnostics.Debug.Assert(h > 0);

                int rt = columns[c, h - 1];
                columns[c, h - 1] = 0;

                return rt;
            }

            private void Push(int c, int n)
            {
                int h = Height(c);
                System.Diagnostics.Debug.Assert(h < Layer);

                columns[c, h] = n;
            }

            private void Draw()
            {
                char C = 'C';
                char S = ' ';
                char V = ' ';

                int layer = Layer;

                System.Text.StringBuilder sb = new System.Text.StringBuilder(layer * 2 * 3 + 2);
                for (int i = layer - 1; i >= 0; --i)
                {
                    sb.Clear();
                    for (int j = 0; j < 3; ++j)
                    {
                        int n = columns[j, i];
                        sb.Append(new string(S, layer - n));
                        sb.Append(new string(C, n * 2));
                        sb.Append(new string(S, layer - n));
                        if (j < 2)
                            sb.Append(V);
                    }
                    Console.WriteLine(sb);
                }
                Console.WriteLine(new string('-', layer * 2 * 3 + 2));
                Console.WriteLine();
            }

            int maxDepth = 0;
            int depth = 0;
            int moveCount = 0;
            private void Move(int from, int to, int layer)
            {
                ++depth;
                if (depth > maxDepth)
                    maxDepth = depth;

                int remainColumn = FindOtherColumn(from, to);
                if (TopLayer(from) == layer)
                {
                    int plate = Pop(from);
                    Push(to, plate);

                    ++moveCount;


                    if (ShowGraph)
                    {
                        System.Threading.Thread.Sleep(50);
                        Console.Clear();
                        Draw();
                    }
                    Console.WriteLine("{0} : {1} -> {2} (depth : {3} / {4}, move count : {5})", plate, from + 1, to + 1, depth, maxDepth, moveCount);

                    int bLayer = BottomLayer(remainColumn, plate);
                    if (bLayer >= 0)
                        Move(remainColumn, to, bLayer);
                }
                else
                {
                    Move(from, remainColumn, layer + 1);
                    Move(from, to, layer);
                }
                --depth;
            }
        }
        static void Impl_1161(int n, bool showGraph)
        {
            HanoiTower h = new HanoiTower(n);

            h.Solve(showGraph);
        }
        static void _1161()
        {
            //for(int i=1; i<=15; ++i)
            //{
            //    Impl_1161(i, true);
            //    System.Threading.Thread.Sleep(5000);
            //}

            Impl_1161(3, false);
            //Impl_1161(12, false);
        }

        //--------------------------------------------------
        // 1459 숫자고르기
        //
        // 풀이 - 끝과 시작이 연결된 경우를 찾아보자
        //--------------------------------------------------
        static bool Impl_1459_IsConnected(int[] nums, int i)
        {
            int si = i + 1;
            List<int> tbl = new List<int>();
            tbl.Add(si);

            while (tbl.IndexOf(nums[i]) == -1)
            {
                i = nums[i] - 1;
                tbl.Add(i + 1);
            }

            return nums[i] == si;
        }
        static void Impl_1459(int n, params int[] nums)
        {
            System.Diagnostics.Debug.Assert(n == nums.Length);


            List<int> list = new List<int>();
            for (int i = 0; i < n; ++i)
            {
                if (Impl_1459_IsConnected(nums, i))
                {
                    list.Add(i + 1);
                }
            }

            Console.WriteLine(list.Count);
            for (int i = 0; i < list.Count; ++i)
                Console.WriteLine(list[i]);
        }
        static void _1459()
        {
            Impl_1459(7, 3, 1, 1, 5, 5, 4, 6);
        }

        //--------------------------------------------------
        // 1021 장난감조립
        //--------------------------------------------------
        struct stImpl_1021_NoCount
        {
            public int No { get; set; }
            public int Count { get; set; }
        }
        static void Impl_1021_rcsv(Dictionary<int, int> rt, int key, Dictionary<int, List<stImpl_1021_NoCount>> dic)
        {
            List<stImpl_1021_NoCount> list = dic[key];
            foreach (stImpl_1021_NoCount e in list)
            {
                int no = e.No;
                if (dic.ContainsKey(no))
                {
                    for (int i = 0; i < e.Count; ++i)
                        Impl_1021_rcsv(rt, no, dic);
                }
                else
                {
                    if (rt.ContainsKey(e.No))
                    {
                        rt[e.No] += e.Count;
                    }
                    else
                    {
                        rt[e.No] = e.Count;
                    }
                }
            }
        }
        static void Impl_1021(string multiLines)
        {
            Dictionary<int, List<stImpl_1021_NoCount>> dic = new Dictionary<int, List<stImpl_1021_NoCount>>();

            string[] lines = multiLines.Split(new char[] { '\n' });
            int nComplete = Convert.ToInt32(lines[0]);
            int nLines = Convert.ToInt32(lines[1]);

            for (int i = 0; i < nLines; ++i)
            {
                string line = lines[i + 2];
                string[] words = line.Split();

                int key = Convert.ToInt32(words[0]);
                int no = Convert.ToInt32(words[1]);
                int cnt = Convert.ToInt32(words[2]);


                if (dic.ContainsKey(key))
                {
                    List<stImpl_1021_NoCount> list = dic[key];
                    list.Add(new stImpl_1021_NoCount { No = no, Count = cnt });
                }
                else
                {
                    List<stImpl_1021_NoCount> list = new List<stImpl_1021_NoCount>();
                    list.Add(new stImpl_1021_NoCount { No = no, Count = cnt });
                    dic[key] = list;
                }
            }


            Dictionary<int, int> result = new Dictionary<int, int>();
            Impl_1021_rcsv(result, nComplete, dic);


            var keys = result.Keys.ToList();
            keys.Sort();
            foreach (var k in keys)
            {
                Console.WriteLine("{0} {1}", k, result[k]);
            }
        }
        static void _1021()
        {
            string input = @"7
8
5 1 2
5 2 2
7 5 2
6 5 2
6 3 3
6 4 4
7 6 3
7 4 5";
            Impl_1021(input);

            Console.WriteLine();
            input = @"5
6
1 3 4
1 4 2
3 4 1
5 3 2
5 1 3
3 2 3";
            Impl_1021(input);
        }

        //--------------------------------------------------
        // 2567 싸이클
        //--------------------------------------------------
        static void Impl_2567(string input)
        {
            var words = input.Split(' ');
            System.Diagnostics.Debug.Assert(2 == words.Length);

            int N = int.Parse(words[0]);
            int P = int.Parse(words[1]);

            var list = new List<int>();

            int n = N * N % P;
            while (list.IndexOf(n) == -1)
            {
                list.Add(n);
                //Console.Write($"{n} ");
                n = n * N % P;
            }
            //Console.Write("--");
            Console.WriteLine(list.Count);
        }
        static void _2567()
        {
            Impl_2567("67 31");
            Impl_2567("96 61");
        }
    }
}
