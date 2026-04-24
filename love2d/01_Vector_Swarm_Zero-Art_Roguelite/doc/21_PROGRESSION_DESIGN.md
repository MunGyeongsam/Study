# 21. 진행 시스템 설계 (Progression Design)

> **목적**: 업그레이드 트리, 업적, Codex 해금 등 모든 진행 시스템의 **개발자용 밸런싱 레퍼런스**.
> 플레이어 대상 설명은 `06_PLAY_GUIDE.md` 참조.

**최종 갱신: 2026-04-24**

---

## 목차
1. [영구 업그레이드 트리](#1-영구-업그레이드-트리-upgrade-tree)
2. [인런 레벨업 카드](#2-인런-레벨업-카드-level-up-cards)
3. [업적 / 도전과제](#3-업적--도전과제-achievements)
4. [Fragment 경제](#4-fragment-경제)
5. [Codex 연동 계획](#5-codex-연동-계획)
6. [세이브 데이터 구조](#6-세이브-데이터-구조)
7. [밸런싱 로그](#7-밸런싱-로그)

---

## 1. 영구 업그레이드 트리 (Upgrade Tree)

**파일**: `src/03_game/states/upgradeTree.lua`
**UI**: `src/03_game/scenes/upgradeScene.lua`
**접근**: 타이틀 메뉴 UPGRADES / 게임오버 화면 U키

### 1.1 노드 일람 (9종, 3브랜치)

#### Attack 브랜치
| ID | 이름 | 최대 레벨 | 비용 (Lv1→2→3) | 효과 (레벨당) | 수치 근거 |
|----|------|-----------|-----------------|--------------|-----------|
| `atk_damage` | Core Damage | 3 | 10 / 25 / 50 | 탄환 기본 데미지 +1 | 기본 데미지 3 기준, +33%씩 증가 |
| `atk_firerate` | Overclock | 3 | 15 / 35 / 60 | 공격 속도 ×(1 + 0.15×lv) | Lv3 = +45%, 체감 DPS ~2배 |
| `atk_range` | Long Reach | 3 | 10 / 20 / 40 | 자동조준 범위 ×(1 + 0.20×lv) | Lv3 = +60%, 화면 밖 적도 조준 |

#### Defense 브랜치
| ID | 이름 | 최대 레벨 | 비용 (Lv1→2→3) | 효과 (레벨당) | 수치 근거 |
|----|------|-----------|-----------------|--------------|-----------|
| `def_hp` | Reinforced Core | 3 | 10 / 25 / 50 | Max HP +1, 현재 HP +1 | 기본 HP 5 기준, Lv3 = 8 (60% 증가) |
| `def_iframe` | Phase Shield | 3 | 15 / 30 / 55 | 피격 무적 +0.3초 | 기본 0.5초 → Lv3 = 1.4초 |
| `def_dash` | Quick Phase | 3 | 10 / 20 / 40 | 대시 쿨다운 ×(1 - 0.15×lv) | Lv3 = -45%, 공격적 대시 플레이 가능 |

#### Utility 브랜치
| ID | 이름 | 최대 레벨 | 비용 (Lv1→2→3) | 효과 (레벨당) | 수치 근거 |
|----|------|-----------|-----------------|--------------|-----------|
| `util_magnet` | Data Magnet | 3 | 10 / 20 / 40 | XP 흡수 범위 ×(1 + 0.30×lv) | Lv3 = +90%, 거의 자동 수집 |
| `util_speed` | Turbo | 3 | 15 / 30 / 50 | 이동 속도 ×(1 + 0.10×lv) | Lv3 = +30%, 회피력 강화 |
| `util_fragment` | Data Miner | 3 | 10 / 25 / 40 | Fragment 드롭률 +5%/lv | Lv3 = +15%, 장기 투자 보상 |

### 1.2 비용 총계

| 브랜치 | 노드 수 | 합계 비용 |
|--------|---------|-----------|
| Attack | 3 | 85 + 110 + 70 = **265** |
| Defense | 3 | 85 + 100 + 70 = **255** |
| Utility | 3 | 70 + 95 + 75 = **240** |
| **전체** | **9** | **525 Fragments** |

> **설계 의도**: 풀 업그레이드까지 약 5~10런 필요 (중후반 런당 ~100 Fragment 기준). 너무 빨리 끝나지 않되, 꾸준한 진전이 느껴지는 구간.

### 1.3 적용 시점

- `upgradeTree.applyToPlayer(ecs, playerId)` — 게임 시작 시 1회 호출
- 인런 레벨업과 **독립적** (중첩 적용, 곱연산)

---

## 2. 인런 레벨업 카드 (Level-Up Cards)

**파일**: `src/03_game/states/levelUp.lua`
**UI**: `src/03_game/scenes/levelUpScene.lua`

### 2.1 카드 풀 (10종)

| ID | 이름 | 카테고리 | 기본값 | 최대 선택 | 효과 | 감쇠 적용 |
|----|------|----------|--------|-----------|------|-----------|
| `fire_rate` | Fire Rate | Weapon | 0.20 | ∞ | 공격 속도 ×(1 + 0.20×factor) | O |
| `bullet_count` | Multi Shot | Weapon | +2 | **2** | 탄수 +2 (1→3→5) | **X** (고정) |
| `bullet_damage` | Damage | Weapon | +1 | ∞ | 탄환 데미지 +1 | **X** (고정) |
| `bullet_speed` | Bullet Speed | Weapon | 0.25 | ∞ | 탄속 ×(1 + 0.25×factor) | O |
| `attack_range` | Range | Weapon | 0.20 | ∞ | 사거리 ×(1 + 0.20×factor) | O |
| `max_hp` | Max HP +1 | Passive | +1 | ∞ | HP +1 | **X** (고정) |
| `move_speed` | Move Speed | Passive | 0.15 | ∞ | 이동속도 ×(1 + 0.15×factor) | O |
| `magnet_range` | Magnet | Passive | 0.30 | ∞ | XP 흡수 ×(1 + 0.30×factor) | O |
| `dash_cooldown` | Dash CD | Passive | 0.20 | ∞ | 대시 쿨 ×(1 - 0.20×factor) | O |
| `focus_energy` | Focus | Passive | 0.25 | ∞ | 에너지 ×(1+0.25×f), 충전 ×(1+0.15×f) | O |

### 2.2 감쇠 공식 (Diminishing Returns)

```
factor = 0.7 ^ pickCount
```

| 선택 횟수 | factor | 실효 비율 | 누적 효과 (Fire Rate 예시) |
|-----------|--------|-----------|---------------------------|
| 1회 | 1.00 | 100% | +20% |
| 2회 | 0.70 | 70% | +34% |
| 3회 | 0.49 | ~49% | +44% |
| 4회 | 0.34 | ~34% | +51% |
| 5회 | 0.24 | ~24% | +56% |

> **설계 의도**: 같은 카드 몰빵을 허용하되, 효율이 급격히 떨어져서 자연스럽게 다양한 빌드를 유도.

### 2.3 예외 카드 (감쇠 없음)
- **Multi Shot**: 고정 +2, 최대 2회 (5발 캡). 가장 강력한 카드이므로 횟수 제한.
- **Max HP**: 고정 +1. 방어 빌드 보장.
- **Damage**: 고정 +1. 단순 스택형.

### 2.4 카드 선택 로직
- 레벨업마다 **풀에서 랜덤 3장** 제시 (Fisher-Yates)
- `maxPicks` 도달한 카드는 풀에서 제거
- `levelUp.applyRandomUpgrade()` — Start Boost 패시브용 (업적 보상)

---

## 3. 업적 / 도전과제 (Achievements)

**파일**: `src/03_game/states/achievementSystem.lua`
**UI**: `src/03_game/scenes/achievementScene.lua`
**접근**: 타이틀 메뉴 ACHIEVEMENTS / A키

### 3.1 업적 일람 (6종)

| ID | 이름 | 조건 유형 | 목표 | 보상 유형 | 보상 ID | 보상 이름 |
|----|------|-----------|------|-----------|---------|-----------|
| `stage3_clear` | First Contact | stage_clear | 3 | weapon | spread_shot | Spread Shot |
| `stage6_clear` | Deep Dive | stage_clear | 6 | weapon | piercing | Piercing |
| `all_bosses` | Exterminator | all_bosses | 5 | character | debugger | Debugger |
| `total_kills_1000` | Massacre | total_kills | 1000 | character | compiler | Compiler |
| `total_fragments_500` | Data Hoarder | total_fragments | 500 | passive | start_boost | Start Boost |
| `system_exit_0` | SYSTEM.EXIT(0) | game_clear | 1 | passive | endless_unlock | Endless Mode |

### 3.2 추적 메커니즘

| 조건 유형 | 추적 방식 | 체크 시점 |
|-----------|-----------|-----------|
| `stage_clear` | `saveData.bestStage` | `onStageClear(stageNum)` |
| `all_bosses` | `saveData.bossesDefeated{}` 테이블 (5종) | `onBossDefeated(type)` |
| `total_kills` | 세션 `sessionKills` → 런 종료 시 `saveData.totalKills`에 합산 | `onRunEnd()` |
| `total_fragments` | `saveData.totalFragments` (누적, 소비해도 안 줄어듦) | `onRunEnd()` |
| `game_clear` | `saveData.gameClear` | `onVictory()` |

> **세이브 스커밍 방지**: `sessionKills`는 런 내에서만 카운트, `onRunEnd()` 시점에 영구 저장소로 flush.

### 3.3 보상 연쇄

```
[Stage 3 클리어] → Spread Shot 무기 해금
[Stage 6 클리어] → Piercing 무기 해금
[보스 5종 처치] → Debugger 캐릭터 해금
[누적 킬 1000] → Compiler 캐릭터 해금
[누적 Fragment 500] → Start Boost 패시브 (게임 시작 시 랜덤 레벨업 1회)
[게임 클리어] → Endless Mode 해금
```

- 무기/캐릭터: UI에 "(COMING SOON)" 표시 (미구현 상태)
- Start Boost: `achievementSystem.isRewardUnlocked("start_boost")` → `levelUp.applyRandomUpgrade()` 호출
- Endless Mode: 엔드리스 모드 선택 게이팅

---

## 4. Fragment 경제

### 4.1 수입원

| 원천 | 기본량 | Data Miner 보정 |
|------|--------|-----------------|
| 적 처치 시 드롭 | 1 | ×(1 + 0.05×upgradeLv) |
| 보스 처치 보상 | (적 드롭에 포함) | 동일 |

### 4.2 런당 추정 수입

| 구간 | 도달 스테이지 | 예상 Fragment | 비고 |
|------|-------------|--------------|------|
| 초반 (첫 1~3런) | 1~3 | 5~20 | 학습 구간 |
| 중반 (3~10런) | 4~8 | 30~100 | 첫 업그레이드 구매 시작 |
| 후반 (10런+) | 9~15 | 100~300+ | Data Miner 효과 체감 |
| 엔드게임 | 15+ / 클리어 | 200~500+ | 1000+ 적 처치 |

### 4.3 지출 분석

| 목표 | 필요량 | 예상 도달 시점 |
|------|--------|---------------|
| 첫 업그레이드 1개 | 10 | 1~2런 |
| 한 브랜치 Lv1 전부 | 30~40 | 3~5런 |
| 전체 Lv1 | ~105 | 5~8런 |
| 전체 Lv2 | ~105 + 225 = 330 | 10~15런 |
| **풀 업그레이드** | **525** | **15~25런** |
| Data Hoarder 업적 | 500 (누적) | ~15~20런 |

> **설계 노트**: Data Hoarder(500 누적)와 풀 업그레이드(525 소비) 시점이 비슷하게 겹치도록 의도. "다 모았다!" 성취감과 Start Boost 보상이 동시에 온다.

---

## 5. Codex 연동 계획

> **상태**: 🔲 미구현 (ROADMAP 6D.2)

### 5.1 기본 구상

현재 Codex는 53개 곡선을 **전부 해금 상태**로 보여줌. 향후 업적 시스템과 연결하여:

- **기본 해금**: 10~15개 대표 곡선 (Rose 3, Circle, Cardioid 등 기본형)
- **플레이 해금**: 나머지 곡선을 특정 조건 달성 시 해금
- **Deity 곡선**: deity 선택 시 자동 해금 (또는 deity 관련 업적)

### 5.2 해금 조건 후보 (초안)

| 해금 조건 유형 | 예시 | 대상 곡선 |
|---------------|------|-----------|
| 스테이지 진행 | Stage N 클리어 | 해당 스테이지에 등장하는 적 곡선 |
| 적 처치 수 | 특정 타입 N마리 처치 | 해당 적 타입의 곡선 |
| 보스 처치 | 보스 N 격파 | 보스 전용 곡선 |
| Deity 선택 | 해당 신으로 1회 플레이 | 신의 시그니처 곡선 |
| 누적 플레이 | N런 달성 | 희귀/복잡 곡선 |
| 히든 | 특수 조건 | Easter egg 곡선 |

### 5.3 해금 연출 (초안)

- 잠긴 곡선: 실루엣(어두운 윤곽) + 자물쇠 아이콘 + "???" 이름
- 해금 순간: 파티클 폭발 + 글로우 강화 + 곡선이 빠르게 그려지는 연출
- 해금 후: 정상 표시 + "NEW" 태그 (1회 열람 시 제거)

---

## 6. 세이브 데이터 구조

**파일**: `src/00_common/saveData.lua`
**저장 위치**: `save/progress.dat` (love.filesystem)

```lua
{
    fragments           = 0,        -- 현재 소비 가능 Fragment
    totalFragments      = 0,        -- 누적 Fragment (업적 추적용, 소비해도 안 줄어듦)
    upgrades            = {},       -- { [upgradeId] = level }  예: { atk_damage = 2 }
    bestScore           = 0,        -- 최장 생존 시간 (초)
    bestStage           = 0,        -- 최고 도달 스테이지
    totalRuns           = 0,        -- 총 플레이 횟수
    totalKills          = 0,        -- 누적 처치 수
    bossesDefeated      = {},       -- { ["NULL"]=true, ["STACK"]=true, ... }
    achievements        = {},       -- { ["stage3_clear"]=true, ... }
    selectedCharacter   = "default",-- 선택된 캐릭터
    tutorialDone        = false,    -- 튜토리얼 완료 플래그
    gameClear           = false,    -- 게임 클리어 플래그
    bestEndlessStage    = 0,        -- 엔드리스 최고 스테이지
}
```

### 6.1 핵심 분리 원칙

| 필드 | 용도 | 증가 | 감소 |
|------|------|------|------|
| `fragments` | 소비 가능 재화 | 적 처치 드롭 | 업그레이드 구매 |
| `totalFragments` | 업적 추적용 누적 | 적 처치 드롭 | **절대 안 줄어듦** |

> 이 분리 덕분에 "500 모으기" 업적은 "쓰면 리셋"되지 않음. 플레이어 친화적.

---

## 7. 밸런싱 로그

> 수치 변경 시 날짜, 변경 내용, 이유를 기록. 롤백 판단의 근거.

| 날짜 | 대상 | 변경 | 이유 |
|------|------|------|------|
| 2026-04-24 | (문서 초기 작성) | 현행 코드 기준 전체 기록 | 밸런싱 레퍼런스 구축 |

---

## 부록: 시스템 간 관계도

```
┌─────────────┐     게임 시작 시 적용     ┌─────────────────┐
│ Upgrade Tree │ ───────────────────────→ │ Player Stats    │
│ (영구)       │                          │ (ECS 컴포넌트)   │
└──────┬──────┘                          └────────┬────────┘
       │ Fragment 소비                            │ 인런 강화
       │                                         │
┌──────┴──────┐                          ┌────────┴────────┐
│ Fragment    │ ←── 적 처치 드롭 ──────── │ Level-Up Cards  │
│ Economy     │                          │ (인런, 감쇠)     │
└──────┬──────┘                          └─────────────────┘
       │ 누적 추적
       ▼
┌─────────────┐     보상 해금            ┌─────────────────┐
│ Achievement │ ───────────────────────→ │ Unlocks         │
│ System      │                          │ (무기/캐릭터/    │
└──────┬──────┘                          │  패시브/모드)    │
       │                                 └────────┬────────┘
       │ 해금 조건 (6D.2 예정)                     │
       ▼                                          │
┌─────────────┐                                   │
│ Codex       │ ←── 곡선 해금 연동 ────────────────┘
│ (곡선 도감)  │
└─────────────┘
```
