//1. GUI 폼 빌더
#include <iostream>
#include <string>
#include <vector>

class Form {
    std::vector<std::string> fields;
public:
    void addField(const std::string& field) { fields.push_back(field); }
    void show() const {
        std::cout << "Form Fields: ";
        for (const auto& f : fields) std::cout << "[" << f << "] ";
        std::cout << std::endl;
    }
};

class FormBuilder {
    Form form;
public:
    FormBuilder& addTextField(const std::string& name) {
        form.addField("Text: " + name);
        return *this;
    }
    FormBuilder& addCheckBox(const std::string& name) {
        form.addField("CheckBox: " + name);
        return *this;
    }
    FormBuilder& addButton(const std::string& name) {
        form.addField("Button: " + name);
        return *this;
    }
    Form build() { return form; }
};

// 사용 예시
void guiFormBuilderExample() {
    FormBuilder builder;
    Form form = builder.addTextField("이름")
                       .addCheckBox("동의")
                       .addButton("제출")
                       .build();
    form.show();
}

//2. HTTP 요청 빌더
#include <iostream>
#include <string>
#include <map>

class HttpRequest {
    std::string method, url;
    std::map<std::string, std::string> headers;
public:
    void setMethod(const std::string& m) { method = m; }
    void setUrl(const std::string& u) { url = u; }
    void addHeader(const std::string& k, const std::string& v) { headers[k] = v; }
    void show() const {
        std::cout << method << " " << url << std::endl;
        for (const auto& h : headers)
            std::cout << h.first << ": " << h.second << std::endl;
    }
};

class HttpRequestBuilder {
    HttpRequest req;
public:
    HttpRequestBuilder& method(const std::string& m) { req.setMethod(m); return *this; }
    HttpRequestBuilder& url(const std::string& u) { req.setUrl(u); return *this; }
    HttpRequestBuilder& header(const std::string& k, const std::string& v) { req.addHeader(k, v); return *this; }
    HttpRequest build() { return req; }
};

// 사용 예시
void httpRequestBuilderExample() {
    HttpRequestBuilder builder;
    HttpRequest req = builder.method("GET")
                            .url("/api/data")
                            .header("Accept", "application/json")
                            .header("User-Agent", "BuilderExample")
                            .build();
    req.show();
}

//3. 게임 캐릭터 생성 빌더
#include <iostream>
#include <string>

class Character {
    std::string name, job;
    int hp = 0, mp = 0;
public:
    void setName(const std::string& n) { name = n; }
    void setJob(const std::string& j) { job = j; }
    void setHP(int h) { hp = h; }
    void setMP(int m) { mp = m; }
    void show() const {
        std::cout << "캐릭터: " << name << ", 직업: " << job << ", HP: " << hp << ", MP: " << mp << std::endl;
    }
};

class CharacterBuilder {
    Character c;
public:
    CharacterBuilder& name(const std::string& n) { c.setName(n); return *this; }
    CharacterBuilder& job(const std::string& j) { c.setJob(j); return *this; }
    CharacterBuilder& hp(int h) { c.setHP(h); return *this; }
    CharacterBuilder& mp(int m) { c.setMP(m); return *this; }
    Character build() { return c; }
};

// 사용 예시
void characterBuilderExample() {
    CharacterBuilder builder;
    Character hero = builder.name("아더").job("기사").hp(150).mp(30).build();
    hero.show();
}


//단순한 객체에는 Builder가 불필요할 수 있지만,
//옵션이 많고, 생성 과정이 복잡하거나, 불변 객체가 필요한 경우
// Builder 패턴이 훨씬 안전하고, 유지보수에 강합니다.
