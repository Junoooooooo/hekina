using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ��J VideoPlayer ����
    public AudioSource audioSource; // ���Ĩӷ�
    public AudioClip keyPressSound; // ���䭵��
    private bool videoEnded = false; // �T�{�v���O�_����

    void Start()
    {
        // �T�O VideoPlayer ���|�`������
        videoPlayer.isLooping = false;

        // �q�\�v�����񧹦����ƥ�
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // �p�G�v���w�g�����A�����N��^�� Start
        if (videoEnded && Input.anyKeyDown)
        {
            PlayKeyPressSound(); // ������䭵��
            SceneManager.LoadScene("start"); // �T�O "Start" �O�A�������W��
        }
    }

    // ��v�����񧹦��ɰ���
    void OnVideoFinished(VideoPlayer vp)
    {
        videoEnded = true;
        Debug.Log("�v�����񧹲��A�����N��^��}�l����");
    }

    // ������䭵��
    void PlayKeyPressSound()
    {
        if (audioSource != null && keyPressSound != null)
        {
            audioSource.PlayOneShot(keyPressSound);  // ������䭵��
        }
    }
}
