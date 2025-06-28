//1. 항공 교통 관제 시스템
#include <iostream>
#include <vector>
#include <string>

class Airplane;

class ControlTower {
public:
    virtual void requestLanding(Airplane* plane) = 0;
    virtual void notifyLanded(Airplane* plane) = 0;
    virtual ~ControlTower() = default;
};

class Airplane {
    std::string name;
    ControlTower* tower;
public:
    Airplane(const std::string& n, ControlTower* t) : name(n), tower(t) {}
    void requestLanding() {
        std::cout << name << ": 착륙 요청\n";
        tower->requestLanding(this);
    }
    void land() {
        std::cout << name << ": 착륙 완료\n";
        tower->notifyLanded(this);
    }
    std::string getName() const { return name; }
};

class ConcreteControlTower : public ControlTower {
    std::vector<Airplane*> queue;
    bool runwayFree = true;
public:
    void requestLanding(Airplane* plane) override {
        if (runwayFree) {
            std::cout << "관제탑: " << plane->getName() << " 착륙 허가\n";
            runwayFree = false;
            plane->land();
        } else {
            std::cout << "관제탑: " << plane->getName() << " 대기\n";
            queue.push_back(plane);
        }
    }
    void notifyLanded(Airplane* plane) override {
        std::cout << "관제탑: " << plane->getName() << " 착륙 확인\n";
        runwayFree = true;
        if (!queue.empty()) {
            Airplane* next = queue.front();
            queue.erase(queue.begin());
            requestLanding(next);
        }
    }
};

int main_1() {
    ConcreteControlTower tower;
    Airplane a1("비행기1", &tower);
    Airplane a2("비행기2", &tower);
    Airplane a3("비행기3", &tower);

    a1.requestLanding(); // 비행기1 착륙
    a2.requestLanding(); // 비행기2 대기
    a3.requestLanding(); // 비행기3 대기

    // 실제로는 착륙 완료가 자동 호출되지만, 예시에서는 즉시 land() 호출
    // 실제 상황에서는 land()가 비동기로 호출될 수 있음
    // 여기서는 흐름을 보여주기 위해 land()가 내부에서 바로 호출됨

    return 0;
}



// 2. 채팅방(채팅 서버) Mediator 패턴 예제

#include <iostream>
#include <string>
#include <vector>

class User;

class ChatRoom {
public:
    virtual void sendMessage(const std::string& msg, User* sender) = 0;
    virtual void addUser(User* user) = 0;
    virtual ~ChatRoom() = default;
};

class User {
    std::string name;
    ChatRoom* room;
public:
    User(const std::string& n) : name(n), room(nullptr) {}
    void setRoom(ChatRoom* r) { room = r; }
    void send(const std::string& msg) {
        if (room) room->sendMessage(msg, this);
    }
    void receive(const std::string& msg) {
        std::cout << name << " received: " << msg << std::endl;
    }
    std::string getName() const { return name; }
};

class ConcreteChatRoom : public ChatRoom {
    std::vector<User*> users;
public:
    void addUser(User* user) override {
        users.push_back(user);
        user->setRoom(this);
    }
    void sendMessage(const std::string& msg, User* sender) override {
        for (auto u : users) {
            if (u != sender) {
                u->receive(sender->getName() + ": " + msg);
            }
        }
    }
};

int main_2() {
    ConcreteChatRoom room;
    User alice("Alice"), bob("Bob"), charlie("Charlie");
    room.addUser(&alice);
    room.addUser(&bob);
    room.addUser(&charlie);

    alice.send("안녕하세요!"); // Bob, Charlie가 메시지 받음
    bob.send("반가워요!");   // Alice, Charlie가 메시지 받음

    return 0;
}



// 3. 게임 내 유닛 간 상호작용 (RTS 게임) Mediator 패턴 예제

#include <iostream>
#include <vector>
#include <string>

class Unit;
class GameMediator {
public:
    virtual void notifyAttack(Unit* attacker, Unit* target) = 0;
    virtual void addUnit(Unit* unit) = 0;
    virtual ~GameMediator() = default;
};

class Unit {
    std::string name;
    int hp;
    GameMediator* mediator;
public:
    Unit(const std::string& n, int h, GameMediator* m) : name(n), hp(h), mediator(m) {}
    void attack(Unit* target) {
        std::cout << name << "이(가) " << target->getName() << "을(를) 공격합니다!\n";
        mediator->notifyAttack(this, target);
    }
    void takeDamage(int dmg) {
        hp -= dmg;
        std::cout << name << "이(가) " << dmg << " 데미지를 입음 (HP: " << hp << ")\n";
        if (hp <= 0) std::cout << name << "이(가) 쓰러졌습니다!\n";
    }
    std::string getName() const { return name; }
};

class ConcreteGameMediator : public GameMediator {
    std::vector<Unit*> units;
public:
    void addUnit(Unit* unit) override {
        units.push_back(unit);
    }
    void notifyAttack(Unit* attacker, Unit* target) override {
        // 예시: 공격자는 항상 10 데미지
        if (target) target->takeDamage(10);
        // 추가로, 모든 유닛에게 알림(예: 로그, 반응 등)
        for (auto u : units) {
            if (u != attacker && u != target)
                std::cout << u->getName() << "이(가) 공격 상황을 관찰함\n";
        }
    }
};

int main_3() {
    ConcreteGameMediator mediator;
    Unit marine("해병", 30, &mediator);
    Unit zergling("저글링", 20, &mediator);
    Unit medic("메딕", 25, &mediator);

    mediator.addUnit(&marine);
    mediator.addUnit(&zergling);
    mediator.addUnit(&medic);

    marine.attack(&zergling); // 해병이 저글링 공격
    zergling.attack(&marine); // 저글링이 해병 공격

    return 0;
}



// 4. 스마트홈 허브 Mediator 패턴 예제

#include <iostream>
#include <string>
#include <vector>

class SmartDevice;

class SmartHomeHub {
public:
    virtual void notify(SmartDevice* sender, const std::string& event) = 0;
    virtual void addDevice(SmartDevice* device) = 0;
    virtual ~SmartHomeHub() = default;
};

class SmartDevice {
protected:
    SmartHomeHub* hub;
    std::string name;
public:
    SmartDevice(const std::string& n, SmartHomeHub* h) : hub(h), name(n) {}
    virtual void triggerEvent(const std::string& event) {
        std::cout << name << ": 이벤트 발생 - " << event << std::endl;
        if (hub) hub->notify(this, event);
    }
    std::string getName() const { return name; }
    virtual ~SmartDevice() = default;
};

class Light : public SmartDevice {
public:
    Light(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void turnOn() { std::cout << name << ": 불이 켜졌습니다.\n"; }
    void turnOff() { std::cout << name << ": 불이 꺼졌습니다.\n"; }
};

class DoorLock : public SmartDevice {
public:
    DoorLock(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void lock() { std::cout << name << ": 문이 잠겼습니다.\n"; }
    void unlock() { std::cout << name << ": 문이 열렸습니다.\n"; }
};

class AirConditioner : public SmartDevice {
public:
    AirConditioner(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void turnOn() { std::cout << name << ": 에어컨이 켜졌습니다.\n"; }
    void turnOff() { std::cout << name << ": 에어컨이 꺼졌습니다.\n"; }
};

class Window : public SmartDevice {
public:
    Window(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void open() { std::cout << name << ": 창문이 열렸습니다.\n"; }
    void close() { std::cout << name << ": 창문이 닫혔습니다.\n"; }
};

class Heater : public SmartDevice {
public:
    Heater(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void turnOn() { std::cout << name << ": 온풍기가 켜졌습니다.\n"; }
    void turnOff() { std::cout << name << ": 온풍기가 꺼졌습니다.\n"; }
};

class TV : public SmartDevice {
public:
    TV(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void turnOn() { std::cout << name << ": TV가 켜졌습니다.\n"; }
    void turnOff() { std::cout << name << ": TV가 꺼졌습니다.\n"; }
};

class Radio : public SmartDevice {
public:
    Radio(const std::string& n, SmartHomeHub* h) : SmartDevice(n, h) {}
    void turnOn() { std::cout << name << ": 라디오가 켜졌습니다.\n"; }
    void turnOff() { std::cout << name << ": 라디오가 꺼졌습니다.\n"; }
};

class ConcreteSmartHomeHub : public SmartHomeHub {
    std::vector<SmartDevice*> devices;
public:
    void addDevice(SmartDevice* device) override {
        devices.push_back(device);
    }
    void notify(SmartDevice* sender, const std::string& event) override {
        // 예시: 현관문이 열리면 조명, 에어컨, TV, 라디오 켜짐
        if (event == "door unlocked") {
            for (auto d : devices) {
                if (d != sender) {
                    if (auto light = dynamic_cast<Light*>(d)) light->turnOn();
                    if (auto ac = dynamic_cast<AirConditioner*>(d)) ac->turnOn();
                    if (auto tv = dynamic_cast<TV*>(d)) tv->turnOn();
                    if (auto radio = dynamic_cast<Radio*>(d)) radio->turnOn();
                }
            }
        }
        // 예시: 현관문이 잠기면 조명, 에어컨, TV, 라디오 꺼짐
        if (event == "door locked") {
            for (auto d : devices) {
                if (d != sender) {
                    if (auto light = dynamic_cast<Light*>(d)) light->turnOff();
                    if (auto ac = dynamic_cast<AirConditioner*>(d)) ac->turnOff();
                    if (auto tv = dynamic_cast<TV*>(d)) tv->turnOff();
                    if (auto radio = dynamic_cast<Radio*>(d)) radio->turnOff();
                }
            }
        }
        // 예시: 창문이 열리면 에어컨, 온풍기 꺼짐
        if (event == "window opened") {
            for (auto d : devices) {
                if (auto ac = dynamic_cast<AirConditioner*>(d)) ac->turnOff();
                if (auto heater = dynamic_cast<Heater*>(d)) heater->turnOff();
            }
        }
        // 예시: 창문이 닫히면 에어컨, 온풍기 켜짐
        if (event == "window closed") {
            for (auto d : devices) {
                if (auto ac = dynamic_cast<AirConditioner*>(d)) ac->turnOn();
                if (auto heater = dynamic_cast<Heater*>(d)) heater->turnOn();
            }
        }
    }
};

int main_4() {
    ConcreteSmartHomeHub hub;
    Light light("거실 조명", &hub);
    DoorLock door("현관문", &hub);
    AirConditioner ac("에어컨", &hub);
    Window window("거실 창문", &hub);
    Heater heater("온풍기", &hub);
    TV tv("TV", &hub);
    Radio radio("라디오", &hub);

    hub.addDevice(&light);
    hub.addDevice(&door);
    hub.addDevice(&ac);
    hub.addDevice(&window);
    hub.addDevice(&heater);
    hub.addDevice(&tv);
    hub.addDevice(&radio);

    door.triggerEvent("door unlocked"); // 현관문 열림 → 조명, 에어컨, TV, 라디오 켜짐
    window.triggerEvent("window opened"); // 창문 열림 → 에어컨, 온풍기 꺼짐
    window.triggerEvent("window closed"); // 창문 닫힘 → 에어컨, 온풍기 켜짐
    door.triggerEvent("door locked");   // 현관문 잠김 → 조명, 에어컨, TV, 라디오 꺼짐

    return 0;
}