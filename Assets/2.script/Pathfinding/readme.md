# A* Pathfinding 2D Grid Game Controller

##  Overview
2D 그리드 기반 게임을 위한 A* 경로찾기와 플레이어 컨트롤러 시스템입니다. 마우스 클릭으로 캐릭터를 이동시키고, 장애물을 피하며, 보석을 수집하는 게임플레이를 구현합니다.

## 주요 기능

### A* Pathfinding (경로찾기)
- 4방향 이동이 가능한 그리드 기반 경로찾기
- 장애물을 고려한 최적 경로 탐색
- 맨해튼 거리를 사용한 휴리스틱 적용

### Player Controller
- 마우스 클릭 기반 이동 시스템
- 부드러운 캐릭터 이동과 애니메이션
- 보석 수집과 적 회피 게임플레이

##  구성 요소

### Pathfinding.cs
- Node 클래스: 경로탐색을 위한 그리드 노드
- FindPath: A* 알고리즘 구현
- 장애물 회피와 최단경로 계산

### PlayerControllerWithPathfinding.cs
- 캐릭터 이동과 애니메이션 제어
- 충돌 처리 (적, 보석)
- 게임플레이 상태 관리

##  설정 방법
1. Pathfinding 컴포넌트를 플레이어 오브젝트에 추가
2. PlayerControllerWithPathfinding 컴포넌트 설정:
   - Move Speed: 이동 속도
   - Obstacle Layer: 장애물 레이어
   - Animator: 캐릭터 애니메이터

##  주요 특징
- Unity Vector2Int를 활용한 2D 그리드 시스템
- 모듈화된 구조로 확장 용이
- 최적화된 경로탐색 알고리즘
- 직관적인 컨트롤 시스템

##  애니메이션 요구사항
- idle: 정지 상태
- walk: 좌우 이동
- up: 위로 이동
- down: 아래로 이동

##  게임플레이 구현
- 마우스 클릭으로 목적지 설정
- 자동 경로탐색 및 이동
- 보석 수집 시스템
- 적과 충돌 시 시작 지점으로 귀환
