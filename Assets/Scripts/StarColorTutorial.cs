using UnityEngine;
using System.Collections;

public class StarColorTutorial : MonoBehaviour
{
    public Renderer starRenderer; // 3D 星星的 Renderer
    public Light tutorialLight; // 場景中的燈光
    public AudioSource correctSound, wrongSound; // 成功/失敗音效

    private Color[] starColors = { Color.red, new Color(0.2f, 0.4f, 0.5f), Color.yellow }; // 固定順序
    private KeyCode[] keyBindings = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow }; // 鍵盤對應
    private int currentStep = 0; // 當前步驟
    private Material starMaterial; // 星星材質
    private Color originalEmission; // 原始 Emission 顏色

    public bool isTutorialComplete = false; // 新增檢查遊戲教學是否完成的變數

    void Start()
    {
        tutorialLight.enabled = false; // 確保燈光關閉
        starMaterial = starRenderer.material;
        originalEmission = starMaterial.GetColor("_EmissionColor");
        SetStarColor(); // 設定第一顆星星顏色
    }

    void Update()
    {
        if (currentStep < starColors.Length)
        {
            if (Input.GetKeyDown(keyBindings[currentStep])) // 檢測對應按鍵
            {
                correctSound.Play();
                StartCoroutine(LightUp());
            }
            else if (Input.anyKeyDown) // 如果按錯鍵
            {
                wrongSound.Play();
                StartCoroutine(FlashStar());
            }
        }
    }

    void SetStarColor()
    {
        if (currentStep < starColors.Length)
        {
            Color newEmission = starColors[currentStep] * 1.0f; // 增加亮度
            starMaterial.SetColor("_EmissionColor", newEmission);
            starMaterial.EnableKeyword("_EMISSION"); // 確保 Emission 啟用
        }
        else
        {
            Debug.Log("教學完成！");
            isTutorialComplete = true; // 當教學結束後設置為 true
        }
    }

    IEnumerator LightUp()
    {
        tutorialLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        tutorialLight.enabled = false;

        currentStep++;
        SetStarColor();
    }

    IEnumerator FlashStar()
    {
        starMaterial.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.2f);
        SetStarColor();
    }
}
