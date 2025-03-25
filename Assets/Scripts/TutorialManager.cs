using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    [Header("UI 相關")]
    public GameObject dialoguePanel; // 對話框
    public Text dialogueText; // 文字顯示

    [Header("遊戲教學")]
    public StarColorTutorial starTutorial; // 連結剛才的教學腳本
    public SecondTutorial secondTutorial; // 第二個教學腳本 (你需要創建一個新的 `SecondTutorial` 腳本)
    private string[] dialogues =
    {
        "歡迎來到遊戲教學！",
        "當你看到紅色的星星，請按搖桿上紅色星星按鍵點亮燈光。",
        "看到藍色星星，請按搖桿上藍色星星按鍵。",
        "看到黃色星星，請按搖桿上黃色星星按鍵。",
        "準備好了嗎？開始吧！"
    };

    private string[] newDialogues = {
        "現在我們已經熟悉如何在黑暗的地方點亮燈光",
        "接著我們要試著控制好方向",
        "才不會在黑暗的地方迷失方向！"
    };

    private int dialogueIndex = 0;
    private int newDialogueIndex = 0; // 新對話的索引
    private bool isDialogueActive = true;
    private bool isTutorialFinished = false; // 檢查遊戲教學是否完成

    void Start()
    {
        dialoguePanel.SetActive(true); // 顯示對話框
        dialogueText.text = dialogues[dialogueIndex]; // 顯示第一句
        starTutorial.enabled = false; // 先關閉遊戲教學
        secondTutorial.enabled = false; // 確保第二個教學開始時沒有啟動
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }

        // 檢查遊戲教學是否完成
        if (starTutorial.isTutorialComplete && !isTutorialFinished)
        {
            isTutorialFinished = true;
            ShowNewDialogue();
        }

        // 按空白鍵顯示第二段對話
        if (!isDialogueActive && newDialogueIndex < newDialogues.Length && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }

        // 如果第二段對話完成，啟動 secondTutorial
        if (newDialogueIndex >= newDialogues.Length && !secondTutorial.enabled)
        {
            StartCoroutine(StartSecondTutorial());  // 使用 StartCoroutine 啟動協程
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[dialogueIndex];
        }
        else
        {
            dialoguePanel.SetActive(false); // 關閉對話框
            isDialogueActive = false; // 停止第一段對話
            starTutorial.enabled = true; // 啟動遊戲教學
        }
    }

    void ShowNewDialogue()
    {
        dialoguePanel.SetActive(true); // 顯示對話框
        DisplayNextSentence(); // 顯示第一句
    }

    // 顯示下一句對話
    void DisplayNextSentence()
    {
        if (newDialogueIndex < newDialogues.Length)
        {
            dialogueText.text = newDialogues[newDialogueIndex]; // 顯示下一句對話
            newDialogueIndex++;
        }
        else
        {
            dialoguePanel.SetActive(false); // 所有對話顯示完後關閉對話框
        }
    }

    // 啟動第二個教學
    IEnumerator StartSecondTutorial()
    {
        yield return new WaitForSeconds(2f); // 停留2秒
        SceneManager.LoadScene("secondtutorial");  // 輸入你想要載入的場景名稱
    }

}
