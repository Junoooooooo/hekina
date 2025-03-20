using UnityEngine;
using UnityEngine.UI;


public class LevelIntro1 : MonoBehaviour
{
    public PlayerController spawnManager;
    public PlayerController playerEnergy;  // 直接在 Inspector 內拖入對應的物件
    public PlayerController updateEnergybar;
    public PlayerController updateEnergy;
    public RawImage rawImage;          // 拖入 RawImage
    public Sprite[] videoFrames;       // 拖入所有 PNG 序列圖
    public GameObject player;          // 角色物件
    public GameObject enemies;         // 敵人物件 (如果有)
    public Image endImage;             // 第一張結束圖片
    //public Image nextImage;            // 第二張圖片 (長按 3 秒空白鍵後顯示)
    public AudioSource introAudioSource; // 關卡動畫音樂
    public AudioSource mainAudioSource; // 遊戲背景音樂
    public AudioClip introMusic;       // 關卡動畫的音樂 (mp3 或 wav)
    public float frameRate = 30f;      // 每秒播放的幀數

    private int currentFrame = 0;
    private bool isPlaying = true;
    private bool showEndImage = false;
    private bool gameStarted = false;
    private bool showNextImage = false;
    private float spaceKeyPressTime = 0f;  // 空白鍵按住時間

    void Start()
    {
        if (updateEnergy != null)
        {
            updateEnergy.UpdateEnergy();  // ✅ 調用 RecoverEnergy
        }
        else
        {
            Debug.LogWarning("playerEnergy 未指定！");
        }
        if (playerEnergy != null)
        {
            playerEnergy.RecoverEnergy();  // ✅ 調用 RecoverEnergy
        }
        else
        {
            Debug.LogWarning("playerEnergy 未指定！");
        }
        if (updateEnergybar != null)
        {
            updateEnergybar.UpdateEnergyBar();  // ✅ 調用 RecoverEnergy
        }
        else
        {
            Debug.LogWarning("playerEnergy 未指定！");
        }

        if (videoFrames == null || videoFrames.Length == 0)
        {
            Debug.LogError("videoFrames 陣列為空！請確保在 Inspector 設定 PNG 序列圖");
            return;
        }

        currentFrame = 0;
        rawImage.texture = videoFrames[currentFrame].texture;

        if (player != null) player.SetActive(false);
        if (enemies != null) enemies.SetActive(false);
        if (endImage != null) endImage.gameObject.SetActive(false);
       // if (nextImage != null) nextImage.gameObject.SetActive(false);

        if (introAudioSource != null && introMusic != null)
        {
            introAudioSource.clip = introMusic;
            introAudioSource.Play();
        }

        if (mainAudioSource != null)
        {
            mainAudioSource.Stop();
        }

        InvokeRepeating("PlayNextFrame", 0f, 1f / frameRate);
    }

    void PlayNextFrame()
    {
        if (!isPlaying) return;

        currentFrame++;

        if (currentFrame >= videoFrames.Length)
        {
            currentFrame = videoFrames.Length - 1;
            CancelInvoke("PlayNextFrame");
            rawImage.gameObject.SetActive(false);

            if (endImage != null)
            {
                endImage.gameObject.SetActive(true);
                showEndImage = true;
            }
        }
        else
        {
            rawImage.texture = videoFrames[currentFrame].texture;
        }
    }

   void Update()
    {
        if (showEndImage && Input.GetMouseButton(1))
        {
            StartGame();
        }

       /* if (gameStarted && !showNextImage)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                spaceKeyPressTime += Time.deltaTime;

               /* if (spaceKeyPressTime >= 3f)  // 長按 3 秒
                {
                    if (nextImage != null)
                    {
                        nextImage.gameObject.SetActive(true);
                    }

                    showNextImage = true;
                    Time.timeScale = 0f; // 暫停遊戲
                }
            }
            else
            {
                spaceKeyPressTime = 0f;
            }
        }*/

        // 如果顯示 nextImage，並且玩家按下任意鍵
        /*if (showNextImage && Input.GetMouseButton(1))
        {
            if (nextImage != null)
            {
                nextImage.gameObject.SetActive(false); // 隱藏 nextImage
            }

            // 恢復遊戲並啟動玩家和敵人
            StartGame();
        }*/
    }
    
    void StartGame()
    {
        if (endImage != null) endImage.gameObject.SetActive(false);
        if (player != null) player.SetActive(true);
        if (enemies != null) enemies.SetActive(true);

        if (introAudioSource != null) introAudioSource.Stop();
        if (mainAudioSource != null) mainAudioSource.Play();

        showEndImage = false;
        gameStarted = true;

        Time.timeScale = 1f; // 恢復遊戲
    
    }
}
