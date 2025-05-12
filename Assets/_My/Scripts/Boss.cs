using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;

    private int health = 50; // ���� ü��
    private bool isDead = false; // ������ �׾����� ����
    
    [SerializeField] private float laserInterval = 1f; // ������ �߻� ����
    [SerializeField] private float laserSpeed = 8f; // ������ �ӵ�

    GameManager gameManager;
    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        gameManager = GameManager.instance;

        // Rigidbody2D�� Kinematic���� �����Ͽ� ������ ������ ����
        rigidbody2D.bodyType = RigidbodyType2D.Kinematic;

        StartCoroutine(ShootLasers()); // ������ �߻� ����
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ShootLasers()
    {
        while (!isDead)
        {
            // ������ X ��ġ���� ������ �߻�
            for (int i = 0; i < 6; i++)
            {
                float randomX = Random.Range(-5f, 5f); // X �� ���� ����
                ShootLaser(new Vector2(randomX, transform.position.y));
            }

            yield return new WaitForSeconds(laserInterval); // ������ �߻� ����
        }
    }

    private void ShootLaser(Vector2 position)
    {
        // PoolManager�� ���� ������ ��û
        GameObject laser = gameManager.poolManager.RequestLaserObject(PoolManager.ObjectType.EnemyLaser);
        laser.transform.position = position; // ������ ��ġ ����

        Rigidbody2D laserRb = laser.GetComponent<Rigidbody2D>();
        laser.SetActive(true); // ������ Ȱ��ȭ

        // �������� �Ʒ��� �̵��ϵ��� ����
        laserRb.linearVelocity = new Vector2(0, -laserSpeed);
    }

    // �������� ������ �¾��� �� ó��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerLaser"))
        {
            health -= 1;  // ���� ü�� ����

            if (health <= 0)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true; // ������ �׾��ٰ� ����
        animator.SetBool("Die", true);  // "Die"��� bool�� true�� �����Ͽ� �ִϸ��̼� ����
        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        // �ִϸ��̼��� ���̸�ŭ ��� (�ִϸ��̼��� ���̴� �ִϸ��̼� Ŭ���� ���� �ð��� ���� ����)
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        // ���� ��Ȱ��ȭ
        gameObject.SetActive(false);
        gameManager.Victory();
    }
}
