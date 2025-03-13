using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public AudioSource buttonClickSound; // 用來播放按鍵音效的 AudioSource

    void Start()
    {
        // 確保一開始沒有播放音效
        if (buttonClickSound != null)
        {
            buttonClickSound.Stop();
        }
    }

    void Update()
    {
        // 檢查是否按下任意鍵
        if (Input.anyKeyDown)
        {
            // 播放按鍵音效
            if (buttonClickSound != null)
            {
                buttonClickSound.Play();
            }

            // 當按下任意鍵時，載入 intro 場景
            SceneManager.LoadScene("intro");
        }
    }
}
