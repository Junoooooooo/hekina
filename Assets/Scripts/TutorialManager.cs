using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Image characterPortrait;

    [Header("�Y��")]
    public Sprite[] characterPortraits;
    public Sprite[] newCharacterPortraits;

    [Header("�C���о�")]
    public StarColorTutorial starTutorial;
    public SecondTutorial secondTutorial;

    private string[] dialogues = {
        "�w��Ө�C���оǡI",
        "�ڭ̭n�b�o�·t�����Ҥ���^���G",
        "��A�ݨ���⪺�P�P�A�Ы��n��W����P�P�����I�G�O���C",
        "�ݨ��Ŧ�P�P�A�Ы��n��W�Ŧ�P�P����C",
        "�ݨ����P�P�A�Ы��n��W����P�P����C",
        "�ǳƦn�F�ܡH�}�l�a�I"
    };

    private string[] newDialogues = {
        "�{�b�ڭ̤w�g���x�p��b�·t���a���I�G�O��",
        "���ۧڭ̭n�յ۱���n��V",
        "�~���|�b�·t���a��g����V�I"
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
            // **�o�̰���ӨB�J�A��������ܮءA�M�������Y��**
            dialoguePanel.SetActive(false);
            characterPortrait.enabled = false;
            isDialogueActive = false;
            starTutorial.enabled = true;
        }
    }

    void ShowNewDialogue()
    {
        dialoguePanel.SetActive(true);
        characterPortrait.enabled = true;  // **�ĤG�q��ܶ}�l�A����Y��**
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
            // **�ĤG�q��ܵ����A�T�O��ܮةM�Y������**
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
