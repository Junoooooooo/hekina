using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DialogueSystem2 : MonoBehaviour
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
            "�ڭ̲ש�Ө�o�̡A�o�O�̱���������a��C���Y�K�p���A�·t���¦b�G��C",
            "�o�����W���C�@�B���R���F�D�ԡC�A�ݭn�ӫG�ڪ��D���A�ӧګh�ݭn�ǽT�a�˷ǨC�@���I�A�קK�Q�v�l�l�W�C",
            "�C�@�ӿ��~���B�ﳣ�|���v�l��a��ڭ̡C�ڭ̥����X�@�A�O���N�R�A�~���F�̲ת������C",
            "�ڤw�g�ǳƦn�F�C�ڭ̱N�@�P��V�o�D���������e�A�����·t�A�I�k�ڭ̪��@�ɡC",
            "�o�O�ڭ̪��̫�@�q�ȵ{�C�@�_���V�����A��^�ڭ̥��h����m�I"
        };

        // �}�l��ܹ��
        StartCoroutine(ShowText());
    }

    void Update()
    {
        // �ˬd��ܬO�_�w�g�����õ��ݪ��a���U��Ӷi�J�U�@��
        if (isDialogueFinished && Input.anyKeyDown)
        {
            LoadLevel1(); // �[���U�@��
        }
        // �p�G�ť���Q���U�B�����ܧ����A�i�J�U�@�q���
        else if (Input.anyKeyDown && dialogueText.text == dialogues[currentDialogueIndex])
        {
            NextDialogue();
        }
    }

    IEnumerator ShowText()
    {
        dialogueText.text = "";

        // �ھڷ�e��ܯ��ިӨM�w��ܭ��쨤�⪺���
        if (currentDialogueIndex == 0 || currentDialogueIndex == 2 || currentDialogueIndex == 4)
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

        // ��Ҧ���ܵ����ɡA�аO��ܵ���
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
        SceneManager.LoadScene("level3");
    }
}
