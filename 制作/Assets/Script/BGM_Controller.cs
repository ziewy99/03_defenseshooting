using UnityEngine;

public class BGM_Controller : MonoBehaviour
{
    public static BGM_Controller Instance;

    [SerializeField] private AudioClip bgmClip;
    private AudioSource audioSource;

    void Awake()
    {
        // シングルトン（重複防止）
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // シーンをまたいでもBGM継続

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
    }

    void Start()
    {
        PlayBGM();
    }

    public void PlayBGM()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }
}
