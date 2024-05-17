using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Jungol
{
    class Algorithm
    {
        public static void Run()
        {
            Util.Call(_1370);               // 1370	회의실 배정
            Util.Call(_1669);               // 1669	소시지 공장
            Util.Call(_1828);               // 1828	냉장고
            Util.Call(_2247);               // 2247	도서관
            Util.Call(_2499);               // 2499	저울
            Util.Call(_2461);               // 2461	공주님의 정원
            Util.Call(_2194);               // 2194	요플레 공장
            Util.Call(_1183);               // 1183	동전 자판기(下)
            Util.Call(_1060);               // 1060	최소비용신장트리
            Util.Call(_2641);               // 2641	택배
            /*
            Util.Call(_1889);               // 1889	N Queen
            Util.Call(_1681);               // 1681	해밀턴 순환회로
            Util.Call(_1027);               // 1027	좋은수열
            Util.Call(_1824);               // 1824	스도쿠
            Util.Call(_1662);               // 1662	비숍
            Util.Call(_1695);               // 1695	단지번호붙이기
            Util.Call(_1457);               // 1457	영역 구하기
            Util.Call(_1840);               // 1840	치즈
            Util.Call(_1409);               // 1409	벽장문의 이동
            Util.Call(_1106);               // 1106	장기
            Util.Call(_1078);               // 1078	저글링 방사능 오염
            Util.Call(_1462);               // 1462	보물섬
            Util.Call(_2261);               // 2261	경로 찾기
            Util.Call(_1082);               // 1082	화염에서탈출
            Util.Call(_2613);               // 2613	토마토(고)
            Util.Call(_1336);               // 1336	소수와 함께 하는 여행
            Util.Call(_2578);               // 2578	버스 갈아타기
            Util.Call(_1006);               // 1006	로봇
            Util.Call(_1411);               // 1411	두 줄로 타일 깔기
            Util.Call(_1848);               // 1848	극장 좌석
            Util.Call(_1407);               // 1407	숫자카드
            Util.Call(_1520);               // 1520	계단 오르기
            Util.Call(_1822);               // 1822	짚신벌레
            Util.Call(_2000);               // 2000	동전교환
            Util.Call(_1077);               // 1077	배낭채우기1
            Util.Call(_1491);               // 1491	자동차경주대회
            Util.Call(_1871);               // 1871	줄세우기
            Util.Call(_1510);               // 1510	색종이 올려 놓기
            Util.Call(_1408);               // 1408	전깃줄(초)
            Util.Call(_1539);               // 1539	가장높은탑쌓기
            Util.Call(_2251);               // 2251	전구
            Util.Call(_2584);               // 2584	전시장
            Util.Call(_1278);               // 1278	배낭채우기2
            Util.Call(_1382);               // 1382	동전 바꿔주기
            Util.Call(_1352);               // 1352	양팔저울
            Util.Call(_2616);               // 2616	앱
            Util.Call(_1220);               // 1220	최장 공통 부분서열
            Util.Call(_2191);               // 2191	최소 편집
            Util.Call(_1749);               // 1749	구슬게임
            Util.Call(_1677);               // 1677	경로찾기
            Util.Call(_1024);               // 1024	내리막 길
            Util.Call(_1019);               // 1019	소형기관차
            Util.Call(_1825);               // 1825	기업투자
            Util.Call(_1014);               // 1014	돌다리 건너기
            Util.Call(_2112);               // 2112	세 줄로 타일 깔기
            Util.Call(_2138);               // 2138	네 줄로 타일 깔기
            Util.Call(_2913);               // 2913	카드게임
            Util.Call(_1701);               // 1701	유전자
            Util.Call(_1235);               // 1235	악수
            Util.Call(_2500);               // 2500	전구
            Util.Call(_1703);               // 1703	검은점과 하얀점 연결
            Util.Call(_2993);               // 2993	리조트(초등/고등)
            Util.Call(_2264);               // 2264	색상환
            Util.Call(_1335);               // 1335	색종이 만들기
            Util.Call(_2543);               // 2543	타일 채우기
            Util.Call(_1092);               // 1092	제곱수 출력
            Util.Call(_1053);               // 1053	피보나치
            Util.Call(_2097);               // 2097	지하철
            Util.Call(_1108);               // 1108	페이지 전환
            Util.Call(_1111);               // 1111	등산로 찾기
            Util.Call(_1208);               // 1208	귀가
            Util.Call(_2109);               // 2109	꿀꿀이 축제(Festival)
            Util.Call(_1141);               // 1141	불쾌한 날
            Util.Call(_1328);               // 1328	빌딩
            Util.Call(_1809);               // 1809	탑
            Util.Call(_1214);               // 1214	히스토그램
            Util.Call(_1972);               // 1972	정렬(SORT)
            Util.Call(_1240);               // 1240	제곱근
            Util.Call(_1219);               // 1219	모자이크
            Util.Call(_1300);               // 1300	숫자구슬
            Util.Call(_1137);               // 1137	책 복사하기
            Util.Call(_1156);               // 1156	책 복사하기2
            Util.Call(_2574);               // 2574	사회망 서비스(SNS)
            Util.Call(_2916);               // 2916	트리
            Util.Call(_2223);               // 2223	Black Hole(블랙홀)
            Util.Call(_2614);               // 2614	탐사
            Util.Call(_2643);               // 2643	수족관1
            Util.Call(_1726);               // 1726	구간의 최대값 구하기
            Util.Call(_2615);               // 2615	공장
            Util.Call(_2469);               // 2469	줄세우기
            Util.Call(_2467);               // 2467	비용
            Util.Call(_1105);               // 1105	수식 계산기
            Util.Call(_1563);               // 1563	가지치기
            Util.Call(_1863);               // 1863	종교
            Util.Call(_1378);               // 1378	괄호의 값
            Util.Call(_1912);               // 1912	미로 탐색
            Util.Call(_2437);               // 2437	순서찾기
            Util.Call(_2082);               // 2082	힙정렬2 (Heap_Sort)
            Util.Call(_1929);               // 1929	책꽂이 만들기
            Util.Call(_1318);               // 1318	못생긴 수
            Util.Call(_1570);               // 1570	중앙값
            Util.Call(_1941);               // 1941	최단경로
            Util.Call(_2287);               // 2287	맛있는 걸 좋아하는 소들
            Util.Call(_1285);               // 1285	물먹는 용(Enter The Dragon)
            Util.Call(_1364);               // 1364	방면적 넓히기
            Util.Call(_1419);               // 1419	엔디안
            Util.Call(_2468);               // 2468	비밀번호
            Util.Call(_1622);               // 1622	유명한 소
            Util.Call(_2423);               // 2423	현금 인출기(ATM)
            Util.Call(_1379);               // 1379	절점 찾기
            Util.Call(_1180);               // 1180	Dessert
            Util.Call(_1405);               // 1405	하노이 4
            Util.Call(_1841);               // 1841	월드컵
            Util.Call(_2250);               // 2250	루빅의 사각형
            Util.Call(_1013);               // 1013	Fivestar
            Util.Call(_1171);               // 1171	두수의 합
            Util.Call(_1033);               // 1033	회로배치
            Util.Call(_2606);               // 2606	토마토(초)
            Util.Call(_1603);               // 1603	경비행기
            Util.Call(_2058);               // 2058	고돌이 고소미
            Util.Call(_2503);               // 2503	그리드 게임
            Util.Call(_2263);               // 2263	해밍 경로
            Util.Call(_2387);               // 2387	원숭이 사냥
            Util.Call(_2098);               // 2098	자리 배치
            Util.Call(_1257);               // 1257	전깃줄(중)
            Util.Call(_2573);               // 2573	먹이사슬
            Util.Call(_2635);               // 2635	막대기
            Util.Call(_2754);               // 2754	암호
            Util.Call(_2862);               // 2862	정사각형 만들기
            Util.Call(_2864);               // 2864	L 모양의 종이 자르기
            Util.Call(_1869);               // 1869	보드게임
            Util.Call(_2038);               // 2038	기지국
            Util.Call(_2063);               // 2063	택배
            Util.Call(_1139);               // 1139	Bitonic
            Util.Call(_1820);               // 1820	경찰차
            Util.Call(_1017);               // 1017	공주 구하기
            Util.Call(_1249);               // 1249	건물 세우기
            Util.Call(_2043);               // 2043	Permanent Computation
            Util.Call(_1545);               // 1545	해밀턴 순환회로2
            Util.Call(_1442);               // 1442	여러 줄로 타일 깔기
            Util.Call(_1993);               // 1993	두부 모판 자르기
            Util.Call(_2356);               // 2356	키위 주스
            Util.Call(_1883);               // 1883	숫자 맞추기
            Util.Call(_2588);               // 2588	회전 테이블
            Util.Call(_1808);               // 1808	의좋은 형제
            Util.Call(_2502);               // 2502	모둠
            Util.Call(_1479);               // 1479	초고속철도
            Util.Call(_1635);               // 1635	Palindrome
            Util.Call(_1083);               // 1083	숫자박스
            Util.Call(_1558);               // 1558	DNA유사도
            Util.Call(_1389);               // 1389	수열 축소
            Util.Call(_1332);               // 1332	작명하기
            Util.Call(_2612);               // 2612	바이러스 / 백신
            Util.Call(_2842);               // 2842	검열2
            Util.Call(_1885);               // 1885	접두사
            Util.Call(_2918);               // 2918	구간 성분
            Util.Call(_2150);               // 2150	검열3
            Util.Call(_3137);               // 3137	전화번호 검색
            Util.Call(_3143);               // 3143	생명체 분류
            Util.Call(_1551);               // 1551	활자인쇄기(Type Printer)
            Util.Call(_1110);               // 1110	파이프(pipe)
            Util.Call(_1117);               // 1117	매점
            Util.Call(_1248);               // 1248	Black-white Grid
            Util.Call(_1118);               // 1118	비숍2(bishop)
            Util.Call(_1882);               // 1882	우주여행
            Util.Call(_1129);               // 1129	평면내 선분의 교점
            Util.Call(_3122);               // 3122	2차원 평면에서 두 선분이 만나는 경우의 수
            Util.Call(_3005);               // 3005	단순다각형의 면적
            Util.Call(_1151);               // 1151	볼록다각형(convexhull)
            Util.Call(_2395);               // 2395	다각형 안의 점
            Util.Call(_2917);               // 2917	미술관
            Util.Call(_2998);               // 2998	레이저 센서
            Util.Call(_1403);               // 1403	달팽이
            Util.Call(_1694);               // 1694	정사각형자르기
            //*/
        }

        //--------------------------------------------------
        // 1370	회의실 배정
        //--------------------------------------------------
        struct stImpl_1370_Meeting
        {
            public int No
            {
                get; set;
            }
            public int Start
            {
                get; set;
            }
            public int End
            {
                get; set;
            }

            public stImpl_1370_Meeting(string input)
            {
                string[] words = input.Trim().Split();
                Debug.Assert(3 == words.Length);

                No = Convert.ToInt32(words[0]);
                Start = Convert.ToInt32(words[1]);
                End = Convert.ToInt32(words[2]);

                Debug.Assert(Start < End);
            }

            public bool IsSame(stImpl_1370_Meeting rhs)
            {
                return No == rhs.No;
            }
            public bool InterSect(stImpl_1370_Meeting rhs)
            {
                if(rhs.Start >= End || rhs.End <= Start)
                    return false;

                return true;
            }
            public override string ToString()
            {
                return string.Format("{0} : [{1,3} ~ {2,3}]", No, Start, End);
            }
        }
        static void Impl_1370(string input)
        {
            string[] lines = input.Split(new char[] {'\n'});

            int cnt = Convert.ToInt32(lines[0]);
            Debug.Assert(cnt == lines.Length - 1);

            stImpl_1370_Meeting[] list = new stImpl_1370_Meeting[cnt];

            for(int i=0; i<cnt; ++i)
            {
                list[i] = new stImpl_1370_Meeting(lines[i+1]);
            }

            for(int i=0; i<cnt; ++i)
            {
                stImpl_1370_Meeting cur = list[i];

                Console.WriteLine(cur);
                for (int j = 0; j < cnt; ++j)
                {
                    if (i==j)
                        continue;

                    if(cur.InterSect(list[j]))
                        Console.WriteLine("    {0}", list[j]);
                }
                Console.WriteLine();
            }
        }
        static void _1370()
        {
            string input = @"6
1 1 10
2 5 6
3 13 15
4 14 17
5 8 14
6 3 12";
            Impl_1370(input);
        }



        //--------------------------------------------------
        // 1669	소시지 공장
        //--------------------------------------------------
        static void Impl_1669()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1669()
        {
            Impl_1669();
        }



        //--------------------------------------------------
        // 1828	냉장고
        //--------------------------------------------------
        static void Impl_1828()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1828()
        {
            Impl_1828();
        }



        //--------------------------------------------------
        // 2247	도서관
        //--------------------------------------------------
        static void Impl_2247()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2247()
        {
            Impl_2247();
        }



        //--------------------------------------------------
        // 2499	저울
        //--------------------------------------------------
        static void Impl_2499()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2499()
        {
            Impl_2499();
        }



        //--------------------------------------------------
        // 2461	공주님의 정원
        //--------------------------------------------------
        static void Impl_2461()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2461()
        {
            Impl_2461();
        }



        //--------------------------------------------------
        // 2194	요플레 공장
        //--------------------------------------------------
        static void Impl_2194()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2194()
        {
            Impl_2194();
        }



        //--------------------------------------------------
        // 1183	동전 자판기(下)
        //--------------------------------------------------
        static void Impl_1183()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1183()
        {
            Impl_1183();
        }



        //--------------------------------------------------
        // 1060	최소비용신장트리
        //--------------------------------------------------
        static void Impl_1060()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1060()
        {
            Impl_1060();
        }



        //--------------------------------------------------
        // 2641	택배
        //--------------------------------------------------
        static void Impl_2641()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2641()
        {
            Impl_2641();
        }



        //--------------------------------------------------
        // 1889	N Queen
        //--------------------------------------------------
        static void Impl_1889()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1889()
        {
            Impl_1889();
        }



        //--------------------------------------------------
        // 1681	해밀턴 순환회로
        //--------------------------------------------------
        static void Impl_1681()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1681()
        {
            Impl_1681();
        }



        //--------------------------------------------------
        // 1027	좋은수열
        //--------------------------------------------------
        static void Impl_1027()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1027()
        {
            Impl_1027();
        }



        //--------------------------------------------------
        // 1824	스도쿠
        //--------------------------------------------------
        static void Impl_1824()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1824()
        {
            Impl_1824();
        }



        //--------------------------------------------------
        // 1662	비숍
        //--------------------------------------------------
        static void Impl_1662()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1662()
        {
            Impl_1662();
        }



        //--------------------------------------------------
        // 1695	단지번호붙이기
        //--------------------------------------------------
        static void Impl_1695()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1695()
        {
            Impl_1695();
        }



        //--------------------------------------------------
        // 1457	영역 구하기
        //--------------------------------------------------
        static void Impl_1457()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1457()
        {
            Impl_1457();
        }



        //--------------------------------------------------
        // 1840	치즈
        //--------------------------------------------------
        static void Impl_1840()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1840()
        {
            Impl_1840();
        }



        //--------------------------------------------------
        // 1409	벽장문의 이동
        //--------------------------------------------------
        static void Impl_1409()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1409()
        {
            Impl_1409();
        }



        //--------------------------------------------------
        // 1106	장기
        //--------------------------------------------------
        static void Impl_1106()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1106()
        {
            Impl_1106();
        }



        //--------------------------------------------------
        // 1078	저글링 방사능 오염
        //--------------------------------------------------
        static void Impl_1078()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1078()
        {
            Impl_1078();
        }



        //--------------------------------------------------
        // 1462	보물섬
        //--------------------------------------------------
        static void Impl_1462()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1462()
        {
            Impl_1462();
        }



        //--------------------------------------------------
        // 2261	경로 찾기
        //--------------------------------------------------
        static void Impl_2261()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2261()
        {
            Impl_2261();
        }



        //--------------------------------------------------
        // 1082	화염에서탈출
        //--------------------------------------------------
        static void Impl_1082()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1082()
        {
            Impl_1082();
        }



        //--------------------------------------------------
        // 2613	토마토(고)
        //--------------------------------------------------
        static void Impl_2613()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2613()
        {
            Impl_2613();
        }



        //--------------------------------------------------
        // 1336	소수와 함께 하는 여행
        //--------------------------------------------------
        static void Impl_1336()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1336()
        {
            Impl_1336();
        }



        //--------------------------------------------------
        // 2578	버스 갈아타기
        //--------------------------------------------------
        static void Impl_2578()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2578()
        {
            Impl_2578();
        }



        //--------------------------------------------------
        // 1006	로봇
        //--------------------------------------------------
        static void Impl_1006()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1006()
        {
            Impl_1006();
        }



        //--------------------------------------------------
        // 1411	두 줄로 타일 깔기
        //--------------------------------------------------
        static void Impl_1411()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1411()
        {
            Impl_1411();
        }



        //--------------------------------------------------
        // 1848	극장 좌석
        //--------------------------------------------------
        static void Impl_1848()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1848()
        {
            Impl_1848();
        }



        //--------------------------------------------------
        // 1407	숫자카드
        //--------------------------------------------------
        static void Impl_1407()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1407()
        {
            Impl_1407();
        }



        //--------------------------------------------------
        // 1520	계단 오르기
        //--------------------------------------------------
        static void Impl_1520()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1520()
        {
            Impl_1520();
        }



        //--------------------------------------------------
        // 1822	짚신벌레
        //--------------------------------------------------
        static void Impl_1822()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1822()
        {
            Impl_1822();
        }



        //--------------------------------------------------
        // 2000	동전교환
        //--------------------------------------------------
        static void Impl_2000()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2000()
        {
            Impl_2000();
        }



        //--------------------------------------------------
        // 1077	배낭채우기1
        //--------------------------------------------------
        static void Impl_1077()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1077()
        {
            Impl_1077();
        }



        //--------------------------------------------------
        // 1491	자동차경주대회
        //--------------------------------------------------
        static void Impl_1491()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1491()
        {
            Impl_1491();
        }



        //--------------------------------------------------
        // 1871	줄세우기
        //--------------------------------------------------
        static void Impl_1871()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1871()
        {
            Impl_1871();
        }



        //--------------------------------------------------
        // 1510	색종이 올려 놓기
        //--------------------------------------------------
        static void Impl_1510()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1510()
        {
            Impl_1510();
        }



        //--------------------------------------------------
        // 1408	전깃줄(초)
        //--------------------------------------------------
        static void Impl_1408()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1408()
        {
            Impl_1408();
        }



        //--------------------------------------------------
        // 1539	가장높은탑쌓기
        //--------------------------------------------------
        static void Impl_1539()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1539()
        {
            Impl_1539();
        }



        //--------------------------------------------------
        // 2251	전구
        //--------------------------------------------------
        static void Impl_2251()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2251()
        {
            Impl_2251();
        }



        //--------------------------------------------------
        // 2584	전시장
        //--------------------------------------------------
        static void Impl_2584()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2584()
        {
            Impl_2584();
        }



        //--------------------------------------------------
        // 1278	배낭채우기2
        //--------------------------------------------------
        static void Impl_1278()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1278()
        {
            Impl_1278();
        }



        //--------------------------------------------------
        // 1382	동전 바꿔주기
        //--------------------------------------------------
        static void Impl_1382()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1382()
        {
            Impl_1382();
        }



        //--------------------------------------------------
        // 1352	양팔저울
        //--------------------------------------------------
        static void Impl_1352()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1352()
        {
            Impl_1352();
        }



        //--------------------------------------------------
        // 2616	앱
        //--------------------------------------------------
        static void Impl_2616()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2616()
        {
            Impl_2616();
        }



        //--------------------------------------------------
        // 1220	최장 공통 부분서열
        //--------------------------------------------------
        static void Impl_1220()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1220()
        {
            Impl_1220();
        }



        //--------------------------------------------------
        // 2191	최소 편집
        //--------------------------------------------------
        static void Impl_2191()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2191()
        {
            Impl_2191();
        }



        //--------------------------------------------------
        // 1749	구슬게임
        //--------------------------------------------------
        static void Impl_1749()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1749()
        {
            Impl_1749();
        }



        //--------------------------------------------------
        // 1677	경로찾기
        //--------------------------------------------------
        static void Impl_1677()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1677()
        {
            Impl_1677();
        }



        //--------------------------------------------------
        // 1024	내리막 길
        //--------------------------------------------------
        static void Impl_1024()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1024()
        {
            Impl_1024();
        }



        //--------------------------------------------------
        // 1019	소형기관차
        //--------------------------------------------------
        static void Impl_1019()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1019()
        {
            Impl_1019();
        }



        //--------------------------------------------------
        // 1825	기업투자
        //--------------------------------------------------
        static void Impl_1825()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1825()
        {
            Impl_1825();
        }



        //--------------------------------------------------
        // 1014	돌다리 건너기
        //--------------------------------------------------
        static void Impl_1014()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1014()
        {
            Impl_1014();
        }



        //--------------------------------------------------
        // 2112	세 줄로 타일 깔기
        //--------------------------------------------------
        static void Impl_2112()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2112()
        {
            Impl_2112();
        }



        //--------------------------------------------------
        // 2138	네 줄로 타일 깔기
        //--------------------------------------------------
        static void Impl_2138()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2138()
        {
            Impl_2138();
        }



        //--------------------------------------------------
        // 2913	카드게임
        //--------------------------------------------------
        static void Impl_2913()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2913()
        {
            Impl_2913();
        }



        //--------------------------------------------------
        // 1701	유전자
        //--------------------------------------------------
        static void Impl_1701()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1701()
        {
            Impl_1701();
        }



        //--------------------------------------------------
        // 1235	악수
        //--------------------------------------------------
        static void Impl_1235()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1235()
        {
            Impl_1235();
        }



        //--------------------------------------------------
        // 2500	전구
        //--------------------------------------------------
        static void Impl_2500()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2500()
        {
            Impl_2500();
        }



        //--------------------------------------------------
        // 1703	검은점과 하얀점 연결
        //--------------------------------------------------
        static void Impl_1703()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1703()
        {
            Impl_1703();
        }



        //--------------------------------------------------
        // 2993	리조트(초등/고등)
        //--------------------------------------------------
        static void Impl_2993()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2993()
        {
            Impl_2993();
        }



        //--------------------------------------------------
        // 2264	색상환
        //--------------------------------------------------
        static void Impl_2264()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2264()
        {
            Impl_2264();
        }



        //--------------------------------------------------
        // 1335	색종이 만들기
        //--------------------------------------------------
        static void Impl_1335()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1335()
        {
            Impl_1335();
        }



        //--------------------------------------------------
        // 2543	타일 채우기
        //--------------------------------------------------
        static void Impl_2543()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2543()
        {
            Impl_2543();
        }



        //--------------------------------------------------
        // 1092	제곱수 출력
        //--------------------------------------------------
        static void Impl_1092()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1092()
        {
            Impl_1092();
        }



        //--------------------------------------------------
        // 1053	피보나치
        //--------------------------------------------------
        static void Impl_1053()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1053()
        {
            Impl_1053();
        }



        //--------------------------------------------------
        // 2097	지하철
        //--------------------------------------------------
        static void Impl_2097()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2097()
        {
            Impl_2097();
        }



        //--------------------------------------------------
        // 1108	페이지 전환
        //--------------------------------------------------
        static void Impl_1108()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1108()
        {
            Impl_1108();
        }



        //--------------------------------------------------
        // 1111	등산로 찾기
        //--------------------------------------------------
        static void Impl_1111()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1111()
        {
            Impl_1111();
        }



        //--------------------------------------------------
        // 1208	귀가
        //--------------------------------------------------
        static void Impl_1208()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1208()
        {
            Impl_1208();
        }



        //--------------------------------------------------
        // 2109	꿀꿀이 축제(Festival)
        //--------------------------------------------------
        static void Impl_2109()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2109()
        {
            Impl_2109();
        }



        //--------------------------------------------------
        // 1141	불쾌한 날
        //--------------------------------------------------
        static void Impl_1141()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1141()
        {
            Impl_1141();
        }



        //--------------------------------------------------
        // 1328	빌딩
        //--------------------------------------------------
        static void Impl_1328()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1328()
        {
            Impl_1328();
        }



        //--------------------------------------------------
        // 1809	탑
        //--------------------------------------------------
        static void Impl_1809()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1809()
        {
            Impl_1809();
        }



        //--------------------------------------------------
        // 1214	히스토그램
        //--------------------------------------------------
        static void Impl_1214()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1214()
        {
            Impl_1214();
        }



        //--------------------------------------------------
        // 1972	정렬(SORT)
        //--------------------------------------------------
        static void Impl_1972()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1972()
        {
            Impl_1972();
        }



        //--------------------------------------------------
        // 1240	제곱근
        //--------------------------------------------------
        static void Impl_1240()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1240()
        {
            Impl_1240();
        }



        //--------------------------------------------------
        // 1219	모자이크
        //--------------------------------------------------
        static void Impl_1219()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1219()
        {
            Impl_1219();
        }



        //--------------------------------------------------
        // 1300	숫자구슬
        //--------------------------------------------------
        static void Impl_1300()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1300()
        {
            Impl_1300();
        }



        //--------------------------------------------------
        // 1137	책 복사하기
        //--------------------------------------------------
        static void Impl_1137()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1137()
        {
            Impl_1137();
        }



        //--------------------------------------------------
        // 1156	책 복사하기2
        //--------------------------------------------------
        static void Impl_1156()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1156()
        {
            Impl_1156();
        }



        //--------------------------------------------------
        // 2574	사회망 서비스(SNS)
        //--------------------------------------------------
        static void Impl_2574()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2574()
        {
            Impl_2574();
        }



        //--------------------------------------------------
        // 2916	트리
        //--------------------------------------------------
        static void Impl_2916()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2916()
        {
            Impl_2916();
        }



        //--------------------------------------------------
        // 2223	Black Hole(블랙홀)
        //--------------------------------------------------
        static void Impl_2223()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2223()
        {
            Impl_2223();
        }



        //--------------------------------------------------
        // 2614	탐사
        //--------------------------------------------------
        static void Impl_2614()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2614()
        {
            Impl_2614();
        }



        //--------------------------------------------------
        // 2643	수족관1
        //--------------------------------------------------
        static void Impl_2643()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2643()
        {
            Impl_2643();
        }



        //--------------------------------------------------
        // 1726	구간의 최대값 구하기
        //--------------------------------------------------
        static void Impl_1726()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1726()
        {
            Impl_1726();
        }



        //--------------------------------------------------
        // 2615	공장
        //--------------------------------------------------
        static void Impl_2615()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2615()
        {
            Impl_2615();
        }



        //--------------------------------------------------
        // 2469	줄세우기
        //--------------------------------------------------
        static void Impl_2469()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2469()
        {
            Impl_2469();
        }



        //--------------------------------------------------
        // 2467	비용
        //--------------------------------------------------
        static void Impl_2467()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2467()
        {
            Impl_2467();
        }



        //--------------------------------------------------
        // 1105	수식 계산기
        //--------------------------------------------------
        static void Impl_1105()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1105()
        {
            Impl_1105();
        }



        //--------------------------------------------------
        // 1563	가지치기
        //--------------------------------------------------
        static void Impl_1563()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1563()
        {
            Impl_1563();
        }



        //--------------------------------------------------
        // 1863	종교
        //--------------------------------------------------
        static void Impl_1863()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1863()
        {
            Impl_1863();
        }



        //--------------------------------------------------
        // 1378	괄호의 값
        //--------------------------------------------------
        static void Impl_1378()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1378()
        {
            Impl_1378();
        }



        //--------------------------------------------------
        // 1912	미로 탐색
        //--------------------------------------------------
        static void Impl_1912()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1912()
        {
            Impl_1912();
        }



        //--------------------------------------------------
        // 2437	순서찾기
        //--------------------------------------------------
        static void Impl_2437()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2437()
        {
            Impl_2437();
        }



        //--------------------------------------------------
        // 2082	힙정렬2 (Heap_Sort)
        //--------------------------------------------------
        static void Impl_2082()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2082()
        {
            Impl_2082();
        }



        //--------------------------------------------------
        // 1929	책꽂이 만들기
        //--------------------------------------------------
        static void Impl_1929()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1929()
        {
            Impl_1929();
        }



        //--------------------------------------------------
        // 1318	못생긴 수
        //--------------------------------------------------
        static void Impl_1318()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1318()
        {
            Impl_1318();
        }



        //--------------------------------------------------
        // 1570	중앙값
        //--------------------------------------------------
        static void Impl_1570()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1570()
        {
            Impl_1570();
        }



        //--------------------------------------------------
        // 1941	최단경로
        //--------------------------------------------------
        static void Impl_1941()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1941()
        {
            Impl_1941();
        }



        //--------------------------------------------------
        // 2287	맛있는 걸 좋아하는 소들
        //--------------------------------------------------
        static void Impl_2287()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2287()
        {
            Impl_2287();
        }



        //--------------------------------------------------
        // 1285	물먹는 용(Enter The Dragon)
        //--------------------------------------------------
        static void Impl_1285()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1285()
        {
            Impl_1285();
        }



        //--------------------------------------------------
        // 1364	방면적 넓히기
        //--------------------------------------------------
        static void Impl_1364()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1364()
        {
            Impl_1364();
        }



        //--------------------------------------------------
        // 1419	엔디안
        //--------------------------------------------------
        static void Impl_1419()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1419()
        {
            Impl_1419();
        }



        //--------------------------------------------------
        // 2468	비밀번호
        //--------------------------------------------------
        static void Impl_2468()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2468()
        {
            Impl_2468();
        }



        //--------------------------------------------------
        // 1622	유명한 소
        //--------------------------------------------------
        static void Impl_1622()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1622()
        {
            Impl_1622();
        }



        //--------------------------------------------------
        // 2423	현금 인출기(ATM)
        //--------------------------------------------------
        static void Impl_2423()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2423()
        {
            Impl_2423();
        }



        //--------------------------------------------------
        // 1379	절점 찾기
        //--------------------------------------------------
        static void Impl_1379()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1379()
        {
            Impl_1379();
        }



        //--------------------------------------------------
        // 1180	Dessert
        //--------------------------------------------------
        static void Impl_1180()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1180()
        {
            Impl_1180();
        }



        //--------------------------------------------------
        // 1405	하노이 4
        //--------------------------------------------------
        static void Impl_1405()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1405()
        {
            Impl_1405();
        }



        //--------------------------------------------------
        // 1841	월드컵
        //--------------------------------------------------
        static void Impl_1841()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1841()
        {
            Impl_1841();
        }



        //--------------------------------------------------
        // 2250	루빅의 사각형
        //--------------------------------------------------
        static void Impl_2250()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2250()
        {
            Impl_2250();
        }



        //--------------------------------------------------
        // 1013	Fivestar
        //--------------------------------------------------
        static void Impl_1013()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1013()
        {
            Impl_1013();
        }



        //--------------------------------------------------
        // 1171	두수의 합
        //--------------------------------------------------
        static void Impl_1171()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1171()
        {
            Impl_1171();
        }



        //--------------------------------------------------
        // 1033	회로배치
        //--------------------------------------------------
        static void Impl_1033()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1033()
        {
            Impl_1033();
        }



        //--------------------------------------------------
        // 2606	토마토(초)
        //--------------------------------------------------
        static void Impl_2606()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2606()
        {
            Impl_2606();
        }



        //--------------------------------------------------
        // 1603	경비행기
        //--------------------------------------------------
        static void Impl_1603()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1603()
        {
            Impl_1603();
        }



        //--------------------------------------------------
        // 2058	고돌이 고소미
        //--------------------------------------------------
        static void Impl_2058()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2058()
        {
            Impl_2058();
        }



        //--------------------------------------------------
        // 2503	그리드 게임
        //--------------------------------------------------
        static void Impl_2503()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2503()
        {
            Impl_2503();
        }



        //--------------------------------------------------
        // 2263	해밍 경로
        //--------------------------------------------------
        static void Impl_2263()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2263()
        {
            Impl_2263();
        }



        //--------------------------------------------------
        // 2387	원숭이 사냥
        //--------------------------------------------------
        static void Impl_2387()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2387()
        {
            Impl_2387();
        }



        //--------------------------------------------------
        // 2098	자리 배치
        //--------------------------------------------------
        static void Impl_2098()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2098()
        {
            Impl_2098();
        }



        //--------------------------------------------------
        // 1257	전깃줄(중)
        //--------------------------------------------------
        static void Impl_1257()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1257()
        {
            Impl_1257();
        }



        //--------------------------------------------------
        // 2573	먹이사슬
        //--------------------------------------------------
        static void Impl_2573()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2573()
        {
            Impl_2573();
        }



        //--------------------------------------------------
        // 2635	막대기
        //--------------------------------------------------
        static void Impl_2635()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2635()
        {
            Impl_2635();
        }



        //--------------------------------------------------
        // 2754	암호
        //--------------------------------------------------
        static void Impl_2754()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2754()
        {
            Impl_2754();
        }



        //--------------------------------------------------
        // 2862	정사각형 만들기
        //--------------------------------------------------
        static void Impl_2862()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2862()
        {
            Impl_2862();
        }



        //--------------------------------------------------
        // 2864	L 모양의 종이 자르기
        //--------------------------------------------------
        static void Impl_2864()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2864()
        {
            Impl_2864();
        }



        //--------------------------------------------------
        // 1869	보드게임
        //--------------------------------------------------
        static void Impl_1869()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1869()
        {
            Impl_1869();
        }



        //--------------------------------------------------
        // 2038	기지국
        //--------------------------------------------------
        static void Impl_2038()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2038()
        {
            Impl_2038();
        }



        //--------------------------------------------------
        // 2063	택배
        //--------------------------------------------------
        static void Impl_2063()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2063()
        {
            Impl_2063();
        }



        //--------------------------------------------------
        // 1139	Bitonic
        //--------------------------------------------------
        static void Impl_1139()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1139()
        {
            Impl_1139();
        }



        //--------------------------------------------------
        // 1820	경찰차
        //--------------------------------------------------
        static void Impl_1820()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1820()
        {
            Impl_1820();
        }



        //--------------------------------------------------
        // 1017	공주 구하기
        //--------------------------------------------------
        static void Impl_1017()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1017()
        {
            Impl_1017();
        }



        //--------------------------------------------------
        // 1249	건물 세우기
        //--------------------------------------------------
        static void Impl_1249()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1249()
        {
            Impl_1249();
        }



        //--------------------------------------------------
        // 2043	Permanent Computation
        //--------------------------------------------------
        static void Impl_2043()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2043()
        {
            Impl_2043();
        }



        //--------------------------------------------------
        // 1545	해밀턴 순환회로2
        //--------------------------------------------------
        static void Impl_1545()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1545()
        {
            Impl_1545();
        }



        //--------------------------------------------------
        // 1442	여러 줄로 타일 깔기
        //--------------------------------------------------
        static void Impl_1442()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1442()
        {
            Impl_1442();
        }



        //--------------------------------------------------
        // 1993	두부 모판 자르기
        //--------------------------------------------------
        static void Impl_1993()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1993()
        {
            Impl_1993();
        }



        //--------------------------------------------------
        // 2356	키위 주스
        //--------------------------------------------------
        static void Impl_2356()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2356()
        {
            Impl_2356();
        }



        //--------------------------------------------------
        // 1883	숫자 맞추기
        //--------------------------------------------------
        static void Impl_1883()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1883()
        {
            Impl_1883();
        }



        //--------------------------------------------------
        // 2588	회전 테이블
        //--------------------------------------------------
        static void Impl_2588()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2588()
        {
            Impl_2588();
        }



        //--------------------------------------------------
        // 1808	의좋은 형제
        //--------------------------------------------------
        static void Impl_1808()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1808()
        {
            Impl_1808();
        }



        //--------------------------------------------------
        // 2502	모둠
        //--------------------------------------------------
        static void Impl_2502()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2502()
        {
            Impl_2502();
        }



        //--------------------------------------------------
        // 1479	초고속철도
        //--------------------------------------------------
        static void Impl_1479()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1479()
        {
            Impl_1479();
        }



        //--------------------------------------------------
        // 1635	Palindrome
        //--------------------------------------------------
        static void Impl_1635()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1635()
        {
            Impl_1635();
        }



        //--------------------------------------------------
        // 1083	숫자박스
        //--------------------------------------------------
        static void Impl_1083()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1083()
        {
            Impl_1083();
        }



        //--------------------------------------------------
        // 1558	DNA유사도
        //--------------------------------------------------
        static void Impl_1558()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1558()
        {
            Impl_1558();
        }



        //--------------------------------------------------
        // 1389	수열 축소
        //--------------------------------------------------
        static void Impl_1389()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1389()
        {
            Impl_1389();
        }



        //--------------------------------------------------
        // 1332	작명하기
        //--------------------------------------------------
        static void Impl_1332()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1332()
        {
            Impl_1332();
        }



        //--------------------------------------------------
        // 2612	바이러스 / 백신
        //--------------------------------------------------
        static void Impl_2612()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2612()
        {
            Impl_2612();
        }



        //--------------------------------------------------
        // 2842	검열2
        //--------------------------------------------------
        static void Impl_2842()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2842()
        {
            Impl_2842();
        }



        //--------------------------------------------------
        // 1885	접두사
        //--------------------------------------------------
        static void Impl_1885()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1885()
        {
            Impl_1885();
        }



        //--------------------------------------------------
        // 2918	구간 성분
        //--------------------------------------------------
        static void Impl_2918()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2918()
        {
            Impl_2918();
        }



        //--------------------------------------------------
        // 2150	검열3
        //--------------------------------------------------
        static void Impl_2150()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2150()
        {
            Impl_2150();
        }



        //--------------------------------------------------
        // 3137	전화번호 검색
        //--------------------------------------------------
        static void Impl_3137()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _3137()
        {
            Impl_3137();
        }



        //--------------------------------------------------
        // 3143	생명체 분류
        //--------------------------------------------------
        static void Impl_3143()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _3143()
        {
            Impl_3143();
        }



        //--------------------------------------------------
        // 1551	활자인쇄기(Type Printer)
        //--------------------------------------------------
        static void Impl_1551()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1551()
        {
            Impl_1551();
        }



        //--------------------------------------------------
        // 1110	파이프(pipe)
        //--------------------------------------------------
        static void Impl_1110()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1110()
        {
            Impl_1110();
        }



        //--------------------------------------------------
        // 1117	매점
        //--------------------------------------------------
        static void Impl_1117()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1117()
        {
            Impl_1117();
        }



        //--------------------------------------------------
        // 1248	Black-white Grid
        //--------------------------------------------------
        static void Impl_1248()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1248()
        {
            Impl_1248();
        }



        //--------------------------------------------------
        // 1118	비숍2(bishop)
        //--------------------------------------------------
        static void Impl_1118()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1118()
        {
            Impl_1118();
        }



        //--------------------------------------------------
        // 1882	우주여행
        //--------------------------------------------------
        static void Impl_1882()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1882()
        {
            Impl_1882();
        }



        //--------------------------------------------------
        // 1129	평면내 선분의 교점
        //--------------------------------------------------
        static void Impl_1129()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1129()
        {
            Impl_1129();
        }



        //--------------------------------------------------
        // 3122	2차원 평면에서 두 선분이 만나는 경우의 수
        //--------------------------------------------------
        static void Impl_3122()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _3122()
        {
            Impl_3122();
        }



        //--------------------------------------------------
        // 3005	단순다각형의 면적
        //--------------------------------------------------
        static void Impl_3005()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _3005()
        {
            Impl_3005();
        }



        //--------------------------------------------------
        // 1151	볼록다각형(convexhull)
        //--------------------------------------------------
        static void Impl_1151()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1151()
        {
            Impl_1151();
        }



        //--------------------------------------------------
        // 2395	다각형 안의 점
        //--------------------------------------------------
        static void Impl_2395()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2395()
        {
            Impl_2395();
        }



        //--------------------------------------------------
        // 2917	미술관
        //--------------------------------------------------
        static void Impl_2917()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2917()
        {
            Impl_2917();
        }



        //--------------------------------------------------
        // 2998	레이저 센서
        //--------------------------------------------------
        static void Impl_2998()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _2998()
        {
            Impl_2998();
        }



        //--------------------------------------------------
        // 1403	달팽이
        //--------------------------------------------------
        static void Impl_1403()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1403()
        {
            Impl_1403();
        }



        //--------------------------------------------------
        // 1694	정사각형자르기
        //--------------------------------------------------
        static void Impl_1694()
        {
            Console.WriteLine("==== NOT IMPLEMENTED");
        }
        static void _1694()
        {
            Impl_1694();
        }

    }
}
