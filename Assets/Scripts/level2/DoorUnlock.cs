using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class DoorUnlock : MonoBehaviour
{
    public GameObject[] doors; // 儲存多道大門的 GameObject 陣列
    public GameObject[] sephers; // 儲存需要隱藏的 SEPHER GameObject 陣列
    public AudioClip leftKeySound;  // 左鍵音效
    public AudioClip rightKeySound; // 右鍵音效
    public AudioClip downKeySound; // 右鍵音效
    public Image nextImage;
    private AudioSource audioSource; // 音源
    private int currentDoorIndex = 0; // 當前大門索引
    private string[][] sequences = new string[][] // 所有門的序列
    {
        new string[] { "right", "right", "left", "down", "right" }, // 第一道門
        new string[] { "right", "left", "right", "left", "right" }, // 第二道門
        new string[] { "left", "right", "left", "left", "left" }, // 第三道門
        new string[] { "right", "right", "left", "down", "right" }, // 第四道門
        new string[] { "right", "left", "left", "right", "right" }, // 第五道門
        new string[] { "left", "right", "left", "right", "right" }, // 第六道門
        new string[] { "right", "right", "left", "left", "left" }, // 第七道門
        new string[] { "left", "right", "right", "left", "left" }, // 第八道門
        new string[] { "right", "right", "right", "left", "right"}, // 第九道門
        new string[] { "left", "right", "left", "right", "left" }, // 第十道門
        // 其餘門序列...
    };

    private Queue<string> inputQueue = new Queue<string>(); // 用於存儲玩家輸入的按鍵序列
    private bool canAcceptInput = true; // 用於控制是否接受輸入

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 初始化 AudioSource
        StartUnlocking(); // 開始解鎖流程
    }

    void Update()
    {
        CheckInput(); // 保持原本的輸入檢查

        // **按下滑鼠右鍵時隱藏圖片**
        if (Input.GetMouseButtonDown(1) && nextImage != null && nextImage.gameObject.activeSelf)
        {
            nextImage.gameObject.SetActive(false);
            Time.timeScale = 1; // 恢復遊戲
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
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(HandleInputWithDelay("up"));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                StartCoroutine(HandleInputWithDelay("down"));
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
        else if (input == "down")
        {
            PlaySound(downKeySound); // 播放右鍵音效
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

        // **如果是第一道門，顯示圖片**
        if (currentDoorIndex == 0 && nextImage != null)
        {
            nextImage.gameObject.SetActive(true);
            Time.timeScale = 0; // 暫停遊戲
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
