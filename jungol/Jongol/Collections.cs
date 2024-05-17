using System;
using System.Collections;
using System.Collections.Generic;

using System.Collections.Concurrent;    //ConcurrentBag, ConcurrentQueue, ConcurrentStack, ConcurrentDictionary
using System.Threading;                 //Thread
using System.Threading.Tasks;           //Task

namespace Jungol
{
    //System.Collections.ArrayList
    //System.Collections.BitArray
    //System.Collections.Hashtable
    //System.Collections.Queue
    //System.Collections.SortedList
    //System.Collections.Stack

    class Collections
    {
        //http://m.csharpstudy.com/DS

        static Random rnd = new Random();

        public static void Run()
        {
            //Util.Call(Test_Array);
            //Util.Call(Test_DynamicArray);
            //Util.Call(Test_LinkedList);
            //Util.Call(Test_Quee);
            //Util.Call(Test_Stack);
            //Util.Call(Test_HashTable);
            //Util.Call(Test_Tree);
            //Util.Call(Test_BinarySearchTree);
            //Util.Call(Test_Graph);

            Util.Call(Test_Heap);
        }

        static void PrintIndexContainer(ICollection t)
        {
            Console.WriteLine("Count : {0}", t.Count);
            foreach (object o in t) {
                Console.WriteLine("{0} ", o);
            }
            
            Console.WriteLine();
        }

        static void PrintIndexContainer2(Hashtable t)
        {
            Console.WriteLine("Count : {0}", t.Count);
            foreach (DictionaryEntry entry in t) { 
                Console.WriteLine("[{0}] : {1}", entry.Key, entry.Value);
            }
            
            Console.WriteLine();
        }
        
        static void Test_Array()
        {
            /* 배열은 연속적인 메모리상에 동일한 타입(혹은 그의 파생타입)의 요소를 
             * 일렬로 저장하는 자료 구조로서 배열 요소는 인덱스를 사용하여 직접적으로 
             * 엑세스할 수 있다. 배열은 고정된 크기를 가지며, 배열의 사이즈와 
             * 상관없이 한 요소를 엑세스하는 시간은 인덱스를 사용할 경우 O(1)이 된다. 
             * 하지만 인덱스를 알지 못하고 소트되지 않은 배열에서 값으로 데이타를 찾기 
             * 위해서는 O(n)의 시간이 소요된다. 
             * 소트된 배열에서 값을 찾는 경우는 Binary Search를 이용할 수 있으므로 
             * O(log N)의 시간이 소요된다. 
             * 
             * 모든 C# 배열은 내부적으로 .NET Framework의 System.Array에서 파생된 것으로 
             * 상속에 의해 System.Array의 메소드, 프로퍼티를 사용할 수 있다. 
             */

            //1차원
            int[] scores = new int[10];
            for(int i=0; i<10; ++i)
            {
                scores[i] = rnd.Next(100);
            }
            Console.WriteLine(scores.Rank);
            Console.WriteLine(scores.Length);
            Console.WriteLine(scores[5]);
            scores[5] = 99;
            Console.WriteLine(scores[5]);
            Util.PrintArray(scores);

            int[] tst1d = {1,2,3 };
            tst1d = new int[4] {1,2,3,4 };

            //@{ using the class 'Array' for 1-demension array
            //Array.IndexOf
            Console.WriteLine(Array.IndexOf<int>(scores, 99));  //5
            Console.WriteLine(Array.IndexOf<int>(scores, 100)); //not found
            
            //Array.Sort
            Array.Sort<int>(scores);
            Util.PrintArray(scores);

            //Array.BinarySearch : only for sorted array
            Console.WriteLine(Array.BinarySearch(scores, scores[8]));   //8
            Console.WriteLine(Array.BinarySearch(scores, 100));         //not found

            //Array.Reverse
            Array.Reverse(scores);
            Util.PrintArray(scores);
            //@}

            //2차원
            const int ROW = 4;
            const int COL = 4;
            int[,] map = new int[ROW, COL];
            for(int r=0; r<ROW; ++r)
            {
                for(int c=0; c<COL; ++c)
                {
                    map[r,c] = rnd.Next(10);
                }
            }
            Util.PrintArray(map);
            
            Console.WriteLine(map.Rank);
            Console.WriteLine(map.Length);
            Console.WriteLine(map.GetLength(0));
            Console.WriteLine(map.GetLength(1));

            //Console.WriteLine(map.GetLength(2));    //exception


            int[,] tst2d = {
                {1,2,3, },
                {11,2,3, },
                {21,2,3, },
            };
            Util.PrintArray(tst2d);
            tst2d = new int[2,2] { {1,2 }, {4,5 } };
            Util.PrintArray(tst2d);
        }//Test_Array


        static void Test_DynamicArray()
        {
            /*배열은 고정된 크기의 연속된 배열요소들의 집합이므로 배열을 초기화 할 때 총 
             * 배열 요소의 수를 미리 지정해야 한다. 하지만 경우에 따라 배열요소가 몇 
             * 개나 필요한 지 미리 알 수 없는 경우가 있으며, 중간에 필요에 따라 배열을 
             * 확장해야 하는 경우도 있다. 
             * .NET에는 이러한 동적 배열을 지원하는 클래스로 ArrayList와 List<T>이 있다. 
             * 이들 동적 배열 클래스들은 배열 확장이 필요한 경우, 내부적으로 배열 크기가 
             * 2배인 새로운 배열을 생성하고 모든 기존 배열 요소들을 새로운 배열에 복사한 
             * 후 기존 배열을 해제한다. 
             * 동적 배열의 Time Complexity는 배열과 같이 인덱스를 통할 경우 O(1), 
             * 값으로 검색할 경우 O(n)을 갖는다. 
             */


            //@{ -- ArrayList
            /*ArrayList는 모든 배열 요소가 object 타입인 Non-generic 동적 배열 클래스이다. 
             * .NET의 Non-generic 클래스들은 System.Collections 네임스페이스 안에 있으며, 
             * 단점으로 박싱 / 언박싱이 일어나게 된다. 
             * ArrayList는 배열 요소를 읽어 사용할 때 object를 리턴하므로 일반적으로 원하는 
             * 타입으로 먼저 캐스팅(Casting)한 후 사용하게 된다. 
             */
            {
                ArrayList myList = new ArrayList();
                myList.Add(90);
                myList.Add(88);
                myList.Add(75);

                // int로 casting
                int val = (int)myList[1];

                //myList.Count;
                //myList.Clear;
                //myList.Add;
                //myList.AddRange;
                //myList.BinarySearch;
                //myList.IndexOf;
                //myList.LastIndexOf;
                //myList.Insert;
                //myList.InsertRange;
                //myList.Remove;
                //myList.RemoveAt;
                //myList.RemoveRange;
                //myList.Reverse;
                //myList.Sort;
                //myList.TrimToSize;
            }
            //@}


            //@{ -- List<T>
            /* List<T>는 배열요소가 T 타입인 Generics로서 동적 배열을 지원하는 클래스이다. 
             * .NET의 Generic 클래스들은 System.Collections.Generic 네임스페이스 안에 있다. 
             * List클래스는 내부적으로 배열을 가지고 있으며, 동일한(Homogeneous) 타입의 
             * 데이타를 저장한다. 만약 미리 할당된 배열 크기(Capacity라 부른다)가 부족하면 
             * 내부적으로 배열을 2배로 늘려 동적으로 배열을 확장한다. 
             * ArrayList와 다르게 캐스팅을 할 필요가 없으며, 박싱 / 언박싱의 문제를 발생시키지 않는다. 
             */
            {
                List<int> myList = new List<int>();
                myList.Add(90);
                myList.Add(88);
                myList.Add(75);
                int val = myList[1];


                /** 
                 * AddRange
                 * BinarySearch
                 * Clear
                 * Contains
                 * ConvertAll
                 * IndexOf
                 * LastIndexOf
                 * Remove
                 * RemoveAt
                 * RemoveRange
                 * Reverse
                 * Sort
                 * 
                 * ForEach
                 * 
                 * -- with 'Predicate<T>'
                 * Exists
                 * Find
                 * FindAll
                 * FindIndex
                 * FindLast
                 * FindLastIndex
                 * RemoveAll
                 */
            }
            //@}

            //@{ -- SortedList<TKey,TValue>
            /* SortedList클래스는 Key값으로 Value를 찾는 Map ADT 타입 (ADT: Abstract Data Type)을 
             * 내부적으로 배열을 이용해 구현한 클래스이다. .NET에서 MAP ADT를 구현한 클래스로는 
             * 해시테이블을 이용한 Hashtable/Dictionary클래스, 이진검색트리를 이용한 SortedDictionary, 
             * 그리고 배열을 이용한 SortedList 등이 있다. SortedList클래스는 내부적으로 키값으로 소트된 
             * 배열을 가지고 있으며, 따라서 이진검색(Binary Search)가 가능하기 때문에 O(log n)의 검색 
             * 시간이 소요된다. 만약 미리 할당된 배열 크기(Capacity라 부른다)가 부족하면 내부적으로 
             * 배열을 2배로 늘려 동적으로 배열을 확장한다. 
             */
            {
                SortedList<int, string> list = new SortedList<int, string>();
                list.Add(1001, "Tim");
                list.Add(1020, "Ted");
                list.Add(1010, "Kim");

                string name = list[1001];

                foreach (KeyValuePair<int, string> kv in list)
                {
                    Console.WriteLine("{0}:{1}", kv.Key, kv.Value);
                }
                // 출력
                //1001:Tim
                //1010:Kim
                //1020:Ted



                /** 
                 * Count
                 * Keys
                 * Values
                 * 
                 * Add
                 * Clear
                 * ContainsKey
                 * ContainsValue
                 * Remove
                 */
            }
            //@}

            //@{ -- ConcurrentBag
            /* .NET 4.0 부터 멀티쓰레딩 환경에서 리스트를 보다 간편하게 사용할 수 있는 새로운 
             * 클래스인 ConcurrentBag<T> 가 제공되었다. ConcurrentBag<T> 클래스는 리스트와 비슷하게 
             * 객체들의 컬렉션을 저장하는데, List<T> 와는 달리 입력 순서를 보장하지는 않는다. 
             * ConcurrentBag 에 데이타를 추가하기 위해 Add() 메서드를 사용하고, 데이타를 읽기 
             * 위해서는 foreach문 혹은 TryPeek(), TryTake() 메서드를 사용한다. 
             * TryPeek()은 ConcurrentBag에서 데이타를 읽기만 하는 것이고, 
             * TryTake()는 데이타를 읽을 후 해당 요소를 ConcurrentBag에서 삭제하게 된다. 
             * 
             * ConcurrentBag는 멀티쓰레드가 동시에 엑세스할 수 있는데, 
             * 예를 들어 ThreadA와 ThreadB가 ConcurrentBag에 데이타를 쓸 때, 
             * ThreadA가 1,2,3 을 넣고, 
             * ThreadB가 4,5,6 을 넣으면, 
             * ThreadA는 ConcurrentBag을 다시 읽을 때, 
             * 자신이 쓴 1,2,3을 우선순위로 먼저 읽은 다음, 
             * 나머지 다른 쓰레드에 의해 입력된 요소들 (4,5,6)을 읽게 된다. 
             * 
             * 아래 예제에서 첫번째 쓰레드는 100개의 숫자를 ConcurrentBag에 넣게 되고, 
             * 동시에 두번째 쓰레드는 1초마다 10회에 걸쳐 해당 ConcurrentBag의 내용을 출력하는 것이다. 
             */
            {
                var bag = new ConcurrentBag<int>();
                var t1 = Task.Factory.StartNew(() =>
                {
                    for (int i = 0; i < 100; ++i)
                    {
                        bag.Add(i);
                        Thread.Sleep(rnd.Next(10, 100));
                    }
                });

                var t2 = Task.Factory.StartNew(() =>
                {
                    int n = 1;
                    while (n <= 10)
                    {
                        Console.WriteLine("{0} iteration", n);

                        int count = 0;
                        foreach (int i in bag)
                        {
                            Console.WriteLine(i);
                            count++;
                        }
                        Console.WriteLine("Count = {0}", count);

                        System.Threading.Thread.Sleep(400);
                        ++n;
                    }
                });

                Task.WaitAll(t1, t2);

                
                /** 
                 * Count
                 * IsEmpty
                 * 
                 * Add
                 * CopyTo
                 * ToArray
                 * TryPeek
                 * TryTake
                 */
            }
            //@}
        }//Test_DynamicArray

        static void Test_LinkedList()
        {
            // 링크드 리스트 (Linked List)
            /* 링크드 리스트 (Linked List, 연결 리스트)는 데이타를 포함하는 노드들을 
             * 연결하여 컬렉션을 만든 자료 구조로서 각 노드는 데이타와 다음/이전 링크 포인터를 
             * 갖게 된다. 단일 연결 리스트(Singly Linked List)는 노드를 다음 링크로만 연결한 
             * 리스트이고 이중 연결 리스트는 각 노드를 다음 링크와 이전 링크 모두 연결한 리스트이다. 
             * 만약 링크를 순환해서 마지막 노드의 다음 링크가 처음 노드를 가리키게 했을 경우 
             * 이를 순환 연결 리스트 (Circular Linked List)라 부른다. 
             * 링크드 리스트는 특정 노드에서 노드를 삽입, 삭제하기 편리 하지만 ( O(1) ), 
             * 특정 노드를 검색하기 위해서는 O(n)의 시간이 소요된다. 
             */

            //LinkedList<T> 클래스
            /* .NET에는 링크드 리스트를 구현한 LinkedList<T> 클래스가 있다. 
             * 이 LinkedList 클래스는 이중 링크드 리스트로 구현되어 있으며, 
             * 리스트 노드는 LinkedListNode 클래스로 표현된다. 
             * 노드의 추가는 AddFirst, AddLast, AddBefore, AddAfter 등의 메서드들을 호출하여 
             * 처음 또는 끝, 혹은 특정 노드의 앞, 뒤에 새 노드를 추가할 수 있다. 
             * 아래 예는 Banana 노드 뒤에 Grape노드를 추가하는 예이다. 
             */

            LinkedList<string> list = new LinkedList<string>();

            list.AddLast("Apple");
            list.AddLast("Banana");
            list.AddLast("Lemon");

            LinkedListNode<string> node = list.Find("Banana");

            list.AddAfter(node, "Grape");

            foreach(string v in list)
            {
                Console.WriteLine(v);
            }

            /**
             * AddFirst
             * AddLast
             * AddBefore
             * AddAfter
             * Clear
             * Contains
             * Find
             * FindLast
             * Remove
             * RemoveFirst
             * RemoveLast
            //*/
        }//Test_LinkedList

        static void Test_Quee()
        {
            /**
             * 자료구조 : 큐 (Queue) 
             * 
             * 큐 (Queue)는 먼저 추가된 데이타가 먼저 출력 처리되는(FIFO, First In First Out) 
             * 자료 구조로서 입력된 순서대로 처리해야 하는 상황에 이용된다. 
             * Queue는 맨 뒤(tail)에 데이타를 계속 추가하고, 맨 앞(head)에서만 데이타를 읽기 
             * 때문에 순차적으로 데이타를 처리하게 된다. 
             **/


            {
                /**
                 * Queue 클래스 
                 * 
                 * .NET에는 큐를 구현한 Queue클래스와 이의 Generic 형태인 Queue<T> 클래스가 
                 * 있다. 이 Queue클래스는 내부적으로 순환 배열 (Circular Array)로 구현되어 
                 * 있는데, 배열의 마지막 요소에 다다른 경우 다시 배열 처음 요소로 순환하는 
                 * 구조(next % arrsize)를 가지고 있다. 
                 * Queue는 내부적으로 head와 tail 포인터를 가지고 있는데, tail에 데이타를 
                 * 추가하고(Enqueue) head에서 데이타를 읽고 제거(Dequeue)한다. 
                 * 만약 데이타 양이 많아 순환 배열이 모두 찰 경우, Queue는 Capacity를 2배로 
                 * (디폴트 Growth Factor가 2이다) 증가시키고, 모든 배열 요소를 새 순환 
                 * 배열에 자동으로 복사하여 큐를 확장한다. 
                 **/

                Queue<int> q = new Queue<int>();
                q.Enqueue(120);
                q.Enqueue(130);
                q.Enqueue(150);
            
                Console.WriteLine(q.Dequeue());//120
                Console.WriteLine(q.Dequeue());//130
                Console.WriteLine(q.Dequeue());//150
                //Console.WriteLine(q.Dequeue());//System.InvalidOperationException

                /**
                 * Count
                 * 
                 * Clear
                 * Contains
                 * 
                 * Enqueue
                 * Dequeue
                 * 
                 * Peek
                 * ToArray
                 **/
            }

            {
                /**
                 * ConcurrentQueue 클래스
                 * 
                 * 멀티쓰레딩 환경에서 위의 Queue 클래스를 사용하기 위해서는 전통적인 방식인 
                 * lock 을 사용하는 방법과 Queue.Synchronized() 를 사용하여 Thread-safe한 
                 * Wrapper 큐를 사용하는 방법이 있다.
                 * 
                 * .NET 4.0 부터 멀티쓰레딩 환경에서 큐를 보다 간편하게 사용할 수 있는 새로운 
                 * 클래스인 ConcurrentQueue<T> 가 제공되었다. Queue 클래스와 비슷하게 
                 * ConcurrentQueue 의 기본 동작 메서드는 Enqueue() 와 TryDequeue() 인데, 
                 * ConcurrentQueue 에는 Dequeue() 메서드가 없고 대신 TryDequeue() 메서드를 
                 * 사용한다. 또한 마찬가지로 ConcurrentQueue에서는 Peek() 메서드 대신 TryPeek() 
                 * 메서드를 사용한다.
                 * 
                 * 아래 예제는 하나의 쓰레드가 큐(ConcurrentQueue)에 0부터 99까지 계속 집어 넣을 때, 
                 * 동시에 다른 쓰레드에서는 계속 큐에서 데이타를 빼내 읽어 오는 작업을 하는 샘플 코드이다. 
                 **/


                ConcurrentQueue<int> q = new ConcurrentQueue<int>();

                Task tEnqueue = Task.Factory.StartNew(()=>
                {
                    for(int i=0; i<100; ++i)
                    {
                        q.Enqueue(i);
                        Thread.Sleep(rnd.Next(100, 500));
                    }
                });

                Task tDequeue = Task.Factory.StartNew(()=>
                {
                    int n= 0;
                    int result = 0;

                    while(n<100)
                    {
                        if(q.TryDequeue(out result))
                        {
                            Console.WriteLine(result);
                            ++n;
                        }
                        else
                        {
                            Console.WriteLine("empty");
                        }

                        Thread.Sleep(100);
                    }
                });

                Task.WaitAll(tEnqueue, tDequeue);

                /**
                 * Enqueue
                 * TryDequeue
                 * TryPeek
                 * 
                 * CopyTo
                 * ToArray
                 **/
            }
        }//Test_Quee

        static void Test_Stack()
        {
            /**
             * 자료구조 : 스택 (Stack) 
             * 
             * 스택 (Stack)은 가장 나중에 추가된 데이타가 먼저 출력 처리되는
             * (LIFO, Last In First Out) 자료 구조로서 가장 최신 입력된 순서대로 
             * 처리해야 하는 상황에 이용된다. 스택은 개념적으로 한 쪽 끝에서만 
             * 자료를 넣거나 뺄 수 있는 구조로 되어 있다. 
             * 자료는 스택에 저장하는 것은 Push라 하고, 가장 최근 것부터 꺼내는 
             * 것은 Pop이라 한다. 
             * Stack의 Push와 Pop은 일반적으로 O(1)의 시간이 소요되지만, 
             * Push의 경우 만약 스택이 Full되어 동적 확장이 
             * 일어난다면 O(n)의 시간이 소요된다. 
             **/

            {
                /**
                 * Stack 클래스 
                 * 
                 * .NET에는 스택을 구현한 Non-generic인 Stack클래스와 이의 Generic 
                 * 형태인 Stack<T> 클래스가 있다. 
                 * Queue와 마찬가지로 .NET의 Stack클래스는 내부적으로 순환 배열 
                 * (Circular Array)으로 구현되어 있으며, 스택이 가득 차면 자동으로 
                 * 배열을 동적으로 확장하게 된다. 
                 **/


                Stack<int> s = new Stack<int>();
                s.Push(3);
                s.Push(1);
                s.Push(4);

                Console.WriteLine(s.Pop());//4

                /**
                 * Count
                 * 
                 * Clear
                 * Contains
                 * 
                 * Push
                 * Pop
                 * Peek
                 * 
                 * CopyTo
                 * ToArray
                 **/
            }

            {
                /**
                 * ConcurrentStack 클래스 
                 * 
                 * .NET 4.0 부터 멀티쓰레딩 환경에서 스택을 보다 간편하게 사용할 수 
                 * 있는 새로운 클래스인 ConcurrentStack<T> 가 제공되었다. 
                 * Stack 클래스와 비슷하게 ConcurrentStack 의 기본 동작 메서드는 Push() 
                 * 와 TryPop() 인데, ConcurrentStack 에는 Pop() 메서드가 없고 대신 
                 * TryPop() 메서드를 사용한다.
                 * 
                 * 아래 예제는 하나의 쓰레드가 ConcurrentStack 에 0부터 99까지 계속 
                 * 집어 넣을 때, 동시에 다른 쓰레드에서는 계속 그 스택에서 데이타를 빼내 
                 * 읽어 오는 작업을 하는 샘플 코드이다. 
                 * 스택을 Pop 하는 속도를 약간 늦춤으로 해서 0 부터 99까지 순차적으로 
                 * 출력하지 않을 가능성이 더 커지게 하였다. 
                 */

                ConcurrentStack<int> s = new ConcurrentStack<int>();

                Task t1 = Task.Factory.StartNew(()=>
                {
                    for(int i=0; i<100; ++i)
                    {
                        s.Push(i);
                        Thread.Sleep(rnd.Next(100, 500));
                    }
                });

                Task t2 = Task.Factory.StartNew(()=>
                {
                    int n=0;
                    int result;

                    while(n<100)
                    {
                        if(s.TryPop(out result))
                        {
                            Console.WriteLine(result);
                            ++n;
                        }
                        else
                        {
                            Console.WriteLine("empty");
                        }
                        Thread.Sleep(100);
                    }
                });

                Task.WaitAll(t1, t2);
            }
        }//Test_Stack

        static void Test_HashTable()
        {
            /**
             * 자료구조 : 해시테이블 (Hash Table) 
             * 
             * 해시(Hash)는 키 값을 해시 함수(Hash function)으로 해싱하여 
             * 해시테이블의 특정 위치로 직접 엑세스하도록 만든 방식이다.
             * 키 값을 통해 직접 엑세스하기 위해서 모든 가능한 키 값을 갖는 
             * 배열을 만들면, 배열크기가 엄청나게 커지게 된다. 
             * 예를 들어, 주민등록번호를 키 값으로 하는 경우, 
             * 000000-0000000 부터 999999-9999999까지 10의 13승의 배열 
             * 공간이 필요한데, 만약 회원수가 1000명인 경우, 1000명을 저장하기 
             * 위해 10^13의 엄청난 배열 공간이 필요하게 된다. 
             * 이렇게 낭비되는 공간을 줄이기 위해 해시 함수를 사용하게 되는데, 
             * 이 함수는 적은 공간 안에서 모든 키를 직접 찾아갈 수 있도록 해준다. 
             * 하지만 경우에 따라 서로 다른 키가 동일한 해시테이블 버켓 위치를 
             * 가리킬 수 있는데, 이를 해결하기 위해 여러 Collision Resolution 
             * 방식이 사용된다. 
             * Collision Resolution의 방식으로 Linear Probing, Quadratic Probing, 
             * Rehashing (Double Hashing), Chaining 등 여러가지가 있다. 
             * 해시테이블 자료구조는 추가, 삭제, 검색에서 O(1)의 시간이 소요된다. 
             **/

            {
                /**
                 * Hashtable 클래스 
                 * 
                 * .NET에 해시테이블을 구현한 Non-generic 클래스로 Hashtable 
                 * 클래스가 있다. Hashtable은 Key값과 Value값 모두 object 타입을 
                 * 받아들이며, 박싱/언박싱을 하게 된다. Hashtable은 Rehashing 
                 * (Double Hashing)방식을 사용하여 Collision Resolution을 하게 된다. 
                 * 즉, 해시함수를 H1(Key) 부터 Hk(Key) 까지 k개를 가지고 있으며, 
                 * 키 충돌(Collision)이 발생하면, 차기 해시함수를 계속 사용하여 빈 
                 * 버켓을 찾게된다. 이 자료구조는 추가, 삭제, 검색에서 O(1)의 시간이 
                 * 소요된다. 
                 **/

                Hashtable h = new Hashtable();
                h.Add("irina", "Irina SP");
                h.Add("tom", "Tom Cr");

                if(h.Contains("tom"))
                {
                    Console.WriteLine(h["tom"]);
                }


                /**
                 * Count
                 * Keys
                 * Values
                 * 
                 * Add
                 * Remove
                 * Clear
                 * 
                 * Contains
                 * CopyTo
                 **/
            }

            {
                /**
                 * Dictionary<Tkey,TValue> 클래스 
                 * 
                 * .NET에 Generic방식으로 해시테이블을 구현한 클래스로 
                 * Dictionary<Tkey,TValue> 클래스가 있다. Dictionary는 Key값과 
                 * Value값 모두 Strong type을 받아들이며, 박싱/언박싱을 일으키지 
                 * 않는다. Dictionary는 Chaining 방식을 사용하여 
                 * Collision Resolution을 하게 된다. 
                 * 
                 * 이 자료구조는 추가, 삭제, 검색에서 O(1)의 시간이 소요된다. 
                 **/

                Dictionary<int, string> dic = new Dictionary<int, string>();
                dic.Add(1001, "Jane");
                dic.Add(1002, "Tom");
                dic.Add(1003, "Cindy");

                Console.WriteLine(dic[1002]);

                /**
                 * Count
                 * Keys
                 * Values
                 * 
                 * Add
                 * Remove
                 * Clear
                 * TryGetValue
                 * ContainsKey
                 * 
                 * KeyCollection.CopyTo
                 * ValueCollection.CopyTo
                 **/
            }

            {
                /**
                 * ConcurrentDictionary<Tkey,TValue> 클래스 
                 * 
                 * .NET 4.0 부터 멀티쓰레딩 환경에서 Dictionary를 보다 간편하게 사용할 수 
                 * 있는 새로운 클래스인 ConcurrentDictionary<T> 가 제공되었다. 
                 * ConcurrentDictionary 클래스에서는 기본적으로 데이타를 추가하기 위해 
                 * TryAdd() 메서드를 사용하고, 키값을 읽기 위해서는 TryGetValue() 메서드를 
                 * 사용한다. 또한 기존 키값을 갱신하기 위해서 TryUpdate() 
                 * 메서드를, 기존 키를 지우기 위해서는 TryRemove() 메서드를 사용한다.
                 * 
                 * 아래 예제는 하나의 쓰레드가 ConcurrentDictionary 에 Key 1부터 100까지 
                 * 계속 집어 넣을 때, 동시에 다른 쓰레드에서는 계속 그 해시테이블에서 Key 
                 * 1부터 100까지의 데이타를 빼내 (순차적으로) 읽어 오는 작업을 하는 샘플 
                 * 코드이다. 
                 **/

                ConcurrentDictionary<int, string> dic = new ConcurrentDictionary<int, string>();

                Task t1 = Task.Factory.StartNew(()=>
                {
                    int key = 1;
                    while(key <=100)
                    {
                        if(dic.TryAdd(key, "D"+key))
                        {
                            key++;
                        }
                        Thread.Sleep(rnd.Next(100, 500));
                    }
                });

                Task t2 = Task.Factory.StartNew(()=>
                {
                    int key=1;
                    string val;
                    while(key<=100)
                    {
                        if(dic.TryGetValue(key, out val))
                        {
                            Console.WriteLine("[{0,3}] : {1}", key, val);
                            key++;
                        }
                        else
                        {
                            Console.WriteLine("{0} isn't added yet", key);
                        }
                        Thread.Sleep(100);
                    }
                });

                Task.WaitAll(t1,t2);
            }

            {
                /**
                 * C#으로 간단한 해시테이블 구현 : SimpleHashTable
                 * 
                 * 해시테이블은 기본적으로 배열을 저장 장소로 사용한다. 아래의 간단한 
                 * 해시테이블 구현에서도 buckets라는 배열을 기본 데이타 멤버로 사용하고 
                 * 있다. 해시함수를 사용하여 정해진 배열내에서 특정 배열요소 위치를 
                 * 계산하게 되며, 만약 키 중복이 발생하는 경우 Chaining방식을 사용하여 
                 * 링크드 리스트로 연결하여 저장하게 된다. 이 예제는 기본 개념을 예시하기 
                 * 위한 것으로 많은 기능들이 생략되어 있다. 
                 **/

                SimpleHashTable ht = new SimpleHashTable(1);
                //SimpleHashTable ht = new SimpleHashTable();
                ht.Put("Kim D", "Sales 01");
                ht.Put("Lee K", "Sales 02");
                ht.Put("Park S", "IT 01");
                ht.Put("Shin O", "IT 02");

                Console.WriteLine(ht.Get("Lee K"));
                Console.WriteLine(ht.Get("Shin O"));
                Console.WriteLine(ht.Contains("Unknown"));
            }
        } //Test_HashTable

        class SimpleHashTable
        {
            private class Node
            {
                public object Key {get; set; }
                public object Value {get; set; }
                public Node Next {get; set; }

                public Node(object key, Object value)
                {
                    Key = key;
                    Value = value;
                    Next = null;
                }
            }

            private const int INITIAL_SIZE = 16;
            private int size;
            private Node[] buckets;


            public SimpleHashTable()
            {
                size = INITIAL_SIZE;
                buckets = new Node[size];
            }
            public SimpleHashTable(int capacity)
            {
                size = capacity;
                buckets = new Node[size];
            }
            public void Put(Object key, Object value)
            {
                int index = HashFunction(key);
                if (buckets[index] == null)
                {
                    buckets[index] = new Node(key, value);
                }
                else
                {
                    Node node = new Node(key, value);
                    node.Next = buckets[index];
                    buckets[index] = node;
                }
            }
            public Object Get(Object key)
            {
                int index = HashFunction(key);
                Node n = buckets[index];
                while (null != n)
                {
                    if (n.Key == key)
                        return n.Value;
                    n = n.Next;
                }

                return null;
            }
            public bool Contains(object key)
            {
                int index = HashFunction(key);
                Node n = buckets[index];
                while(null != n)
                {
                    if (n.Key == key)
                        return true;
                    n = n.Next;
                }

                return false;
            }

            int HashFunction(Object key)
            {
                int hashCode = key.GetHashCode();
                return Math.Abs(hashCode + 1 + ((hashCode >> 5) + 1) % size) % size;
            }
        }//class SimpleHashTable

        static void Test_Tree()
        {
            /**
             * 자료구조 : 트리 (Tree) 
             * 
             * 트리(Tree)는 계층적인 자료를 나타내는데 자주 사용되는 자료 구조로서 
             * 하나이하의 부모노드와 복수 개의 자식노드들을 가질 수 있다. 트리는 
             * 하나의 루트(Root) 노드에서 출발하여 자식노드들을 갖게 되며, 각각의 
             * 자식 노드는 또한 자신의 자식노드들을 가질 수 있다. 트리 구조는 한 
             * 노드에서 출발하여 다시 자기 자신의 노드로 돌아오는 순환(Cycle)구조를 
             * 가질 수 없다. 트리구조는 계층적인 정부 혹은 기업 조직도, 대중소 지역 
             * 구조, 데이타 인덱스 파일 등에 적합한 자료구조이다. 
             **/


            /**
             * 자료구조 : 이진 트리 (Binary Tree) 
             * 
             * 트리(Tree)에서 많이 사용되는 특별한 트리로서 이진트리를 들 수 있는데, 
             * 이진 트리는 자식노드가 0개 ~ 2개인 트리를 말한다. 따라서 이진트리 
             * 노드는 데이타필드와 왼쪽노드 및 오른쪽노드를 갖는 자료 구조로 되어 
             * 있다. 이진 트리는 루트 노드로부터 출발하여 원하는 특정 노드에 도달할 
             * 수 있는데, 이때의 검색 시간(Search Time)은 O(n)이 소요된다. 
             **/

            /**
             * C#으로 간단한 이진트리 구현 : BinaryTree<T>
             * 
             * .NET Framework은 트리 혹은 이진 트리와 관련된 클래스를 제공하지 
             * 않는다. 이진트리를 구현하는 방식은 일반적으로 배열을 이용하는 방법과 
             * 연결리스트(Linked List)를 이용하는 방법이 있다. 아래 소스는 
             * 연결리스트를 사용하여 간단한 이진 트리를 C#으로 구현해 본 것으로서, 
             * 이진 트리의 기본적인 개념을 예시하기 위한 코드이다. 
             **/
            BinaryTree<int> btree = new BinaryTree<int>();
            BTreeNode<int> root = new BTreeNode<int>(1);
            btree.Root = root;

            root.Left = new BTreeNode<int>(2);
            root.Right = new BTreeNode<int>(3);
            root.Left.Left = new BTreeNode<int>(4);

            btree.PreOrderTraversal();
        }//Test_Tree

        class BTreeNode<T>
        {
            public T Data {get; set; }
            public BTreeNode<T> Left {get; set; }
            public BTreeNode<T> Right {get; set; }

            public BTreeNode(T data)
            {
                Data = data;
                Left = null;
                Right = null;
            }
        }
        class BinaryTree<T>
        {
            public BTreeNode<T> Root {get; set; }

            public void PreOrderTraversal()
            {
                PreOrderTraversalImpl(Root);
            }

            static private void PreOrderTraversalImpl(BTreeNode<T> n)
            {
                if(null == n)
                    return;

                Console.WriteLine(n.Data);
                PreOrderTraversalImpl(n.Left);
                PreOrderTraversalImpl(n.Right);
            }
        }//class BinaryTree<T>

        static void Test_BinarySearchTree()
        {
            /**
             * 자료구조 : 이진검색트리 (Binary Search Tree) 
             * 
             * 이진트리(Tree)의 특수한 형태로 자주 사용되는 트리로서 이진검색트리 
             * (Binary Search Tree)가 있다. 이진검색트리는 이진트리의 모든 속성을 
             * 가짐과 동시에 중요한 또 하나의 속성을 가지고 있는데, 그것은 특정 
             * 노드에서 자신의 노드보다 작은 값들은 모두 왼쪽에 있고, 큰 값들은 
             * 모두 오른쪽에 위치한다는 점이다. 또한 중복된 값을 허락하지 않는다. 
             * 따라서 전체 트리가 소트되어 있는 것과 같은 효과를 같게 되어 검색에 
             * 있어 배열이나 이진트리처럼 순차적으로 모든 노드를 검색하는 것(O(n))이 
             * 아니라, 매 검색마다 검색영역을 절반으로 줄여 O(log n)으로 검색할 수 
             * 있게 된다. 하지만 노드들이 한쪽으로 일렬로 기울어진 Skewed Tree인 
             * 경우, 검색영역을 n-1로만 줄이기 때문에 O(n)만큼의 시간이 소요된다. 즉, 
             * 예를 들어 소트된 데이타를 이진검색트리에 추가하게 되면, 이렇게 한쪽으로 
             * 치우쳐 진 트리가 생겨 검색시간이 O(n)으로 떨어지게 되는데, 이러한 
             * 현상을 막기 위하여 노드 추가/갱신시 트리 스스로 다시 밸런싱(Self 
             * balancing)하여 검색 최적화를 유지할 수 있다. 이러한 트리를 
             * Self-Balancing Binary Search Tree 또는 Balanced Search Tree라 하는데, 
             * 가장 보편적인 방식으로 AVL Tree, Red-Black Tree 등을 들 수 있다.
             * 
             * NOTE: 참고로 Search Tree에는 최대 2개의 자식노드를 갖는 Binary Search 
             * Tree 이외에, 여러 개의 자식노드들을 갖는 N-Way 검색 트리 
             * (n-way Search Tree)가 있는데, 대표적으로 B-Tree (혹은 이의 변형인 
             * B* Tree, B+ Tree)가 있으며 흔히 SQL Server와 같은 관계형 DB 인덱스로 
             * 주로 사용된다. 
             **/

            {
                /**
                 * C#으로 간단한 이진검색트리 구현 : class BST<T>
                 * 
                 * .NET Framework은 이진검색트리와 관련된 클래스를 제공하지 않는다. 아래 
                 * 소스는 간단한 이진검색트리를 C#으로 구현해 본 것으로서, 이진검색트리의 
                 * 기본적인 개념을 예시하기 위한 코드이다. 
                 **/
                BST<int> bst = new BST<int>();
                bst.Insert(4);
                bst.Insert(2);
                bst.Insert(6);
                bst.Insert(1);
                bst.Insert(7);

                bst.PreOrderTraversal();
            }

            {
                /**
                 * SortedDictionary<Tkey,TValue> 클래스 
                 * 
                 * .NET의 SortedDictionary클래스는 내부적으로 이진검색트리(BST, Binary 
                 * Search Tree)를 사용하여 Key를 갖고 Value를 찾는 Map ADT 타입 (ADT: 
                 * Abstract Data Type)을 구현한 클래스이다. (.NET에서 MAP ADT를 구현한 
                 * 클래스로는 해시테이블을 이용한 Hashtable/Dictionary클래스, 
                 * 이진검색트리를 이용한 SortedDictionary, 그리고 배열을 이용한 SortedList 
                 * 등이 있다) 비록 .NET이 이진검색트리를 클래스를 public으로 제공하지 
                 * 않지만, 내부적으로 BST를 사용해 SortedDictionary를 구현하고 있다. 
                 * SortedDictionary는 기본적으로 BST 자료구조이므로 중복된 키를 허용하지 
                 * 않으며, 추가, 삭제, 검색에서 O(log n)의 시간이 소요된다. 
                 **/

                SortedDictionary<int, string> map = new SortedDictionary<int, string>();

                map.Add(1001, "Tom");
                map.Add(1003, "John");
                map.Add(1010, "Irina");
                //map.Add(1001, "Tom");

                Console.WriteLine(map[1010]);
                foreach(KeyValuePair<int,string> kv in map)
                {
                    Console.WriteLine("{0}:{1}", kv.Key, kv.Value);
                }
            }
        }//Test_BinarySearchTree
        class BST<T>
        {
            private BTreeNode<T> root = null;
            private Comparer<T> compare = Comparer<T>.Default;

            public void Insert(T val)
            {
                if(null == root)
                {
                    root = new BTreeNode<T>(val);
                    return;
                }

                BTreeNode<T> node = root;
                while(node != null)
                {
                    int rt = compare.Compare(node.Data, val);
                    if(0 == rt)
                    {
                        //throw new error
                        return;
                    }

                    if (rt > 0)
                    {
                        if(null == node.Left)
                        {
                            node.Left = new BTreeNode<T>(val);
                            return;
                        }
                        node = node.Left;
                    }
                    else
                    {
                        if (null == node.Right)
                        {
                            node.Right = new BTreeNode<T>(val);
                            return;
                        }
                        node = node.Right;
                    }
                }
            }

            public void PreOrderTraversal()
            {
                PreOrderRecursive(root);
            }
            private void PreOrderRecursive(BTreeNode<T> node)
            {
                if(null == node)
                    return;

                Console.WriteLine(node.Data);
                PreOrderRecursive(node.Left);
                PreOrderRecursive(node.Right);
            }
        }//class BST<T>

        static void Test_Graph()
        {
            /**
             * 자료구조 : 그래프 (Graph) 
             * 
             * 그래프(Graph)는 노드(꼭지점, Vertex)과 변(Edge)로 구성되어 있는 
             * 자료 구조로서 트리와 다르게 사이클(Cycle)을 허용한다. Edge 에 
             * 방향을 허용하느냐 마느냐에 따라서 방향이 있는 그래프(Directed Graph), 
             * 혹은 방향이 없는 그래프(Undirected Graph)로 나눌 수 있다. 또한 각 
             * 변에 가중치(Weight)를 주어 노드와 노드 사이에 숫자로 거리, 비용등의 
             * 관계를 표현할 수 있다. 일반적으로 Graph 자료구조는 인접리스트
             * (Adjacency List)나 인접 행렬 (Adjacency Matrix) 등의 방법으로 
             * 표현한다. 그래프는 도시간 최단 행로를 구하거나 웹 링크 연결도를 
             * 표현하는 등 여러 종류의 자료를 표현하는데 사용될 수 있다. 
             **/

            /**
             * C#으로 간단한 그래프 구현 
             * 
             * .NET Framework은 그래프와 관련된 클래스를 제공하지 않는다. 그래프를 
             * 구현하는 한 방식으로 아래 코드는 각 노드마다 인접한 노드들의 리스트를 
             * 가지고 있는 인접리스트(Adjacency List)를 사용하고 있다. Graph클래스는 
             * GraphNode들을 갖는 리스트를 기본 필드로 갖고 있으며, 각 GraphNode는 
             * 데이타 (혹은 키+데이타) 필드와 인접 노드 리스트를 기본적인 필드로 
             * 가지고 있다. GraphNode는 필요에 따라 Weight 배열을 가질 수 있는데, 
             * 이는 각 변(Edge)의 가중치를 저장할 필요가 있을 때 사용된다. 
             **/

            Graph<int> g = new Graph<int>();
            var n1 = g.AddNode(10);
            var n2 = g.AddNode(20);
            var n3 = g.AddNode(30);
            var n4 = g.AddNode(40);
            var n5 = g.AddNode(50);

            g.AddEdge(n1, n3);
            g.AddEdge(n2, n4);
            g.AddEdge(n3, n4);
            g.AddEdge(n3, n5);

            g.DebugPrintLinks();
        }//Test_Graph
        class GraphNode<T>
        {
            private List<GraphNode<T>> neighbors;
            private List<int> weights;

            public T Data {get; set; }

            public GraphNode()
            {

            }
            public GraphNode(T value)
            {
                Data = value;
            }
            public List<GraphNode<T>> Neighbors
            {
                get
                {
                    neighbors = neighbors ?? new List<GraphNode<T>>();
                    return neighbors;
                }
            }
            public List<int> Weights
            {
                get
                {
                    weights = weights ?? new List<int>();
                    return weights;
                }
            }
        }//class GraphNode<T>
        class Graph<T>
        {
            private List<GraphNode<T>> nodeList;

            public Graph()
            {
                nodeList = new List<GraphNode<T>>();
            }

            public GraphNode<T> AddNode(T data)
            {
                GraphNode<T> node = new GraphNode<T>(data);
                return AddNode(node);
            }
            public GraphNode<T> AddNode(GraphNode<T> node)
            {
                nodeList.Add(node);
                return node;
            }
            public void AddEdge(GraphNode<T> from, GraphNode<T> to, bool oneway=true, int weight=0)
            {
                from.Neighbors.Add(to);
                from.Weights.Add(weight);

                if(false == oneway)
                {
                    to.Neighbors.Add(from);
                    to.Weights.Add(weight);
                }
            }
            internal void DebugPrintLinks()
            {
                foreach(GraphNode<T> node in nodeList)
                {
                    foreach(GraphNode<T> node2 in node.Neighbors)
                    {
                        Console.WriteLine(node.Data + "-" + node2.Data);
                    }
                }
            }
        }//class Graph<T>

        static void Test_Heap()
        {
            /**
             * 자료구조 : Heap
             * 
             * Heap은 힙 속성(Heap Property)를 만족하는 트리 기반의 자료구조이다. 
             * 힙 속성에 따르면 부모와 자식 노드간의 순서는 일정해야 하고, 같은 
             * 자식노드들 사이의 순서는 상관이 없다. Heap은 부모 노드가 항상 자식 
             * 노드보다 크거나 같아야 하는 경우(max heap)와 부모 노드가 항상 자식 
             * 노드보다 작거나 같아야 하는 경우(min heap)로 나눌 수 있다. 즉, 
             * Max Heap은 루트노드에 데이타를 꺼내면 항상 해당 컬렉션의 최대값을 
             * 리턴하고, Min Heap은 최소값을 리턴한다. Heap은 여러가지 형태로 구현 
             * 가능한데, 일반적인 구현의 하나로 Binary Tree를 기반으로 한 Binary 
             * Heap을 들 수 있다. 자료구조에서 말하는 Heap은 메모리 구조에서 말하는 
             * Heap과는 별개인 서로 다른 개념이다.
             **/

            /**
             * 간단한 BinaryHeap 클래스
             * 
             * 위에서 언급한 바와 같이 Binary Tree 개념에 기반한 Heap을 Binary 
             * Heap이라 부른다. Binary Heap 클래스의 핵심 메서드는 데이타를 새로 
             * 추가하는 Add(혹은 Insert)와 하나의 최상위 루트노드 데이타를 가져오는 
             * Remove 메서드이다. Heap은 한번에 하나씩 최대 혹은 최소의 데이타를 
             * 가져오는 기능이 가장 핵심적인 기능이다. 이런 Remove() 메서드를 여러 
             * 번 호출하면, 완전히 정렬(Sort)되어 있지 않은 자료 구조로부터 최대 
             * (혹은 최소)의 Top N 개 데이타를 가져오는 효과를 갖게 된다. 
             * Binary Heap 클래스를 구현하기 위해 Binary Tree를 사용할 수도 있고, 
             * 동적 배열을 사용하여 이진트리를 구현하는 방식으로 아래 예제와 같이 
             * .NET의 동적배열인 List 템플릿 클래스를 활용할 수도 있다. 
             * 아래는 예제를 단순화하기 위해 내부 데이타로 int 를 사용하였으나, 
             * 약간의 수정을 통해 C#의 Generics를 사용하여 일반화할 수 있다. 
             * Binary Heap으로 Max Heap 혹은 Min Heap을 구현할 수 있는데, 
             * 차이는 Add, Remove 메서드의 키값 비교에서 부등호를 서로 반대로 하는 
             * 것 정도이다. (참조: 아래는 Max Heap의 예만 표현하였는데, IComparer를 
             * 입력 받아 한 클래스에서 Max Heap/Min Heap을 같이 구현할 수도 있다) 
             **/

            HeapMax<char> h = new HeapMax<char>();

            char[] arr = {'H', 'E', 'A', 'P', 'S', 'O', 'R', 'T'};
            foreach(var c in arr)
            {
                h.Add(c);
                h.DebugPrint();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("remove : {0}", h.Remove());
            h.DebugPrint();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("remove : {0}", h.Remove());
            h.DebugPrint();


            Console.WriteLine("-------- int");
            {
                for(int i=0; i<10; ++i)
                {
                    int[] arr2 = {100,19,36,17,12,25,5,9,15,6,11,13,8,1,4,3 };
                    Util.Shuffle(rnd, arr2, 999);
                    HeapMax<int> h2 = new HeapMax<int>(arr2);
                    h2.DebugPrint();
                }
            }

        }//Test_Heap

        class HeapMax<T>
        {
            List<T> list = new List<T>();
            Comparer<T> cmp = Comparer<T>.Default;

            void Swap(int i1, int i2)
            {
                T tmp = list[i1];
                list[i1] = list[i2];
                list[i2] = tmp;
            }

            public HeapMax()
            {
            }
            public HeapMax(T[] arr)
            {
                HeapSort(arr);
                list.AddRange(arr);
            }
            public void Add(T val)
            {
                list.Add(val);

                int i = list.Count - 1;
                while(i > 0)
                {
                    int pi = (i - 1) / 2;

                    if (cmp.Compare(list[pi], list[i]) < 0)
                    {
                        Swap(pi, i);
                        i = pi;
                    }
                    else
                        break;
                }
            }

            public T Remove()
            {
                if (list.Count < 1)
                    throw new InvalidOperationException();

                T rt = list[0];

                list[0] = list[list.Count-1];
                list.RemoveAt(list.Count-1);

                int i = 0;
                int last = list.Count-1;
                while(i < list.Count-1)
                {
                    int ci = i*2 + 1;
                    if (ci+1 < last && cmp.Compare(list[ci], list[ci+1]) < 0)
                        ++ci;

                    if (ci > last || cmp.Compare(list[i], list[ci]) >= 0)
                        break;

                    Swap(i, ci);
                    i = ci;
                }
                
                return rt;
            }

            void DebugPrintRecursive(int index, int depth=0)
            {
                if (index >= list.Count)
                    return;

                Console.Write(new string(' ', depth*4));
                Console.WriteLine(list[index]);

                int child1 = index * 2 + 1;
                int child2 = child1 + 1;
                System.Diagnostics.Debug.Assert(child1 >= list.Count || cmp.Compare(list[index], list[child1]) >= 0);
                System.Diagnostics.Debug.Assert(child2 >= list.Count || cmp.Compare(list[index], list[child2]) >= 0);

                DebugPrintRecursive(child1, depth + 1);
                DebugPrintRecursive(child2, depth + 1);
            }
            public void DebugPrint2()
            {
                Util.PrintArray(list.ToArray());
            }
            public void DebugPrint()
            {
                Console.WriteLine();
                Util.PrintArray(list.ToArray());
                Console.WriteLine();
                DebugPrintRecursive(0);
            }

            void HeapSort(T[] arr)
            {
                int len = arr.Length;
                if (len <= 1)
                    return;

                for(int i = len/2 - 1; i>=0; --i)
                {
                    int pi = i;
                    int ci = pi*2 + 1;
                    while(ci < len)
                    {
                        if (ci+1 < len && cmp.Compare(arr[ci], arr[ci+1]) < 0)
                        {
                            ci = ci+1;
                        }

                        if(cmp.Compare(arr[pi], arr[ci]) >= 0)
                            break;

                        T tmp = arr[pi];
                        arr[pi] = arr[ci];
                        arr[ci] = tmp;
                        
                        pi = ci;
                        ci = pi*2 + 1;
                    }
                }
            }
        }

    }//class Collections
}


/*
1 2 3 4 5 6 7 8
H E A P S O R T     //중간지점인 p(4번째 인덱스)에서 시작
      ^       ^     //그의 자식은 (왼쪽 자노드 인덱스8)T>P이기 때문에 교환
H E A T S O R P     //A(3번째 인덱스)로 계속되며 A의 왼쪽 자노드는 O(6번째 인덱스)가 되며 오른쪽 자노드는
    ^     ^ ^       //R(7번째 인덱스값)이 되는데 R>O,R>A이므로 R,A교환
H E R T S O A P     //E(인덱스 2)와 그 왼쪽 자노드T(인덱스 4),오른쪽 자노드S(인덱스 5)비교
  ^   ^ ^           //T>S,T>E 결국 T,E교환
H T R E S O A P     //계속 E(인덱스 4)는 왼쪽 자노드P(인덱스 8)과 비교교환하는데 P>E
      ^       ^
H T R P S O A E     //H(인덱스 1)은 그의 왼쪽 자노드T(인덱스 2)와 오른쪽 자노드R(인덱스 3)
^ ^ ^               //T>R,T>H비교 교환한다.
T H R P S O A E     //계속 H를 비교 교환하게 되는데,H(인덱스 2)이므로 왼쪽 자노드P(인덱스 4),오른쪽 자노드
  ^   ^ ^           //S(인덱스5)에서 S>P,S>H비교 교환하게 된다.
T S R P H O A E     //힙 구성 끝.
*/