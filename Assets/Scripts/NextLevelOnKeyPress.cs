using UnityEngine;
using UnityEngine.SceneManagement; // �Ω��������

public class NextLevelOnKeyPress : MonoBehaviour
{
    public AudioSource audioSource;  // ���Ĩӷ�
    public AudioClip keyPressSound;  // ���䭵��

    void Update()
    {
        if (Input.anyKeyDown) // ����U���N��
        {
            audioSource.PlayOneShot(keyPressSound);  // ������䭵��
            LoadNextLevel();
        }
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("�w�g�O�̫�@���I");
        }
    }
}
