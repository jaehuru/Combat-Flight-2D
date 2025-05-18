# Combat_Flight_2D

## 장르
- ' 2D 비행 슈팅 게임 '

## 개발 환경
- ' Unity 6000.0.47f1 '
- ' Rider '
- ' MacOS '

## 호환 OS
- ' iOS '

## 주요 기능
1. GameManager (Singleton Pattern)
  - 전역 게임 상태 관리
  - 싱글톤 패턴을 적용해 어디서든 접근 가능

2. Object Pooling System
  - PoolManager를 통한 반복 생성/삭제 최소화 (오브젝트 생성 비용 감소)
  - 메모리 할당 및 가비지 컬렉션(GC) 부담 완화

3. Coroutine-based Timing
  - 플레이어 리스폰 딜레이
  - 보스 등장 전 경고 텍스트 표시
  - 논블로킹 방식으로 시간 기반 이벤트 처리

4. 모바일 환경에 최적화된 터치 기반 UI 및 조작 시스템

## 기억에 남는 버그 및 해결 과정
적(Enemy) 공격 중단 버그
시간이 경과할수록 적들이 더 이상 공격을 하지 않는 현상 발생
적(Enemy)이 공격 도중 즉시 사망할 경우, 공격을 담당하는 코루틴이 while문을 순회하던 중 canAttack 변수가 false인 상태로 남게 되는 문제를 발견
공격 루프(while문) 진입 시마다 canAttack = true로 강제 초기화를 통해 문제 해결
