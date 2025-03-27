using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI 相關")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Image characterPortrait;

    [Header("頭像")]
    public Sprite[] characterPortraits;
    public Sprite[] newCharacterPortraits;

    [Header("遊戲教學")]
    public StarColorTutorial starTutorial;
    public SecondTutorial secondTutorial;

    private string[] dialogues = {
        "歡迎來到遊戲教學！",
        "我們要在這黑暗的環境中找回光亮",
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
    private int newDialogueIndex = 0;
    private bool isDialogueActive = true;
    private bool isTutorialFinished = false;

    void Start()
    {
        dialoguePanel.SetActive(true);
        characterPortrait.enabled = true;
        dialogueText.text = dialogues[dialogueIndex];
        characterPortrait.sprite = characterPortraits[dialogueIndex];

        starTutorial.enabled = false;
        secondTutorial.enabled = false;
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }

        if (starTutorial.isTutorialComplete && !isTutorialFinished)
        {
            isTutorialFinished = true;
            ShowNewDialogue();
        }

        if (!isDialogueActive && newDialogueIndex < newDialogues.Length && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }

        if (newDialogueIndex >= newDialogues.Length && !secondTutorial.enabled)
        {
            StartCoroutine(StartSecondTutorial());
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;

        if (dialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[dialogueIndex];
            characterPortrait.sprite = characterPortraits[dialogueIndex];
        }
        else
        {
            // **這裡做兩個步驟，先關閉對話框，然後關閉頭像**
            dialoguePanel.SetActive(false);
            characterPortrait.enabled = false;
            isDialogueActive = false;
            starTutorial.enabled = true;
        }
    }

    void ShowNewDialogue()
    {
        dialoguePanel.SetActive(true);
        characterPortrait.enabled = true;  // **第二段對話開始，顯示頭像**
        DisplayNextSentence();
    }

    void DisplayNextSentence()
    {
        if (newDialogueIndex < newDialogues.Length)
        {
            dialogueText.text = newDialogues[newDialogueIndex];
            characterPortrait.sprite = newCharacterPortraits[newDialogueIndex];
            newDialogueIndex++;
        }
        else
        {
            // **第二段對話結束，確保對話框和頭像關閉**
            dialoguePanel.SetActive(false);
            characterPortrait.enabled = false;
        }
    }

    IEnumerator StartSecondTutorial()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("secondtutorial");
    }
}
