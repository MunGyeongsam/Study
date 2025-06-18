#include <iostream>
#include <memory>
// Strategy 인터페이스
class SortStrategy {
public:
	virtual void sort() = 0;
	virtual ~SortStrategy() = default;
};
// ConcreteStrategyA
class BubbleSort : public SortStrategy {
public:
	void sort() override { std::cout << "Bubble Sort\n"; }
};
// ConcreteStrategyB
class QuickSort : public SortStrategy {
public:
	void sort() override { std::cout << "Quick Sort\n"; }
};
// Context
class Sorter {
	std::unique_ptr<SortStrategy> strategy;
public:
	void setStrategy(std::unique_ptr<SortStrategy> s) {
		strategy = std::move(s);
	}
	void sort() {
		if (strategy) strategy->sort();
		else std::cout << "No strategy set\n";
	}
};
//// 사용 예시
//int main() {
//	Sorter sorter;
//	sorter.setStrategy(std::make_unique<BubbleSort>());
//	sorter.sort(); // Bubble Sort
//	sorter.setStrategy(std::make_unique<QuickSort>());
//	sorter.sort(); // Quick Sort
//}



#include <iostream>
#include <fstream>
#include <memory>
#include <string>

class OutputStrategy {
public:
	virtual void output(const std::string& msg) = 0;
	virtual ~OutputStrategy() = default;
};

class ConsoleOutput : public OutputStrategy {
public:
	void output(const std::string& msg) override {
		std::cout << "Console: " << msg << std::endl;
	}
};

class FileOutput : public OutputStrategy {
	std::string filename;
public:
	FileOutput(const std::string& fname) : filename(fname) {}
	void output(const std::string& msg) override {
		std::ofstream ofs(filename, std::ios::app);
		ofs << "File: " << msg << std::endl;
	}
};

class Logger {
	std::unique_ptr<OutputStrategy> strategy;
public:
	void setStrategy(std::unique_ptr<OutputStrategy> s) {
		strategy = std::move(s);
	}
	void log(const std::string& msg) {
		if (strategy) strategy->output(msg);
		else std::cout << "No output strategy set\n";
	}
};

// 사용 예시
// Logger logger;
// logger.setStrategy(std::make_unique<ConsoleOutput>());
// logger.log("Hello"); // Console: Hello
// logger.setStrategy(std::make_unique<FileOutput>("log.txt"));
// logger.log("World"); // log.txt에 기록