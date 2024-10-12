using UnityEngine;
using System.Collections;

public class ColorSequenceGenerator : MonoBehaviour
{
    private Color[] colors = { Color.red, Color.yellow, Color.blue }; // 可用顏色
    private int sequenceLength; // 顏色序列的長度
    private Color[] colorSequence; // 儲存生成的顏色序列
    public GameObject colorObjectPrefab; // 顏色物件的預製件
    private GameObject currentColorObject; // 當前顏色物件
    private int currentInputIndex = 0; // 當前輸入索引
    private bool isInputActive = false; // 是否可以進行輸入
    public Light pointLight; // 用於指示是否完成的燈光

    void Start()
    {
        GenerateColorSequence();
        StartCoroutine(DisplayColorSequence());
    }

    void GenerateColorSequence()
    {
        sequenceLength = Random.Range(3, 6); // 隨機生成 3 到 5 的顏色序列長度
        colorSequence = new Color[sequenceLength]; // 初始化顏色序列

        // 隨機生成顏色序列
        for (int i = 0; i < sequenceLength; i++)
        {
            colorSequence[i] = colors[Random.Range(0, colors.Length)]; // 隨機選擇顏色
        }
    }

    IEnumerator DisplayColorSequence()
    {
        for (int i = 0; i < colorSequence.Length; i++)
        {
            // 創建顏色物件
            if (currentColorObject != null)
            {
                Destroy(currentColorObject); // 如果已有顏色物件，刪除
            }

            currentColorObject = Instantiate(colorObjectPrefab); // 創建新的顏色物件
            currentColorObject.GetComponent<Renderer>().material.color = colorSequence[i]; // 設置顏色

            yield return new WaitForSeconds(1.0f); // 顯示顏色 1 秒

            Destroy(currentColorObject); // 刪除顏色物件
            yield return new WaitForSeconds(0.5f); // 等待 0.5 秒再顯示下一個顏色
        }

        // 所有顏色生成完畢，現在可以接收 Makey Makey 輸入
        isInputActive = true;
        currentInputIndex = 0; // 重置輸入索引
    }

    void Update()
    {
        if (isInputActive) // 當輸入活動時，監聽 Makey Makey 按鍵輸入
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // 上鍵對應紅色
            {
                CheckInput(Color.red);
            }
            if (Input.GetKeyDown(KeyCode.Space)) // 空白鍵對應藍色
            {
                CheckInput(Color.blue);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) // 下鍵對應黃色
            {
                CheckInput(Color.yellow);
            }
        }
    }

    void CheckInput(Color inputColor)
    {
        // 檢查輸入的顏色是否正確
        if (inputColor == colorSequence[currentInputIndex])
        {
            Debug.Log("Correct color: " + inputColor);
            currentInputIndex++;

            if (currentInputIndex >= colorSequence.Length)
            {
                Debug.Log("Congratulations! You've completed the sequence.");
                // 在這裡可以加入完成後的行為，例如重置遊戲或顯示訊息
                pointLight.enabled = true; // 點亮燈光
                isInputActive = false; // 禁用輸入
            }
        }
        else
        {
            Debug.Log("Incorrect color! Try again.");
            currentInputIndex = 0; // 如果錯誤，重置輸入索引
        }
    }
}
