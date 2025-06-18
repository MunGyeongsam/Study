
/*
// basic singleton pattern in C++
class Singleton {
private:
	//c++ 17부터 inline 사용 가능
	static inline Singleton* instance = nullptr;	
	Singleton() {} // 생성자를 private으로
public:
	static Singleton* getInstance() {
		if (instance == nullptr)
			instance = new Singleton();
		return instance;
	}
	// 예시 메서드
	void doSomething() {  }
};

// C++17 이전 버전에서는 아래와 같이 작성해야 합니다.
//Singleton* Singleton::instance = nullptr;
//*/


/*
// Double-Checked Locking
#include <mutex>
class Singleton {
private:
	static Singleton* instance;
	static std::mutex mtx;
	Singleton() {}
public:
	static Singleton* getInstance() {
		// C++98/03에서는 안전하지 않음
		//   컴파일러 최적화, CPU 명령어 재정렬, 캐시 등으로 인해
		//   instance = new Singleton();가 완전히 끝나기 전에
		//   다른 스레드가 instance != nullptr로 판단할 수 있습니다.
		// 즉, 부분적으로 초기화된 객체를 반환할 위험이 있습니다.
		if (instance == nullptr) {           // 1차 체크
			std::lock_guard<std::mutex> lock(mtx);
			if (instance == nullptr) {       // 2차 체크
				instance = new Singleton();
			}
		}
		return instance;
	}
};
Singleton* Singleton::instance = nullptr;
std::mutex Singleton::mtx;
//*/


// C++11 이후 스레드 안전한 싱글턴 패턴

/*
// C++11 이후 스레드 안전한 싱글턴 패턴(static 지역 변수 사용)
class Singleton {
public:
	static Singleton& getInstance() {
		static Singleton instance; // C++11 이후 스레드 안전
		return instance;
	}
	// ...
};
//*/

/*
// C++11 이후 스레드 안전한 싱글턴 패턴(call_once를 사용)
#include <mutex>
class Singleton {
private:
	static Singleton* instance;
	static std::once_flag flag;
	Singleton() {}
public:
	static Singleton* getInstance() {
		std::call_once(flag, []() {
			instance = new Singleton();
			});
		return instance;
	}
};
Singleton* Singleton::instance = nullptr;
std::once_flag Singleton::flag;
//*/

/*
// C++11 이후 스레드 안전한 싱글턴 패턴 (atomic과 mutex 사용)
#include <atomic>
#include <mutex>

class Singleton {
private:
	static std::atomic<Singleton*> instance;
	static std::mutex mtx;
	Singleton() {}

public:
	static Singleton* getInstance() {
		Singleton* tmp = instance.load(std::memory_order_acquire);
		if (tmp == nullptr) {
			std::lock_guard<std::mutex> lock(mtx);
			tmp = instance.load(std::memory_order_relaxed);
			if (tmp == nullptr) {
				tmp = new Singleton();
				instance.store(tmp, std::memory_order_release);
			}
		}
		return tmp;
	}
	// 예시 메서드
	void doSomething() { }
};

std::atomic<Singleton*> Singleton::instance{ nullptr };
std::mutex Singleton::mtx;

+----------------------------------------------------------------------------+
| 구분                | std::atomic + DCL         | std::call_once           |
|---------------------|---------------------------|--------------------------|
| 코드 복잡도         | 높음                      | 낮음                     |
| 스레드 안전성       | 직접 구현에 따라 다름     | 표준에서 완전 보장       |
| 락 오버헤드         | 생성 후 없음              | 생성 후 거의 없음        |
| 메모리 관리         | 직접 관리 필요            | 직접 관리 필요           |
| 실수 가능성         | 높음 (메모리 오더 등)     | 낮음                     |
| 표준 지원           | C++11 이상                | C++11 이상               |
+----------------------------------------------------------------------------+

//*/



// Singleton.h
#pragma once
template <typename T>
class Singleton {
public:
	// 복사 및 이동 금지
	Singleton(const Singleton&) = delete;
	Singleton& operator=(const Singleton&) = delete;
	Singleton(Singleton&&) = delete;
	Singleton& operator=(Singleton&&) = delete;
	static T& Instance() {
		static T instance; // C++11 이후 스레드 안전
		return instance;
	}
protected:
	Singleton() = default;
	virtual ~Singleton() = default;
};

class MyManager : public Singleton<MyManager> {
	friend class Singleton<MyManager>; // 생성자 보호
private:
	MyManager() = default;
public:
	void doSomething() { /* ... */ }
};


static int main()
{
	// MyManager의 인스턴스를 가져와서 사용
	MyManager::Instance().doSomething();
	return 0;
}