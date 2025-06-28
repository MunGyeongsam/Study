// 1. 도형(Shape) 집합에 다양한 연산 추가
class ShapeVisitor;
class Shape {
public:
    virtual ~Shape() = default;
    virtual void accept(ShapeVisitor* v) = 0;
};
class Circle;
class Rectangle;
class ShapeVisitor {
public:
    virtual void visit(Circle* c) = 0;
    virtual void visit(Rectangle* r) = 0;
};
class Circle : public Shape {
public:
    void accept(ShapeVisitor* v) override { v->visit(this); }
};
class Rectangle : public Shape {
public:
    void accept(ShapeVisitor* v) override { v->visit(this); }
};
class DrawVisitor : public ShapeVisitor {
public:
    void visit(Circle*) override { std::cout << "원을 그립니다.\n"; }
    void visit(Rectangle*) override { std::cout << "사각형을 그립니다.\n"; }
};
void visitorShapeExample() {
    std::vector<Shape*> shapes = { new Circle(), new Rectangle() };
    DrawVisitor draw;
    for (auto s : shapes) s->accept(&draw);
    for (auto s : shapes) delete s;
}

// 2. 파일 시스템(Composite 구조) + Visitor
class FileVisitor;
class FileSystemNode {
public:
    virtual ~FileSystemNode() = default;
    virtual void accept(FileVisitor* v) = 0;
};
class File;
class Directory;
class FileVisitor {
public:
    virtual void visit(File* f) = 0;
    virtual void visit(Directory* d) = 0;
};
class File : public FileSystemNode {
    std::string name;
public:
    File(const std::string& n) : name(n) {}
    void accept(FileVisitor* v) override { v->visit(this); }
    std::string getName() const { return name; }
};
class Directory : public FileSystemNode {
    std::string name;
    std::vector<FileSystemNode*> children;
public:
    Directory(const std::string& n) : name(n) {}
    void add(FileSystemNode* node) { children.push_back(node); }
    void accept(FileVisitor* v) override { v->visit(this); }
    std::vector<FileSystemNode*>& getChildren() { return children; }
    std::string getName() const { return name; }
};
class PrintVisitor : public FileVisitor {
public:
    void visit(File* f) override { std::cout << "파일: " << f->getName() << std::endl; }
    void visit(Directory* d) override {
        std::cout << "디렉토리: " << d->getName() << std::endl;
        for (auto c : d->getChildren()) c->accept(this);
    }
};
void visitorFileSystemExample() {
    Directory* root = new Directory("root");
    root->add(new File("a.txt"));
    Directory* sub = new Directory("sub");
    sub->add(new File("b.txt"));
    root->add(sub);
    PrintVisitor printer;
    root->accept(&printer);
    // 메모리 해제 생략
}

// 3. 수식 트리(Interpreter/Composite)에서 Visitor로 평가
class ExprVisitor;
class Expr {
public:
    virtual ~Expr() = default;
    virtual void accept(ExprVisitor* v) = 0;
};
class Number;
class Add;
class ExprVisitor {
public:
    virtual void visit(Number* n) = 0;
    virtual void visit(Add* a) = 0;
};
class Number : public Expr {
    int value;
public:
    Number(int v) : value(v) {}
    int getValue() const { return value; }
    void accept(ExprVisitor* v) override { v->visit(this); }
};
class Add : public Expr {
    Expr* left; Expr* right;
public:
    Add(Expr* l, Expr* r) : left(l), right(r) {}
    Expr* getLeft() const { return left; }
    Expr* getRight() const { return right; }
    void accept(ExprVisitor* v) override { v->visit(this); }
};
class EvalVisitor : public ExprVisitor {
    int result;
public:
    void visit(Number* n) override { result = n->getValue(); }
    void visit(Add* a) override {
        a->getLeft()->accept(this);
        int l = result;
        a->getRight()->accept(this);
        int r = result;
        result = l + r;
    }
    int getResult() const { return result; }
};
void visitorExprExample() {
    Expr* expr = new Add(new Number(3), new Number(4));
    EvalVisitor eval;
    expr->accept(&eval);
    std::cout << "결과: " << eval.getResult() << std::endl; // 7
    delete expr;
}

// 4. HTML/XML 트리에서 Visitor로 렌더링/검증
class NodeVisitor;
class Node {
public:
    virtual ~Node() = default;
    virtual void accept(NodeVisitor* v) = 0;
};
class TextNode;
class ElementNode;
class NodeVisitor {
public:
    virtual void visit(TextNode* n) = 0;
    virtual void visit(ElementNode* n) = 0;
};
class TextNode : public Node {
    std::string text;
public:
    TextNode(const std::string& t) : text(t) {}
    std::string getText() const { return text; }
    void accept(NodeVisitor* v) override { v->visit(this); }
};
class ElementNode : public Node {
    std::string tag;
    std::vector<Node*> children;
public:
    ElementNode(const std::string& t) : tag(t) {}
    void add(Node* n) { children.push_back(n); }
    std::string getTag() const { return tag; }
    std::vector<Node*>& getChildren() { return children; }
    void accept(NodeVisitor* v) override { v->visit(this); }
};
class RenderVisitor : public NodeVisitor {
public:
    void visit(TextNode* n) override { std::cout << n->getText(); }
    void visit(ElementNode* n) override {
        std::cout << "<" << n->getTag() << ">";
        for (auto c : n->getChildren()) c->accept(this);
        std::cout << "</" << n->getTag() << ">";
    }
};
void visitorHtmlExample() {
    ElementNode* root = new ElementNode("p");
    root->add(new TextNode("Hello, "));
    root->add(new ElementNode("b"));
    root->getChildren()[1]->add(new TextNode("world!"));
    RenderVisitor render;
    root->accept(&render); // <p>Hello, <b>world!</b></p>
    // 메모리 해제 생략
}

// 5. 금융 상품(Composite)에서 Visitor로 수익률/위험 평가
class ProductVisitor;
class Product {
public:
    virtual ~Product() = default;
    virtual void accept(ProductVisitor* v) = 0;
};
class Stock;
class Portfolio;
class ProductVisitor {
public:
    virtual void visit(Stock* s) = 0;
    virtual void visit(Portfolio* p) = 0;
};
class Stock : public Product {
    double price;
public:
    Stock(double p) : price(p) {}
    double getPrice() const { return price; }
    void accept(ProductVisitor* v) override { v->visit(this); }
};
class Portfolio : public Product {
    std::vector<Product*> items;
public:
    void add(Product* p) { items.push_back(p); }
    std::vector<Product*>& getItems() { return items; }
    void accept(ProductVisitor* v) override { v->visit(this); }
};
class ValueVisitor : public ProductVisitor {
    double total = 0;
public:
    void visit(Stock* s) override { total += s->getPrice(); }
    void visit(Portfolio* p) override {
        for (auto item : p->getItems()) item->accept(this);
    }
    double getTotal() const { return total; }
};
void visitorFinanceExample() {
    Portfolio* pf = new Portfolio();
    pf->add(new Stock(100));
    pf->add(new Stock(200));
    Portfolio* sub = new Portfolio();
    sub->add(new Stock(50));
    pf->add(sub);
    ValueVisitor value;
    pf->accept(&value);
    std::cout << "총 자산: " << value.getTotal() << std::endl; // 350
    // 메모리 해제 생략
}