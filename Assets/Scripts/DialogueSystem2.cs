using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem2 : MonoBehaviour
{
    public Text dialogueText;            // ��ܮت�Text����
    public GameObject character1;        // ����1��GameObject
    public GameObject character2;        // ����2��GameObject
    public Animator character1Animator;  // ����1��Animator
    public Animator character2Animator;  // ����2��Animator
    public float typingSpeed = 0.05f;    // �v�r��ܪ��t��
    private string[] character1Dialogues; // ����1�����
    private string[] character2Dialogues; // ����2�����
    private int index = 0;               // ��e��ܪ���ܯ���
    private bool isCharacter1Turn = true; // ��e�O����1����
    private bool isDialogueFinished = false;  // �O����ܬO�_����

    void Start()
    {
        character1Dialogues = new string[]
        {
            "����1�G�A�n�I\n�o�O����1���Ĥ@�q��ܡC",
            "����1�G�A���ˡH\n����1�ݭԨ���2�C",
        };

        character2Dialogues = new string[]
        {
            "����2�G�١I\n�o�O����2���^���C",
            "����2�G�ګܦn�I\n���§A�����ߡC",
        };

        // �}�l��ܲĤ@�q���
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // �p�G��ܤw�g�����A�ˬd�O�_���U���N��i�JLEVEL1
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1();
        }
        // �p�G�ť���Q���U�B�����ܧ����A�i�J�U�@�q���
        else if (Input.anyKeyDown && dialogueText.text == GetCurrentDialogue())
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // ��ܷ�e���ܪ�����A���åt�@�Ө���
        if (isCharacter1Turn)
        {
            // ����1���ܮ���ܨ���1�A���è���2
            character1.SetActive(true);
            character2.SetActive(false);
            character1Animator.SetTrigger("dia3");
        }
        else
        {
            // ����2���ܮ���ܨ���2�A���è���1
            character1.SetActive(false);
            character2.SetActive(true);
            character2Animator.SetTrigger("dia3-2");
        }

        // �v�r��ܷ�e���⪺���
        foreach (char letter in GetCurrentDialogue().ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // ����2�����̫�@�y�ܡA�аO��ܵ���
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
                // ����1������A�����쨤��2
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
                // ����2������A�аO��ܵ����÷ǳƶi�J�U�@����
                index = 0;
                isDialogueFinished = true; // �аO����ܵ���
            }
        }

        // �i�J�U�@�q���
        StartCoroutine(ShowText());
    }

    string GetCurrentDialogue()
    {
        if (isCharacter1Turn)
            return character1Dialogues[index];
        else
            return character2Dialogues[index];
    }

    // �[�� LEVEL1 ����
    void LoadLevel1()
    {
        SceneManager.LoadScene("level3");
    }
}
