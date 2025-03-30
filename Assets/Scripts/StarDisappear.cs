using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �ޤJ�����޲z

public class StarDisappear : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject dialoguePanel; // UI ��ܮ�
    public Text dialogueText; // UI ��r
    public Image characterPortrait; // �Y�����

    [Header("�Y��")]
    public Sprite[] startPortraits;  // �}�l��ܪ��Y��
    public Sprite[] finishPortraits; // ������ܪ��Y��

    [Header("����")]
    public AudioClip normalStarSound; // �@��P�P����
    public AudioClip finishStarSound; // FINISH �P�P����
    private AudioSource audioSource;  // ���ļ��񤸥�

    private string[] startDialogues =
    {
        "�o�̦��X���o�����P�P�C",
        "�Ш̷��C�⦬�����̡G����B����B�Ŧ�C",
        "�ϥΤ�V��θ��D�Ӳ��ʡA�e���������̧a�I",
    };

    private string[] finishDialogues =
    {
        "�{�b�A�A�w�g���x�F�b�·t���x����V�A",
        "�åB���\�����F�o�����P�P�I",
        "���C�����d�������P���D�ԻP�ܤơA",
        "�۫H�ۤv�A���ڭ̪����i�J�D�ԧa�I"
    };

    private int dialogueIndex = 0;
    private bool isDialogueActive = false;      // �T�O��ܥu�b�ݭn�ɥX�{
    private bool isFinishDialogueActive = false; // �O�_��ܧ�����ܮ�

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // �@�}�l���ù�ܮ�
        }

        if (characterPortrait != null)
        {
            characterPortrait.enabled = false; // �@�}�l�����Y��
        }

        audioSource = gameObject.AddComponent<AudioSource>(); // �۰ʲK�[ AudioSource �ե�
        ShowStartDialogue(); // �}�l����ܹ�ܮ�
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("MainCamera"))
        {
            if (gameObject.CompareTag("Finish"))
            {
                PlaySound(finishStarSound);
                ShowFinishDialogue();
            }
            else
            {
                PlaySound(normalStarSound);
                Invoke("DestroyStar", 0.5f); // ���� 0.5 ��R���P�P
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // ���񭵮�
        }
    }

    void DestroyStar()
    {
        Destroy(gameObject); // ����R���P�P�A�T�O���ļ��񧹾�
    }

    void ShowStartDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true); // ��ܹ�ܮ�
            characterPortrait.enabled = true; // ����Y��
            dialogueIndex = 0;
            UpdateDialogueUI(startDialogues, startPortraits);
            isDialogueActive = true;
        }
    }

    void ShowFinishDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true); // ��ܹ�ܮ�
            characterPortrait.enabled = true; // ����Y��
            dialogueIndex = 0;
            UpdateDialogueUI(finishDialogues, finishPortraits);
            isFinishDialogueActive = true;
        }
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextSentence();
        }

        if (isFinishDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextFinishSentence();
        }
    }

    void NextSentence()
    {
        dialogueIndex++;

        if (dialogueIndex < startDialogues.Length)
        {
            UpdateDialogueUI(startDialogues, startPortraits);
        }
        else
        {
            EndDialogue();
        }
    }

    void NextFinishSentence()
    {
        dialogueIndex++;

        if (dialogueIndex < finishDialogues.Length)
        {
            UpdateDialogueUI(finishDialogues, finishPortraits);
        }
        else
        {
            EndDialogue();
            Invoke("LoadNextScene", 2f); // ���� 2 ����������
        }
    }

    void UpdateDialogueUI(string[] dialogues, Sprite[] portraits)
    {
        dialogueText.text = dialogues[dialogueIndex]; // ��s��ܤ��e

        if (portraits != null && dialogueIndex < portraits.Length)
        {
            characterPortrait.sprite = portraits[dialogueIndex]; // ��s�������Y��
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false); // ���ù�ܮ�
        characterPortrait.enabled = false; // �����Y��
        isDialogueActive = false;
        isFinishDialogueActive = false;
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("dia1"); // �i�J���� "dia1"
    }
}
