using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public AudioSource buttonClickSound; // �ΨӼ�����䭵�Ī� AudioSource

    void Start()
    {
        // �T�O�@�}�l�S�����񭵮�
        if (buttonClickSound != null)
        {
            buttonClickSound.Stop();
        }
    }

    void Update()
    {
        // �ˬd�O�_���U���N��
        if (Input.anyKeyDown)
        {
            // ������䭵��
            if (buttonClickSound != null)
            {
                buttonClickSound.Play();
            }

            // ����U���N��ɡA���J intro ����
            SceneManager.LoadScene("intro");
        }
    }
}
