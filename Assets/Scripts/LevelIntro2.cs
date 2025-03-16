using UnityEngine;
using UnityEngine.UI;

public class LevelIntro2 : MonoBehaviour
{
    public RawImage rawImage;          // ��J RawImage
    public Sprite[] videoFrames;       // ��J�Ҧ� PNG �ǦC��
    public GameObject player;          // ���⪫��
    public GameObject enemies;         // �ĤH���� (�p�G��)
    public AudioSource introAudioSource; // ��J���d�ʵe���֪� AudioSource
    public AudioSource mainAudioSource; // ��J�C���I�����֪� AudioSource
    public AudioClip introMusic;       // ��J���d�ʵe������ (�Ҧp�Gmp3 �� wav)
    public float frameRate = 30f;      // �C���񪺴V��
    public Image endImage;             // ��ܪ� Image�]��ܰʵe���񧹲��᪺�e���^
    public Image second;

    private int currentFrame = 0;
    private bool isPlaying = true;
    private bool hasTriggered = false;

    void Start()
    {
        // �T�O�ʵe�q�Y�}�l����
        currentFrame = 0;
        rawImage.texture = videoFrames[currentFrame].texture;  // �q�Ĥ@�V�}�l���

        // �T�Ϊ��a�P�ĤH����
        if (player != null)
        {
            player.SetActive(false);
        }
        if (enemies != null)
        {
            enemies.SetActive(false);
        }

        // �������d�ʵe����
        if (introAudioSource != null && introMusic != null)
        {
            introAudioSource.clip = introMusic;
            introAudioSource.Play();  // �������d�ʵe������
        }

        // ����C���I������
        if (mainAudioSource != null)
        {
            mainAudioSource.Stop();
        }

        // �}�l���� PNG �ǦC�ʵe
        InvokeRepeating("PlayNextFrame", 0f, 1f / frameRate);  // ���ӳ]�w�� frameRate ����ʵe
    }

    void PlayNextFrame()
    {
        if (!isPlaying) return;

        currentFrame++;
        if (currentFrame >= videoFrames.Length)
        {
            // �ʵe���񧹲��A��������� RawImage
            CancelInvoke("PlayNextFrame");
            rawImage.gameObject.SetActive(false);

            // ��ܵ����Ϲ�
            if (endImage != null)
            {
                endImage.gameObject.SetActive(true);
            }

            // �Ȱ��C��
            Time.timeScale = 0f;

            // �ҰʹC��
            if (player != null)
            {
                player.SetActive(true);
            }
            if (enemies != null)
            {
                enemies.SetActive(true);
            }

            // �������d�ʵe������
            if (introAudioSource != null)
            {
                introAudioSource.Stop();
            }

            // ����C���I������
            if (mainAudioSource != null)
            {
                mainAudioSource.Play(); // �}�l����C���I������
            }
        }
        else
        {
            rawImage.texture = videoFrames[currentFrame].texture;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�O�_���a�i�JĲ�o��
        if (other.CompareTag("Player") && !hasTriggered)
        {
            second.gameObject.SetActive(true);
            hasTriggered = true; // �����Ĳ�o

        }
        Time.timeScale = 0f;
    }

    // �Ω��_�C������k
    public void ResumeGame()
    {
        // ��_�C���ɶ��y�u
        Time.timeScale = 1f;

        // ���õ����e��
        if (endImage != null)
        {
            endImage.gameObject.SetActive(false);
        }

        if (second != null)
        {
            second.gameObject.SetActive(false);
        }
        // �ҰʹC������M�ĤH
        if (player != null)
        {
            player.SetActive(true);
        }
        if (enemies != null)
        {
            enemies.SetActive(true);
        }
    }

    void Update()
    {
        // �ˬd�O�_���U�ƹ��k��
        if (Time.timeScale == 0f && Input.GetMouseButtonDown(1)) // �k��O 1
        {
            ResumeGame(); // �I�s��_�C������k
        }
    }
}