using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class start : MonoBehaviour
{
   

    void Start()
    {
      
     
    }

    void Update()
    {
        // �ˬd�O�_���U���N��
        if (Input.anyKeyDown)
        {
            // ����U���N��ɡA���J Level1
            SceneManager.LoadScene("intro");
        }
    }
}
