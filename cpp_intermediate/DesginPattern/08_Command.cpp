//1. 오디오 플레이어 제어 (Play, Stop, Pause)

#include <iostream>
#include <memory>
#include <vector>

// Receiver
class AudioPlayer {
public:
    void play() { std::cout << "Play music\n"; }
    void stop() { std::cout << "Stop music\n"; }
    void pause() { std::cout << "Pause music\n"; }
};

// Command 인터페이스
class Command {
public:
    virtual void execute() = 0;
    virtual ~Command() = default;
};

// ConcreteCommand
class PlayCommand : public Command {
    AudioPlayer& player;
public:
    PlayCommand(AudioPlayer& p) : player(p) {}
    void execute() override { player.play(); }
};
class StopCommand : public Command {
    AudioPlayer& player;
public:
    StopCommand(AudioPlayer& p) : player(p) {}
    void execute() override { player.stop(); }
};
class PauseCommand : public Command {
    AudioPlayer& player;
public:
    PauseCommand(AudioPlayer& p) : player(p) {}
    void execute() override { player.pause(); }
};

// Invoker
class Remote {
    std::vector<std::unique_ptr<Command>> buttons;
public:
    void setCommand(std::unique_ptr<Command> cmd) {
        buttons.push_back(std::move(cmd));
    }
    void press(int idx) {
        if (idx < buttons.size()) buttons[idx]->execute();
    }
};

// 사용 예시
int test1() {
    AudioPlayer player;
    Remote remote;
    remote.setCommand(std::make_unique<PlayCommand>(player));
    remote.setCommand(std::make_unique<PauseCommand>(player));
    remote.setCommand(std::make_unique<StopCommand>(player));
    remote.press(0); // Play music
    remote.press(1); // Pause music
    remote.press(2); // Stop music
}



//2. 매크로 커맨드(여러 명령을 한 번에 실행)
#include <iostream>
#include <vector>
#include <memory>

// Command 인터페이스
class Command {
public:
    virtual void execute() = 0;
    virtual ~Command() = default;
};

// MacroCommand
class MacroCommand : public Command {
    std::vector<std::unique_ptr<Command>> commands;
public:
    void add(std::unique_ptr<Command> cmd) {
        commands.push_back(std::move(cmd));
    }
    void execute() override {
        for (auto& cmd : commands) cmd->execute();
    }
};

// 예시용 간단한 커맨드
class HelloCommand : public Command {
public:
    void execute() override { std::cout << "Hello "; }
};
class WorldCommand : public Command {
public:
    void execute() override { std::cout << "World!\n"; }
};

// 사용 예시
int test2() {
    MacroCommand macro;
    macro.add(std::make_unique<HelloCommand>());
    macro.add(std::make_unique<WorldCommand>());
    macro.execute(); // Hello World!
}


//3. 메뉴 시스템(Undo 포함)
#include <iostream>
#include <stack>
#include <memory>
#include <string>

// Receiver
class TextBuffer {
    std::string text;
public:
    void append(const std::string& s) { text += s; }
    void erase(size_t n) { text.erase(text.size() - n, n); }
    void show() { std::cout << text << std::endl; }
};

// Command 인터페이스
class Command {
public:
    virtual void execute() = 0;
    virtual void undo() = 0;
    virtual ~Command() = default;
};

// ConcreteCommand
class AppendText : public Command {
    TextBuffer& buf;
    std::string str;
public:
    AppendText(TextBuffer& b, const std::string& s) : buf(b), str(s) {}
    void execute() override { buf.append(str); }
    void undo() override { buf.erase(str.size()); }
};

// Invoker
class Menu {
    std::stack<std::unique_ptr<Command>> history;
public:
    void runCommand(std::unique_ptr<Command> cmd) {
        cmd->execute();
        history.push(std::move(cmd));
    }
    void undo() {
        if (!history.empty()) {
            history.top()->undo();
            history.pop();
        }
    }
};

// 사용 예시
int test3() {
    TextBuffer buf;
    Menu menu;
    menu.runCommand(std::make_unique<AppendText>(buf, "Hi "));
    menu.runCommand(std::make_unique<AppendText>(buf, "there!"));
    buf.show(); // Hi there!
    menu.undo();
    buf.show(); // Hi 
}