class CPU {
public:
    void start() { std::cout << "CPU started\n"; }
};
class Memory {
public:
    void load() { std::cout << "Memory loaded\n"; }
};
class HardDrive {
public:
    void read() { std::cout << "Hard drive read\n"; }
};
// Facade 클래스
class ComputerFacade {
    CPU cpu;
    Memory memory;
    HardDrive hd;
public:
    void startComputer() {
        cpu.start();
        memory.load();
        hd.read();
        std::cout << "Computer started!\n";
    }
};
// 사용 예시
//int main() {
//    ComputerFacade computer;
//    computer.startComputer(); // 복잡한 내부 과정을 한 번에 처리
//}


// 퍼사드 패턴 예제: 파일 읽기, 암호화, 쓰기를 단순화하는 클래스
#include <string>

class FileReader {
public:
	std::string read(const std::string& path) { /* 파일 읽기 */ return "data"; }
};

class Encryptor {
public:
	std::string encrypt(const std::string& data) { /* 암호화 */ return "encrypted_" + data; }
};

class FileWriter {
public:
	void write(const std::string& path, const std::string& data) { /* 파일 쓰기 */ }
};

class FileEncryptorFacade {
	FileReader reader;
	Encryptor encryptor;
	FileWriter writer;
public:
	void encryptFile(const std::string& inputPath, const std::string& outputPath) {
		std::string data = reader.read(inputPath);
		std::string encrypted = encryptor.encrypt(data);
		writer.write(outputPath, encrypted);
	}
};

// 사용 예시
// FileEncryptorFacade facade;
// facade.encryptFile("a.txt", "a.enc");


// 퍼사드 패턴 예제: 홈 시어터 시스템을 단순화하는 클래스
class TV {
public:
	void on() { /* TV 켜기 */ }
};

class DVDPlayer {
public:
	void play() { /* DVD 재생 */ }
};

class Speaker {
public:
	void setVolume(int v) { /* 볼륨 설정 */ }
};

class HomeTheaterFacade {
	TV tv;
	DVDPlayer dvd;
	Speaker speaker;
public:
	void watchMovie() {
		tv.on();
		speaker.setVolume(10);
		dvd.play();
	}
};

// 사용 예시
// HomeTheaterFacade theater;
// theater.watchMovie();


// 퍼사드 패턴 예제: 은행 계좌 이체를 단순화하는 클래스
#include <string>

class AccountChecker {
public:
	bool isActive(const std::string& acc) { return true; }
};

class CreditChecker {
public:
	bool hasGoodCredit(const std::string& acc) { return true; }
};

class TransferService {
public:
	void transfer(const std::string& from, const std::string& to, double amt) { /* 송금 */ }
};

class BankFacade {
	AccountChecker accountChecker;
	CreditChecker creditChecker;
	TransferService transferService;
public:
	bool transferMoney(const std::string& from, const std::string& to, double amt) {
		if (!accountChecker.isActive(from) || !creditChecker.hasGoodCredit(from))
			return false;
		transferService.transfer(from, to, amt);
		return true;
	}
};

// 사용 예시
// BankFacade bank;
// bank.transferMoney("A", "B", 1000.0);