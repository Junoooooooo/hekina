using UnityEngine;
using UnityEngine.UI;

public class LevelIntro2 : MonoBehaviour
{
    public RawImage rawImage;          // 拖入 RawImage
    public Sprite[] videoFrames;       // 拖入所有 PNG 序列圖
    public GameObject player;          // 角色物件
    public GameObject enemies;         // 敵人物件 (如果有)
    public AudioSource introAudioSource; // 拖入關卡動畫音樂的 AudioSource
    public AudioSource mainAudioSource; // 拖入遊戲背景音樂的 AudioSource
    public AudioClip introMusic;       // 拖入關卡動畫的音樂 (例如：mp3 或 wav)
    public float frameRate = 30f;      // 每秒播放的幀數
    public Image endImage;             // 顯示的 Image（顯示動畫播放完畢後的畫面）
    public Image second;

    private int currentFrame = 0;
    private bool isPlaying = true;
    private bool hasTriggered = false;

    void Start()
    {
        // 確保動畫從頭開始播放
        currentFrame = 0;
        rawImage.texture = videoFrames[currentFrame].texture;  // 從第一幀開始顯示

        // 禁用玩家與敵人控制
        if (player != null)
        {
            player.SetActive(false);
        }
        if (enemies != null)
        {
            enemies.SetActive(false);
        }

        // 播放關卡動畫音樂
        if (introAudioSource != null && introMusic != null)
        {
            introAudioSource.clip = introMusic;
            introAudioSource.Play();  // 播放關卡動畫的音樂
        }

        // 停止遊戲背景音樂
        if (mainAudioSource != null)
        {
            mainAudioSource.Stop();
        }

        // 開始播放 PNG 序列動畫
        InvokeRepeating("PlayNextFrame", 0f, 1f / frameRate);  // 按照設定的 frameRate 播放動畫
    }

    void PlayNextFrame()
    {
        if (!isPlaying) return;

        currentFrame++;
        if (currentFrame >= videoFrames.Length)
        {
            // 動畫播放完畢，停止並隱藏 RawImage
            CancelInvoke("PlayNextFrame");
            rawImage.gameObject.SetActive(false);

            // 顯示結束圖像
            if (endImage != null)
            {
                endImage.gameObject.SetActive(true);
            }

            // 暫停遊戲
            Time.timeScale = 0f;

            // 啟動遊戲
            if (player != null)
            {
                player.SetActive(true);
            }
            if (enemies != null)
            {
                enemies.SetActive(true);
            }

            // 停止關卡動畫的音樂
            if (introAudioSource != null)
            {
                introAudioSource.Stop();
            }

            // 播放遊戲背景音樂
            if (mainAudioSource != null)
            {
                mainAudioSource.Play(); // 開始播放遊戲背景音樂
            }
        }
        else
        {
            rawImage.texture = videoFrames[currentFrame].texture;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 檢查是否玩家進入觸發區
        if (other.CompareTag("Player") && !hasTriggered)
        {
            second.gameObject.SetActive(true);
            hasTriggered = true; // 防止重複觸發

        }
        Time.timeScale = 0f;
    }

    // 用於恢復遊戲的方法
    public void ResumeGame()
    {
        // 恢復遊戲時間流逝
        Time.timeScale = 1f;

        // 隱藏結束畫面
        if (endImage != null)
        {
            endImage.gameObject.SetActive(false);
        }

        if (second != null)
        {
            second.gameObject.SetActive(false);
        }
        // 啟動遊戲角色和敵人
        if (player != null)
        {
            player.SetActive(true);
        }
        if (enemies != null)
        {
            enemies.SetActive(true);
        }
    }

    void Update()
    {
        // 檢查是否按下滑鼠右鍵
        if (Time.timeScale == 0f && Input.GetMouseButtonDown(1)) // 右鍵是 1
        {
            ResumeGame(); // 呼叫恢復遊戲的方法
        }
    }
}