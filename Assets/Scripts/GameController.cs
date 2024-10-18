using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Light[] sceneLights; // 場景中的燈光
    public GameObject spherePrefab; // 用於生成的Sphere預製件
    public float player1HoldTime = 0f; // 玩家一的持續按壓時間
    public float player2ClickFrequency; // 玩家二的右鍵按壓頻率
    public Slider countdownSlider; // 倒數計時的量條

    public float holdTimeTargetMin = 5f; // 玩家一按住的最小時間
    public float holdTimeTargetMax = 10f; // 玩家一按住的最大時間
    public float frequencyTargetMin = 1f; // 目標按壓頻率最小值
    public float frequencyTargetMax = 5f; // 目標按壓頻率最大值
    public float timeFrame = 1f; // 計算頻率的時間窗口（秒）

    private int completedChallenges = 0; // 已完成挑戰數
    private float previousIntensity = 1f; // 保存上一次成功的亮度
    private bool challengeActive = false; // 是否正在進行挑戰
    private Coroutine activeChallengeRoutine;
    private int clickCount = 0; // 用來計算玩家二的右鍵按壓次數
    private float totalTime = 60f; // 總計時60秒
    private float challengeTimeLimit = 60f; // 挑戰時間限制
    private float challengeStartTime; // 記錄挑戰開始時間

    private GameObject currentSphere; // 當前生成的Sphere實例
    private bool isSphereActive = false; // Sphere是否處於活躍狀態
    private int spheresSpawned = 0; // 已生成的Sphere數量

    private Vector3 fixedPosition = new Vector3(465.6f, 295f, 0f); // 固定位置

    private bool isReactionTimeActive = false; // 反應時間狀態
    private float reactionTimeDuration = 0.5f; // 反應時間持續時間
    private float reactionTimeStart; // 反應時間開始的時間

    private void Start()
    {
        countdownSlider.maxValue = challengeTimeLimit; // 設置量條的最大值
        countdownSlider.value = challengeTimeLimit; // 初始設置量條的值

        StartCoroutine(SpawnSpheresCoroutine()); // 啟動Sphere生成協程
        StartCoroutine(CountPlayer2Clicks()); // 開始計算玩家二的按壓次數
        StartChallengeTimer(); // 開始挑戰計時

        GenerateNewTargets(); // 生成新的範圍
        Debug.Log($"初始挑戰目標範圍: 按住時間: {holdTimeTargetMin} - {holdTimeTargetMax}, 頻率: {frequencyTargetMin} - {frequencyTargetMax}");
    }

    private void Update()
    {
        // 更新玩家一的按壓時間
        if (Input.GetKey(KeyCode.Space) && !isSphereActive) // 如果Sphere不活躍時才能按壓
        {
            player1HoldTime += Time.deltaTime; // 按住時累加時間
        }
        else
        {
            player1HoldTime = Mathf.Max(0f, player1HoldTime - Time.deltaTime); // 當未按下時，逐漸減少時間
        }

        // 檢查玩家二的右鍵按壓次數
        if (Input.GetKeyDown(KeyCode.RightArrow)) // 鍵盤的右鍵
        {
            clickCount++;
        }

        // 檢查反應時間內的按鍵輸入
        if (isReactionTimeActive && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            Debug.Log("反應時間內按鍵，遊戲重置！");
            ResetGame(); // 重置遊戲
            return; // 跳過其他檢查
        }

        // 檢查挑戰條件
        CheckConditions();

        // 更新倒數計時器
        UpdateChallengeTimer();
    }
    private void StartChallengeTimer()
    {
        challengeStartTime = Time.time; // 記錄挑戰開始的時間
    }

    private void UpdateChallengeTimer()
    {
        float timeRemaining = challengeStartTime + challengeTimeLimit - Time.time; // 計算剩餘時間
        countdownSlider.value = timeRemaining; // 更新量條值

        if (timeRemaining <= 0)
        {
            // 如果時間到，處理挑戰結束邏輯
            Debug.Log("時間到，挑戰失敗！");
            ResetGame(); // 重置遊戲
        }
    }
    private void ResetGame()
    {
        // 重置遊戲狀態
        completedChallenges = 0;
        previousIntensity = 1f;
        countdownSlider.value = challengeTimeLimit; // 重置量條
        GenerateNewTargets(); // 重置挑戰目標
    }
    private void CheckConditions()
    {
        // 檢查玩家一是否在按壓範圍內和玩家二的頻率範圍
        if (player1HoldTime >= holdTimeTargetMin && player1HoldTime <= holdTimeTargetMax &&
            player2ClickFrequency >= frequencyTargetMin && player2ClickFrequency <= frequencyTargetMax)
        {
            // 當兩人都在範圍內時，立即增強燈光
           
            Debug.Log("成功達成條件！玩家一按住時間和玩家二的按壓頻率都在範圍內。");

            // 調用挑戰協程
            if (!challengeActive) // 確保只有在挑戰未啟動的情況下啟動
            {
                challengeActive = true;
                StartCoroutine(ChallengeRoutine());
            }
        }
    }
    private IEnumerator ChallengeRoutine()
    {
        // 當兩人保持在範圍內時，立即處理挑戰成功
        completedChallenges++;
        Debug.Log("挑戰成功！");

        // 永久增強燈光亮度
        MaintainIncreasedLightIntensity();

        // 生成新的範圍
        GenerateNewTargets(); // 生成新的範圍
        Debug.Log($"新的挑戰目標範圍: 按住時間: {holdTimeTargetMin} - {holdTimeTargetMax}, 頻率: {frequencyTargetMin} - {frequencyTargetMax}");

        // 完成五次挑戰後，結束挑戰
        if (completedChallenges >= 5)
        {
            Debug.Log("所有挑戰完成！");
            ResetGame(); // 重置遊戲
        }
      
        challengeActive = false;

        yield return null; // 確保協程正確結束
    }
    private void MaintainIncreasedLightIntensity()
    {
        previousIntensity += 30f; // 成功後保存新亮度
        foreach (Light sceneLight in sceneLights)
        {
            sceneLight.intensity = previousIntensity;
        }
    }
    private IEnumerator CountPlayer2Clicks()
    {
        while (true)
        {
            // 每秒重置一次按壓次數
            clickCount = 0;
            yield return new WaitForSeconds(timeFrame);

            // 計算玩家二的右鍵按壓頻率
            player2ClickFrequency = clickCount / timeFrame;

            // 輸出玩家二的頻率到控制台
            Debug.Log($"玩家二的按壓頻率: {player2ClickFrequency}");
        }
    }
    private void GenerateNewTargets()
    {
        // 隨機生成新的按壓和頻率範圍
        holdTimeTargetMin = Random.Range(2f, 5f);
        holdTimeTargetMax = holdTimeTargetMin + 2f; // 可以調整的範圍
        frequencyTargetMin = Random.Range(1f, 3f);
        frequencyTargetMax = frequencyTargetMin + 2f; 
    }
    private IEnumerator SpawnSpheresCoroutine()
    {
        float totalDelay = 60f; // 總共的時間限制
        float spawnInterval = totalDelay / 3; // 每次生成的間隔

        while (spheresSpawned < 3) // 隨機生成5次
        {
            float delay = Random.Range(0f, spawnInterval);
            yield return new WaitForSeconds(delay); // 等待這段時間

            // 隨機持續時間（最小2秒，最大5秒）
            float lifespan = Random.Range(0.5f, 1.5f);
            currentSphere = Instantiate(spherePrefab, fixedPosition, Quaternion.identity); // 使用固定位置生成Sphere
            currentSphere.SetActive(false); // 初始為不可見

            // 顯示Sphere並在指定的時間後隱藏
            currentSphere.SetActive(true);
            isSphereActive = true; // 設置為活躍狀態
            isReactionTimeActive = true; // 啟動反應時間
            reactionTimeStart = Time.time; // 記錄反應時間開始的時間

            yield return new WaitForSeconds(lifespan); // 等待Sphere的壽命

            // 等待0.5秒以給予玩家反應時間
            while (Time.time < reactionTimeStart + reactionTimeDuration)
            {
                yield return null; // 等待0.5秒
            }

            // 隱藏Sphere
            currentSphere.SetActive(false); // 隱藏Sphere
            isSphereActive = false; // 設置為不活躍狀態
            isReactionTimeActive = false; // 重置反應時間狀態

            spheresSpawned++; // 增加已生成的Sphere數量
        }
    }
}