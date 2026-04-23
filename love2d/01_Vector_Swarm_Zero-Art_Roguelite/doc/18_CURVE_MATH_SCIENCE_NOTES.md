# 18_CURVE_MATH_SCIENCE_NOTES.md

## 목적

`curveDefs.lua`의 각 곡선에 대해 수학/과학 배경(기원, 의미, 발견 히스토리)을 기록한다.
스토리/아이템/업적 기획 시 "감성"뿐 아니라 "지식 근거"를 함께 제공하는 문서다.

주의:
- 일부 곡선은 고전 수학 명칭이 아닌 프로젝트 내 변형/관용 명칭이다.
- 그런 항목은 `확실도: Low` 또는 `검증 필요`로 표기했다.

---

## 표기 규칙

- 기원: 곡선이 처음 등장하거나 널리 정리된 역사적 맥락
- 의미: 수학적/기하학적 핵심 성질
- 발견 히스토리: 인물/시대/응용 맥락
- 확실도: `High / Medium / Low`

---

## A. Polar 계열

### Butterfly (Fay)
- 기원: Temple H. Fay가 1989년에 제시한 butterfly curve 계열
- 의미: 지수+삼각함수 결합으로 유기적 파형 형성
- 발견 히스토리: 현대 수학 시각화 커뮤니티에서 널리 확산
- 확실도: High

### Rose 5/4
- 기원: rhodonea(rose) 곡선 계열(17~18세기 고전 연구)
- 의미: `r = cos(k t)`에서 유리수 `k`로 장주기 꽃잎 생성
- 발견 히스토리: Grandi 계열 장미곡선 연구로 널리 알려짐
- 확실도: Medium

### Rose 3
- 기원: rhodonea 고전형
- 의미: 홀수 `k`에서 꽃잎 수가 `k`
- 발견 히스토리: 18세기 이후 교과 곡선으로 정착
- 확실도: High

### Rose 5
- 기원: rhodonea 고전형
- 의미: 5잎 대칭 꽃 패턴
- 발견 히스토리: 극좌표 곡선 표준 예제로 널리 사용
- 확실도: High

### Rose 7/3
- 기원: rose 변형(유리수 주파수)
- 의미: 정수 rose보다 긴 폐곡선 주기
- 발견 히스토리: 계산 시각화 시대에 활용 증가
- 확실도: Medium

### Rose 8/3
- 기원: rose 변형
- 의미: 고주파 꽃잎 + 장주기 결합
- 발견 히스토리: 파라미터 탐색 기반 생성형 패턴에서 자주 사용
- 확실도: Medium

### Cardioid
- 기원: 17~18세기 고전 곡선(명칭은 "심장형")
- 의미: 에피사이클로이드의 특수형으로도 해석 가능
- 발견 히스토리: 광학(카우스틱), 음향 반사 문제와 연결
- 확실도: High

### Lemniscate
- 기원: Jakob Bernoulli(17세기 말) 계열
- 의미: 무한대(∞) 형태의 대표적 쌍루프 곡선
- 발견 히스토리: 타원적분/복소해석 문맥으로 확장
- 확실도: High

### Limacon (inner loop)
- 기원: limacon of Pascal 계열(17세기)
- 의미: 파라미터에 따라 볼록/오목/내부루프 변화
- 발견 히스토리: 극좌표 곡선 분류의 대표 사례
- 확실도: High

### Logarithmic Spiral
- 기원: 17세기(Descartes/Bernoulli 문맥)
- 의미: 자기유사, 등각 성질(Spira mirabilis)
- 발견 히스토리: 자연계 성장 패턴(조개, 은하) 해석에 빈번
- 확실도: High

### Lituus
- 기원: 18세기 초 고전 곡선 문헌
- 의미: `r`이 각도 증가에 따라 역비례적으로 감소하는 수렴형
- 발견 히스토리: 고전 극좌표 곡선 분류에서 등장
- 확실도: Medium

### Folium (3-leaf)
- 기원: folium/rose 혼합 계열의 관용적 명칭
- 의미: 3엽 대칭을 강조한 파형
- 발견 히스토리: 프로젝트형 시각 패턴 네이밍 성격이 큼
- 확실도: Low (검증 필요)

### Wavy Circle (19:3)
- 기원: 원에 사인 변조를 준 현대 파생형
- 의미: 폐곡선 안정성과 표면 디테일을 동시에 확보
- 발견 히스토리: 생성형 그래픽/탄막 문양 설계에서 실용적
- 확실도: Medium

### Cissoid
- 기원: Cissoid of Diocles (고대 그리스)
- 의미: 특이점 근처 급격한 변형/분기 성질
- 발견 히스토리: 고전 작도/입방 문제와 연관
- 확실도: High

### Cayley's Sextic
- 기원: 19세기 Arthur Cayley 계열 대수곡선
- 의미: 6차 대수곡선의 복합 루프 성질
- 발견 히스토리: 대수기하학 문맥에서 연구
- 확실도: Medium

### Bifolium
- 기원: 고전 대수곡선 명칭으로 알려짐
- 의미: 이엽(두 잎) 구조
- 발견 히스토리: 정확한 최초 정식화는 문헌 확인 권장
- 확실도: Low (검증 필요)

### Quadrifolium
- 기원: 4엽 곡선 계열 관용 명칭
- 의미: 4중 대칭 잎 구조
- 발견 히스토리: 교육/시각화 문맥에서 널리 사용
- 확실도: Medium

### Freeth's Nephroid
- 기원: nephroid 변형 계열로 알려진 이름
- 의미: nephroid 파라미터 변조형
- 발견 히스토리: 원전 인물/연도는 별도 검증 권장
- 확실도: Low (검증 필요)

### Ophiuride
- 기원: 고전 곡선 명칭으로 전승된 계열
- 의미: 특이점 및 분기 성질이 강한 형태
- 발견 히스토리: 정확한 원전 표기는 검증 필요
- 확실도: Low (검증 필요)

---

## B. Parametric / Roulette 계열

### Astroid
- 기원: 17세기 이후 연구된 hypocycloid 특수형(4첨점)
- 의미: `x^(2/3)+y^(2/3)=a^(2/3)`로도 표현 가능
- 발견 히스토리: 광학 카우스틱/기하학 문제에 빈번
- 확실도: Medium

### Deltoid
- 기원: 3첨점 hypocycloid
- 의미: 내접 구름판(roulette) 운동에서 자연 발생
- 발견 히스토리: 고전 기하학 및 기구학 연구에 등장
- 확실도: High

### Superellipse (n=4)
- 기원: Lamé curve(19세기)
- 의미: 원과 사각형 사이 형태를 연속적으로 연결
- 발견 히스토리: 산업디자인(슈퍼엘립스)에서 재조명
- 확실도: High

### Epicycloid (k=3)
- 기원: roulette 곡선 고전 계열
- 의미: 원 위를 구르는 원의 자취
- 발견 히스토리: 기구학/기어 프로파일 연구와 연결
- 확실도: High

### Epicycloid (k=4)
- 기원: epicycloid 파라미터 확장
- 의미: 첨점 수 증가로 톱니 밀도 상승
- 발견 히스토리: 동일 family 확장형
- 확실도: High

### Epicycloid (k=5)
- 기원: epicycloid 파라미터 확장
- 의미: 고밀도 대칭 톱니 구조
- 발견 히스토리: 수학적 원형은 고전, 파라미터는 프로젝트 설정
- 확실도: High

### Epicycloid (k=6)
- 기원: epicycloid 파라미터 확장
- 의미: 높은 대칭 차수의 폐곡선
- 발견 히스토리: 학술 배경은 고전, 시각 응용은 현대
- 확실도: High

### Nephroid
- 기원: epicycloid/caustic 문맥의 고전 곡선
- 의미: 2첨점의 부드러운 쌍곡면 형태
- 발견 히스토리: 커피컵 반사 패턴 분석으로 대중화
- 확실도: High

### Heart Curve
- 기원: 여러 heart curve 중 하나의 대표 파라미터식
- 의미: 감성적 심장 실루엣을 갖는 닫힌 곡선
- 발견 히스토리: 현대 수학 시각화에서 표준 아이콘화
- 확실도: Medium

### Cornoid
- 기원: horn/cornoid 계열 명칭으로 알려진 변형
- 의미: 뿔형 왜곡을 갖는 매개변수 곡선
- 발견 히스토리: 정확 원전은 문헌 검증 권장
- 확실도: Low (검증 필요)

### Hypocycloid (k=5)
- 기원: hypocycloid 고전 계열
- 의미: 내접 구름 운동에서 생성되는 첨점 곡선
- 발견 히스토리: 기구학 및 치형 연구 연계
- 확실도: High

### Lissajous (3:2)
- 기원: Jules Lissajous (19세기)
- 의미: 서로 다른 주파수 비율의 직교 진동 궤적
- 발견 히스토리: 음향/진동 분석 실험에서 사용
- 확실도: High

### Lissajous (5:4)
- 기원: Lissajous family 확장
- 의미: 더 높은 주파수비의 복합 리듬 패턴
- 발견 히스토리: 오실로스코프 패턴으로 널리 알려짐
- 확실도: High

### Gerono Lemniscate
- 기원: Camille-Christophe Gerono(19세기) 계열
- 의미: 단순하고 안정적인 ∞ 형태
- 발견 히스토리: 교육용 곡선으로 널리 사용
- 확실도: Medium

### Booth's Lemniscate
- 기원: James Booth(19세기) 계열로 알려짐
- 의미: lemniscate의 두툼한 변형
- 발견 히스토리: 대수곡선 분류/시각화 문헌에 등장
- 확실도: Medium

### Hypotrochoid (R=7,r=3,d=2)
- 기원: roulette 계열의 hypotrochoid
- 의미: 파라미터 `d`로 팔 길이/밀도 조절 가능
- 발견 히스토리: 기어/스피로그래프 원리와 동일 계보
- 확실도: High

### Hypotrochoid (R=7,r=3,d=4)
- 기원: hypotrochoid 파라미터 확장
- 의미: `d` 증가로 외곽 진폭 강조
- 발견 히스토리: 동일 family 변형
- 확실도: High

### Epitrochoid (R=5,r=2,d=2)
- 기원: epitrochoid 고전 계열
- 의미: 외접 구름 운동 기반 확장 패턴
- 발견 히스토리: 기구학/생성 디자인에서 자주 활용
- 확실도: High

### Spirograph
- 기원: hypotrochoid/epitrochoid 원리를 대중화한 장난감(20세기)
- 의미: 정밀 주기 패턴의 대표 시각화
- 발견 히스토리: 교육/디자인 분야에서 폭넓게 사용
- 확실도: High

### Strophoid
- 기원: 고전 대수곡선(strophoid) 계열
- 의미: 꼬리형 분기 구조
- 발견 히스토리: 작도 및 대수기하 문맥에서 연구
- 확실도: Medium

### Bicorn
- 기원: bicorn 곡선 계열 명칭
- 의미: 이중 뿔 형태의 대칭 곡선
- 발견 히스토리: 정확 원전은 검증 필요
- 확실도: Low (검증 필요)

### Ranunculoid (k=5)
- 기원: 꽃잎형 곡선 관용 명칭(ranunculoid)
- 의미: 고차 조화항으로 꽃+기어형 결합
- 발견 히스토리: 문헌 표준화가 약해 프로젝트 네이밍 성격 포함
- 확실도: Low (검증 필요)

### Hypotrochoid (R=5,r=3,d=5)
- 기원: hypotrochoid 파라미터 변형
- 의미: 과장된 외곽 팔 구조
- 발견 히스토리: family 내부 파라미터 실험형
- 확실도: High

### Cassini Oval
- 기원: Giovanni Domenico Cassini(17세기)
- 의미: 초점까지 거리의 곱이 일정한 점의 자취
- 발견 히스토리: 천문 궤도 모형 논의와 함께 등장
- 확실도: High

### Epitrochoid (R=3,r=1,d=1)
- 기원: epitrochoid 파라미터형
- 의미: 균형 잡힌 외접 로터 패턴
- 발견 히스토리: family 기본형 프리셋
- 확실도: High

### Devil's Curve
- 기원: 고전 대수곡선 별칭으로 알려짐
- 의미: 분리 브랜치와 특이 구간이 공존
- 발견 히스토리: 정확한 명명/최초 제안은 별도 검증 권장
- 확실도: Low (검증 필요)

---

## C. Custom / Geometric Construction 계열

### Reuleaux Triangle
- 기원: Franz Reuleaux(19세기 기구학)
- 의미: 정폭도형(constant width) 대표 예시
- 발견 히스토리: 기계공학/설계/드릴 형상에서 응용
- 확실도: High

### Vesica Piscis
- 기원: 고대 기하 및 신성기하 전통
- 의미: 두 원의 교집합 렌즈형
- 발견 히스토리: 종교/상징/건축 문양으로 광범위 사용
- 확실도: High

### Kampyle of Eudoxus
- 기원: 고대 그리스(Eudoxus 관련 명칭 전승)
- 의미: 분리 브랜치 구조의 곡선
- 발견 히스토리: 고전 기하 텍스트에서 인용되나 원전 검증 권장
- 확실도: Medium

### Conchoid of Nicomedes
- 기원: Nicomedes (기원전 2세기)
- 의미: 선과 고정점 기준 거리 구성으로 생성
- 발견 히스토리: 각의 삼등분/작도 문제 문맥으로 유명
- 확실도: High

### Koch Edge (iter 2)
- 기원: Helge von Koch, 1904
- 의미: 프랙탈 경계(비정수 차원) 대표 사례
- 발견 히스토리: 현대 프랙탈 기하학의 출발점 중 하나
- 확실도: High

### Fermat Spiral
- 기원: Pierre de Fermat(17세기)
- 의미: 면적 균등 분포 성질로 씨앗 배열과 연결
- 발견 히스토리: 식물 배열(phyllotaxis) 설명에서 자주 등장
- 확실도: High

### Maurer Rose (n=6, d=71)
- 기원: Peter Maurer(1980s) 선분 연결 장미곡선
- 의미: rose 샘플 점들을 직선으로 연결해 격자 미학 생성
- 발견 히스토리: 컴퓨터 그래픽 시대에 널리 대중화
- 확실도: High

---

## D. 곡선별 수식 레퍼런스

실제 구현(`curveDefs.lua`) 기준으로 사용한 대표식을 기록한다.

### Polar / 반극좌표

- Butterfly (Fay): `r = e^(cos t) - 2 cos(4t) + sin^5(t/12)`
- Rose 5/4: `r = cos((5/4)t)`
- Rose 3: `r = cos(3t)`
- Rose 5: `r = cos(5t)`
- Rose 7/3: `r = cos((7/3)t)`
- Rose 8/3: `r = cos((8/3)t)`
- Cardioid: `r = 1 + cos t`
- Lemniscate: `r^2 = cos(2t)` (`cos(2t) < 0` 구간은 클램프)
- Limacon (inner loop): `r = 0.5 + cos t`
- Logarithmic Spiral: `r = 0.1 e^(0.15t)`
- Lituus: `r = 1/sqrt(t)`
- Folium (3-leaf): `r = cos t * sin(2t)`
- Wavy Circle (19:3): `r = 0.9 + 0.1 sin((19/3)t)`
- Cissoid (project form): `r = sin^2(t)/cos(t)`
- Cayley's Sextic (project polar form): `r = 4 cos^3(t/3)`
- Bifolium (project form): `r = sin(t)cos^2(t)`
- Quadrifolium: `r = cos(2t)`
- Freeth's Nephroid (project form): `r = (1 + 2 sin(t/2))/3`
- Ophiuride (project form): `r = (sin t - 0.5)tan t`
- Cassini Oval (implicit): `(x^2+y^2)^2 - 2a^2(x^2-y^2) + a^4 - b^4 = 0`

### Parametric / Roulette

- Astroid: `x = cos^3 t, y = sin^3 t`
- Deltoid (normalized): `x = (2cos t + cos 2t)/3, y = (2sin t - sin 2t)/3`
- Superellipse (n=4, project form): `x = sign(cos t)|cos t|^(1/2), y = sign(sin t)|sin t|^(1/2)`
- Epicycloid (k=3): `R=1, r=1/3`
	- `x = (R+r)cos t - r cos((R+r)t/r)`
	- `y = (R+r)sin t - r sin((R+r)t/r)`
- Epicycloid (k=4): `R=1, r=1/4` (동일식)
- Epicycloid (k=5): `R=1, r=1/5` (동일식)
- Epicycloid (k=6): `R=1, r=1/6` (동일식)
- Nephroid (normalized project form): `x = (3cos t - cos 3t)/4, y = (3sin t - sin 3t)/4`
- Heart Curve (project form):
	- `x = 16 sin^3 t / 17`
	- `y = (13cos t - 5cos 2t - 2cos 3t - cos 4t)/17`
- Cornoid (project form): `x = cos t(1-2sin^2 t), y = sin t(1+2cos^2 t)`
- Hypocycloid (k=5, normalized): `x = (4cos t + cos 4t)/5, y = (4sin t - sin 4t)/5`
- Lissajous (3:2): `x = cos 3t, y = sin 2t`
- Lissajous (5:4): `x = cos 5t, y = sin 4t`
- Gerono Lemniscate: `x = cos t, y = sin t cos t`
- Booth's Lemniscate (project form): `x = cos t/(1+sin^2 t), y = sin t cos t/(1+sin^2 t)`
- Hypotrochoid (R=7,r=3,d=2):
	- `x = ((R-r)cos t + d cos((R-r)t/r))/((R-r)+d)`
	- `y = ((R-r)sin t - d sin((R-r)t/r))/((R-r)+d)`
- Hypotrochoid (R=7,r=3,d=4): 동일식, `d=4`
- Epitrochoid (R=5,r=2,d=2):
	- `x = ((R+r)cos t - d cos((R+r)t/r))/((R+r)+d)`
	- `y = ((R+r)sin t - d sin((R+r)t/r))/((R+r)+d)`
- Spirograph (project preset): `R=1, r=0.4, d=0.6`
	- `x = (R-r)cos t + d cos((R-r)t/r)`
	- `y = (R-r)sin t - d sin((R-r)t/r)`
- Strophoid (project param form): `x=(1-t^2)/(1+t^2), y=t(1-t^2)/(1+t^2)`
- Bicorn (project param form): `x=cos t, y=sin^2 t/(2+sin t)`
- Ranunculoid (k=5, normalized project form):
	- `x = (6cos t - cos 6t)/7`
	- `y = (6sin t - sin 6t)/7`
- Hypotrochoid (R=5,r=3,d=5, normalized project form):
	- `x = (2cos t + 5cos(2t/3))/7`
	- `y = (2sin t - 5sin(2t/3))/7`
- Epitrochoid (R=3,r=1,d=1, normalized):
	- `x = ((R+r)cos t - d cos((R+r)t/r))/((R+r)+d)`
	- `y = ((R+r)sin t - d sin((R+r)t/r))/((R+r)+d)`

### Custom / Geometric Construction

- Reuleaux Triangle: 정삼각형 꼭짓점을 중심으로 반지름 `sqrt(3)` 원호 3개를 연결
- Vesica Piscis: 중심 간 거리 `d=0.5`인 두 단위원 교집합 경계
- Kampyle of Eudoxus (project form): `x = sec t, y = tan t sec t` (스케일/branch 분리)
- Conchoid of Nicomedes (project polar form): `r = a/cos t + b`
- Devil's Curve (implicit): `y^2 = x^2(x^2-1)/(x^2+1)` (상/하 branch 샘플링)
- Koch Edge (iter 2): 선분 1/3 분할 + 정삼각 돌출 재귀 2회
- Fermat Spiral: `r = ±sqrt(t)`
- Maurer Rose (n=6,d=71):
	- rose 샘플: `r = sin(6t)`
	- 선분 연결 각: `t_k = k * d * pi / 180`, `d=71`

---

## 부록: 스토리 설계 시 권장 연결

- High 확실도 곡선: 메인 퀘스트/핵심 업적에 사용
- Medium 곡선: 서브 퀘스트/지역 파벌 설정에 사용
- Low 곡선: "금서", "분실 문헌", "해석 불가 룬" 같은 메타 설정으로 사용

이렇게 하면 지식 정확도와 세계관 몰입을 동시에 확보할 수 있다.
