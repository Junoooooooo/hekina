using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DoorUnlock : MonoBehaviour
{
    public float energy = 100f;                // 玩家能量
    public float energyConsumptionRate = 1f;   // 能量消耗速率
    public float energyRecoveryAmount = 10f;   // 能量恢復量
    public Light[] lightSources;                // 燈光源陣列
    public float minEnergyThreshold = 0f;      // 最小能量閾值
    public Slider energyBar;                    // 能量量條的 UI 元件
    public GameObject[] doors; // 儲存多道大門的 GameObject 陣列
    public GameObject[] sephers; // 儲存需要隱藏的 SEPHER GameObject 陣列
    public AudioClip leftKeySound;  // 左鍵音效
    public AudioClip rightKeySound; // 右鍵音效
    public Image nextImage;
    public float timeRemaining = 300f;    // 3分鐘 = 180秒
    public TMP_Text timerText;                  // 連接 UI 的 Text 元件
    private AudioSource audioSource; // 音源
    private int currentDoorIndex = 0; // 當前大門索引
    private string[][] sequences = new string[][] // 所有門的序列
    {
        new string[] { "right", "right", "left" }, // 第一道門
        new string[] { "right", "left", "right" }, // 第二道門
        new string[] { "left", "right", "left"}, // 第三道門
        new string[] { "right", "right", "left"}, // 第四道門
        new string[] { "right", "left", "left"}, // 第五道門
        new string[] { "left", "right", "left"}, // 第六道門
        new string[] { "right", "right", "left" }, // 第七道門
        new string[] { "left", "right", "right"}, // 第八道門
        new string[] { "right", "right", "right"} ,// 第九道門
        new string[] { "left", "right", "left"}, // 第十道門
        // 其餘門序列...
    };

    private Queue<string> inputQueue = new Queue<string>(); // 用於存儲玩家輸入的按鍵序列
    private bool canAcceptInput = true; // 用於控制是否接受輸入

    private bool isHoldingSpace = false;        // 是否按住空白鍵
    private float holdTime = 0f;                // 按住空白鍵的時間
    private float targetLightIntensity = 0f;    // 目標燈光亮度
    private float lightIntensityDecayRate = 5f; // 燈光衰減速率

    void Start()
    {
        StartCoroutine(WaitForGameStart());

        UpdateTimer();  // 初始化顯示時間 
        UpdateEnergyBar(); // 初始化能量條
        audioSource = GetComponent<AudioSource>(); // 初始化 AudioSource
        StartUnlocking(); // 開始解鎖流程
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
    }

    void Update()
    {
        CheckInput(); // 保持原本的輸入檢查
        Debug.Log("Time.timeScale: " + Time.timeScale); // 確認時間縮放狀態
        UpdateTimer();
        HandleInput();
        //UpdateEnergy();
        UpdateLightIntensity(); // 更新燈光亮度
        UpdateEnergy();
    }

    private void HandleInput()
    {

        if (Input.GetKey(KeyCode.Space) ||
    Input.GetKey(KeyCode.DownArrow) ||
    Input.GetMouseButton(0)) // 滑鼠左鍵)
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
    private void CheckInput()
    {
        if (canAcceptInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(HandleInputWithDelay("left"));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(HandleInputWithDelay("right"));
            }
        }
    }

    private IEnumerator HandleInputWithDelay(string input)
    {
        canAcceptInput = false; // 暫時禁用輸入

        // 播放對應的音效
        if (input == "left")
        {
            PlaySound(leftKeySound); // 播放左鍵音效
        }
        else if (input == "right")
        {
            PlaySound(rightKeySound); // 播放右鍵音效
        }

        HandleInput(input); // 處理輸入邏輯
        yield return new WaitForSeconds(0.2f); // 等待 0.4 秒
        canAcceptInput = true; // 恢復輸入
    }

    private void HandleInput(string input)
    {
        // 獲取當前正確序列
        string[] correctSequence = sequences[currentDoorIndex];

        // 添加玩家輸入到佇列中
        inputQueue.Enqueue(input);

        // 當輸入佇列達到序列長度時進行檢查
        if (inputQueue.Count == correctSequence.Length)
        {
            if (CheckSequence())
            {
                UnlockDoor(); // 如果正確，解鎖門

                RecoverEnergy();

            }
            else
            {
                Debug.Log("Incorrect sequence! Please try again.");
                inputQueue.Clear(); // 清空佇列，允許重新輸入
            }
        }
    }

    private bool CheckSequence()
    {
        // 檢查輸入的順序是否正確
        string[] correctSequence = sequences[currentDoorIndex];
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (inputQueue.Dequeue() != correctSequence[i])
            {
                return false; // 如果任何一步不正確，返回 false
            }
        }
        return true; // 全部正確
    }

    private void UnlockDoor()
    {
        Debug.Log("Door " + (currentDoorIndex + 1) + " Unlocked!");
        doors[currentDoorIndex].SetActive(false); // 隱藏大門

        // 隱藏相應的 SEPHER
        if (currentDoorIndex < sephers.Length)
        {
            sephers[currentDoorIndex].SetActive(false); // 隱藏對應的 SEPHER
        }

        currentDoorIndex++; // 移動到下一道門
        inputQueue.Clear(); // 清空隊列以便下一扇門使用
        if (currentDoorIndex < doors.Length)
        {
            StartUnlocking(); // 開始下一道門的解鎖流程

        }
        else
        {
            Debug.Log("All doors unlocked!");
        }
    }

    public void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = energy; // 更新量條顯示
        }
    }

    public void RecoverEnergy()
    {
        energy += energyRecoveryAmount;
        energy = Mathf.Min(energy, 100f);
        UpdateEnergyBar(); // 更新能量條
        Debug.Log("能量回復！");
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, 1.0f); // 最大音量播放音效
        }
    }

    public void StartUnlocking()
    {
        if (currentDoorIndex < doors.Length)
        {
            // 重置步驟
        }
    }
}