using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private Vector2 moveDirection;
    new Rigidbody2D rigidbody2D;

    GameManager gameManager;
    EnemyManager enemyManager; // EnemyManager 참조
    PoolManager poolManager;

    int scoreValue = 10;

    public float attackInterval = 3f;  // 공격 간격 (3초)
    private bool canAttack = true;     // 공격 가능 여부
    private Coroutine attackCoroutine;  // AttackRoutine을 제어할 변수

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        gameManager = GameManager.instance;
        poolManager = GameManager.instance.poolManager; // PoolManager 가져오기
        enemyManager = GameManager.instance.enemyManager;  // EnemyManager 초기화
    }

    void OnEnable()
    {
        canAttack = true;  // 공격 가능 상태로 설정
                           // 공격 코루틴이 이미 실행 중이면 먼저 중지하고, 다시 시작
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine(AttackRoutine());  // 코루틴 시작
    }

    void OnDisable()
    {
        // 적이 비활성화되면 코루틴 중지
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    void Start()
    {
        InitializeEnemy();  // 이동 방향 초기화만
    }

    // 적 초기화 함수
    public void InitializeEnemy()
    {
        moveDirection = new Vector2(0, -1);  // 기본적으로 아래로 이동
        // 추가적인 초기화 로직이 필요하다면 여기에 작성
    }

    private void Update()
    {
        // 이동 처리
        transform.Translate(moveDirection * Time.deltaTime);
    }

    // 적이 화면을 벗어나면 풀로 반환 (비활성화)
    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);  // 화면을 벗어난 적을 비활성화
    }

    // 공격을 위한 코루틴
    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            canAttack = true;  // 공격 가능 상태로 설정
            if (canAttack)
            {
                Shoot();  // 공격 메서드 호출
                canAttack = false;  // 공격 후 대기
                yield return new WaitForSeconds(attackInterval);  // 공격 간격만큼 대기
                canAttack = true;   // 다시 공격 가능
            }
            yield return null; // 매 프레임마다 대기
        }
    }

    private void Shoot()
    {
        GameObject laserObj = gameManager.poolManager.RequestLaserObject(PoolManager.ObjectType.EnemyLaser);
        laserObj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            gameManager.AddScore(scoreValue);
            enemyManager.AddScore(scoreValue); // EnemyManager에 점수 추가

            gameObject.SetActive(false);
            StartDestroyEffect();
        }
    }

    private void StartDestroyEffect()
    {
        GameObject destroyEffect = gameManager.poolManager.RequestLaserObject(PoolManager.ObjectType.DestroyEffect);

        destroyEffect.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }
}
