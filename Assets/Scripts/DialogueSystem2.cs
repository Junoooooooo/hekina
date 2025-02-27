using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem2 : MonoBehaviour
{
    public Text dialogueText;            // 對話框的Text元件
    public GameObject character1;        // 角色1（光之守護者）的GameObject
    public GameObject character2;        // 角色2（影之使者）的GameObject
    public Animator character1Animator;  // 角色1的Animator
    public Animator character2Animator;  // 角色2的Animator
    public float typingSpeed = 0.05f;    // 逐字顯示的速度
    private string[] dialogues;          // 所有對話
    private int currentDialogueIndex = 0; // 當前對話的索引
    private bool isDialogueFinished = false; // 記錄對話是否結束

    void Start()
    {
        // 所有角色的對話，根據順序排列
        dialogues = new string[]
        {
            "我們終於來到這裡，這是最接近光明的地方。但即便如此，黑暗依舊在逼近。",
            "這條路上的每一步都充滿了挑戰。你需要照亮我的道路，而我則需要準確地瞄準每一個點，避免被影子追上。",
            "每一個錯誤的步伐都會讓影子更靠近我們。我們必須合作，保持冷靜，才能抵達最終的光明。",
            "我已經準備好了。我們將一同跨越這道光明的門檻，不讓黑暗再侵蝕我們的世界。",
            "這是我們的最後一段旅程。一起走向光明，找回我們失去的色彩！"
        };

        // 開始顯示對話
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // 檢查對話是否已經結束並等待玩家按下鍵來進入下一關
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1(); // 加載下一關
        }
        // 如果空白鍵被按下且對話顯示完畢，進入下一段對話
        else if (Input.anyKeyDown && dialogueText.text == dialogues[currentDialogueIndex])
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // 根據當前對話索引來決定顯示哪位角色的對話
        if (currentDialogueIndex == 0 || currentDialogueIndex == 2 || currentDialogueIndex == 4)
        {
            // 角色1（光之守護者）說話
            character1.SetActive(true);
            character2.SetActive(false);
            character1Animator.SetTrigger("dia1");
        }
        else if (currentDialogueIndex == 1 || currentDialogueIndex == 3)
        {
            // 角色2（影之使者）說話
            character1.SetActive(false);
            character2.SetActive(true);
            character2Animator.SetTrigger("dia2");
        }

        // 逐字顯示對話
        foreach (char letter in dialogues[currentDialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void NextDialogue()
    {
        // 進入下一段對話
        currentDialogueIndex++;

        // 當所有對話結束時，標記對話結束
        if (currentDialogueIndex == dialogues.Length)
        {
            isDialogueFinished = true;
        }
        else
        {
            // 進入下一段對話
            StartCoroutine(ShowText());
        }
    }

    // 加載 LEVEL1 場景
    void LoadLevel1()
    {
        SceneManager.LoadScene("level3");
    }
}
