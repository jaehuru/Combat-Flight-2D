using UnityEngine;

public class DestoyEffect : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        audioSource.volume = GameManager.instance.effectVolume;
    }

    public void EndEffect()
    {
        gameObject.SetActive(false);
    }
}

