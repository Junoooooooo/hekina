using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MOLEGAME : MonoBehaviour
{
    public GameObject[] moles; // 存放所有地鼠物件
    public Color[] moleColors = { Color.red, Color.blue, Color.yellow }; // 地鼠顏色
    public KeyCode[] colorKeys = { KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow }; // 對應顏色的按鍵
    public Text scoreText; // 顯示分數的UI文本
    public float moleAppearanceTime = 1f; // 地鼠顯示時間
    public float moleCooldownTime = 0.5f; // 地鼠再次顯示的冷卻時間
    public float gameDuration = 30f; // 遊戲時間

    private int score = 0; // 分數
    private bool gameActive = false; // 遊戲是否開始
    private float gameTimer; // 遊戲計時器
    private int currentMoleIndex = -1; // 當前顯示的地鼠

    void Start()
    {
        gameTimer = gameDuration;
        StartGame();
    }

    void Update()
    {
        if (gameActive)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                EndGame();
            }
        }

        // 玩家輸入檢測
        if (currentMoleIndex >= 0 && moles[currentMoleIndex].activeSelf)
        {
            // 檢查玩家按下的鍵是否正確
            if (Input.GetKeyDown(colorKeys[0]) && moles[currentMoleIndex].GetComponent<Renderer>().material.GetColor("_EmissionColor") == moleColors[0])
            {
                CorrectHit();
            }
            else if (Input.GetKeyDown(colorKeys[1]) && moles[currentMoleIndex].GetComponent<Renderer>().material.GetColor("_EmissionColor") == moleColors[1])
            {
                CorrectHit();
            }
            else if (Input.GetKeyDown(colorKeys[2]) && moles[currentMoleIndex].GetComponent<Renderer>().material.GetColor("_EmissionColor") == moleColors[2])
            {
                CorrectHit();
            }
            else if (Input.anyKeyDown)
            {
                IncorrectHit();
            }
        }
    }

    void StartGame()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        gameActive = true;
        StartCoroutine(SpawnMoles());
    }

    void EndGame()
    {
        gameActive = false;
        scoreText.text = "Game Over! Final Score: " + score;
    }

    void CorrectHit()
    {
        score++;
        scoreText.text = "Score: " + score;
        moles[currentMoleIndex].SetActive(false);
        StartCoroutine(SpawnMoles()); // 隨機重生地鼠
    }

    void IncorrectHit()
    {
        score--;
        scoreText.text = "Score: " + score;
        moles[currentMoleIndex].SetActive(false);
        StartCoroutine(SpawnMoles()); // 隨機重生地鼠
    }

    // 隨機選擇地鼠並顯示它
    IEnumerator SpawnMoles()
    {
        yield return new WaitForSeconds(moleCooldownTime); // 等待冷卻時間

        // 隨機選擇一個地鼠
        currentMoleIndex = Random.Range(0, moles.Length);

        // 隨機選擇一個顏色並設置給地鼠
        Color moleColor = moleColors[Random.Range(0, moleColors.Length)];
        Renderer moleRenderer = moles[currentMoleIndex].GetComponent<Renderer>();

        // 設置發光顏色
        moleRenderer.material.SetColor("_EmissionColor", moleColor); // 設置發光顏色
        DynamicGI.SetEmissive(moleRenderer, moleColor); // 更新全局光照系統

        // 讓地鼠顯示
        moles[currentMoleIndex].SetActive(true);

        // 等待地鼠顯示時間
        yield return new WaitForSeconds(moleAppearanceTime);

        // 隱藏地鼠
        moles[currentMoleIndex].SetActive(false);

        // 重置當前地鼠索引
        currentMoleIndex = -1;
    }
}
