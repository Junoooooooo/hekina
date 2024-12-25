using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    public GameObject introImage;    // �ѻ��Ϥ��� GameObject
    public AudioSource backgroundMusic; // ��J���֪� AudioSource
    private bool isStarted = false;

    void Start()
    {
        // �T�O�ѻ��Ϥ��b�}�l�����
        introImage.SetActive(true);
        Time.timeScale = 0f; // �Ȱ��C��
        if (backgroundMusic != null)
        {
            backgroundMusic.Stop(); // �T�O���֦b�}�l�ɤ�����
        }
    }

    void Update()
    {
        if (!isStarted && Input.GetKeyDown(KeyCode.F))
        {
            // ���U F ��}�l�C��
            introImage.SetActive(false);
            Time.timeScale = 1f; // ��_�C��
            if (backgroundMusic != null)
            {
                backgroundMusic.Play(); // ���񭵼�
            }
            isStarted = true;
        }
    }
}
