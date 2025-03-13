using UnityEngine;
using UnityEngine.UI;

public class LevelIntro : MonoBehaviour
{
    public RawImage rawImage;          // 拖入 RawImage
    public Sprite[] videoFrames;       // 拖入所有 PNG 序列圖
    public GameObject player;          // 角色物件
    public GameObject enemies;         // 敵人物件 (如果有)
    public AudioSource introAudioSource; // 拖入關卡動畫音樂的 AudioSource
    public AudioSource mainAudioSource; // 拖入遊戲背景音樂的 AudioSource
    public AudioClip introMusic;       // 拖入關卡動畫的音樂 (例如：mp3 或 wav)
    public float frameRate = 30f;      // 每秒播放的幀數

    private int currentFrame = 0;
    private bool isPlaying = true;

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
}
