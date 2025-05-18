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
            fireTimer -= Time.deltaTime;
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

    public void Move(float input)
    {
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

    public void TryShoot()
    {
        if (fireTimer > 0) return;

        fireTimer = fireDelay;

        Shoot();
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

        StartCoroutine(InvincibilityCoroutine());
    }

    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        StartCoroutine(FlashInvincibility());
        yield return new WaitForSeconds(1.5f);
        isInvincible = false; 
    }

    private IEnumerator FlashInvincibility()
    {
        float flashDuration = 0.1f; 
        float totalInvincibleTime = 1.5f;
        float timeElapsed = 0f;

        isInvincible = true;
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        while (timeElapsed < totalInvincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashDuration);
            timeElapsed += flashDuration;
        }

        spriteRenderer.enabled = true;
        rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        isInvincible = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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