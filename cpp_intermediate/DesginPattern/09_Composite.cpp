//1. 그래픽 도형(Graphic) 트리
#include <iostream>
#include <vector>
#include <memory>

// Component
class Graphic {
public:
    virtual void draw() const = 0;
    virtual ~Graphic() = default;
};

// Leaf
class Circle : public Graphic {
public:
    void draw() const override { std::cout << "Draw Circle\n"; }
};
class Rectangle : public Graphic {
public:
    void draw() const override { std::cout << "Draw Rectangle\n"; }
};

// Composite
class Picture : public Graphic {
    std::vector<std::unique_ptr<Graphic>> children;
public:
    void add(std::unique_ptr<Graphic> g) { children.push_back(std::move(g)); }
    void draw() const override {
        std::cout << "Draw Picture:\n";
        for (const auto& child : children)
            child->draw();
    }
};

// 사용 예시
int example1() {
    Picture scene;
    scene.add(std::make_unique<Circle>());
    scene.add(std::make_unique<Rectangle>());
    auto group = std::make_unique<Picture>();
    group->add(std::make_unique<Circle>());
    scene.add(std::move(group));
    scene.draw();
    // 출력:
    // Draw Picture:
    // Draw Circle
    // Draw Rectangle
    // Draw Picture:
    // Draw Circle
    return 0;
}


//2. 조직도(Organization Chart)
#include <iostream>
#include <vector>
#include <memory>
#include <string>

// Component
class Employee {
public:
    virtual void show(int indent = 0) const = 0;
    virtual ~Employee() = default;
};

// Leaf
class Worker : public Employee {
    std::string name;
public:
    Worker(const std::string& n) : name(n) {}
    void show(int indent = 0) const override {
        std::cout << std::string(indent, ' ') << "- " << name << std::endl;
    }
};

// Composite
class Manager : public Employee {
    std::string name;
    std::vector<std::unique_ptr<Employee>> team;
public:
    Manager(const std::string& n) : name(n) {}
    void add(std::unique_ptr<Employee> e) { team.push_back(std::move(e)); }
    void show(int indent = 0) const override {
        std::cout << std::string(indent, ' ') << "+ " << name << std::endl;
        for (const auto& member : team)
            member->show(indent + 2);
    }
};

// 사용 예시
int example2() {
    auto ceo = std::make_unique<Manager>("CEO");
    auto devMgr = std::make_unique<Manager>("Dev Manager");
    devMgr->add(std::make_unique<Worker>("Alice"));
    devMgr->add(std::make_unique<Worker>("Bob"));
    ceo->add(std::move(devMgr));
    ceo->add(std::make_unique<Worker>("HR"));
    ceo->show();
    // 출력:
    // + CEO
    //   + Dev Manager
    //     - Alice
    //     - Bob
    //   - HR
    return 0;
}


//3. 수식(Expression) 트리
#include <iostream>
#include <memory>

// Component
class Expr {
public:
    virtual int eval() const = 0;
    virtual ~Expr() = default;
};

// Leaf
class Number : public Expr {
    int value;
public:
    Number(int v) : value(v) {}
    int eval() const override { return value; }
};

// Composite
class Add : public Expr {
    std::unique_ptr<Expr> left, right;
public:
    Add(std::unique_ptr<Expr> l, std::unique_ptr<Expr> r)
        : left(std::move(l)), right(std::move(r)) {}
    int eval() const override { return left->eval() + right->eval(); }
};
class Multiply : public Expr {
    std::unique_ptr<Expr> left, right;
public:
    Multiply(std::unique_ptr<Expr> l, std::unique_ptr<Expr> r)
        : left(std::move(l)), right(std::move(r)) {}
    int eval() const override { return left->eval() * right->eval(); }
};

// 사용 예시
int example3() {
    // (3 + 4) * 5
    auto expr = std::make_unique<Multiply>(
        std::make_unique<Add>(
            std::make_unique<Number>(3),
            std::make_unique<Number>(4)
        ),
        std::make_unique<Number>(5)
    );
    std::cout << expr->eval() << std::endl; // 35
    return 0;
}


//4. 간단한 파서 구현 (재귀 하향 파싱)
class Parser {
    const std::string& s;
    size_t pos = 0;

    void skip() {
        while (pos < s.size() && std::isspace(s[pos])) ++pos;
    }

    int parseNumber() {
        skip();
        int num = 0;
        while (pos < s.size() && std::isdigit(s[pos])) {
            num = num * 10 + (s[pos++] - '0');
        }
        return num;
    }

    std::unique_ptr<Expr> parseFactor() {
        skip();
        if (s[pos] == '(') {
            ++pos; // '('
            auto node = parseExpr();
            skip();
            if (s[pos] == ')') ++pos;
            return node;
        }
        return std::make_unique<Number>(parseNumber());
    }

    std::unique_ptr<Expr> parseTerm() {
        auto node = parseFactor();
        skip();
        while (pos < s.size() && s[pos] == '*') {
            ++pos;
            node = std::make_unique<Multiply>(std::move(node), parseFactor());
            skip();
        }
        return node;
    }

    std::unique_ptr<Expr> parseExpr() {
        auto node = parseTerm();
        skip();
        while (pos < s.size() && s[pos] == '+') {
            ++pos;
            node = std::make_unique<Add>(std::move(node), parseTerm());
            skip();
        }
        return node;
    }

public:
    Parser(const std::string& str) : s(str) {}
    std::unique_ptr<Expr> parse() { return parseExpr(); }
};

// 사용 예시
int example4() {
    std::string input = "(3 + 4) * 5 + 2";
    Parser parser(input);
    auto expr = parser.parse();
    std::cout << expr->eval() << std::endl; // 37
    return 0;
}