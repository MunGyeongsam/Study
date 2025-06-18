#include <iostream>
#include <memory>

/* 대표 예 : dialog & button
// Product 인터페이스
class Button {
public:
	virtual void render() = 0;
	virtual ~Button() = default;
};
// ConcreteProductA
class WindowsButton : public Button {
public:
	void render() override { std::cout << "Windows Button\n"; }
};
// ConcreteProductB
class MacButton : public Button {
public:
	void render() override { std::cout << "Mac Button\n"; }
};
// Creator
class Dialog {
public:
	virtual std::unique_ptr<Button> createButton() = 0;
	void renderWindow() {
		auto btn = createButton();
		btn->render();
	}
	virtual ~Dialog() = default;
};
// ConcreteCreatorA
class WindowsDialog : public Dialog {
public:
	std::unique_ptr<Button> createButton() override {
		return std::make_unique<WindowsButton>();
	}
};
// ConcreteCreatorB
class MacDialog : public Dialog {
public:
	std::unique_ptr<Button> createButton() override {
		return std::make_unique<MacButton>();
	}
};


// 사용 예시
int main() {
	std::unique_ptr<Dialog> dialog;
#ifdef _WIN32
	dialog = std::make_unique<WindowsDialog>();
#else
	dialog = std::make_unique<MacDialog>();
#endif
	dialog->renderWindow(); // 플랫폼에 맞는 버튼이 생성되어 렌더링됨
}
//*/


/* 동물 객체 생성 (기본 Factory Method)
#include <iostream>
#include <memory>

class Animal {
public:
	virtual void Speak() = 0;
	virtual ~Animal() = default;
};

class Dog : public Animal {
public:
	void Speak() override { std::cout << "Woof!" << std::endl; }
};

class Cat : public Animal {
public:
	void Speak() override { std::cout << "Meow!" << std::endl; }
};

class AnimalFactory {
public:
	virtual std::unique_ptr<Animal> CreateAnimal() = 0;
	virtual ~AnimalFactory() = default;
};

class DogFactory : public AnimalFactory {
public:
	std::unique_ptr<Animal> CreateAnimal() override { return std::make_unique<Dog>(); }
};

class CatFactory : public AnimalFactory {
public:
	std::unique_ptr<Animal> CreateAnimal() override { return std::make_unique<Cat>(); }
};

// 사용 예시
// DogFactory dogFactory;
// auto dog = dogFactory.CreateAnimal();
// dog->Speak(); // Woof!
//*/


/* 문서 객체 생성 (파라미터 기반 Factory Method)
#include <iostream>
#include <memory>
#include <string>

class Document {
public:
	virtual void Print() = 0;
	virtual ~Document() = default;
};

class WordDocument : public Document {
public:
	void Print() override { std::cout << "Word Document" << std::endl; }
};

class PdfDocument : public Document {
public:
	void Print() override { std::cout << "PDF Document" << std::endl; }
};

class DocumentFactory {
public:
	static std::unique_ptr<Document> CreateDocument(const std::string& type) {
		if (type == "word") return std::make_unique<WordDocument>();
		if (type == "pdf") return std::make_unique<PdfDocument>();
		return nullptr;
	}
};

// 사용 예시
// auto doc = DocumentFactory::CreateDocument("pdf");
// doc->Print(); // PDF Document
//*/


/* 게임 무기 객체 생성 (함수 기반 Factory Method)
#include <iostream>
#include <memory>
#include <string>

class Weapon {
public:
	virtual void Attack() = 0;
	virtual ~Weapon() = default;
};

class Sword : public Weapon {
public:
	void Attack() override { std::cout << "Swing sword!" << std::endl; }
};

class Bow : public Weapon {
public:
	void Attack() override { std::cout << "Shoot arrow!" << std::endl; }
};

std::unique_ptr<Weapon> CreateWeapon(const std::string& type) {
	if (type == "sword") return std::make_unique<Sword>();
	if (type == "bow") return std::make_unique<Bow>();
	return nullptr;
}

// 사용 예시
// auto weapon = CreateWeapon("bow");
// weapon->Attack(); // Shoot arrow!
//*/