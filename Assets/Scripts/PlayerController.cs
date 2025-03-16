using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float energy = 100f;                // 玩家能量
    public float energyConsumptionRate = 1f;   // 能量消耗速率
    public float energyRecoveryAmount = 10f;   // 能量恢復量
    public float minEnergyThreshold = 0f;      // 最小能量閾值
    public Light[] lightSources;                // 燈光源陣列
    public GameObject cubePrefab;               // 立方體預製體
    public float existenceTime = 5f; // 立方體存在時間

  public float minCubeSpawnInterval = 1f;    // 生成間隔的最小值
  public float maxCubeSpawnInterval = 3f;    // 生成間隔的最大值
    public Slider energyBar;                    // 能量量條的 UI 元件
  

    private bool isHoldingSpace = false;        // 是否按住空白鍵
    private float holdTime = 0f;                // 按住空白鍵的時間
    private float targetLightIntensity = 0f;    // 目標燈光亮度
    private float lightIntensityDecayRate = 5f; // 燈光衰減速率




    public float timeRemaining = 300f;    // 3分鐘 = 180秒
    public TMP_Text timerText;                  // 連接 UI 的 Text 元件

    private void Start()
    {
        StartCoroutine(WaitForGameStart());

        UpdateTimer();  // 初始化顯示時間 
        UpdateEnergyBar(); // 初始化能量條

        // 設定燈光的初始亮度為0
        foreach (var lightSource in lightSources)
        {
            if (lightSource != null)
            {
                lightSource.intensity = 0f; // 將燈光亮度設為0
            }
        }
    }
    IEnumerator WaitForGameStart()
    {
        Debug.Log("Waiting for game to start...");
        // 等待遊戲開始（避免 Time.timeScale = 0 影響生成）
        yield return new WaitUntil(() => Time.timeScale == 1);
        Debug.Log("Game Started, Spawning Cubes...");
        StartCoroutine(SpawnCubes());
    }


    private void Update()
    {
        Debug.Log("Time.timeScale: " + Time.timeScale); // 確認時間縮放狀態
        UpdateTimer();
        HandleInput();
        UpdateEnergy();
        UpdateLightIntensity(); // 更新燈光亮度
        if (Time.timeScale > 0 && Input.GetMouseButtonDown(0))
        {
            RecoverEnergy();
        }
    }

    private void HandleInput()
    {

        if (Input.GetKey(KeyCode.Space))
        {
         
            isHoldingSpace = true;
            holdTime += Time.deltaTime; // 計算按住空白鍵的時間

            // 檢查能量是否足夠，若足夠則根據持續按住的時間來增加燈光的亮度
            if (energy > minEnergyThreshold)
            {
                targetLightIntensity = Mathf.Clamp(holdTime * 2f, 0f, 8f); // 目標亮度範圍
            }
        }
        else
        {
            isHoldingSpace = false;

            // 當空白鍵未被按住時，將目標亮度設定為0
            targetLightIntensity = 0f;
            holdTime = 0f; // 重置持續按住的時間
        }
    }

    public void UpdateEnergy()
    {
        if (isHoldingSpace && energy > minEnergyThreshold)
        {
            energy -= energyConsumptionRate * Time.deltaTime;
            energy = Mathf.Max(energy, minEnergyThreshold);
            UpdateEnergyBar(); // 更新能量條
        }
    }

    private void UpdateLightIntensity()
    {
        // 遍歷所有燈光源，更新它們的亮度
        foreach (var lightSource in lightSources)
        {
            if (lightSource != null)
            {
                // 使用 Lerp 平滑變化燈光亮度
                float currentIntensity = lightSource.intensity;

                // 當能量值大於0時，才能增加燈光亮度
                if (energy > minEnergyThreshold)
                {
                    lightSource.intensity = Mathf.Lerp(currentIntensity, targetLightIntensity, Time.deltaTime * lightIntensityDecayRate);
                }
                else
                {
                    // 能量耗盡後，燈光亮度逐漸減少至0
                    lightSource.intensity = Mathf.Lerp(currentIntensity, 0f, Time.deltaTime * lightIntensityDecayRate);
                }
            }
        }
    }
    private void UpdateTimer()
    {
        // 減少剩餘時間
        timeRemaining -= Time.deltaTime;

        // 檢查 TimerText 是否為 null
        if (timerText != null)
        {
            // 檢查時間是否大於 0
            if (timeRemaining > 0)
            {
                // 將剩餘時間轉換為分鐘和秒
                int minutes = Mathf.FloorToInt(timeRemaining / 60F);
                int seconds = Mathf.FloorToInt(timeRemaining % 60F);

                // 設定格式為 "分鐘:秒" 的形式
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
     
        
        else
            {
                // 如果時間已經結束，顯示 "Game Over"
                timerText.text = "00:00";
            }
        }
    }


    public void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = energy; // 更新量條顯示
        }
    }

    public IEnumerator SpawnCubes()
    {
        Camera mainCamera = Camera.main; // 獲取主攝影機

        while (true) // 無限循環生成立方體
        {
            // 1. 隨機生成立方體位置
            float randomX = Random.Range(0.2f, 0.8f);
            float randomY = Random.Range(0.2f, 0.8f);
            Vector3 randomViewportPos = new Vector3(randomX, randomY, mainCamera.nearClipPlane + 45f);
            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(randomViewportPos);

            // 2. 生成立方體
            GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"[生成] 立方體生成於 {spawnPosition}");

            cube.isStatic = false; // 確保立方體不是靜態物件

            // 3. 設定立方體的存在時間
            float existenceTime = Random.Range(1f, 3f);
            float elapsedTime = 0f;
            bool isCollected = false;

            while (elapsedTime < existenceTime)
            {
                Debug.Log($"[計時] elapsedTime: {elapsedTime} / {existenceTime}");

                // 檢查滑鼠點擊
                if (Input.GetMouseButtonDown(0) && cube != null)
                {
                    Debug.Log("[點擊] 立方體被點擊，執行銷毀");
                    RecoverEnergy();

                    Destroy(cube); // 直接銷毀立方體
                    isCollected = true;
                    yield return null;
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 4. 時間到了還沒點擊，則自動銷毀
            if (!isCollected && cube != null)
            {
                Debug.Log("[超時] 立方體時間到，執行銷毀");
                Destroy(cube);
            }

            // 5. 等待下一個立方體生成
            float randomSpawnInterval = Random.Range(minCubeSpawnInterval, maxCubeSpawnInterval);
            Debug.Log($"[等待] 等待 {randomSpawnInterval} 秒後生成下一個立方體");
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    public void RecoverEnergy()
    {
        energy += energyRecoveryAmount;
        energy = Mathf.Min(energy, 100f);
        UpdateEnergyBar(); // 更新能量條
        Debug.Log("能量回復！");
    }

}