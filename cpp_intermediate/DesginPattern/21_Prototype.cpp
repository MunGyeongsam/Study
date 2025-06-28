
//---------------------------------------------------------------------
// Prototype 패턴은
// "복잡한 객체를 빠르게, 상태 그대로 여러 개 만들어야 할 때"
// 템플릿(원본) 객체를 복제해서 사용하면
// 코드가 단순해지고, 성능도 좋아집니다.
//---------------------------------------------------------------------

//  1. 게임에서 몬스터/유닛/아이템 복제
//  상황:
//  게임에서 다양한 몬스터, 유닛, 아이템을 빠르게 여러 개 생성해야 할 때
//  (예: 몬스터 스폰, 아이템 드롭 등)
class Monster {
public:
    virtual ~Monster() = default;
    virtual Monster* clone() const = 0;
    virtual void info() const = 0;
};
class Goblin : public Monster {
    int hp;
public:
    Goblin(int h) : hp(h) {}
    Monster* clone() const override { return new Goblin(*this); }
    void info() const override { std::cout << "Goblin (HP: " << hp << ")\n"; }
};
void prototypeMonsterExample() {
    Goblin prototype(100);
    Monster* m1 = prototype.clone();
    Monster* m2 = prototype.clone();
    m1->info(); m2->info();
    delete m1; delete m2;
}

//  2. 문서 편집기에서 도형/오브젝트 복제 (복사-붙여넣기)
//  상황:
//  사용자가 도형을 복사-붙여넣기 할 때,
//  원본과 동일한 속성을 가진 새 도형이 필요
class Shape {
public:
    virtual ~Shape() = default;
    virtual Shape* clone() const = 0;
    virtual void draw() const = 0;
};
class Rectangle : public Shape {
    int x, y, w, h;
public:
    Rectangle(int x, int y, int w, int h) : x(x), y(y), w(w), h(h) {}
    Shape* clone() const override { return new Rectangle(*this); }
    void draw() const override { std::cout << "Rect(" << x << "," << y << "," << w << "," << h << ")\n"; }
};
void prototypeShapeExample() {
    Rectangle r1(10, 10, 100, 50);
    Shape* r2 = r1.clone();
    r1.draw(); r2->draw();
    delete r2;
}


//  3. 데이터베이스 레코드/설정 템플릿 복제
//  상황:
//  기본 설정(템플릿)에서 여러 사용자/레코드를 빠르게 생성해야 할 때
//  (예: 기본 권한, 기본 환경설정 등)
class UserSetting {
    std::string theme;
    bool notification;
public:
    UserSetting(const std::string& t, bool n) : theme(t), notification(n) {}
    UserSetting* clone() const { return new UserSetting(*this); }
    void show() const { std::cout << "Theme: " << theme << ", Noti: " << notification << "\n"; }
};
void prototypeSettingExample() {
    UserSetting defaultSetting("dark", true);
    UserSetting* user1 = defaultSetting.clone();
    UserSetting* user2 = defaultSetting.clone();
    user1->show(); user2->show();
    delete user1; delete user2;
}


//  4. 네트워크 패킷/메시지 복제
//  상황:
//  네트워크에서 동일한 구조의 패킷을 여러 번 전송해야 할 때
//  (예: 브로드캐스트, 멀티캐스트 등)
class Packet {
    std::string header;
    std::string payload;
public:
    Packet(const std::string& h, const std::string& p) : header(h), payload(p) {}
    Packet* clone() const { return new Packet(*this); }
    void send() const { std::cout << "Send: " << header << " - " << payload << "\n"; }
};
void prototypePacketExample() {
    Packet proto("HDR", "DATA");
    Packet* p1 = proto.clone();
    Packet* p2 = proto.clone();
    p1->send(); p2->send();
    delete p1; delete p2;
}

//  5. GUI 위젯/폼 템플릿 복제
//  상황:
//  동일한 UI 폼이나 위젯을 여러 개 동적으로 생성해야 할 때
//  (예: 다수의 입력 폼, 버튼 등)
class Widget {
    std::string type;
    int width, height;
public:
    Widget(const std::string& t, int w, int h) : type(t), width(w), height(h) {}
    Widget* clone() const { return new Widget(*this); }
    void render() const { std::cout << "Widget: " << type << " (" << width << "x" << height << ")\n"; }
};
void prototypeWidgetExample() {
    Widget buttonProto("Button", 80, 30);
    Widget* b1 = buttonProto.clone();
    Widget* b2 = buttonProto.clone();
    b1->render(); b2->render();
    delete b1; delete b2;
}