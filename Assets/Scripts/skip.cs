using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    private void Update()
    {
        // 檢測玩家是否按下按鍵 M 進入下一關
        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToNextLevel(); // 呼叫進入下一關的方法
        }

        // 檢測玩家是否按下 ESC 鍵退出遊戲
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame(); // 呼叫退出遊戲的方法
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

    private void QuitGame()
    {
        Debug.Log("遊戲已退出"); // 顯示退出提示
        Application.Quit(); // 退出遊戲

        // 如果是在編輯模式下運行，這行代碼會無效，為了測試退出遊戲的效果，你可以使用以下代碼：
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
