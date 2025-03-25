using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class TutorialManager : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject dialoguePanel; // ��ܮ�
    public Text dialogueText; // ��r���

    [Header("�C���о�")]
    public StarColorTutorial starTutorial; // �s����~���оǸ}��
    public SecondTutorial secondTutorial; // �ĤG�ӱоǸ}�� (�A�ݭn�Ыؤ@�ӷs�� `SecondTutorial` �}��)
    private string[] dialogues =
    {
        "�w��Ө�C���оǡI",
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
    private int newDialogueIndex = 0; // �s��ܪ�����
    private bool isDialogueActive = true;
    private bool isTutorialFinished = false; // �ˬd�C���оǬO�_����

    void Start()
    {
        dialoguePanel.SetActive(true); // ��ܹ�ܮ�
        dialogueText.text = dialogues[dialogueIndex]; // ��ܲĤ@�y
        starTutorial.enabled = false; // �������C���о�
        secondTutorial.enabled = false; // �T�O�ĤG�ӱоǶ}�l�ɨS���Ұ�
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }

        // �ˬd�C���оǬO�_����
        if (starTutorial.isTutorialComplete && !isTutorialFinished)
        {
            isTutorialFinished = true;
            ShowNewDialogue();
        }

        // ���ť�����ܲĤG�q���
        if (!isDialogueActive && newDialogueIndex < newDialogues.Length && Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }

        // �p�G�ĤG�q��ܧ����A�Ұ� secondTutorial
        if (newDialogueIndex >= newDialogues.Length && !secondTutorial.enabled)
        {
            StartCoroutine(StartSecondTutorial());  // �ϥ� StartCoroutine �Ұʨ�{
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
            dialoguePanel.SetActive(false); // ������ܮ�
            isDialogueActive = false; // ����Ĥ@�q���
            starTutorial.enabled = true; // �ҰʹC���о�
        }
    }

    void ShowNewDialogue()
    {
        dialoguePanel.SetActive(true); // ��ܹ�ܮ�
        DisplayNextSentence(); // ��ܲĤ@�y
    }

    // ��ܤU�@�y���
    void DisplayNextSentence()
    {
        if (newDialogueIndex < newDialogues.Length)
        {
            dialogueText.text = newDialogues[newDialogueIndex]; // ��ܤU�@�y���
            newDialogueIndex++;
        }
        else
        {
            dialoguePanel.SetActive(false); // �Ҧ������ܧ���������ܮ�
        }
    }

    // �ҰʲĤG�ӱо�
    IEnumerator StartSecondTutorial()
    {
        yield return new WaitForSeconds(2f); // ���d2��
        SceneManager.LoadScene("secondtutorial");  // ��J�A�Q�n���J�������W��
    }

}
