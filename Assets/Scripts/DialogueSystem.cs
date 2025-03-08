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
            "�o�O�@�ӨI�Ϊ��@�ɡC\n�C�ӥͩR�����ݩ�ۤv����m�A���ڭ̫o�Q�·t�ҥ}�T�C",
            "���Ǧ�m�A���O�ڭ̪��Ʊ�A�{�b�o�T�S�b�L�ɪ��·t���C",
            "�ߦ����~��a�ӧƱ�A���K�¼v�����F�ڭ̪����C",
            "����A�ڭ̸Ӧp���^�����O�H",
            "�ڭ̥����@�_���L�o���G�D�C\n�A�ݭn�޻�ڡA�T�O�ڭ̯��Ĳ�o���T�������C",
            "�ڱN�Ѷ}�����C�⪺���D�A�ڻݭn�A���U�ګO���e�i����V�C\n�ڭ̪����u���b�������~��ݱo���C",
            "�S���D�A�ڷ|�޻�ڭ̨��V�����C\n�ڭ̤@�_�N�·t�X�v�I"
        };

        StartCoroutine(ShowText());
    }

    void Update()
    {
        // �p�G��ܤw�g�����A�B���U������A�N�[�� LEVEL1 ����
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1();
        }
        // �p�G���U���N��A�B��e��ܤw�g��ܧ����A�N�i�J�U�@�q���
        else if (Input.anyKeyDown && dialogueText.text == dialogues[currentDialogueIndex])
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // �ھڷ�e��ܯ�����ܤ��P���⪺���
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

        // �v�r��ܹ��
        foreach (char letter in dialogues[currentDialogueIndex].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void NextDialogue()
    {
        // �T�O�b��ܵ�����A���|�A��ܤU�@�q���
        if (currentDialogueIndex < dialogues.Length - 1)
        {
            currentDialogueIndex++;
            StartCoroutine(ShowText());
        }
        else
        {
            isDialogueFinished = true; // ��ܹ�ܵ���
        }
    }

    void LoadLevel1()
    {
        // �[�� LEVEL1 ����
        SceneManager.LoadScene("instruction1");
    }
}
