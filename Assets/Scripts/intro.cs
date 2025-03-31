using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class intro : MonoBehaviour
{


    void Start()
    {


    }

    void Update()
    {
        // 檢查是否按下任意鍵
        if (Input.anyKeyDown)
        {
            // 當按下任意鍵時，載入 Level1
            SceneManager.LoadScene("initial");
        }
    }
}
