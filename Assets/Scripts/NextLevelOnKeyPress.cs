using UnityEngine;
using UnityEngine.SceneManagement; // 用於切換場景

public class NextLevelOnKeyPress : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown) // 當按下任意鍵
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
            Debug.Log("已經是最後一關！");
        }
    }
}
