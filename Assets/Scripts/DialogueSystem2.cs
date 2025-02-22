using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem2 : MonoBehaviour
{
    public Text dialogueText;            // 對話框的Text元件
    public GameObject character1;        // 角色1的GameObject
    public GameObject character2;        // 角色2的GameObject
    public Animator character1Animator;  // 角色1的Animator
    public Animator character2Animator;  // 角色2的Animator
    public float typingSpeed = 0.05f;    // 逐字顯示的速度
    private string[] character1Dialogues; // 角色1的對話
    private string[] character2Dialogues; // 角色2的對話
    private int index = 0;               // 當前顯示的對話索引
    private bool isCharacter1Turn = true; // 當前是角色1說話
    private bool isDialogueFinished = false;  // 記錄對話是否結束

    void Start()
    {
        character1Dialogues = new string[]
        {
            "角色1：你好！\n這是角色1的第一段對話。",
            "角色1：你怎麼樣？\n角色1問候角色2。",
        };

        character2Dialogues = new string[]
        {
            "角色2：嗨！\n這是角色2的回答。",
            "角色2：我很好！\n謝謝你的關心。",
        };

        // 開始顯示第一段對話
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // 如果對話已經結束，檢查是否按下任意鍵進入LEVEL1
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1();
        }
        // 如果空白鍵被按下且對話顯示完畢，進入下一段對話
        else if (Input.anyKeyDown && dialogueText.text == GetCurrentDialogue())
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // 顯示當前講話的角色，隱藏另一個角色
        if (isCharacter1Turn)
        {
            // 角色1說話時顯示角色1，隱藏角色2
            character1.SetActive(true);
            character2.SetActive(false);
            character1Animator.SetTrigger("dia3");
        }
        else
        {
            // 角色2說話時顯示角色2，隱藏角色1
            character1.SetActive(false);
            character2.SetActive(true);
            character2Animator.SetTrigger("dia3-2");
        }

        // 逐字顯示當前角色的對話
        foreach (char letter in GetCurrentDialogue().ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // 當角色2講完最後一句話，標記對話結束
        if (!isCharacter1Turn && index == character2Dialogues.Length - 1)
        {
            isDialogueFinished = true;
        }
    }

    void NextDialogue()
    {
        if (isCharacter1Turn)
        {
            if (index < character1Dialogues.Length - 1)
            {
                index++;
            }
            else
            {
                // 角色1講完後，切換到角色2
                index = 0;
                isCharacter1Turn = false;
            }
        }
        else
        {
            if (index < character2Dialogues.Length - 1)
            {
                index++;
            }
            else
            {
                // 角色2講完後，標記對話結束並準備進入下一場景
                index = 0;
                isDialogueFinished = true; // 標記為對話結束
            }
        }

        // 進入下一段對話
        StartCoroutine(ShowText());
    }

    string GetCurrentDialogue()
    {
        if (isCharacter1Turn)
            return character1Dialogues[index];
        else
            return character2Dialogues[index];
    }

    // 加載 LEVEL1 場景
    void LoadLevel1()
    {
        SceneManager.LoadScene("level3");
    }
}
