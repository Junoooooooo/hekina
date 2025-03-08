using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public Text dialogueText;
    public GameObject character1;
    public GameObject character2;
    public Animator character1Animator;
    public Animator character2Animator;
    public float typingSpeed = 0.08f;
    private string[] dialogues;
    private int currentDialogueIndex = 0;
    private bool isDialogueFinished = false;

    void Start()
    {
        dialogues = new string[]
        {
            "這是一個沉睡的世界。\n每個生命都有屬於自己的色彩，但我們卻被黑暗所囚禁。",
            "那些色彩，曾是我們的希望，現在卻淹沒在無盡的黑暗中。",
            "唯有光芒能帶來希望，但…黑影偷走了我們的光。",
            "那麼，我們該如何找回光明呢？",
            "我們必須一起走過這條隧道。\n你需要引領我，確保我們能夠觸發正確的光源。",
            "我將解開那些顏色的謎題，我需要你幫助我保持前進的方向。\n我們的路只有在光明中才能看得見。",
            "沒問題，我會引領我們走向光明。\n我們一起將黑暗驅逐！"
        };

        StartCoroutine(ShowText());
    }

    void Update()
    {
        // 如果對話已經結束，且按下任何鍵，就加載 LEVEL1 場景
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1();
        }
        // 如果按下任意鍵，且當前對話已經顯示完畢，就進入下一段對話
        else if (Input.anyKeyDown && dialogueText.text == dialogues[currentDialogueIndex])
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // 根據當前對話索引顯示不同角色的對話
        if (currentDialogueIndex == 0 || currentDialogueIndex == 1 || currentDialogueIndex == 2 || currentDialogueIndex == 4 || currentDialogueIndex == 5)
        {
            character1.SetActive(true);
            character2.SetActive(false);
            character1Animator.SetTrigger("dia1");
        }
        else if (currentDialogueIndex == 3 || currentDialogueIndex == 6)
        {
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
        // 確保在對話結束後，不會再顯示下一段對話
        if (currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++;
            StartCoroutine(ShowText());
        }
        else
        {
            isDialogueFinished = true; // 表示對話結束
        }
    }

    void LoadLevel1()
    {
        // 加載 LEVEL1 場景
        SceneManager.LoadScene("instruction1");
    }
}
