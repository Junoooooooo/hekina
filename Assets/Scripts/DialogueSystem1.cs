using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem1 : MonoBehaviour
{
    public Text dialogueText;            // ��ܮت�Text����
    public GameObject character1;        // ����1�]�����u�@�̡^��GameObject
    public GameObject character2;        // ����2�]�v���Ϫ̡^��GameObject
    public Animator character1Animator;  // ����1��Animator
    public Animator character2Animator;  // ����2��Animator
    public float typingSpeed = 0.05f;    // �v�r��ܪ��t��
    private string[] dialogues;          // �Ҧ����
    private int currentDialogueIndex = 0; // ��e��ܪ�����
    private bool isDialogueFinished = false; // �O����ܬO�_����

    void Start()
    {
        // �Ҧ����⪺��ܡA�ھڶ��ǱƦC
        dialogues = new string[]
        {
            "�o�y�����A�O�I�ó\�h�Q�¼v�̰��������~���a��A���p�������F�}�T�ڭ̪������C�ڭ̻ݭn�X�@�A�~��k��o�̡C",
            "�C�@���������F�ڭ̪����ӡC�A�����O���O�����G�סA�o�˧ڤ~��ݨ�Ѷ}�j�����u���C",
            "�O������q�����A�ڻݭn�A���U�ڦ������I�Ӻ������C�ڭ̪��ɶ����h�A�������b�·t�A�����{���e���}�C",
            "�ک��աA�ڷ|�J�Ӹ�Ū�C�@�D���ܡA���U�A���}�C�@�����C�ڭ̥������t�A����A���¼v�A���ܨ��ڭ̪��Ʊ�C"
        };

        // �}�l��ܹ��
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // �ˬd��ܬO�_�w�g�����A�p�G�O�A���ݪ��a���U���N��i�J�U�@��
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1();
        }
        // �p�G���U��åB��e��ܤw�g��ܧ����A�i�J�U�@�q���
        else if (Input.anyKeyDown && dialogueText.text == dialogues[currentDialogueIndex])
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // �ھڷ�e��ܯ��ިӨM�w��ܭ��쨤�⪺���
        if (currentDialogueIndex == 0 || currentDialogueIndex == 2)
        {
            // ����1�]�����u�@�̡^����
            character1.SetActive(true);
            character2.SetActive(false);
            character1Animator.SetTrigger("dia1");
        }
        else if (currentDialogueIndex == 1 || currentDialogueIndex == 3)
        {
            // ����2�]�v���Ϫ̡^����
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
        // �i�J�U�@�q���
        currentDialogueIndex++;

        // ��Ҧ������ܧ����A�аO��ܵ���
        if (currentDialogueIndex == dialogues.Length)
        {
            isDialogueFinished = true;
        }
        else
        {
            // �i�J�U�@�q���
            StartCoroutine(ShowText());
        }
    }

    // �[�� LEVEL1 ����
    void LoadLevel1()
    {
        // ���ܵ�����A����ܤU�@��
        SceneManager.LoadScene("level2");
    }
}
