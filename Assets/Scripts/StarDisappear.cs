using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // 引入場景管理

public class StarDisappear : MonoBehaviour
{
    [Header("UI 相關")]
    public GameObject dialoguePanel; // UI 對話框
    public Text dialogueText; // UI 文字

    [Header("音效")]
    public AudioClip normalStarSound; // 一般星星音效
    public AudioClip finishStarSound; // FINISH 星星音效
    private AudioSource audioSource; // 音效播放元件

    private string[] startDialogues =
    {
        "這裡有幾顆發光的星星。",
        "請依照顏色收集它們：紅色、黃色、藍色。",
        "使用方向鍵來移動，前往收集它們吧！"
    };

    private string[] finishDialogues =
    {
        "現在，你已經熟悉了在黑暗中掌握方向，",
        "並且成功收集了發光的星星！",
        "但每個關卡都有不同的挑戰與變化，",
        "相信自己，讓我們直接進入挑戰吧！"
    };

    private int dialogueIndex = 0;
    private bool isDialogueActive = false; // 確保對話只在需要時出現
    private bool isFinishDialogueActive = false; // 是否顯示完成對話框

    void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false); // 一開始隱藏對話框
        }

        audioSource = gameObject.AddComponent<AudioSource>(); // 自動添加 AudioSource 組件
        ShowStartDialogue(); // 開始時顯示對話框
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
                Invoke("DestroyStar", 0.5f); // 延遲 0.5 秒刪除星星
            }
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // 播放音效
        }
    }

    void DestroyStar()
    {
        Destroy(gameObject); // 延遲刪除星星，確保音效播放完整
    }

    void ShowStartDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true); // 顯示對話框
            dialogueIndex = 0;
            dialogueText.text = startDialogues[dialogueIndex]; // 顯示第一句
            isDialogueActive = true;
        }
    }

    void ShowFinishDialogue()
    {
        if (dialoguePanel != null && dialogueText != null)
        {
            dialoguePanel.SetActive(true); // 顯示對話框
            dialogueIndex = 0;
            dialogueText.text = finishDialogues[dialogueIndex]; // 顯示第一句
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
            dialogueText.text = startDialogues[dialogueIndex]; // 顯示下一句
        }
        else
        {
            dialoguePanel.SetActive(false); // 對話結束後關閉對話框
            isDialogueActive = false;
        }
    }

    void NextFinishSentence()
    {
        dialogueIndex++;

        if (dialogueIndex < finishDialogues.Length)
        {
            dialogueText.text = finishDialogues[dialogueIndex]; // 顯示下一句
        }
        else
        {
            dialoguePanel.SetActive(false); // 對話結束後關閉對話框
            isFinishDialogueActive = false;
            Invoke("LoadNextScene", 2f); // 延遲 2 秒後切換場景
        }
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("dia1"); // 進入場景 "dia1"
    }
}
