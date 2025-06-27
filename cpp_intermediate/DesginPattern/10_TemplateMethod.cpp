//1. 게임 캐릭터의 턴 행동(공격 알고리즘)
#include <iostream>

// Template Method 패턴의 상위 클래스
class Character {
public:
    void takeTurn() { // 알고리즘의 뼈대
        move();
        attack();
        endTurn();
    }
    virtual ~Character() = default;
protected:
    virtual void move() = 0;    // 하위 클래스에서 구현
    virtual void attack() = 0;  // 하위 클래스에서 구현
    virtual void endTurn() { std::cout << "턴 종료\n"; } // 기본 구현
};

// 하위 클래스
class Warrior : public Character {
protected:
    void move() override { std::cout << "전사가 앞으로 이동\n"; }
    void attack() override { std::cout << "전사가 칼로 공격\n"; }
};

class Archer : public Character {
protected:
    void move() override { std::cout << "궁수가 뒤로 이동\n"; }
    void attack() override { std::cout << "궁수가 화살을 쏨\n"; }
};

// 사용 예시
int example1() {
    Warrior w;
    Archer a;
    w.takeTurn();
    a.takeTurn();
    return 0;
}


//2. 커피/차 만들기(음료 제조 알고리즘)
#include <iostream>

class CaffeineBeverage {
public:
    void prepareRecipe() { // 템플릿 메서드
        boilWater();
        brew();
        pourInCup();
        addCondiments();
    }
    virtual ~CaffeineBeverage() = default;
protected:
    void boilWater() { std::cout << "물을 끓인다\n"; }
    void pourInCup() { std::cout << "컵에 따른다\n"; }
    virtual void brew() = 0;           // 하위 클래스에서 구현
    virtual void addCondiments() = 0;  // 하위 클래스에서 구현
};

class Coffee : public CaffeineBeverage {
protected:
    void brew() override { std::cout << "커피를 내린다\n"; }
    void addCondiments() override { std::cout << "설탕과 우유를 넣는다\n"; }
};

class Tea : public CaffeineBeverage {
protected:
    void brew() override { std::cout << "찻잎을 우린다\n"; }
    void addCondiments() override { std::cout << "레몬을 넣는다\n"; }
};

// 사용 예시
int example2() {
    Coffee coffee;
    Tea tea;
    coffee.prepareRecipe();
    tea.prepareRecipe();
    return 0;
}


//3. 파일 처리(읽기-처리-쓰기 알고리즘)
#include <iostream>
#include <string>

class FileProcessor {
public:
    void processFile(const std::string& filename) { // 템플릿 메서드
        openFile(filename);
        readData();
        processData();
        writeResult();
        closeFile();
    }
    virtual ~FileProcessor() = default;
protected:
    virtual void openFile(const std::string& filename) { std::cout << filename << " 파일 오픈\n"; }
    virtual void readData() = 0;      // 하위 클래스에서 구현
    virtual void processData() = 0;   // 하위 클래스에서 구현
    virtual void writeResult() { std::cout << "결과 저장\n"; }
    virtual void closeFile() { std::cout << "파일 닫기\n"; }
};

class CsvFileProcessor : public FileProcessor {
protected:
    void readData() override { std::cout << "CSV 데이터 읽기\n"; }
    void processData() override { std::cout << "CSV 데이터 처리\n"; }
};

class JsonFileProcessor : public FileProcessor {
protected:
    void readData() override { std::cout << "JSON 데이터 읽기\n"; }
    void processData() override { std::cout << "JSON 데이터 처리\n"; }
};

// 사용 예시
int example3() {
    CsvFileProcessor csv;
    JsonFileProcessor json;
    csv.processFile("data.csv");
    json.processFile("data.json");
    return 0;
}