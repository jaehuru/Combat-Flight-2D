using UnityEngine;

public class Laser : MonoBehaviour
{
    public Vector2 direction = Vector2.up;
    float speed = 10f;
    new Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.linearVelocity = direction * speed;

        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource.volume = GameManager.instance.effectVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spriteRenderer.isVisible)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        gameObject.SetActive(false);
    }
}
