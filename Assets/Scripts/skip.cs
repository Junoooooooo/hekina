using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    private void Update()
    {
        // �˴����a�O�_���U���� M �i�J�U�@��
        if (Input.GetKeyDown(KeyCode.M))
        {
            GoToNextLevel(); // �I�s�i�J�U�@������k
        }

        // �˴����a�O�_���U ESC ��h�X�C��
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame(); // �I�s�h�X�C������k
        }
    }

    private void GoToNextLevel()
    {
        // �����e�����W��
        string currentSceneName = SceneManager.GetActiveScene().name;

        // �ھڷ�e�����W�٤�����U�@��
       if (currentSceneName == "dia1")
        {
            SceneManager.LoadScene("instruction1");
        }
        else if (currentSceneName == "instruction1")
        {
            SceneManager.LoadScene("level1");
        }
        else if (currentSceneName == "level1")
        {
            SceneManager.LoadScene("dia2");
        }
        else if (currentSceneName == "dia2")
        {
            SceneManager.LoadScene("instruction2");
        }
        else if (currentSceneName == "instruction2")
        {
            SceneManager.LoadScene("level2");
        }
        else if (currentSceneName == "level2")
        {
            SceneManager.LoadScene("dia3");
        }
        else if (currentSceneName == "dia3")
        {
            SceneManager.LoadScene("instruction3");
        }
        else if (currentSceneName == "instruction3")
        {
            SceneManager.LoadScene("level3");
        }
        else if (currentSceneName == "level3")
        {
            SceneManager.LoadScene("end");
        }
        else if (currentSceneName == "end")
        {
            SceneManager.LoadScene("start");
        }
        else
        {
            Debug.Log("�w�g�O�̫�@���Υ��w�q�����d�I"); // ��S���U�@���ɿ�X����
        }
    }

    private void QuitGame()
    {
        Debug.Log("�C���w�h�X"); // ��ܰh�X����
        Application.Quit(); // �h�X�C��

        // �p�G�O�b�s��Ҧ��U�B��A�o��N�X�|�L�ġA���F���հh�X�C�����ĪG�A�A�i�H�ϥΥH�U�N�X�G
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
