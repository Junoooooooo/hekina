using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // �ޤJ�����޲z

public class StarDisappear : MonoBehaviour
{
    [Header("UI ����")]
    public GameObject dialoguePanel; // UI ��ܮ�
    public Text dialogueText; // UI ��r

    [Header("����")]
    public AudioClip normalStarSound; // �@��P�P����
    public AudioClip finishStarSound; // FINISH �P�P����
    private AudioSource audioSource; // ���ļ��񤸥�

    private string[] startDialogues =
    {
        "�o�̦��X���o�����P�P�C",
        "�Ш̷��C�⦬�����̡G����B����B�Ŧ�C",
        "�ϥΤ�V��Ӳ��ʡA�e���������̧a�I"
    };

    private string[] finishDialogues =
    {
        "�{�b�A�A�w�g���x�F�b�·t���x����V�A",
        "�åB���\�����F�o�����P�P�I",
        "���C�����d�������P���D�ԻP�ܤơA",
        "�۫H�ۤv�A���ڭ̪����i�J�D�ԧa�I"
    };

    private int dialogueIndex = 0;
    private bool isDialogueActive = false; // �T�O��ܥu�b�ݭn�ɥX�{
    private bool isFinishDialogueActive = false; // �O�_��ܧ�����ܮ�

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // �@�}�l���ù�ܮ�
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
            dialogueIndex = 0;
            dialogueText.text = startDialogues[dialogueIndex]; // ��ܲĤ@�y
            isDialogueActive = true;
        }
    }

    void ShowFinishDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true); // ��ܹ�ܮ�
            dialogueIndex = 0;
            dialogueText.text = finishDialogues[dialogueIndex]; // ��ܲĤ@�y
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
            dialogueText.text = startDialogues[dialogueIndex]; // ��ܤU�@�y
        }
        else
        {
            dialoguePanel.SetActive(false); // ��ܵ�����������ܮ�
            isDialogueActive = false;
        }
    }

    void NextFinishSentence()
    {
        dialogueIndex++;

        if (dialogueIndex < finishDialogues.Length)
        {
            dialogueText.text = finishDialogues[dialogueIndex]; // ��ܤU�@�y
        }
        else
        {
            dialoguePanel.SetActive(false); // ��ܵ�����������ܮ�
            isFinishDialogueActive = false;
            Invoke("LoadNextScene", 2f); // ���� 2 ����������
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("dia1"); // �i�J���� "dia1"
    }
}
