# 용어집 (Glossary)

> Vector Swarm 프로젝트에서 사용하는 축약어 및 전문 용어 정리.
> 각 문서에서 첫 등장 시 `축약어 (Full Name)` 형태로 병기하고, 이후에는 축약어만 사용.

---

## 게임 용어

| 축약어/용어 | 풀네임 | 설명 |
|------------|--------|------|
| DNA | (비유) Deoxyribonucleic Acid | 게임 내 적(Enemy) 유전자 조합 시스템. 생물학 DNA를 비유한 게임 설계 용어 |
| HP | Hit Points | 체력. 0이 되면 파괴 |
| XP | Experience Points | 경험치. 적 처치 시 드롭되는 오브로 획득 |
| DPS | Damage Per Second | 초당 피해량. 무기 성능 지표 |
| iFrame | Invincibility Frame | 무적 시간. 피격 후 일정 시간 데미지 면역 |
| HUD | Heads-Up Display | 화면 위에 겹쳐 표시되는 UI (체력바, 레벨, 스테이지 등) |

## 아키텍처 & 프로그래밍

| 축약어/용어 | 풀네임 | 설명 |
|------------|--------|------|
| CS | Computer Science | 컴퓨터 과학. 게임 세계관과 적 설계의 기반 |
| ECS | Entity Component System | 게임 아키텍처 패턴. Entity(개체) + Component(데이터) + System(로직) 분리 |
| AI | Artificial Intelligence | 인공지능. 게임 내에서는 적의 행동 패턴을 의미 |
| GC | Garbage Collection | 가비지 컬렉션. 사용하지 않는 메모리의 자동 해제 |
| API | Application Programming Interface | 모듈 간 통신 인터페이스 |

## 자료 구조 (세계관 내 적 타입으로 사용)

| 용어 | 풀네임 | 설명 |
|------|--------|------|
| LIFO | Last In, First Out | 후입선출. Stack 자료 구조의 원칙 |
| FIFO | First In, First Out | 선입선출. Queue 자료 구조의 원칙 |
| Linked List | — | 연결 리스트. 노드가 다음 노드를 가리키는 체인 구조 |
| Hash | Hash Table | 해시 테이블. 키 → 값 매핑으로 O(1) 접근. 충돌 시 예측 불가 |
| Binary Tree | — | 이진 트리. 각 노드가 최대 2개 자식을 갖는 분기 구조 |

## 사운드

| 축약어/용어 | 풀네임 | 설명 |
|------------|--------|------|
| BGM | Background Music | 배경 음악 |
| SFX | Sound Effects | 효과음 |
| ADSR | Attack, Decay, Sustain, Release | 음향 엔벨로프(음량 변화 곡선)의 4단계 |

## 프로젝트 관리

| 축약어/용어 | 풀네임 | 설명 |
|------------|--------|------|
| MVP | Minimum Viable Product | 최소 기능 제품. 핵심만 갖춘 첫 번째 동작 가능 버전 |
| QA | Quality Assurance | 품질 보증. 테스트 및 밸런스 검증 |
| ROI | Return On Investment | 투자 대비 수익. 기능 구현의 비용 대비 효과 평가 |
| UX | User Experience | 사용자 경험. 플레이어가 느끼는 전반적 사용감 |
| UI | User Interface | 사용자 인터페이스. 버튼, 메뉴, 표시 등 시각적 조작 요소 |

## 게임 디자인

| 축약어/용어 | 풀네임 | 설명 |
|------------|--------|------|
| Zero-Art | — | 외부 이미지 에셋 없이 코드만으로 모든 비주얼을 생성하는 컨셉 |
| Juice | — | 게임의 시각·촉각·청각 피드백 총체. "기분 좋은 반응" |
| Danmaku | 弾幕 (탄막) | 일본어 유래. 대량의 탄이 패턴을 이루는 슈팅 장르 |
| Roguelite | — | 매 판 죽으면 처음부터 + 일부 영구 성장이 유지되는 장르 |
| Bloom | — | 밝은 부분이 빛나 보이는 포스트 프로세싱 효과 |

---

> 작성일: 2026-04-22
> 새 축약어를 문서에서 사용할 때: 첫 등장 시 `축약어 (Full Name)` 병기 → 이후 축약어만 사용 → 이 용어집에 추가
