using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;  // 여러 타입의 적들
    [SerializeField] private GameObject bossPrefab;      // 보스

    [SerializeField] private float spawnInterval = 3f; // 그룹 스폰 간격
    [SerializeField] private int minEnemiesPerGroup = 3;
    [SerializeField] private int maxEnemiesPerGroup = 4;

    [SerializeField] private float spawnXMin = -5f;
    [SerializeField] private float spawnXMax = 5f;
    [SerializeField] private float spawnY = 9f; // 화면 위에서 시작

    float bottomMostPosition;
    float bottomLimit = -7;

    private int totalScore = 0;
    private int bossSpawnScore = 200;
    private bool bossSpawned = false;

    // 객체 풀로 관리할 리스트
    private List<GameObject> enemyPool = new List<GameObject>();
    private int poolSize = 12;  // 풀 사이즈 최소 12개

    // GameManager의 인스턴스 참조
    private GameManager gameManager;

    // 적들을 스폰하는 루틴
    private void Start()
    {
        gameManager = GameManager.instance;
        // 풀 초기화
        InitializeEnemyPool();
        StartCoroutine(SpawnEnemiesRoutine());
    }

    // 적 풀 초기화
    private void InitializeEnemyPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            int randomIndex = Random.Range(0, enemyPrefabs.Length);  // 적 프리팹 중 하나를 랜덤으로 선택
            GameObject enemy = Instantiate(enemyPrefabs[randomIndex]);
            enemy.SetActive(false);  // 처음에는 비활성화 상태로
            enemyPool.Add(enemy);    // 풀에 추가
        }
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (!bossSpawned)
        {
            int spawnCount = Random.Range(minEnemiesPerGroup, maxEnemiesPerGroup + 1);  // 적군 수
            for (int i = 0; i < spawnCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.3f); // 적들끼리 시간차 두기
            }

            yield return new WaitForSeconds(spawnInterval); // 그룹 간 시간 차
        }
    }

    private void SpawnEnemy()
    {
        GameObject enemy = GetEnemyFromPool();  // 풀에서 적을 꺼내기

        if (enemy != null)
        {
            // 위치 설정
            float randomX = Random.Range(spawnXMin, spawnXMax);
            enemy.transform.position = new Vector2(randomX, spawnY);

            // 적 초기화
            Enemy enemyScript = enemy.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.InitializeEnemy();  // 적 초기화
            }

            enemy.SetActive(true);  // 적을 활성화
        }
    }

    // 풀에서 적 객체를 가져오는 함수
    private GameObject GetEnemyFromPool()
    {
        foreach (var enemy in enemyPool)
        {
            if (!enemy.activeInHierarchy)  // 비활성화된 적을 찾음
            {
                return enemy;
            }
        }

        // 모든 적이 활성화되어 있으면, 새 적을 추가
        int randomIndex = Random.Range(0, enemyPrefabs.Length);
        GameObject newEnemy = Instantiate(enemyPrefabs[randomIndex]);
        newEnemy.SetActive(false);  // 풀에 추가된 후 비활성화
        enemyPool.Add(newEnemy);  // 풀에 새 적을 추가
        return newEnemy;
    }

    // 점수 추가 함수
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
        yield return new WaitForSeconds(5f);  // 보스 등장 전 딜레이
        GameObject boss = Instantiate(bossPrefab);
        boss.transform.position = new Vector2(0, spawnY - 1f);  // 보스 위치 설정
    }

    private bool CheckEnemyVictory()
    {
        return bottomMostPosition <= bottomLimit;
    }
}
