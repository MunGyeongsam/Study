#include <iostream>
// Target 인터페이스
class Target {
public:
	virtual void request() = 0;
	virtual ~Target() = default;
};
// Adaptee: 기존 인터페이스(호환되지 않음)
class Adaptee {
public:
	void specificRequest() {
		std::cout << "Adaptee specific request\n";
	}
};
// Adapter: Adaptee를 Target 인터페이스로 변환
class Adapter : public Target {
	Adaptee* adaptee;
public:
	Adapter(Adaptee* a) : adaptee(a) {}
	void request() override {
		adaptee->specificRequest();
	}
};
//// 사용 예시
//int main() {
//	Adaptee adaptee;
//	Adapter adapter(&adaptee);
//	adapter.request(); // "Adaptee specific request" 출력
//}



// 1. 레거시 클래스 인터페이스 변환 (클래스 어댑터)
#include <iostream>

// 기존(레거시) 클래스
class OldPrinter {
public:
	void oldPrint(const std::string& msg) {
		std::cout << "OldPrinter: " << msg << std::endl;
	}
};

// 새 인터페이스
class IPrinter {
public:
	virtual void print(const std::string& msg) = 0;
	virtual ~IPrinter() = default;
};

// 어댑터
class PrinterAdapter : public IPrinter {
	OldPrinter* oldPrinter;
public:
	PrinterAdapter(OldPrinter* p) : oldPrinter(p) {}
	void print(const std::string& msg) override {
		oldPrinter->oldPrint(msg);
	}
};

// 사용 예시
// OldPrinter old;
// PrinterAdapter adapter(&old);
// adapter.print("Hello"); // OldPrinter: Hello



// 2. 좌표계 변환(객체 어댑터)
#include <iostream>

// 타사 라이브러리: 2D 좌표
class Point2D {
public:
	int x, y;
	Point2D(int x, int y) : x(x), y(y) {}
};

// 우리 시스템: 3D 좌표 인터페이스
class IPoint3D {
public:
	virtual void print3D() = 0;
	virtual ~IPoint3D() = default;
};

// 어댑터: 2D를 3D로 변환
class Point2DTo3DAdapter : public IPoint3D {
	Point2D* point2d;
public:
	Point2DTo3DAdapter(Point2D* p) : point2d(p) {}
	void print3D() override {
		std::cout << "3D Point: (" << point2d->x << ", " << point2d->y << ", 0)" << std::endl;
	}
};

// 사용 예시
// Point2D p2d(1, 2);
// Point2DTo3DAdapter adapter(&p2d);
// adapter.print3D(); // 3D Point: (1, 2, 0)


// 3. 호환되지 않는 소켓 인터페이스 연결
#include <iostream>

// 미국식 소켓
class USASocket {
public:
	void plugInUSA() {
		std::cout << "Plugged into USA socket" << std::endl;
	}
};

// 유럽식 소켓 인터페이스
class EuropeSocket {
public:
	virtual void plugInEurope() = 0;
	virtual ~EuropeSocket() = default;
};

// 어댑터
class USAToEuropeAdapter : public EuropeSocket {
	USASocket* usaSocket;
public:
	USAToEuropeAdapter(USASocket* s) : usaSocket(s) {}
	void plugInEurope() override {
		usaSocket->plugInUSA();
	}
};

// 사용 예시
// USASocket usa;
// USAToEuropeAdapter adapter(&usa);
// adapter.plugInEurope(); // Plugged into USA socket

//  Adapter 패턴
//  	• 목적 :
//  		서로 다른 인터페이스를 가진 클래스들을 연결(호환)시켜줍니다.
//  		즉, 기존 클래스를 수정하지 않고, 클라이언트가 원하는 인터페이스로 변환합니다.
//  	• 사용 시점 :
//  		기존 코드(레거시 코드, 외부 라이브러리 등)를 새 인터페이스에 맞춰 쓸 때.
//  	• 예시 :
//  		2D 좌표 객체를 3D 좌표 인터페이스로 변환하는 어댑터.
//  
//  Decorator 패턴
//  	• 목적 :
//  		객체에 새로운 기능(책임)을 동적으로 추가합니다.
//  		즉, 기존 객체의 인터페이스는 그대로 두고, 기능을 확장합니다.
//  	• 사용 시점 :
//  		객체의 기능을 조합하거나, 런타임에 유연하게 추가 / 제거하고 싶을 때.
//  	• 예시 :
//  		커피에 우유, 설탕 등 첨가물을 데코레이터로 추가.