#include <iostream>
#include <vector>
#include <memory>
// Observer 인터페이스
class Observer {
public:
	virtual void update(int value) = 0;
	virtual ~Observer() = default;
};
// Subject(발행자)
class Subject {
	std::vector<Observer*> observers;
	int state = 0;
public:
	void attach(Observer* obs) { observers.push_back(obs); }
	void detach(Observer* obs) {
		observers.erase(std::remove(observers.begin(), observers.end(), obs), observers.end());
	}
	void setState(int value) {
		state = value;
		notify();
	}
	int getState() const { return state; }
	void notify() {
		for (auto obs : observers) {
			obs->update(state);
		}
	}
};
// ConcreteObserver
class ConcreteObserver : public Observer {
	std::string name;
public:
	ConcreteObserver(const std::string& n) : name(n) {}
	void update(int value) override {
		std::cout << name << " received update: " << value << std::endl;
	}
};

// 사용 예시
// Subject subject;
// ConcreteObserver obs1, obs2;
// subject.attach(&obs1);
// subject.attach(&obs2);
// subject.SetState(42);
// // 두 옵저버 모두 "Observer notified: 42" 출력