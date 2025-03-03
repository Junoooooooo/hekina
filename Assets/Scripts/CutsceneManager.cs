using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneManager : MonoBehaviour
{
    public RawImage videoScreen;   // 顯示影片的 UI RawImage
    public VideoPlayer videoPlayer; // 影片播放器
    public Image displayImage;     // 顯示圖片的 UI Image
    public Sprite nextImage;       // 需要切換的圖片

    private bool videoSkipped = false; // 是否已跳過影片
    private bool imageDisplayed = false; // 是否已顯示圖片
    private bool inputLocked = false; // 控制輸入是否鎖定

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // 影片播放完畢時執行
        displayImage.gameObject.SetActive(false);   // 初始時隱藏圖片
    }

    void Update()
    {
        if (inputLocked) return; // 防止過快輸入

        if (!videoSkipped && Input.anyKeyDown) // 任何時候都能跳過影片
        {
            Debug.Log("🎬 影片被跳過，切換到圖片");
            StartCoroutine(ShowImage());
        }
        else if (imageDisplayed && Input.anyKeyDown) // 按鍵切換到下一關
        {
            Debug.Log("🚀 進入下一關");
            StartCoroutine(LoadNextLevel());
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (!videoSkipped) // 確保只有影片正常結束時才切換
        {
            Debug.Log("✅ 影片播放完畢，切換到圖片");
            StartCoroutine(ShowImage());
        }
    }

    IEnumerator ShowImage()
    {
        inputLocked = true; // 鎖定輸入，避免連續觸發
        videoSkipped = true; // 確保不會再進入這個流程

        yield return new WaitForSeconds(0.2f); // 等待按鍵緩衝消失

        while (Input.anyKey) // 等待玩家放開按鍵
        {
            yield return null;
        }

        videoScreen.gameObject.SetActive(false); // 隱藏影片
        displayImage.gameObject.SetActive(true); // 顯示圖片
        displayImage.sprite = nextImage;

        imageDisplayed = true;
        inputLocked = false; // 解鎖輸入
    }

    IEnumerator LoadNextLevel()
    {
        inputLocked = true; // 鎖定輸入，避免誤觸

        yield return new WaitForSeconds(0.2f); // 等待按鍵緩衝消失

        while (Input.anyKey) // 等待玩家放開按鍵
        {
            yield return null;
        }

        SceneManager.LoadScene("level3"); // 進入下一關
    }
}
