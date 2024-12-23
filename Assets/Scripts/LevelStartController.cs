using UnityEngine;
using UnityEngine.SceneManagement; // 引入場景管理的命名空間

public class LevelStartController : MonoBehaviour
{
    private bool gameStarted = false;  // 判斷遊戲是否已經開始

    void Update()
    {
        // 檢查玩家是否按下空白鍵
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true; // 標記遊戲開始

        // 加載 Level1 場景
        SceneManager.LoadScene("Level1"); // 請確保 Level1 場景名稱正確
    }
}
