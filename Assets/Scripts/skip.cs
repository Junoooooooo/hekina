using UnityEngine;
using UnityEngine.SceneManagement;

public class skip : MonoBehaviour
{
    private float rightClickTime = 0f; // �O���k����U���ɶ�
    private float holdDuration = 2f; // �ݭn�������ɶ��]4��^
    private void Update()
    {
        // �˴������ƹ��k��
        if (Input.GetMouseButton(1)) // 1 �N��ƹ��k��
        {
            rightClickTime += Time.deltaTime;
            if (rightClickTime >= holdDuration)
            {
                GoToNextLevel(); // �I�s�i�J�U�@������k
                rightClickTime = 0f; // ���m�p�ɾ�
            }
        }
        else
        {
            rightClickTime = 0f; // �Y�P�}����h���m�ɶ�
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
            SceneManager.LoadScene("level1");
        }
        else if (currentSceneName == "level1")
        {
            SceneManager.LoadScene("dia2");
        }
        else if (currentSceneName == "dia2")
        {
            SceneManager.LoadScene("level2");
        }
        else if (currentSceneName == "level2")
        {
            SceneManager.LoadScene("dia3");
        }
        else if (currentSceneName == "dia3")
        {
            SceneManager.LoadScene("level3");
        }
        else if (currentSceneName == "level3")
        {
            SceneManager.LoadScene("end");
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
