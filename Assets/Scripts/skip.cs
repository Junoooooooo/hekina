using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    private void Update()
    {
        // 檢測玩家是否按下按鍵 M
        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToNextLevel(); // 呼叫進入下一關的方法
        }
    }

    private void GoToNextLevel()
    {
        // 獲取當前場景名稱
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 根據當前場景名稱切換到下一關
        if (currentSceneName == "level1")
        {
            SceneManager.LoadScene("level2");
        }
        else if (currentSceneName == "level2")
        {
            SceneManager.LoadScene("level3");
        }
        else
        {
            Debug.Log("已經是最後一關或未定義的關卡！"); // 當沒有下一關時輸出提示
        }
    }
}
