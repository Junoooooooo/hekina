using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSwitcher : MonoBehaviour
{
    public Image displayImage;  // �Ψ���ܪ� UI Image
    public Sprite[] images; // �s��Ҧ��n��ܪ��Ϥ�
    private int currentIndex = 0; // ��e�Ϥ�����

    public AudioSource audioSource;     // ���Ĩӷ�
    public AudioClip keyPressSound;     // ���䭵��

    void Start()
    {
        if (images.Length > 0)
        {
            displayImage.sprite = images[currentIndex]; // ��ܲĤ@�i�Ϥ�
        }
    }

    void Update()
    {
        if (Input.anyKeyDown) // ���U���N��
        {
            PlayKeyPressSound();  // ���񭵮�
            if (currentIndex < images.Length - 1) // �p�G�٦��U�@�i�Ϥ�
            {
                NextImage();
            }
            else // �p�G�w�g�O�̫�@�i�Ϥ��A�h�i�J�U�@�ӳ���
            {
                LoadNextLevel();
            }
        }
    }

    void PlayKeyPressSound()
    {
        if (audioSource != null && keyPressSound != null)
        {
            audioSource.PlayOneShot(keyPressSound);  // ������䭵��
        }
    }

    void NextImage()
    {
        currentIndex++;
        if (currentIndex < images.Length)
        {
            displayImage.sprite = images[currentIndex]; // ��ܤU�@�i�Ϥ�
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene("level2"); // ���� "level2" ���A�������W��
    }
}
