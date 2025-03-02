using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSwitcher : MonoBehaviour
{
    public Image displayImage;  // �Ψ���ܪ� UI Image
    public Sprite[] images; // �s��Ҧ��n��ܪ��Ϥ�
    private int currentIndex = 0; // ��e�Ϥ�����

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
        SceneManager.LoadScene("level2"); // ���� "NextLevel" ���A�������W��
    }
}
