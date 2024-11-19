using UnityEngine;
using System.Collections.Generic;

public class DoorUnlock : MonoBehaviour
{
    public GameObject[] doors; // 儲存多道大門的 GameObject 陣列
    public GameObject[] sephers; // 儲存需要隱藏的 SEPHER GameObject 陣列
    private int currentDoorIndex = 0; // 當前大門索引
    private NodeController nodeController; // 引用 NodeController
    private string[][] sequences = new string[][] // 所有門的序列
    {
        new string[] {  "right", "right", "left", "left", "left"  }, // 第一道門
        new string[] {  "right", "right", "left", "left", "left"  }, // 第二道門
        new string[] { "right", "right", "left", "left", "left" },  // 第三道門
        new string[] { "right", "right", "left", "left", "left" },// 第四道門
        new string[] { "right", "right", "left", "left", "left" },// 第五道門
        new string[] { "right", "right", "left", "left", "left" },// 第六道門
        new string[] { "right", "right", "left", "left", "left" },// 第七道門
        new string[] { "right", "right", "left", "left", "left" },// 第八道門
        new string[] { "right", "right", "left", "left", "left" },// 第九道門
        new string[] { "right", "right", "left", "left", "left" },// 第十道門
        new string[] { "right", "right", "left", "left", "left" },// 第十一道門
    };

    private Queue<string> inputQueue = new Queue<string>(); // 用於存儲玩家輸入的按鍵序列

    void Start()
    {
        nodeController = FindObjectOfType<NodeController>();
        StartUnlocking();
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleInput("left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleInput("right");
        }
    }

    private void HandleInput(string input)
    {
        inputQueue.Enqueue(input); // 將玩家輸入添加到隊列中

        // 檢查當前輸入是否正確
        if (inputQueue.Count == sequences[currentDoorIndex].Length)
        {
            if (CheckSequence())
            {
                UnlockDoor(); // 如果正確，解鎖
            }
            else
            {
                inputQueue.Clear(); // 如果錯誤，清空隊列
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
        if (nodeController != null)
        {
            nodeController.UnlockCurrentDoor();
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
