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
    public float minCubeSpawnInterval = 1f;    // 生成間隔的最小值
    public float maxCubeSpawnInterval = 3f;    // 生成間隔的最大值
    public Vector3[] centerPositions;           // 立方體生成的中心位置陣列
    public float rangeX = 5.0f;                 // X方向的隨機範圍
    public float rangeY = 5.0f;                 // Y方向的隨機範圍
    public float rangeZ = 5.0f;                 // Z方向的隨機範圍
    public Slider energyBar;                    // 能量量條的 UI 元件

    private bool isHoldingSpace = false;        // 是否按住空白鍵
    private float holdTime = 0f;                // 按住空白鍵的時間
    private float targetLightIntensity = 0f;    // 目標燈光亮度
    private float lightIntensityDecayRate = 1f; // 燈光衰減速率

    public float timeRemaining = 180f;    // 3分鐘 = 180秒
    public TMP_Text timerText;                  // 連接 UI 的 Text 元件

    private void Start()
    {
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

        StartCoroutine(SpawnCubes()); // 開始生成立方體
    }

    private void Update()
    {
        UpdateTimer();
        HandleInput();
        UpdateEnergy();
        UpdateLightIntensity(); // 更新燈光亮度
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

    private void UpdateEnergy()
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
    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = energy; // 更新量條顯示
        }
    }

    private IEnumerator SpawnCubes()
    {
        Camera mainCamera = Camera.main; // 獲取主攝影機

        while (true) // 不斷生成立方體
        {
            // 隨機生成一個位置在攝影機視口內
            float randomX = Random.Range(0.1f, 0.9f); // 視口範圍內隨機 X
            float randomY = Random.Range(0.1f, 0.9f); // 視口範圍內隨機 Y
            Vector3 randomViewportPos = new Vector3(randomX, randomY, mainCamera.nearClipPlane + 70f); // Z代表距離攝影機的距離

            // 使用 ViewportToWorldPoint 將視口座標轉換為世界座標
            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(randomViewportPos);

            // 在隨機位置生成立方體
            GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

            // 隨機生成存在時間
            float existenceTime = Random.Range(0.5f, 1f);
            float elapsedTime = 0f; // 記錄經過的時間

            // 在存在時間內檢查玩家是否按下 Enter 鍵
            while (elapsedTime < existenceTime)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    RecoverEnergy(); // 玩家成功獲取能量
                    Destroy(cube); // 刪除立方體
                    break;
                }

                elapsedTime += Time.deltaTime; // 增加經過的時間
                yield return null; // 等待下一幀
            }

            // 如果玩家沒有按下 Enter 鍵則刪除立方體
            Destroy(cube);

            // 隨機生成下一個立方體的生成間隔
            float randomSpawnInterval = Random.Range(minCubeSpawnInterval, maxCubeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval); // 等待下一次生成
        }
    }


    private void RecoverEnergy()
    {
        energy += energyRecoveryAmount;
        energy = Mathf.Min(energy, 100f);
        UpdateEnergyBar(); // 更新能量條
    }

}