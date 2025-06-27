//1. 네트워크 게임에서의 원격 프록시(Remote Proxy)
class IPlayer {
public:
    virtual void move(int x, int y) = 0;
    virtual ~IPlayer() = default;
};

// 실제 내 캐릭터
class LocalPlayer : public IPlayer {
public:
    void move(int x, int y) override {
        // 실제 이동 로직
    }
};

// 네트워크로 제어되는 원격 캐릭터
class RemotePlayerProxy : public IPlayer {
public:
    void move(int x, int y) override {
        // 네트워크 패킷 전송
        // 서버나 다른 클라이언트에 이동 명령 전달
    }
};


//2. 리소스(텍스처, 모델 등) 로딩의 가상 프록시(Virtual Proxy)
class IModel {
public:
    virtual void render() = 0;
    virtual ~IModel() = default;
};

class RealModel : public IModel {
public:
    RealModel(const std::string& file) {/* 실제 모델 로딩 */}
    void render() override {/* 렌더링 */}
};

class ModelProxy : public IModel {
    std::string file;
    RealModel* realModel = nullptr;
public:
    ModelProxy(const std::string& f) : file(f) {}
    ~ModelProxy() { delete realModel; }
    void render() override {
        if (!realModel) realModel = new RealModel(file);
        realModel->render();
    }
};

//3. 보호 프록시(Protection Proxy)로 치트 방지
class IInventory {
public:
    virtual void addItem(int itemId) = 0;
    virtual ~IInventory() = default;
};

class RealInventory : public IInventory {
public:
    void addItem(int itemId) override {/* 실제 아이템 추가 */}
};

class InventoryProxy : public IInventory {
    RealInventory real;
public:
    void addItem(int itemId) override {
        if (/* 치트 감지 로직 */) {
            // 차단 또는 로그 기록
        } else {
            real.addItem(itemId);
        }
    }
};


//4. 스마트 레퍼런스 프록시(Smart Reference Proxy)로 오브젝트 참조 관리
#include <iostream>

class IGameObject {
public:
    virtual void use() = 0;
    virtual ~IGameObject() = default;
};

class RealGameObject : public IGameObject {
public:
    void use() override {
        std::cout << "게임 오브젝트 사용\n";
    }
};

class SmartRefProxy : public IGameObject {
    RealGameObject real;
    int refCount = 0;
public:
    void use() override {
        refCount++;
        std::cout << "[스마트 프록시] 참조 횟수 증가: " << refCount << std::endl;
        real.use();
    }
    void release() {
        if (refCount > 0) {
            refCount--;
            std::cout << "[스마트 프록시] 참조 횟수 감소: " << refCount << std::endl;
        }
        // refCount가 0이 되면 리소스 해제 등 추가 작업 가능
    }
    int getRefCount() const { return refCount; }
};

// 사용 예시
void testSmartRefProxy() {
    SmartRefProxy proxy;
    proxy.use();    // 참조 횟수 증가: 1
    proxy.use();    // 참조 횟수 증가: 2
    proxy.release(); // 참조 횟수 감소: 1
    proxy.release(); // 참조 횟수 감소: 0
}


/*
C++의 **스마트 포인터(std::shared_ptr, std::unique_ptr, std::weak_ptr 등)**는
프록시(Proxy) 패턴의 대표적인 실전 예시 중 하나입니다.

왜 스마트 포인터가 프록시 패턴인가?
  - 스마트 포인터는 실제 객체(메모리 리소스)에 대한 접근을 대리하는 객체입니다.
  - 스마트 포인터는
    - 참조 카운트 관리(shared_ptr)
    - 소유권 이전(unique_ptr)
    - 약한 참조(weak_ptr)
    
    자동 해제, 예외 안전 등
    부가 기능을 제공하면서,
    실제 객체의 포인터처럼 사용할 수 있도록 동일한 인터페이스(operator*, operator->)를 제공합니다.
  - 즉, 실제 객체와 동일하게 사용할 수 있지만,
   내부적으로는 접근 제어, 자원 관리 등 다양한 부가 기능을 수행합니다.
//*/