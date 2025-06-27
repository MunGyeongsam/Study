//1. 문자열 토큰 반복자
//  문자열을 공백이나 구분자로 나누어 하나씩 순회하는 반복자입니다.
#include <iostream>
#include <string>
#include <vector>
#include <sstream>

class StringTokenIterator {
    std::vector<std::string> tokens;
    size_t idx = 0;
public:
    StringTokenIterator(const std::string& str, char delim = ' ') {
        std::istringstream iss(str);
        std::string token;
        while (std::getline(iss, token, delim)) {
            tokens.push_back(token);
        }
    }
    bool hasNext() const { return idx < tokens.size(); }
    std::string next() { return tokens[idx++]; }
};

// 사용 예시
void example_string_token_iterator() {
    StringTokenIterator it("this is an iterator pattern example");
    while (it.hasNext()) {
        std::cout << it.next() << std::endl;
    }
}


//2. 2차원 행렬(Matrix) 반복자
//  행렬의 모든 원소를 순회하는 반복자입니다.
#include <iostream>
#include <vector>

class MatrixIterator {
    const std::vector<std::vector<int>>& mat;
    size_t row = 0, col = 0;
public:
    MatrixIterator(const std::vector<std::vector<int>>& m) : mat(m) {}
    bool hasNext() const {
        return row < mat.size() && col < mat[row].size();
    }
    int next() {
        int val = mat[row][col++];
        if (col >= mat[row].size()) {
            col = 0;
            ++row;
        }
        return val;
    }
};

// 사용 예시
void example_matrix_iterator() {
    std::vector<std::vector<int>> mat = {{1,2,3},{4,5},{6}};
    MatrixIterator it(mat);
    while (it.hasNext()) {
        std::cout << it.next() << " ";
    }
    std::cout << std::endl;
}



//3. 필터(Filter) 반복자 (Decorator 패턴 결합)
//  특정 조건을 만족하는 요소만 순회하는 반복자입니다.
#include <iostream>
#include <vector>
#include <functional>

class IntArrayIterator {
    const std::vector<int>& arr;
    size_t idx = 0;
public:
    IntArrayIterator(const std::vector<int>& a) : arr(a) {}
    bool hasNext() const { return idx < arr.size(); }
    int next() { return arr[idx++]; }
};

class FilterIterator {
    IntArrayIterator& base;
    std::function<bool(int)> pred;
    int nextVal;
public:
    FilterIterator(IntArrayIterator& it, std::function<bool(int)> p) : base(it), pred(p) {}
    bool hasNext() {
        while (base.hasNext()) {
            int val = base.next();
            if (pred(val)) {
                nextVal = val;
                return true;
            }
        }
        return false;
    }
    int next() {
        return nextVal;
    }
};

// 사용 예시
void example_filter_iterator() {
    std::vector<int> arr = {1,2,3,4,5,6};
    IntArrayIterator base(arr);
    FilterIterator evenIt(base, [](int x){ return x % 2 == 0; });
    while (evenIt.hasNext()) {
        std::cout << evenIt.next() << " ";
    }
    std::cout << std::endl;
}