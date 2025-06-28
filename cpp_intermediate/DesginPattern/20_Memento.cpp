// 1. 텍스트 에디터의 Undo/Redo
#include <iostream>
#include <string>
#include <vector>

class TextMemento {
    std::string state;
public:
    TextMemento(const std::string& s) : state(s) {}
    std::string getState() const { return state; }
};

class TextEditor {
    std::string text;
public:
    void type(const std::string& words) { text += words; }
    void show() const { std::cout << "현재 텍스트: " << text << std::endl; }
    TextMemento save() const { return TextMemento(text); }
    void restore(const TextMemento& m) { text = m.getState(); }
};

void mementoTextEditorExample() {
    TextEditor editor;
    std::vector<TextMemento> history;
    editor.type("Hello ");
    history.push_back(editor.save());
    editor.type("World!");
    editor.show(); // Hello World!
    editor.restore(history.back());
    editor.show(); // Hello 
}

// 2. 게임 캐릭터의 상태 저장/복원
#include <iostream>
#include <vector>

class PlayerMemento {
    int hp, x, y;
public:
    PlayerMemento(int h, int x, int y) : hp(h), x(x), y(y) {}
    int getHp() const { return hp; }
    int getX() const { return x; }
    int getY() const { return y; }
};

class Player {
    int hp, x, y;
public:
    Player(int h, int x, int y) : hp(h), x(x), y(y) {}
    void move(int dx, int dy) { x += dx; y += dy; }
    void damage(int d) { hp -= d; }
    void show() const { std::cout << "HP:" << hp << " 위치:(" << x << "," << y << ")\n"; }
    PlayerMemento save() const { return PlayerMemento(hp, x, y); }
    void restore(const PlayerMemento& m) { hp = m.getHp(); x = m.getX(); y = m.getY(); }
};

void mementoPlayerExample() {
    Player p(100, 0, 0);
    std::vector<PlayerMemento> history;
    history.push_back(p.save());
    p.move(5, 0);
    p.damage(10);
    p.show(); // HP:90 위치:(5,0)
    p.restore(history.back());
    p.show(); // HP:100 위치:(0,0)
}


// 3. 그림판의 도형 상태(Undo/Redo)
#include <iostream>
#include <vector>

class ShapeMemento {
    int x, y;
public:
    ShapeMemento(int x, int y) : x(x), y(y) {}
    int getX() const { return x; }
    int getY() const { return y; }
};

class Shape {
    int x, y;
public:
    Shape(int x, int y) : x(x), y(y) {}
    void move(int dx, int dy) { x += dx; y += dy; }
    void show() const { std::cout << "Shape 위치: (" << x << "," << y << ")\n"; }
    ShapeMemento save() const { return ShapeMemento(x, y); }
    void restore(const ShapeMemento& m) { x = m.getX(); y = m.getY(); }
};

void mementoShapeExample() {
    Shape s(0, 0);
    std::vector<ShapeMemento> history;
    history.push_back(s.save());
    s.move(10, 20);
    s.show(); // (10,20)
    s.restore(history.back());
    s.show(); // (0,0)
}

// 4. 커맨드(Command) 패턴과 함께 사용하는 Undo/Redo
#include <iostream>
#include <vector>
#include <stack>
#include <string>

// Memento
class EditorMemento {
    std::string text;
public:
    EditorMemento(const std::string& t) : text(t) {}
    std::string getText() const { return text; }
};

// Originator
class Editor {
    std::string text;
public:
    void setText(const std::string& t) { text = t; }
    std::string getText() const { return text; }
    EditorMemento save() const { return EditorMemento(text); }
    void restore(const EditorMemento& m) { text = m.getText(); }
};

// Command
class Command {
public:
    virtual void execute() = 0;
    virtual void undo() = 0;
    virtual ~Command() = default;
};

class WriteCommand : public Command {
    Editor& editor;
    std::string newText;
    EditorMemento backup;
public:
    WriteCommand(Editor& e, const std::string& t)
        : editor(e), newText(t), backup(e.save()) {}
    void execute() override { editor.setText(newText); }
    void undo() override { editor.restore(backup); }
};

void mementoCommandExample() {
    Editor editor;
    std::stack<Command*> history;

    Command* cmd1 = new WriteCommand(editor, "First line");
    cmd1->execute();
    history.push(cmd1);

    Command* cmd2 = new WriteCommand(editor, "Second line");
    cmd2->execute();
    history.push(cmd2);

    std::cout << "현재: " << editor.getText() << std::endl; // Second line

    // Undo
    history.top()->undo(); history.pop();
    std::cout << "Undo 후: " << editor.getText() << std::endl; // First line

    // Undo
    history.top()->undo(); history.pop();
    std::cout << "Undo 후: " << editor.getText() << std::endl; // (빈 문자열)

    delete cmd1;
    delete cmd2;
}