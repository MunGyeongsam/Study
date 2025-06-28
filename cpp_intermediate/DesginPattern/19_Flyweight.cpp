// 1. 게임 배경(나무, 돌, 풀 등) 오브젝트 플라이웨이트 예제
#include <iostream>
#include <unordered_map>
#include <string>

class TreeType {
    std::string name, texture;
public:
    TreeType(const std::string& n, const std::string& t) : name(n), texture(t) {}
    void draw(int x, int y) {
        std::cout << "나무(" << name << ", " << texture << ") 위치: (" << x << "," << y << ")\n";
    }
};

class TreeFactory {
    std::unordered_map<std::string, TreeType*> pool;
public:
    ~TreeFactory() { for (auto& p : pool) delete p.second; }
    TreeType* get(const std::string& name, const std::string& texture) {
        std::string key = name + "_" + texture;
        if (pool.count(key) == 0) pool[key] = new TreeType(name, texture);
        return pool[key];
    }
};

class Tree {
    int x, y;
    TreeType* type;
public:
    Tree(int x, int y, TreeType* t) : x(x), y(y), type(t) {}
    void draw() { type->draw(x, y); }
};

void flyweightTreeExample() {
    TreeFactory factory;
    Tree t1(1, 2, factory.get("소나무", "green.png"));
    Tree t2(3, 4, factory.get("소나무", "green.png"));
    Tree t3(5, 6, factory.get("참나무", "brown.png"));
    t1.draw();
    t2.draw();
    t3.draw();
}

// 2. 지도 마커 아이콘 플라이웨이트 예제
#include <iostream>
#include <unordered_map>
#include <string>

class Icon {
    std::string imagePath;
public:
    Icon(const std::string& path) : imagePath(path) {}
    void draw(int x, int y) {
        std::cout << "아이콘(" << imagePath << ") 위치: (" << x << "," << y << ")\n";
    }
};

class IconFactory {
    std::unordered_map<std::string, Icon*> pool;
public:
    ~IconFactory() { for (auto& p : pool) delete p.second; }
    Icon* get(const std::string& path) {
        if (pool.count(path) == 0) pool[path] = new Icon(path);
        return pool[path];
    }
};

class Marker {
    int x, y;
    Icon* icon;
public:
    Marker(int x, int y, Icon* i) : x(x), y(y), icon(i) {}
    void draw() { icon->draw(x, y); }
};

void flyweightMarkerExample() {
    IconFactory factory;
    Marker m1(10, 20, factory.get("cafe.png"));
    Marker m2(30, 40, factory.get("cafe.png"));
    Marker m3(50, 60, factory.get("gas.png"));
    m1.draw();
    m2.draw();
    m3.draw();
}


// 3. 그래픽 에디터 도형 속성 플라이웨이트 예제
#include <iostream>
#include <unordered_map>
#include <string>

class Brush {
    std::string color;
public:
    Brush(const std::string& c) : color(c) {}
    void use(const std::string& shape) {
        std::cout << shape << "에 브러시(" << color << ") 사용\n";
    }
};

class BrushFactory {
    std::unordered_map<std::string, Brush*> pool;
public:
    ~BrushFactory() { for (auto& p : pool) delete p.second; }
    Brush* get(const std::string& color) {
        if (pool.count(color) == 0) pool[color] = new Brush(color);
        return pool[color];
    }
};

void flyweightBrushExample() {
    BrushFactory factory;
    Brush* red = factory.get("red");
    Brush* blue = factory.get("blue");
    Brush* red2 = factory.get("red");
    red->use("Circle");
    blue->use("Rectangle");
    red2->use("Triangle");
}


// 4. 텍스트 에디터 문자 플라이웨이트 예제
#include <iostream>
#include <unordered_map>
#include <string>

class Character {
    char symbol;
public:
    Character(char c) : symbol(c) {}
    void display(int row, int col, const std::string& font) {
        std::cout << "문자: " << symbol << " 위치: (" << row << "," << col << ") 폰트: " << font << std::endl;
    }
};

class CharacterFactory {
    std::unordered_map<char, Character*> pool;
public:
    ~CharacterFactory() { for (auto& p : pool) delete p.second; }
    Character* get(char c) {
        if (pool.count(c) == 0) pool[c] = new Character(c);
        return pool[c];
    }
};

void flyweightCharExample() {
    CharacterFactory factory;
    Character* a1 = factory.get('A');
    Character* a2 = factory.get('A');
    Character* b = factory.get('B');
    a1->display(0, 0, "Arial");
    a2->display(0, 1, "Arial");
    b->display(1, 0, "Times");
}

// 플라이웨이트 = 캐싱 + 불변성 + 외부 상태 분리
// 캐싱 = 단순 저장/재사용 (불변성/외부 상태 분리 필요 없음)
//
// 플라이웨이트 패턴은 “불변 공유 객체”와 “외부 상태 분리”가 핵심입니다.
// 캐싱은 단순히 객체/데이터를 저장해 재사용하는 기법입니다.
// 플라이웨이트 구현에 캐싱 기법이 활용될 수 있지만,
// 불변성/외부 상태 분리가 없다면 플라이웨이트 패턴이라고 할 수 없습니다.
//
// 플라이웨이트 예제(4, 5번)
// 공유 객체(브러시)는 불변
// 외부 상태(도형 이름, 위치 등)는 객체에 저장하거나, 메서드 인자로 전달
// 의도적으로 “공유”와 “분리”를 설계
// 객체 수가 많아질 때 메모리 절약 효과가 큼