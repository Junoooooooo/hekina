using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 拖入 VideoPlayer 物件
    public AudioSource audioSource; // 音效來源
    public AudioClip keyPressSound; // 按鍵音效
    private bool videoEnded = false; // 確認影片是否播完

    void Start()
    {
        // 確保 VideoPlayer 不會循環播放
        videoPlayer.isLooping = false;

        // 訂閱影片播放完成的事件
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void Update()
    {
        // 如果影片已經播完，按任意鍵回到 Start
        if (videoEnded && Input.anyKeyDown)
        {
            PlayKeyPressSound(); // 播放按鍵音效
            SceneManager.LoadScene("start"); // 確保 "Start" 是你的場景名稱
        }
    }

    // 當影片播放完成時執行
    void OnVideoFinished(VideoPlayer vp)
    {
        videoEnded = true;
        Debug.Log("影片播放完畢，按任意鍵回到開始場景");
    }

    // 播放按鍵音效
    void PlayKeyPressSound()
    {
        if (audioSource != null && keyPressSound != null)
        {
            audioSource.PlayOneShot(keyPressSound);  // 播放按鍵音效
        }
    }
}
