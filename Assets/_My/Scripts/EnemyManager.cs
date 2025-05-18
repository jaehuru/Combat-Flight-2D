using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;  // ���� Ÿ���� ����
    [SerializeField] private GameObject bossPrefab;      // ����

    [SerializeField] private float spawnInterval = 3f; // �׷� ���� ����
    [SerializeField] private int minEnemiesPerGroup = 3;
    [SerializeField] private int maxEnemiesPerGroup = 4;

    [SerializeField] private float spawnXMin = -5f;
    [SerializeField] private float spawnXMax = 5f;
    [SerializeField] private float spawnY = 9f; // ȭ�� ������ ����

    float bottomMostPosition;
    float bottomLimit = -7;

    private int totalScore = 0;
    private int bossSpawnScore = 200;
    private bool bossSpawned = false;

    // ��ü Ǯ�� ������ ����Ʈ
    private List<GameObject> enemyPool = new List<GameObject>();
    private int poolSize = 12;  // Ǯ ������ �ּ� 12��

    // GameManager�� �ν��Ͻ� ����
    private GameManager gameManager;

    // ������ �����ϴ� ��ƾ
    private void Start()
    {
        gameManager = GameManager.instance;
        // Ǯ �ʱ�ȭ
        InitializeEnemyPool();
        StartCoroutine(SpawnEnemiesRoutine());
    }

    // �� Ǯ �ʱ�ȭ
    private void InitializeEnemyPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);  // �� ������ �� �ϳ��� �������� ����
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex]);
            enemy.SetActive(false);  // ó������ ��Ȱ��ȭ ���·�
            enemyPool.Add(enemy);    // Ǯ�� �߰�
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (!bossSpawned)
        {
            int spawnCount = Random.Range(minEnemiesPerGroup, maxEnemiesPerGroup + 1);  // ���� ��
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.3f); // ���鳢�� �ð��� �α�
            }

            yield return new WaitForSeconds(spawnInterval); // �׷� �� �ð� ��
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = GetEnemyFromPool();  // Ǯ���� ���� ������

        if (enemy != null)
        {
            // ��ġ ����
            float randomX = Random.Range(spawnXMin, spawnXMax);
            enemy.transform.position = new Vector2(randomX, spawnY);

            // �� �ʱ�ȭ
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.InitializeEnemy();  // �� �ʱ�ȭ
            }

            enemy.SetActive(true);  // ���� Ȱ��ȭ
        }
    }

    // Ǯ���� �� ��ü�� �������� �Լ�
    private GameObject GetEnemyFromPool()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)  // ��Ȱ��ȭ�� ���� ã��
            {
                return enemy;
            }
        }

        // ��� ���� Ȱ��ȭ�Ǿ� ������, �� ���� �߰�
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex]);
        newEnemy.SetActive(false);  // Ǯ�� �߰��� �� ��Ȱ��ȭ
        enemyPool.Add(newEnemy);  // Ǯ�� �� ���� �߰�
        return newEnemy;
    }

    // ���� �߰� �Լ�
    public void AddScore(int amount)
    {
        totalScore += amount;
        if (totalScore >= bossSpawnScore && !bossSpawned)
        {
            bossSpawned = true;
            gameManager.BossSpawnText();
            StartCoroutine(SpawnBoss());
        }
    }

    private IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(5f);  // ���� ���� �� ������
        GameObject boss = Instantiate(bossPrefab);
        boss.transform.position = new Vector2(0, spawnY - 1f);  // ���� ��ġ ����
    }

    private bool CheckEnemyVictory()
    {
        return bottomMostPosition <= bottomLimit;
    }
}
