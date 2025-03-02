using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ImageSwitcher : MonoBehaviour
{
    public Image displayImage;  // 用來顯示的 UI Image
    public Sprite[] images; // 存放所有要顯示的圖片
    private int currentIndex = 0; // 當前圖片索引

    void Start()
    {
        if (images.Length > 0)
        {
            displayImage.sprite = images[currentIndex]; // 顯示第一張圖片
        }
    }

    void Update()
    {
        if (Input.anyKeyDown) // 按下任意鍵
        {
            if (currentIndex < images.Length - 1) // 如果還有下一張圖片
            {
                NextImage();
            }
            else // 如果已經是最後一張圖片，則進入下一個場景
            {
                LoadNextLevel();
            }
        }
    }

    void NextImage()
    {
        currentIndex++;
        if (currentIndex < images.Length)
        {
            displayImage.sprite = images[currentIndex]; // 顯示下一張圖片
        }
    }

    void LoadNextLevel()
    {
        SceneManager.LoadScene("level2"); // 替換 "NextLevel" 為你的場景名稱
    }
}
