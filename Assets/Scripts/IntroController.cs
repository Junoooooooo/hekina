using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public GameObject introImage;    // 解說圖片的 GameObject
    public AudioSource backgroundMusic; // 拖入音樂的 AudioSource
    private bool isStarted = false;

    void Start()
    {
        // 確保解說圖片在開始時顯示
        introImage.SetActive(true);
        Time.timeScale = 0f; // 暫停遊戲
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // 確保音樂在開始時不播放
        }
    }

    void Update()
    {
        if (!isStarted && Input.GetKeyDown(KeyCode.F))
        {
            // 按下 F 鍵開始遊戲
            introImage.SetActive(false);
            Time.timeScale = 1f; // 恢復遊戲
            if (backgroundMusic != null)
            {
                backgroundMusic.Play(); // 播放音樂
            }
            isStarted = true;
        }
    }
}
