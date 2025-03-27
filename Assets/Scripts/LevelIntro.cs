using UnityEngine;
using UnityEngine.UI;

public class LevelIntro : MonoBehaviour
{
    public RawImage rawImage;          // ��J RawImage
    public Sprite[] videoFrames;       // ��J�Ҧ� PNG �ǦC��
    public GameObject player;          // ���⪫��
    public GameObject enemies;         // �ĤH���� (�p�G��)
    public AudioSource introAudioSource; // ���d�ʵe����
    public AudioSource mainAudioSource; // �C���I������
    public AudioClip introMusic;       // ���d�ʵe������
    public float frameRate = 30f;      // �C���񪺴V��
    public Image endImage;             // **�ʵe�����᪺�R�A�Ϥ� (UI Image)**

    private int currentFrame = 0;
    private bool isPlaying = true;
    private bool isPaused = false;

    void Start()
    {
        // �T�O�ʵe�q�Y�}�l����
        currentFrame = 0;
        rawImage.texture = videoFrames[currentFrame].texture;

        // �T�Ϊ��a�P�ĤH
        if (player != null) player.SetActive(false);
        if (enemies != null) enemies.SetActive(false);

        // �������d�ʵe����
        if (introAudioSource != null && introMusic != null)
        {
            introAudioSource.clip = introMusic;
            introAudioSource.Play();
        }

        // ����I������
        if (mainAudioSource != null) mainAudioSource.Stop();

        // ���õ����Ϥ�
        if (endImage != null) endImage.gameObject.SetActive(false);

        // �}�l���� PNG �ǦC�ʵe
        InvokeRepeating("PlayNextFrame", 0f, 1f / frameRate);
    }

    void PlayNextFrame()
    {
        if (!isPlaying) return;

        currentFrame++;
        if (currentFrame >= videoFrames.Length)
        {
            // �ʵe���񧹲��A�����ʵe
            CancelInvoke("PlayNextFrame");
            rawImage.gameObject.SetActive(false);

            // **����R�A�Ϥ��üȰ��C��**
            if (endImage != null)
            {
                endImage.gameObject.SetActive(true);
                Time.timeScale = 0;  // �Ȱ��C��
                isPaused = true;
            }
        }
        else
        {
            rawImage.texture = videoFrames[currentFrame].texture;
        }
    }

    void Update()
    {
        // �p�G�C���Ȱ��A���U���N���~��
        if (isPaused && Input.anyKeyDown)
        {
            ResumeGame();
        }
    }

    void ResumeGame()
    {
        if (endImage != null) endImage.gameObject.SetActive(false);
        Time.timeScale = 1;  // ��_�C��
        isPaused = false;

        // �ҰʹC��
        if (player != null) player.SetActive(true);
        if (enemies != null) enemies.SetActive(true);

        // ����I������
        if (mainAudioSource != null) mainAudioSource.Play();
    }
}
