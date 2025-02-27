using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem1 : MonoBehaviour
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
            "這座城堡，是埋藏許多被黑影們偷走的光芒的地方，但如今它成了囚禁我們的陷阱。我們需要合作，才能逃脫這裡。",
            "每一扇門都鎖住了我們的未來。你必須保持燈光的亮度，這樣我才能看到解開大門的線索。",
            "燈光的能量有限，我需要你幫助我收集光點來維持它。我們的時間不多，必須趕在黑暗再次降臨之前離開。",
            "我明白，我會仔細解讀每一道提示，幫助你打開每一扇門。我們必須迅速，不能再讓黑影再次奪走我們的希望。"
        };

        // 開始顯示對話
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // 檢查對話是否已經結束，如果是，等待玩家按下任意鍵進入下一關
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1();
        }
        // 如果按下鍵並且當前對話已經顯示完畢，進入下一段對話
        else if (Input.anyKeyDown && dialogueText.text == dialogues[currentDialogueIndex])
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // 根據當前對話索引來決定顯示哪位角色的對話
        if (currentDialogueIndex == 0 || currentDialogueIndex == 2)
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

        // 當所有對話顯示完畢，標記對話結束
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
        // 當對話結束後，跳轉至下一關
        SceneManager.LoadScene("level2");
    }
}
