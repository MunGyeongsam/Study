#include <iostream>
#include <memory>
// Component 인터페이스
class Coffee {
public:
	virtual int cost() const = 0;
	virtual std::string ingredients() const = 0;
	virtual ~Coffee() = default;
};
// ConcreteComponent
class BasicCoffee : public Coffee {
public:
	int cost() const override { return 2000; }
	std::string ingredients() const override { return "Coffee"; }
};
// Decorator
class CoffeeDecorator : public Coffee {
protected:
	std::unique_ptr<Coffee> coffee;
public:
	CoffeeDecorator(std::unique_ptr<Coffee> c) : coffee(std::move(c)) {}
};
// ConcreteDecoratorA
class MilkDecorator : public CoffeeDecorator {
public:
	MilkDecorator(std::unique_ptr<Coffee> c) : CoffeeDecorator(std::move(c)) {}
	int cost() const override { return coffee->cost() + 500; }
	std::string ingredients() const override { return coffee->ingredients() + ", Milk"; }
};
// ConcreteDecoratorB
class SugarDecorator : public CoffeeDecorator {
public:
	SugarDecorator(std::unique_ptr<Coffee> c) : CoffeeDecorator(std::move(c)) {}
	int cost() const override { return coffee->cost() + 300; }
	std::string ingredients() const override { return coffee->ingredients() + ", Sugar"; }
};

//// 사용 예시
//int main() {
//	std::unique_ptr<Coffee> coffee = std::make_unique<BasicCoffee>();
//	coffee = std::make_unique<MilkDecorator>(std::move(coffee));
//	coffee = std::make_unique<SugarDecorator>(std::move(coffee));
//	std::cout << "Cost: " << coffee->cost() << std::endl;
//	std::cout << "Ingredients: " << coffee->ingredients() << std::endl;
//}


#include <iostream>
#include <memory>
#include <algorithm>

class Text {
public:
	virtual std::string getText() const = 0;
	virtual ~Text() = default;
};

class PlainText : public Text {
	std::string text;
public:
	PlainText(const std::string& t) : text(t) {}
	std::string getText() const override { return text; }
};

class TextDecorator : public Text {
protected:
	std::unique_ptr<Text> inner;
public:
	TextDecorator(std::unique_ptr<Text> t) : inner(std::move(t)) {}
};

class UppercaseDecorator : public TextDecorator {
public:
	UppercaseDecorator(std::unique_ptr<Text> t) : TextDecorator(std::move(t)) {}
	std::string getText() const override {
		std::string s = inner->getText();
		std::transform(s.begin(), s.end(), s.begin(), ::toupper);
		return s;
	}
};

class BracketDecorator : public TextDecorator {
public:
	BracketDecorator(std::unique_ptr<Text> t) : TextDecorator(std::move(t)) {}
	std::string getText() const override {
		return "[" + inner->getText() + "]";
	}
};

// 사용 예시
// auto txt = std::make_unique<BracketDecorator>(std::make_unique<UppercaseDecorator>(std::make_unique<PlainText>("hello")));
// std::cout << txt->getText() << std::endl;
// 출력: [HELLO]