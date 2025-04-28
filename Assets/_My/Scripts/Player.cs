using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    [SerializeField] float moveSpeed = 4;
    [SerializeField] GameObject spawnPosition;

    [SerializeField] float leftLimit = -5;
    [SerializeField] float rightLimit = 5;

    Animator animator;

    float fireDelay = 0.3f;
    float fireTimer;
    bool triggerPull = false;

    GameManager gameManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
