using UnityEngine;
using UnityEngine.SceneManagement; // �ޤJ�����޲z���R�W�Ŷ�

public class LevelStartController : MonoBehaviour
{
    private bool gameStarted = false;  // �P�_�C���O�_�w�g�}�l

    void Update()
    {
        // �ˬd���a�O�_���U�ť���
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            StartGame();
        }
    }

    void StartGame()
    {
        gameStarted = true; // �аO�C���}�l

        // �[�� Level1 ����
        SceneManager.LoadScene("Level1"); // �нT�O Level1 �����W�٥��T
    }
}
