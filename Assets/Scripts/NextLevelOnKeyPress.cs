using UnityEngine;
using UnityEngine.SceneManagement; // �Ω��������

public class NextLevelOnKeyPress : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown) // ����U���N��
        {
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
