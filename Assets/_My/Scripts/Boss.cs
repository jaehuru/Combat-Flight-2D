using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;

    private int health = 50; // 보스 체력
    private bool isDead = false; // 보스가 죽었는지 여부
    
    [SerializeField] private GameObject laserPrefab; // 레이저 프리팹
    [SerializeField] private float laserInterval = 1f; // 레이저 발사 간격
    [SerializeField] private float laserSpeed = 8f; // 레이저 속도

    GameManager gameManager;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = GameManager.instance;

        // Rigidbody2D를 Kinematic으로 설정하여 물리적 반응을 막음
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(ShootLasers()); // 레이저 발사 시작
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShootLasers()
    {
        while (!isDead)
        {
            // 랜덤한 X 위치에서 레이저 발사
            for (int i = 0; i < 6; i++)
            {
                float randomX = Random.Range(-5f, 5f); // X 값 범위 조정
                ShootLaser(new Vector2(randomX, transform.position.y));
            }

            yield return new WaitForSeconds(laserInterval); // 레이저 발사 간격
        }
    }

    private void ShootLaser(Vector2 position)
    {
        GameObject laser = Instantiate(laserPrefab, position, Quaternion.identity);
        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();

        // 레이저가 아래로 이동하도록 설정
        laserRb.linearVelocity = new Vector2(0, -laserSpeed);
    }

    // 레이저가 보스를 맞았을 때 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            health -= 1;  // 보스 체력 차감

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true; // 보스가 죽었다고 설정
        animator.SetBool("Die", true);  // "Die"라는 bool을 true로 설정하여 애니메이션 시작
        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        // 애니메이션의 길이만큼 대기 (애니메이션의 길이는 애니메이션 클립의 지속 시간에 맞춰 설정)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        // 보스 비활성화
        gameObject.SetActive(false);
        gameManager.Victory();
    }
}
