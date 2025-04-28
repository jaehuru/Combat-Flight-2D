✈️ Combat_Flight_2D
프로젝트 개요

항목 | 내용
장르 | 2D 비행 슈팅 게임
개발 환경 | Unity 6000.0.47f1 / Visual Studio 2022 / Windows

주요 시스템
🎯 GameManager (Singleton Pattern)
전역 게임 상태 및 흐름 관리

게임 오버, 승리, 리스폰 처리

BGM, 이펙트 볼륨 조정 기능 제공

💾 Object Pooling System
오브젝트 생성/파괴 비용 감소

메모리 할당 최소화

GC(가비지 컬렉션) 부하 방지

⏳ Coroutine-based Timing
시간 기반 이벤트 흐름 (리스폰 대기, 보스 등장 연출)

부드러운 논블로킹 비동기 처리

