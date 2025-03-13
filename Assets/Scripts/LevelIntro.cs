using UnityEngine;
using UnityEngine.UI;

public class LevelIntro : MonoBehaviour
{
    public RawImage rawImage;          // ��J RawImage
    public Sprite[] videoFrames;       // ��J�Ҧ� PNG �ǦC��
    public GameObject player;          // ���⪫��
    public GameObject enemies;         // �ĤH���� (�p�G��)
    public AudioSource introAudioSource; // ��J���d�ʵe���֪� AudioSource
    public AudioSource mainAudioSource; // ��J�C���I�����֪� AudioSource
    public AudioClip introMusic;       // ��J���d�ʵe������ (�Ҧp�Gmp3 �� wav)
    public float frameRate = 30f;      // �C���񪺴V��

    private int currentFrame = 0;
    private bool isPlaying = true;

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
}
