using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public PoolManager poolManager;
    public EnemyManager enemyManager;

    [SerializeField] Renderer bgRenderer;
    const float bgSpeed = 0.5f;

    [SerializeField] AudioClip bgmList;
    AudioSource audioSource;
    public float bgmVolume = 1;
    public float effectVolume = 1;

    [SerializeField] Player player;
    int totalLives = 3;
    int currentLives;

    int currentScore = 0;

    public GameObject gameOverText;
    public GameObject victoryText;
    public GameObject bossText;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgmList;
        audioSource.loop = true;
        audioSource.Play();

        ResetLives();
    }

    // Update is called once per frame
    void Update()
    {
        BGScrolling();

        VolumeSetting();
    }

    private void BGScrolling()
    {
        float move = Time.deltaTime * bgSpeed;

        bgRenderer.material.mainTextureOffset += Vector2.down * move;
    }

    private void VolumeSetting()
    {
        audioSource.volume = bgmVolume;
    }

    private void ResetLives()
    {
        currentLives = totalLives;
    }

    public void PlayerDestruction()
    {
        currentLives--;

        Debug.Log(currentLives);

        if (currentLives < 0)
        {
            GameOver();
        }
        else
        {
            StartCoroutine(WaitResummon(0.5f));
        }
    }

    IEnumerator WaitResummon(float wait)
    {
        yield return new WaitForSeconds(wait);

        player.gameObject.SetActive(true);
        player.ReSpawn();
    }

    public void AddScore(int value)
    {
        currentScore += value;
        Debug.Log(currentScore);
    }

    // 게임 오버 처리
    public void GameOver()
    {
        gameOverText.SetActive(true);  // 게임 오버 텍스트 활성화
        Time.timeScale = 0f;
    }

    // 승리 처리
    public void Victory()
    {
        victoryText.SetActive(true);  // 승리 텍스트 활성화
        Time.timeScale = 0f;
    }

    public void BossSpawnText()
    {
        bossText.SetActive(true);  // 보스 스폰 텍스트 활성화
        StartCoroutine(HideBossTextAfterDelay(3f));  // 3초 후 텍스트 비활성화
    }

    private IEnumerator HideBossTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // 지정된 시간만큼 대기
        bossText.SetActive(false);  // 텍스트 비활성화
    }
}
