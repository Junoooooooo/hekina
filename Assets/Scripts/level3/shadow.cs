using UnityEngine;
using UnityEngine.SceneManagement; // 引入場景管理

public class shadow : MonoBehaviour
{
    // 可調整的影子追趕速度
    public float chaseSpeed = 2f;

    // 影子的目標位置（通常是通道的頂部）
    public Transform endPoint;

    // 是否正在追趕
    private bool isChasing = true;

    void Update()
    {
        if (isChasing)
        {
            // 使用插值讓影子逐漸向目標位置移動
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, chaseSpeed * Time.deltaTime);

            // 檢查影子是否已經達到目標位置
            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                isChasing = false;
                // 影子達到終點後可以執行的操作，例如結束遊戲
                Debug.Log("Shadow reached the end point!");
            }
        }
    }

    // 當影子碰到玩家時
    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰到的物件是否有 "Player" 標籤
        if (other.CompareTag("Player"))
        {
            Debug.Log("Shadow touched the Player! Restarting the game...");
            RestartGame();
        }
    }

    // 遊戲重啟邏輯
    private void RestartGame()
    {
        // 重新加載當前場景
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // 可選：停止或重啟追趕
    public void StopChase()
    {
        isChasing = false;
    }

    public void StartChase()
    {
        isChasing = true;
    }
}
