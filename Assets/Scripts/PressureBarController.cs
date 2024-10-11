using UnityEngine;
using UnityEngine.UI;

public class PressureBarController : MonoBehaviour
{
    public Slider pressureBar;                 // 量值條的 Slider
    public PressureSensor pressureSensor;      // 壓力感測器腳本引用
    private int targetMinValue;                // 目標範圍的最小值
    private int targetMaxValue;                // 目標範圍的最大值
    private bool isInRange = false;            // 判斷玩家是否達到目標範圍內
    private float timer = 0f;                  // 計時器，用於等待重新生成範圍

    void Start()
    {
        // 隨機選擇範圍
        SetRandomPressureRange();

        // 設置量值條的範圍顯示
        pressureBar.minValue = 70;   // 固定量值條的最小值
        pressureBar.maxValue = 1023; // 固定量值條的最大值
        pressureBar.value = targetMinValue;  // 將量值條初始設置為目標範圍的最小值
    }

    void Update()
    {
        // 從感測器讀取壓力數據
        float currentPressure = pressureSensor.GetPressureValue();

        // 判斷是否達到目標範圍
        if (currentPressure >= targetMinValue && currentPressure <= targetMaxValue)
        {
            if (!isInRange)
            {
                Debug.Log("過關！壓力達到了範圍內！");
                isInRange = true;
                // 在這裡可以添加過關邏輯，比如切換場景、播放特效等
            }
        }
        else
        {
            // 如果壓力值不在範圍內，判定為失敗
            if (isInRange)
            {
                Debug.Log("失敗！壓力未達到範圍或超過範圍，重新開始！");
                isInRange = false;

                // 等待1秒重新生成新範圍
                Invoke("SetRandomPressureRange", 1f);
            }
        }
    }

    // 隨機選擇一個壓力範圍
    void SetRandomPressureRange()
    {
        int randomRange = Random.Range(1, 6); // 隨機生成1~5之間的整數

        switch (randomRange)
        {
            case 1:
                targetMinValue = 30;
                targetMaxValue = 200;
                break;
            case 2:
                targetMinValue = 210;
                targetMaxValue = 500;
                break;
            case 3:
                targetMinValue = 510;
                targetMaxValue = 800;
                break;
            case 4:
                targetMinValue = 810;
                targetMaxValue = 1000;
                break;
            case 5:
                targetMinValue = 1010;
                targetMaxValue = 1023;
                break;
        }

        // 更新量值條的範圍顯示
        pressureBar.value = targetMinValue;

        Debug.Log("隨機選擇的範圍是: " + targetMinValue + " ~ " + targetMaxValue);
    }
}
