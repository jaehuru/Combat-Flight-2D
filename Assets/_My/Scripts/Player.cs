using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Player : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 4;
    [SerializeField] GameObject spawnPosition;

    [SerializeField] float leftLimit = -5;
    [SerializeField] float rightLimit = 5;

    Animator animator;
    SpriteRenderer spriteRenderer;

    float fireDelay = 0.3f;
    float fireTimer;
    bool triggerPull = false;

    GameManager gameManager;

    private bool isInvincible = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = GameManager.instance;

        ReSpawn();
    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer > 0)
        {
            fireTimer -= Time.deltaTime;
            return;
        }

        if (triggerPull)
        {
            triggerPull = false;
            Shoot();
        }
    }

    private void FixedUpdate()
    {
        Vector2 playerPosition = transform.position;

        if (playerPosition.x > rightLimit)
        {
            playerPosition.x = rightLimit;
        }
        else if (playerPosition.x < leftLimit)
        {
            playerPosition.x = leftLimit;
        }

        transform.position = playerPosition;
    }

    public void OnMove(InputValue inputValue)
    {
        float input = inputValue.Get<Vector2>().x;

        if (input > 0)
        {
            animator.SetBool("Right", true);
            animator.SetBool("Left", false);
        }
        else if (input < 0)
        {
            animator.SetBool("Left", true);
            animator.SetBool("Right", false);
        }


        if (Mathf.Abs(input) > 0)
        {
            rigidbody2D.linearVelocity = input * Vector2.right * moveSpeed;
        }
        else
        {
            rigidbody2D.linearVelocity = Vector2.zero;
            animator.SetBool("Left", false);
            animator.SetBool("Right", false);
        }
    }
    public void OnShoot()
    {
        triggerPull = true;
    }

    private void Shoot()
    {
        fireTimer = fireDelay;

        GameObject laserObj = gameManager.poolManager.RequestLaserObject(PoolManager.ObjectType.PlayerLaser);
        laserObj.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }

    public void ReSpawn()
    {
        rigidbody2D.linearVelocity = Vector2.zero;
        transform.position = spawnPosition.transform.position;

        // 부활 후 무적 상태 1.5초
        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;  // 무적 상태로 설정
        StartCoroutine(FlashInvincibility());
        yield return new WaitForSeconds(1.5f);  // 1.5초 대기
        isInvincible = false;  // 무적 상태 해제
    }

    private IEnumerator FlashInvincibility()
    {
        float flashDuration = 0.1f;  // 깜빡이는 간격
        float totalInvincibleTime = 1.5f;
        float timeElapsed = 0f;

        // 무적 시작: 물리 반응 끄기
        isInvincible = true;
        rigidbody2D.isKinematic = true;

        while (timeElapsed < totalInvincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashDuration);
            timeElapsed += flashDuration;
        }

        // 무적 끝: 다시 물리 반응 켜기
        spriteRenderer.enabled = true;
        rigidbody2D.isKinematic = false;
        isInvincible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 무적 상태일 때는 충돌 처리 무시
        if (isInvincible) return;

        if (collision.gameObject.CompareTag("EnemyLaser"))
        {
            StartDestroyEffect();
            gameObject.SetActive(false);
            gameManager.PlayerDestruction();
        }
    }

    private void StartDestroyEffect()
    {
        GameObject destroyEffect = gameManager.poolManager.RequestLaserObject(PoolManager.ObjectType.DestroyEffect);
        destroyEffect.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
    }
}
