#include <iostream>
#include <string>

//1. 예시: 게임 캐릭터의 상태 관리
//  게임 캐릭터가 “일반”, “무적”, “스턴” 등 다양한 상태에 따라
//  이동, 공격, 데미지 처리 방식이 달라져야 할 때
class CharacterState {
public:
    virtual void move() = 0;
    virtual void attack() = 0;
    virtual void takeDamage(int dmg) = 0;
    virtual ~CharacterState() = default;
};

class NormalState : public CharacterState {
public:
    void move() override { std::cout << "보통 속도로 이동\n"; }
    void attack() override { std::cout << "일반 공격\n"; }
    void takeDamage(int dmg) override { std::cout << dmg << " 데미지!\n"; }
};

class InvincibleState : public CharacterState {
public:
    void move() override { std::cout << "빛의 속도로 이동\n"; }
    void attack() override { std::cout << "무적 공격\n"; }
    void takeDamage(int) override { std::cout << "데미지 없음(무적)\n"; }
};

class StunnedState : public CharacterState {
public:
    void move() override { std::cout << "움직일 수 없음(스턴)\n"; }
    void attack() override { std::cout << "공격 불가(스턴)\n"; }
    void takeDamage(int dmg) override { std::cout << dmg << " 데미지!\n"; }
};

class Character {
    CharacterState* state;
public:
    Character(CharacterState* s) : state(s) {}
    void setState(CharacterState* s) { state = s; }
    void move() { state->move(); }
    void attack() { state->attack(); }
    void takeDamage(int dmg) { state->takeDamage(dmg); }
};

// 사용 예시
void example_game_character() {
    NormalState normal;
    InvincibleState invincible;
    StunnedState stunned;
    Character hero(&normal);

    hero.move();         // 보통 속도로 이동
    hero.attack();       // 일반 공격
    hero.takeDamage(10); // 10 데미지!

    hero.setState(&invincible);
    hero.takeDamage(10); // 데미지 없음(무적)

    hero.setState(&stunned);
    hero.move();         // 움직일 수 없음(스턴)
}

//이럴 때 State 패턴이 유용:
//  상태별로 행동이 완전히 달라질 때
//  if/else/switch문이 많아질 때
//  상태 전환이 자주 일어날 때



//2. 예시: UI 버튼의 상태 (Observer, State 결합)
//  버튼이 “활성”, “비활성”, “로딩 중” 등 상태에 따라
//  클릭 동작, 표시, 알림 방식이 달라져야 할 때
//  (Observer 패턴과 결합: 상태 변화 시 UI에 알림)
class ButtonState {
public:
    virtual void onClick() = 0;
    virtual std::string getLabel() = 0;
    virtual ~ButtonState() = default;
};

class EnabledState : public ButtonState {
public:
    void onClick() override { std::cout << "버튼 클릭됨!\n"; }
    std::string getLabel() override { return "확인"; }
};

class DisabledState : public ButtonState {
public:
    void onClick() override { std::cout << "버튼 비활성화됨\n"; }
    std::string getLabel() override { return "비활성"; }
};

class LoadingState : public ButtonState {
public:
    void onClick() override { std::cout << "로딩 중...\n"; }
    std::string getLabel() override { return "로딩..."; }
};

class Button {
    ButtonState* state;
public:
    Button(ButtonState* s) : state(s) {}
    void setState(ButtonState* s) { state = s; /* Observer에게 알림 가능 */ }
    void click() { state->onClick(); }
    void show() { std::cout << "버튼: " << state->getLabel() << std::endl; }
};
//이럴 때 State 패턴이 유용:
//  UI 컴포넌트의 상태별 동작/표시가 다를 때
//  상태 변화에 따라 Observer(구독자)에게 알림이 필요할 때


//3. 예시: 네트워크 연결 상태 (State + Singleton + Strategy 결합)
//  네트워크 연결이 “연결됨”, “연결 끊김”, “재연결 중” 등 상태에 따라
//  데이터 전송 방식, 재시도 전략이 달라져야 할 때
//  (State 객체를 싱글턴으로, 전송 전략은 Strategy로 분리)
class NetworkState {
public:
    virtual void sendData(const std::string& data) = 0;
    virtual ~NetworkState() = default;
};

class ConnectedState : public NetworkState {
public:
    static ConnectedState& instance() {
        static ConnectedState s;
        return s;
    }
    void sendData(const std::string& data) override {
        std::cout << "데이터 전송: " << data << std::endl;
    }
};

class DisconnectedState : public NetworkState {
public:
    static DisconnectedState& instance() {
        static DisconnectedState s;
        return s;
    }
    void sendData(const std::string&) override {
        std::cout << "연결 안 됨: 전송 실패\n";
    }
};

class NetworkConnection {
    NetworkState* state;
public:
    NetworkConnection() : state(&DisconnectedState::instance()) {}
    void setState(NetworkState* s) { state = s; }
    void send(const std::string& data) { state->sendData(data); }
};