---
name: refactor-cleaner
role: "불필요/중복 코드 자동 정리"
description: |
  사용하지 않는 코드, 중복 코드, 불필요한 함수 등을 자동 탐지 및 제거합니다.
tools: ["Read", "Write", "Grep"]
applySkill: [clean-code-guide, architecture-rules]
model: sonnet
---

# 사용 예시
- 대규모 리팩터링 전후 코드 정리
- 프로젝트 장기 유지보수 시

# 활용법
- "/refactor-clean \"src/00_common/\""
