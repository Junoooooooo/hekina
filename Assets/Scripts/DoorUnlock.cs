using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DoorUnlock : MonoBehaviour
{
    public GameObject[] doors; // 儲存多道大門的 GameObject 陣列
    public GameObject[] sephers; // 儲存需要隱藏的 SEPHER GameObject 陣列
    private int currentDoorIndex = 0; // 當前大門索引
    private NodeController nodeController; // 引用 NodeController
    private string[][] sequences = new string[][] // 所有門的序列
    {
        new string[] { "right", "right", "left", "left", "left" }, // 第一道門
        new string[] { "right", "left", "right", "left", "right" }, // 第二道門
        new string[] { "left", "right", "right", "left", "right" }, // 第三道門
        new string[] { "left", "right", "left", "right", "left" }, // 第四道門
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
      //  nodeController = FindObjectOfType<NodeController>();
        StartUnlocking();
    }

    void Update()
    {
        CheckInput();
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
        HandleInput(input);
        yield return new WaitForSeconds(0.2f); // 等待 0.1 秒
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

        // 通知 NodeController 繼續移動
        /*if (nodeController != null)
        {
            nodeController.UnlockCurrentDoor();
        }*/
    }

    public void StartUnlocking()
    {
        if (currentDoorIndex < doors.Length)
        {
            // 重置步驟
        }
    }
}
