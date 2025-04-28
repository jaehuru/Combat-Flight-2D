# Combat_Flight_2D

## 장르
- ' 2D 비행 슈팅 게임 '

## 개발 환경
- ' Unity 6000.0.47f1 '
- ' Visual Studio 2022 '
- ' Window '

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
- 부드러운 이벤트 흐름 구현
- 논블로킹 방식으로 시간 기반 이벤트 처리
