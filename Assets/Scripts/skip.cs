using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    private void Update()
    {
        // �˴����a�O�_���U���� M
        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToNextLevel(); // �I�s�i�J�U�@������k
        }
    }

    private void GoToNextLevel()
    {
        // �����e�����W��
        string currentSceneName = SceneManager.GetActiveScene().name;

        // �ھڷ�e�����W�٤�����U�@��
        if (currentSceneName == "level1")
        {
            SceneManager.LoadScene("level2");
        }
        else if (currentSceneName == "level2")
        {
            SceneManager.LoadScene("level3");
        }
        else
        {
            Debug.Log("�w�g�O�̫�@���Υ��w�q�����d�I"); // ��S���U�@���ɿ�X����
        }
    }
}
