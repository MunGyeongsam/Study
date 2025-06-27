/**
 * The Implementation defines the interface for all implementation classes. It
 * doesn't have to match the Abstraction's interface. In fact, the two
 * interfaces can be entirely different. Typically the Implementation interface
 * provides only primitive operations, while the Abstraction defines higher-
 * level operations based on those primitives.
 */

class Implementation {
 public:
  virtual ~Implementation() {}
  virtual std::string OperationImplementation() const = 0;
};

/**
 * Each Concrete Implementation corresponds to a specific platform and
 * implements the Implementation interface using that platform's API.
 */
class ConcreteImplementationA : public Implementation {
 public:
  std::string OperationImplementation() const override {
    return "ConcreteImplementationA: Here's the result on the platform A.\n";
  }
};
class ConcreteImplementationB : public Implementation {
 public:
  std::string OperationImplementation() const override {
    return "ConcreteImplementationB: Here's the result on the platform B.\n";
  }
};

/**
 * The Abstraction defines the interface for the "control" part of the two class
 * hierarchies. It maintains a reference to an object of the Implementation
 * hierarchy and delegates all of the real work to this object.
 */

class Abstraction {
  /**
   * @var Implementation
   */
 protected:
  Implementation* implementation_;

 public:
  Abstraction(Implementation* implementation) : implementation_(implementation) {
  }

  virtual ~Abstraction() {
  }

  virtual std::string Operation() const {
    return "Abstraction: Base operation with:\n" +
           this->implementation_->OperationImplementation();
  }
};
/**
 * You can extend the Abstraction without changing the Implementation classes.
 */
class ExtendedAbstraction : public Abstraction {
 public:
  ExtendedAbstraction(Implementation* implementation) : Abstraction(implementation) {
  }
  std::string Operation() const override {
    return "ExtendedAbstraction: Extended operation with:\n" +
           this->implementation_->OperationImplementation();
  }
};

/**
 * Except for the initialization phase, where an Abstraction object gets linked
 * with a specific Implementation object, the client code should only depend on
 * the Abstraction class. This way the client code can support any abstraction-
 * implementation combination.
 */
void ClientCode(const Abstraction& abstraction) {
  // ...
  std::cout << abstraction.Operation();
  // ...
}
/**
 * The client code should be able to work with any pre-configured abstraction-
 * implementation combination.
 */

int main() {
  Implementation* implementation = new ConcreteImplementationA;
  Abstraction* abstraction = new Abstraction(implementation);
  ClientCode(*abstraction);
  std::cout << std::endl;
  delete implementation;
  delete abstraction;

  implementation = new ConcreteImplementationB;
  abstraction = new ExtendedAbstraction(implementation);
  ClientCode(*abstraction);

  delete implementation;
  delete abstraction;

  return 0;
}



// 1. 메시지 전송 시스템 (메시지 종류와 전송 방식 분리)
#include <iostream>
#include <string>

// 구현 계층: 전송 방식
class MessageSender {
public:
    virtual void send(const std::string& msg) = 0;
    virtual ~MessageSender() = default;
};
class EmailSender : public MessageSender {
public:
    void send(const std::string& msg) override { std::cout << "[이메일] " << msg << std::endl; }
};
class SMSSender : public MessageSender {
public:
    void send(const std::string& msg) override { std::cout << "[SMS] " << msg << std::endl; }
};

// 기능 계층: 메시지 종류
class Message {
protected:
    MessageSender* sender;
public:
    Message(MessageSender* s) : sender(s) {}
    virtual void sendMessage(const std::string& msg) = 0;
    virtual ~Message() = default;
};
class TextMessage : public Message {
public:
    TextMessage(MessageSender* s) : Message(s) {}
    void sendMessage(const std::string& msg) override { sender->send("[텍스트] " + msg); }
};
class AlertMessage : public Message {
public:
    AlertMessage(MessageSender* s) : Message(s) {}
    void sendMessage(const std::string& msg) override { sender->send("[경고] " + msg); }
};

// 사용 예시
void bridgeMessageExample() {
    EmailSender email;
    SMSSender sms;
    TextMessage text(&email);
    AlertMessage alert(&sms);

    text.sendMessage("안녕하세요!");   // [이메일] [텍스트] 안녕하세요!
    alert.sendMessage("위험 발생!");   // [SMS] [경고] 위험 발생!
}


// 2. 그래픽 라이브러리 추상화 (도형과 렌더링 엔진 분리)
#include <iostream>

// 구현 계층: 렌더링 엔진
class Renderer {
public:
    virtual void renderCircle(float x, float y, float r) = 0;
    virtual ~Renderer() = default;
};
class OpenGLRenderer : public Renderer {
public:
    void renderCircle(float x, float y, float r) override {
        std::cout << "OpenGL: 원 (" << x << "," << y << ") 반지름 " << r << std::endl;
    }
};
class DirectXRenderer : public Renderer {
public:
    void renderCircle(float x, float y, float r) override {
        std::cout << "DirectX: 원 (" << x << "," << y << ") 반지름 " << r << std::endl;
    }
};

// 기능 계층: 도형
class Shape2 {
protected:
    Renderer* renderer;
public:
    Shape2(Renderer* r) : renderer(r) {}
    virtual void draw() = 0;
    virtual ~Shape2() = default;
};
class Circle2 : public Shape2 {
    float x, y, r;
public:
    Circle2(float x, float y, float r, Renderer* ren) : Shape2(ren), x(x), y(y), r(r) {}
    void draw() override { renderer->renderCircle(x, y, r); }
};

// 사용 예시
void bridgeGraphicsExample() {
    OpenGLRenderer ogl;
    DirectXRenderer dx;
    Circle2 c1(0, 0, 5, &ogl);
    Circle2 c2(1, 2, 3, &dx);

    c1.draw(); // OpenGL: 원 (0,0) 반지름 5
    c2.draw(); // DirectX: 원 (1,2) 반지름 3
}



