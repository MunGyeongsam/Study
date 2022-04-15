// examples.cpp : 이 파일에는 'main' 함수가 포함됩니다. 거기서 프로그램 실행이 시작되고 종료됩니다.
//

/*
#include <iostream>
#include <iomanip>

using namespace std;

void Print(int a[][3])
{
    cout << __FUNCSIG__ << __LINE__ << endl;
    cout << sizeof(a) << endl;
    cout << typeid(a).name() << endl << endl;
}
void Print2(int(*a)[3])
{
    cout << __FUNCSIG__ << __LINE__ << endl;
    cout << sizeof(a) << endl;
    cout << typeid(a).name() << endl << endl;
}
void Print3(int(&a)[2][3]) 
{
    cout << __FUNCSIG__ << __LINE__ << endl;
    cout << sizeof(a) << endl;
    cout << typeid(a).name() << endl;

    for (int i = 0; i < _countof(a); ++i)
    {
        for (int j = 0; j < _countof(a[i]); ++j)
            cout << setw(3) << a[i][j];
        cout << endl;
    }
    cout << endl;
}
void ArrayTest()
{
    int n = 1;
    cout << endl << endl;
    cout << __FUNCSIG__ << __LINE__ << endl;
    int a1[3] = { n++,n++,n++ };
    int a2[] = { n++,n++,n++ };
    int b1[2][3] = { {n++,n++,n++}, {n++,n++,n++} };
    int b2[][3] = { {n++,n++,n++}, {n++,n++,n++} };

    cout << "typeid(a1) : " << typeid(a1).name() << endl;
    cout << "typeid(a2) : " << typeid(a2).name() << endl;
    cout << "typeid(b1) : " << typeid(b1).name() << endl;
    cout << "typeid(b2) : " << typeid(b2).name() << endl;
    cout << endl << endl;

    int c1[1][3] = { 0, };
    //Print(a1);
    Print(b1);
    Print(c1);
    Print2(c1);

    Print3(b1);


    cout << "-----------------" << endl;
    cout << sizeof(a1) << endl;
    cout << typeid(a1).name() << endl;
    cout << sizeof(b2) << endl;
    cout << typeid(b2).name() << endl;

    for (int i = 0; i < 4; ++i)
    {
        a1[i] = i + 10;
    }
}

void PointerTest()
{
    cout << endl << endl;
    cout << __FUNCSIG__ << __LINE__ << endl;
    int a[] = { 1,2,3 };
    int* p1 = a;
    int* p2 = new int[3]{ 4,5,6 };

    cout << *p1 << endl;
    cout << p1[1] << endl;
    cout << *(p2+1) << endl;
    cout << p2[1] << endl;

    delete[] p2;
}


void PointerTest2()
{
    cout << endl << endl;
    cout << __FUNCSIG__ << __LINE__ << endl;

    int** pp = new int* [3];
    for (int i = 0; i < 3; ++i)
        pp[i] = new int[i + 1];

    for (int i = 0; i < 3; ++i)
        for (int j = 0; j <= i; ++j)
            pp[i][j] = i+1;

    for (int i = 0; i < 3; ++i)
    {
        for (int j = 0; j <= i; ++j)
            cout << pp[i][j];
        cout << endl;
    }

    for (int i = 0; i < 3; ++i)
        delete[] pp[i];
    delete[] pp;
}

void TestAll()
{
    ArrayTest();
    //PointerTest();
    //PointerTest2();
}

int* create(int n)
{
    return new int[n];
}

//int main()
//{
//    std::cout << "Hello World!\n";
//    TestAll();
//}

//*/

#include <iostream>
using namespace std;
void main()
{
    int a[] = { 1,2,3,4 };
    int b[] = { 10,20,30,40 };
    int c[] = { 1,2,3 };
    int* p = a;

    p = a;
    *(p + 1) = 10;
    *(a + 1) = 60;

    cout << (*a == *p) << endl;
    cout << (a != p) << endl;
    cout << (a[0] == *p) << endl;
    cout << (*(a+1) == p[1]) << endl;

    //cout << "1 : " << a[0] << endl;
    //cout << "2 : " << typeid(a).name() << endl;
    //cout << "3 : " << typeid(b).name() << endl;
    //cout << "4 : " << typeid(c).name() << endl;
    //cout << "5 : " << typeid(p).name() << endl;
    cout << "6 : " << (typeid(a) == typeid(b)) << endl;
    cout << "7 : " << (typeid(a) == typeid(c)) << endl;
    cout << "7 : " << (typeid(b) == typeid(c)) << endl;
    cout << "8 : " << (typeid(a) == typeid(p)) << endl;
}