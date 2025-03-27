using UnityEngine;
using UnityEngine.UI;

public class LevelIntro : MonoBehaviour
{
    public RawImage rawImage;          // 拖入 RawImage
    public Sprite[] videoFrames;       // 拖入所有 PNG 序列圖
    public GameObject player;          // 角色物件
    public GameObject enemies;         // 敵人物件 (如果有)
    public AudioSource introAudioSource; // 關卡動畫音樂
    public AudioSource mainAudioSource; // 遊戲背景音樂
    public AudioClip introMusic;       // 關卡動畫的音樂
    public float frameRate = 30f;      // 每秒播放的幀數
    public Image endImage;             // **動畫結束後的靜態圖片 (UI Image)**

    private int currentFrame = 0;
    private bool isPlaying = true;
    private bool isPaused = false;

    void Start()
    {
        // 確保動畫從頭開始播放
        currentFrame = 0;
        rawImage.texture = videoFrames[currentFrame].texture;

        // 禁用玩家與敵人
        if (player != null) player.SetActive(false);
        if (enemies != null) enemies.SetActive(false);

        // 播放關卡動畫音樂
        if (introAudioSource != null && introMusic != null)
        {
            introAudioSource.clip = introMusic;
            introAudioSource.Play();
        }

        // 停止背景音樂
        if (mainAudioSource != null) mainAudioSource.Stop();

        // 隱藏結束圖片
        if (endImage != null) endImage.gameObject.SetActive(false);

        // 開始播放 PNG 序列動畫
        InvokeRepeating("PlayNextFrame", 0f, 1f / frameRate);
    }

    void PlayNextFrame()
    {
        if (!isPlaying) return;

        currentFrame++;
        if (currentFrame >= videoFrames.Length)
        {
            // 動畫播放完畢，停止播放動畫
            CancelInvoke("PlayNextFrame");
            rawImage.gameObject.SetActive(false);

            // **顯示靜態圖片並暫停遊戲**
            if (endImage != null)
            {
                endImage.gameObject.SetActive(true);
                Time.timeScale = 0;  // 暫停遊戲
                isPaused = true;
            }
        }
        else
        {
            rawImage.texture = videoFrames[currentFrame].texture;
        }
    }

    void Update()
    {
        // 如果遊戲暫停，按下任意鍵繼續
        if (isPaused && Input.anyKeyDown)
        {
            ResumeGame();
        }
    }

    void ResumeGame()
    {
        if (endImage != null) endImage.gameObject.SetActive(false);
        Time.timeScale = 1;  // 恢復遊戲
        isPaused = false;

        // 啟動遊戲
        if (player != null) player.SetActive(true);
        if (enemies != null) enemies.SetActive(true);

        // 播放背景音樂
        if (mainAudioSource != null) mainAudioSource.Play();
    }
}
