using jungol.UT;
using System;
using System.Collections.Generic;
using System.Text;

namespace jungol.Beginner
{
    internal class _07_DataStructure
    {
        public static void Run()
        {
            Util.Call(_1146);
            Util.Call(_1158);
            Util.Call(_1157);
            Util.Call(_1814);
            Util.Call(_1102);
            Util.Call(_1221);
            Util.Call(_2858);
            Util.Call(_1697);
            Util.Call(_5801);
            Util.Call(_6057);
        }

        //--------------------------------------------------
        // 1146 선택정렬
        //--------------------------------------------------
        static void Impl_1146(int n, params int[] nums)
        {
            for (int i = 0; i < n - 1; ++i)
            {
                int min = nums[i];
                int theIndex = -1;
                for (int j = i + 1; j < n; ++j)
                {
                    if (min > nums[j])
                    {
                        min = nums[j];
                        theIndex = j;
                    }
                }

                if (theIndex != -1)
                {
                    int tmp = nums[i];
                    nums[i] = nums[theIndex];
                    nums[theIndex] = tmp;
                }

                Console.Write("[{0}] : ", i);
                for (int ii = 0; ii < nums.Length; ++ii)
                    Console.Write("{0} ", nums[ii]);
                Console.WriteLine();
            }
        }
        static void _1146()
        {
            Impl_1146(5, 6, 4, 8, 3, 1);
        }

        //--------------------------------------------------
        // 1158 삽입정렬
        //
        // 풀이 : 삽입정렬을 구현하려면 배열보단 LinkedList가 유리
        //       배열은 삽입 위치를 찾고 값의 뒤로 이동이 필요하나
        //       링크드리스트는 중간에 추가 삭제에 이동이 필요 없다.
        //--------------------------------------------------
        static void Impl_1158(int n, params int[] nums)
        {
            if (n <= 0)
                return;

            //LinkedList<int> list = new LinkedList<int>();
            //
            //list.AddFirst(nums[0]);
            //for(int i=1; i<n; ++i)
            //{
            //    bool bAdded = false;
            //    LinkedListNode<int> node = list.First;
            //    while(null != node)
            //    {
            //        if (node.Value > nums[i])
            //        {
            //            list.AddBefore(node, nums[i]);
            //            bAdded = true;
            //            break;
            //        }
            //        node = node.Next;
            //    }
            //
            //    if (false == bAdded)
            //    {
            //        list.AddLast(nums[i]);
            //    }
            //
            //
            //    Console.Write("[{0}] : ", i);
            //    node = list.First;
            //    while (null != node)
            //    {
            //        Console.Write("{0} ", node.Value);
            //        node = node.Next;
            //    }
            //    Console.WriteLine();
            //}

            for (int i = 1; i < n; ++i)
            {
                int cur = nums[i];

                for (int j = 0; j < i; ++j)
                {
                    if (cur < nums[j])
                    {
                        //뒤로 한칸씩 이동
                        for (int k = i; k > j; --k)
                            nums[k] = nums[k - 1];

                        //삽입
                        nums[j] = cur;
                        break;
                    }
                }

                Console.Write("[{0}] : ", i);
                for (int ii = 0; ii < nums.Length; ++ii)
                    Console.Write("{0} ", nums[ii]);
                Console.WriteLine();
            }
        }
        static void _1158()
        {
            Impl_1158(5, 5, 4, 3, 7, 6);
        }

        //--------------------------------------------------
        // 1157 버블정렬
        //--------------------------------------------------
        static void Impl_1157(int n, params int[] nums)
        {
            for (int i = 0; i < n - 1; ++i)
            {
                for (int j = 1; j < n - i; ++j)
                {
                    if (nums[j - 1] > nums[j])
                    {
                        int tmp = nums[j];
                        nums[j] = nums[j - 1];
                        nums[j - 1] = tmp;
                    }
                }
                Console.Write("[{0}] : ", i);
                for (int ii = 0; ii < nums.Length; ++ii)
                    Console.Write("{0} ", nums[ii]);
                Console.WriteLine();
            }
        }
        static void _1157()
        {
            Impl_1157(4, 62, 23, 32, 15);
        }

        //--------------------------------------------------
        // 1814 삽입정렬 횟수 세기
        //--------------------------------------------------
        static void Impl_1814(int n, params int[] nums)
        {
            if (n <= 0)
                return;

            int moveCount = 0;
            for (int i = 1; i < n; ++i)
            {
                int cur = nums[i];

                for (int j = 0; j < i; ++j)
                {
                    if (cur < nums[j])
                    {
                        //뒤로 한칸씩 이동
                        for (int k = i; k > j; --k)
                        {
                            ++moveCount;
                            nums[k] = nums[k - 1];
                        }

                        //삽입
                        nums[j] = cur;
                        break;
                    }
                }
            }
            Console.WriteLine(moveCount);
        }
        static void _1814()
        {
            Impl_1814(4, 20, 40, 30, 10);
            Impl_1814(3, -1, 1, 0);
        }

        //--------------------------------------------------
        // 1102 스택 (stack)
        //
        // 풀이 : 문제는 C/C++ 를 가정하여 List 를 이용한 구현을 원하지만
        //       C# 에서는 Stack은 기본적으로 제공됨.
        //--------------------------------------------------
        static void Impl_1102(string multiLine)
        {
            string[] lines = multiLine.Split(new char[] { '\n' });

            Stack<int> stack = new Stack<int>();
            //List<int> list = new List<int>();

            int cnt = Convert.ToInt32(lines[0]);
            for (int i = 1; i <= cnt; ++i)
            {
                string s = lines[i].Trim();
                char cmd = s[0];

                switch (cmd)
                {
                    case 'i':
                        {
                            string[] words = s.Split();
                            int n = Convert.ToInt32(words[1]);
                            //list.Add(n);

                            stack.Push(n);
                        }
                        break;
                    case 'c':
                        {
                            //Console.WriteLine(list.Count);
                            Console.WriteLine(stack.Count);
                        }
                        break;
                    case 'o':
                        {
                            //if (list.Count == 0)
                            //    Console.WriteLine("empty");
                            //else
                            //{
                            //    int index = list.Count - 1;
                            //    int n = list[index];
                            //    list.RemoveAt(index);
                            //    Console.WriteLine(n);
                            //}
                            if (stack.Count == 0)
                                Console.WriteLine("empty");
                            else
                                Console.WriteLine(stack.Pop());
                        }
                        break;
                }
            }
        }
        static void _1102()
        {
            string ml = @"7
i 7
i 5
c
o
o
o
c";
            Impl_1102(ml);
        }

        //--------------------------------------------------
        // 1221 후위표기법
        //--------------------------------------------------
        static void Impl_1221(int n, string exp)
        {
            string[] words = exp.Split();

            int lhs = Convert.ToInt32(words[0]);
            int rhs = Convert.ToInt32(words[1]);
            for (int i = 2; i < n; ++i)
            {
                string op = words[i];
                switch (op)
                {
                    case "+":
                        {
                            lhs += rhs;
                        }
                        break;
                    case "-":
                        {
                            lhs -= rhs;
                        }
                        break;
                    case "*":
                        {
                            lhs *= rhs;
                        }
                        break;
                    case "/":
                        {
                            lhs /= rhs;
                        }
                        break;
                }

                if (i + 1 < n)
                {
                    rhs = Convert.ToInt32(words[++i]);
                }
            }

            Console.WriteLine(lhs);
        }
        static void _1221()
        {
            Impl_1221(3, "2 3 +");
            Impl_1221(3, "9 3 /");
            Impl_1221(5, "5 8 + 2 *");
        }

        //--------------------------------------------------
        // 2858 쇠막대기
        //
        // 풀이 - 새로 만나는 쇠막대의 끝은 바로 전에 등장한 쇠막대의 시작과 연결된 막대임
        //--------------------------------------------------
        static void Impl_2858(string input)
        {
            Stack<int> left = new Stack<int>();
            List<int> laser = new List<int>();
            List<int> stick = new List<int>();

            for (int i = 0; i < input.Length; ++i)
            {
                char curr = input[i];
                if (curr == '(')
                {
                    char next = input[i + 1];
                    if (next == ')')
                        laser.Add(i++);
                    else
                        left.Push(i);
                }
                else
                {
                    stick.Add(left.Pop());
                    stick.Add(i);
                }
            }

            System.Diagnostics.Debug.Assert(left.Count == 0);
            System.Diagnostics.Debug.Assert(stick.Count % 2 == 0);

            int totalCout = 0;
            for (int i = 0; i < stick.Count; i += 2)
            {
                int l = stick[i];
                int r = stick[i + 1];

                int count = 1;
                for (int j = 0; j < laser.Count; ++j)
                {
                    int p = laser[j];
                    if (p > r)
                        break;
                    if (p > l)
                        ++count;
                }
                totalCout += count;
            }
            Console.WriteLine(totalCout);
        }
        static void _2858()
        {
            Impl_2858("()(((()())(())()))(())");
            Impl_2858("(((()(()()))(())()))(()())");
        }

        //--------------------------------------------------
        // 1697 큐(queue)
        //
        // 풀이 : C# 에서는 Queue 가 기본적으로 제공 되지만
        //       구지 구현한다면 List 보단 LinkedList 를 이용한다
        //--------------------------------------------------
        static void Impl_1697(string multiLine)
        {
            string[] lines = multiLine.Split(new char[] { '\n' });

            //Queue<int> queue = new Queue<int>();
            LinkedList<int> list = new LinkedList<int>();

            int cnt = Convert.ToInt32(lines[0]);
            for (int i = 1; i <= cnt; ++i)
            {
                string s = lines[i].Trim();
                char cmd = s[0];

                switch (cmd)
                {
                    case 'i':
                        {
                            string[] words = s.Split();
                            int n = Convert.ToInt32(words[1]);

                            //queue.Enqueue(n);

                            list.AddLast(n);
                        }
                        break;
                    case 'c':
                        {
                            //Console.WriteLine(queue.Count);

                            Console.WriteLine(list.Count);
                        }
                        break;
                    case 'o':
                        {
                            //if (queue.Count == 0)
                            //    Console.WriteLine("empty");
                            //else
                            //    Console.WriteLine(queue.Dequeue());

                            if (list.Count == 0)
                                Console.WriteLine("empty");
                            else
                            {
                                Console.WriteLine(list.First.Value);
                                list.RemoveFirst();
                            }
                        }
                        break;
                }
            }
        }
        static void _1697()
        {
            string ml = @"7
i 7
i 5
c
o
o
o
c";
            Impl_1697(ml);
        }

        //--------------------------------------------------
        // 5801 단순한 카드 셔플
        //--------------------------------------------------
        static void Impl_5801(int n)
        {
            var queue = new Queue<int>();
            for (int i = 1; i <= n; ++i)
                queue.Enqueue(i);

            while (queue.Count > 0)
            {
                int v = queue.Dequeue();
                if (queue.Count > 0)
                    queue.Enqueue(queue.Dequeue());
                Console.Write("{0} ", v);
            }
            Console.WriteLine();
        }
        static void _5801()
        {
            Impl_5801(3);
            Impl_5801(6);
            Impl_5801(7);
        }

        //--------------------------------------------------
        // 6057 피자왕 비룡
        //--------------------------------------------------
        static void Impl_6057(string input)
        {
            string[] lines = input.Split('\n');
            for(int i = 1; i < lines.Length; ++i)
            {
                lines[i] = lines[i].Trim();
            }

            string[] words = lines[0].Split();
            int P = Convert.ToInt32(words[0]);
            int N = Convert.ToInt32(words[1]);

            var pizza = new Queue<int>[P];
            for(int i = 0; i < P; ++i)
            {
                pizza[i] = new Queue<int>();
            }

            int rt = 0;
            for(int i = 0; i < N; ++i)
            {
                words = lines[i + 1].Split();

                int cmd = Convert.ToInt32(words[0]);
                if (cmd == 0)
                {
                    int kind = Convert.ToInt32(words[1]) - 1;
                    int price = Convert.ToInt32(words[2]);

                    pizza[kind].Enqueue(price);
                }
                else
                {
                    int kind = Convert.ToInt32(words[1]) - 1;
                    if (pizza[kind].Count > 0)
                    {
                        rt += pizza[kind].Dequeue();
                    }
                }
            }

            Console.WriteLine(rt);
        }

        static void _6057()
        {
            string input = @"3 9
1 1
0 3 31
0 1 65
0 1 51
1 3
0 1 59
1 1
1 3
0 1 39";
            Impl_6057(input);
        }
    }
}
