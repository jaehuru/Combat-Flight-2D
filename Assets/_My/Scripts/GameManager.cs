using System.Collections;
using UnityEngine;
using UnityEngine.UI;


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

    [Header("UI")]
    [SerializeField] Text livesText;

    int totalLives = 3;
    int currentLives;

    int currentScore = 0;

    public GameObject gameOverText;
    public GameObject victoryText;
    public GameObject bossText;

    [SerializeField] GameObject endButtonGroup;

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
        UpdateLifeUI();
    }

    private void UpdateLifeUI()
    {
        if (livesText != null)
        {
            livesText.text = "Life:" + currentLives.ToString();
        }
        else
        {
            Debug.LogWarning("livesText is not assigned!");
        }
    }

    public void PlayerDestruction()
    {
        currentLives--;

        UpdateLifeUI();

        Debug.Log(currentLives);

        if (currentLives <= 0)
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
    }

    // ���� ���� ó��
    public void GameOver()
    {
        gameOverText.SetActive(true);  // ���� ���� �ؽ�Ʈ Ȱ��ȭ
        endButtonGroup.SetActive(true); // Retry / Exit ��ư ���̱�
        Time.timeScale = 0f;
    }

    // �¸� ó��
    public void Victory()
    {
        victoryText.SetActive(true);  // �¸� �ؽ�Ʈ Ȱ��ȭ
        endButtonGroup.SetActive(true); // Retry / Exit ��ư ���̱�
        Time.timeScale = 0f;
    }

    public void BossSpawnText()
    {
        bossText.SetActive(true);  // ���� ���� �ؽ�Ʈ Ȱ��ȭ
        StartCoroutine(HideBossTextAfterDelay(3f));  // 3�� �� �ؽ�Ʈ ��Ȱ��ȭ
    }

    private IEnumerator HideBossTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);  // ������ �ð���ŭ ���
        bossText.SetActive(false);  // �ؽ�Ʈ ��Ȱ��ȭ
    }
}