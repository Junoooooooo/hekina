using UnityEngine;
using UnityEngine.UI;

public class LevelIntro1 : MonoBehaviour
{
    public RawImage rawImage;          // ��J RawImage
    public Sprite[] videoFrames;       // ��J�Ҧ� PNG �ǦC��
    public GameObject player;          // ���⪫��
    public GameObject enemies;         // �ĤH���� (�p�G��)
    public Image endImage;             // �Ĥ@�i�����Ϥ�
    public Image nextImage;            // �ĤG�i�Ϥ� (���� 3 ��ť�������)
    public AudioSource introAudioSource; // ���d�ʵe����
    public AudioSource mainAudioSource; // �C���I������
    public AudioClip introMusic;       // ���d�ʵe������ (mp3 �� wav)
    public float frameRate = 30f;      // �C���񪺴V��

    private int currentFrame = 0;
    private bool isPlaying = true;
    private bool showEndImage = false;
    private bool gameStarted = false;
    private bool showNextImage = false;
    private float spaceKeyPressTime = 0f;  // �ť������ɶ�

    void Start()
    {
        if (videoFrames == null || videoFrames.Length == 0)
        {
            Debug.LogError("videoFrames �}�C���šI�нT�O�b Inspector �]�w PNG �ǦC��");
            return;
        }

        currentFrame = 0;
        rawImage.texture = videoFrames[currentFrame].texture;

        if (player != null) player.SetActive(false);
        if (enemies != null) enemies.SetActive(false);
        if (endImage != null) endImage.gameObject.SetActive(false);
        if (nextImage != null) nextImage.gameObject.SetActive(false);

        if (introAudioSource != null && introMusic != null)
        {
            introAudioSource.clip = introMusic;
            introAudioSource.Play();
        }

        if (mainAudioSource != null)
        {
            mainAudioSource.Stop();
        }

        InvokeRepeating("PlayNextFrame", 0f, 1f / frameRate);
    }

    void PlayNextFrame()
    {
        if (!isPlaying) return;

        currentFrame++;

        if (currentFrame >= videoFrames.Length)
        {
            currentFrame = videoFrames.Length - 1;
            CancelInvoke("PlayNextFrame");
            rawImage.gameObject.SetActive(false);

            if (endImage != null)
            {
                endImage.gameObject.SetActive(true);
                showEndImage = true;
            }
        }
        else
        {
            rawImage.texture = videoFrames[currentFrame].texture;
        }
    }

    void Update()
    {
        if (showEndImage && Input.GetMouseButton(0))
        {
            StartGame();
        }

        if (gameStarted && !showNextImage)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                spaceKeyPressTime += Time.deltaTime;

                if (spaceKeyPressTime >= 3f)  // ���� 3 ��
                {
                    if (nextImage != null)
                    {
                        nextImage.gameObject.SetActive(true);
                    }

                    showNextImage = true;
                    Time.timeScale = 0f; // �Ȱ��C��
                }
            }
            else
            {
                spaceKeyPressTime = 0f;
            }
        }

        // �p�G��� nextImage�A�åB���a���U���N��
        if (showNextImage && Input.GetMouseButton(0))
        {
            if (nextImage != null)
            {
                nextImage.gameObject.SetActive(false); // ���� nextImage
            }

            // ��_�C���ñҰʪ��a�M�ĤH
            StartGame();
        }
    }

    void StartGame()
    {
        if (endImage != null) endImage.gameObject.SetActive(false);
        if (player != null) player.SetActive(true);
        if (enemies != null) enemies.SetActive(true);

        if (introAudioSource != null) introAudioSource.Stop();
        if (mainAudioSource != null) mainAudioSource.Play();

        showEndImage = false;
        gameStarted = true;

        Time.timeScale = 1f; // ��_�C��
    }
}
