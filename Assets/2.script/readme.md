# 2D Timer-Based Collection Game System

## Overview
Unity 기반의 2D 타이머 게임 시스템을 구현. 제한 시간 내에 보석을 수집하고 몬스터를 피하는 게임플레이를 구현하며, 몬스터 자동 생성과 카메라 추적, 게임 진행 및 결과 표시까지 완벽한 게임 사이클을 제공

## 핵심 컴포넌트

### PlayerController2.cs
플레이어 캐릭터의 기본적인 동작과 상태를 관리한다. 4방향 이동 시스템을 구현하고, 보석 수집 메커니즘을 포함한다. 애니메이터 컴포넌트와 연동하여 캐릭터의 움직임에 따른 적절한 애니메이션을 재생하며, 수집된 보석의 개수를 UI에 실시간으로 반영한다.

### MonsterController.cs
몬스터의 자동 이동과 상태를 관리한다. 지정된 방향으로 상하 움직임을 수행하고, 충돌 시 비활성화되며 리스폰 시스템을 통해 재등장한다. 애니메이터 컴포넌트를 통해 이동 방향에 따른 애니메이션을 제공한다.

### MonsterSpawner.cs
게임 내 몬스터 생성을 담당한다. 설정된 간격으로 지정된 위치에서 몬스터를 생성하고, 프리팹 시스템을 활용하여 효율적인 몬스터 관리를 수행한다.

### CameraController.cs
플레이어를 자동으로 추적하는 카메라 시스템을 구현한다. 부드러운 추적 움직임을 제공하고, 게임 화면이 항상 플레이어를 중심으로 유지되도록 한다.

### TimerController.cs
게임의 시간 제한을 관리한다. UI를 통해 남은 시간을 표시하고, 시간 종료 시 게임 결과 화면으로 자동 전환한다. 플레이어의 수집 결과를 저장하고 다음 씬으로 전달한다.

### ResultController.cs
게임 결과를 표시하고 관리한다. PlayerPrefs를 활용하여 저장된 보석 수집 결과를 화면에 표시하고, 게임의 최종 상태를 보여준다.

### StartButtonController.cs
게임의 시작을 관리한다. 시작 버튼 클릭 시 게임 씬으로 전환하고, 게임의 초기 상태를 설정한다.

## 시스템 요구사항

### 필수 컴포넌트
- Animator: 캐릭터 애니메이션 제어
- Rigidbody2D: 물리 기반 이동 처리
- Collider2D: 충돌 감지
- UI Canvas: 게임 상태 표시

### 애니메이션 상태
플레이어와 몬스터에 다음과 같은 애니메이션 상태를 구현한다:
- idle: 정지 상태 (플레이어)
- walk: 좌우 이동 (플레이어)
- up: 위로 이동
- down: 아래로 이동

## 게임 흐름

게임은 다음과 같은 순서로 진행된다:

1. 시작 화면에서 게임을 시작한다
2. 타이머 시작과 함께 플레이어 조작이 가능해진다
3. 제한 시간 내 보석 수집을 진행한다
4. 몬스터를 회피하며 게임을 진행한다
5. 시간 종료 시 결과 화면으로 전환된다
6. 최종 점수를 확인하고 재시작이 가능하다

StartButtonController로 게임 시작
TimerController에서 제한 시간 설정
PlayerController2로 플레이어 이동 제어
MonsterSpawner로 적 생성 관리

시간 종료 시 자동으로 결과 화면으로 전환
ResultController가 최종 점수 표시
재시작 또는 메인 메뉴로 이동 가능


## 주요 특징

모듈화된 컴포넌트 구조
PlayerPrefs를 활용한 데이터 저장
씬 기반 게임 흐름 관리
직관적인 UI 시스템
