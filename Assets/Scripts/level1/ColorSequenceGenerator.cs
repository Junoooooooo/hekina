﻿using UnityEngine;
using UnityEngine.UI; // 引入UI命名空間
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CubeLightInfo
{

    public GameObject cube; // CUBE物件
    public Light[] lights; // 與該CUBE對應的燈光
    public GameObject[] emissiveObjects; // 與該CUBE對應的發光物件
}

public class ColorSequenceGenerator : MonoBehaviour
{
    public GameObject player; // 車子的參考
    public float triggerDistance = 15f; // 距離CUBE多少時開始生成顏色
    public GameObject colorObjectPrefab; // 顏色物件的預製件
    public CubeLightInfo[] cubeLightInfos; // 存放每個CUBE及其對應燈光的資訊

    // 新增：UI 圖片來顯示
    public Image pauseImage; // 用來顯示的圖片
    public Image correctImage; // 顯示正確圖案
    public Image incorrectImage; // 顯示錯誤圖案
    public AudioSource audioSource; // 用於播放音效
    public AudioClip correctSound;  // 正確音效
    public AudioClip incorrectSound; // 錯誤音效
   
    private Light mylight;
    private bool hasShownImage = false; // 確保圖片只顯示一次
    private bool isPaused = false; // 控制是否遊戲暫停
    private GameObject firstCube; // 用來記錄第一個 Cube




    private Dictionary<GameObject, Color[]> cubeColorSequences = new Dictionary<GameObject, Color[]>(); // 每個CUBE的顏色序列
    private Dictionary<GameObject, int> currentInputIndex = new Dictionary<GameObject, int>(); // 每個CUBE當前的輸入索引
    private Dictionary<GameObject, bool> isInputActive = new Dictionary<GameObject, bool>(); // 每個CUBE是否可接受輸入
    private Dictionary<GameObject, bool> hasTriggered = new Dictionary<GameObject, bool>(); // 每個CUBE是否已觸發顏色生成

    private Color[] colors = {Color.yellow, new Color(0.6f, 1f, 1f),Color.red,  new Color(1f, 0.5f, 0f), Color.white, new Color(0.5f, 0.5f, 0.5f) }; // 可用顏色

    private bool isFirstCubeComplete = false; // 紀錄第一個 Cube 是否完成顏色序列
    void Start()
    {
        mylight = GetComponent<Light>();
        mylight.enabled = false;

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); // 如果沒有 AudioSource，就添加一個
        }

      /*  if (secondImage != null)
        {
            secondImage.gameObject.SetActive(false);
        }*/
        foreach (var cubeLightInfo in cubeLightInfos)
        {
            GameObject cube = cubeLightInfo.cube;
            GenerateColorSequenceForCube(cube);
            currentInputIndex[cube] = 0;
            isInputActive[cube] = false;
            hasTriggered[cube] = false;

            foreach (GameObject emissiveObject in cubeLightInfo.emissiveObjects)
            {
                Renderer emissiveRenderer = emissiveObject.GetComponent<Renderer>();
                emissiveObject.GetComponent<MeshRenderer>().material.DisableKeyword("_EMISSION");
            }
        }

 
        if (pauseImage != null)
        {
            pauseImage.gameObject.SetActive(false);
        }

    }

    void Update()
    {      
        // 如果燈關閉，則允許下一次重新觸發
        if (!mylight.enabled)
        {
            hasShownImage = false;
        }
    
    
        foreach (var cubeLightInfo in cubeLightInfos)
        {
            GameObject cube = cubeLightInfo.cube; // 獲取CUBE
            float distance = Vector3.Distance(cube.transform.position, player.transform.position); // 計算車子與CUBE的距離

            if (distance <= triggerDistance && !hasTriggered[cube])
            {
                hasTriggered[cube] = true; // 確保只觸發一次
                StartCoroutine(DisplayColorSequence(cube)); // 顯示該CUBE的顏色序列
            }

            if (isInputActive[cube]) // 當該CUBE可以進行輸入時，監聽按鍵輸入
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    CheckInput(cube, Color.yellow);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    CheckInput(cube, new Color(0.6f, 1f, 1f));
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    CheckInput(cube, Color.red);
                }
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    CheckInput(cube, new Color(1f, 0.5f, 0f));
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CheckInput(cube, Color.white);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    CheckInput(cube, new Color(0.5f, 0.5f, 0.5f));
                }
            }
        }
    }

    void GenerateColorSequenceForCube(GameObject cube)
    {
        int sequenceLength = 1; // 設定顏色序列長度為3
        Color[] colorSequence = new Color[sequenceLength]; // 初始化顏色序列

            // 其他 Cube 隨機顏色
            for (int i = 0; i < sequenceLength; i++)
            {
                colorSequence[i] = colors[Random.Range(0, colors.Length)];
            }
        

        cubeColorSequences[cube] = colorSequence; // 為該CUBE儲存顏色序列
    }

    IEnumerator DisplayColorSequence(GameObject cube)
    {
        Renderer cubeRenderer = cube.GetComponent<Renderer>();

        // 先關閉發光效果
        cubeRenderer.material.SetColor("_EmissionColor", Color.black);
        DynamicGI.SetEmissive(cubeRenderer, Color.black); // 更新全局光照系統

        for (int i = 0; i < cubeColorSequences[cube].Length; i++)
        {
            Color currentColor = cubeColorSequences[cube][i];

            // 設置發光顏色
            cubeRenderer.material.SetColor("_EmissionColor", currentColor);
            DynamicGI.SetEmissive(cubeRenderer, currentColor);

            // 顏色漸變
            yield return StartCoroutine(ColorFade(cube, currentColor, 0.15f));
            yield return new WaitForSeconds(0.15f); // 顯示顏色

            // 漸變回白色
            yield return StartCoroutine(ColorFade(cube, Color.white, 0.15f));

            // 重置發光顏色
            cubeRenderer.material.SetColor("_EmissionColor", Color.black);
            DynamicGI.SetEmissive(cubeRenderer, Color.black);

            yield return new WaitForSeconds(0.15f); // 等待下一個顏色
        }

       

        // 開始接受輸入
        isInputActive[cube] = true;
        currentInputIndex[cube] = 0;

    }

    IEnumerator ColorFade(GameObject cube, Color targetColor, float duration)
    {
        Renderer cubeRenderer = cube.GetComponent<Renderer>();
        Color initialColor = cubeRenderer.material.color;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            cubeRenderer.material.color = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 等待下一幀
        }

        cubeRenderer.material.color = targetColor; // 確保最終顏色設定為目標顏色
    }

    void CheckInput(GameObject cube, Color inputColor)
    {
        Color[] colorSequence = cubeColorSequences[cube];

        if (correctSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(correctSound);
        }

        // 檢查玩家的輸入是否與顏色序列匹配
        if (inputColor == colorSequence[currentInputIndex[cube]])
        {
            Debug.Log("Correct color for Cube: " + cube.name);
            currentInputIndex[cube]++;
          
            // 若玩家輸入完整序列
            if (currentInputIndex[cube] >= colorSequence.Length)
            {                
                Debug.Log("Cube " + cube.name + " sequence completed!");

                // 獲取 Cube 對應的燈光
                CubeLightInfo cubeLightInfo = GetCubeLightInfo(cube);

                // 開啟燈光
                foreach (Light cubeLight in cubeLightInfo.lights)
                {
                    if (!cubeLight.enabled)
                    {
                        cubeLight.enabled = true;
                        Debug.Log("Light enabled: " + cubeLight.name);
                    }
                }

                // 啟用發光物件的發光效果
                foreach (GameObject emissiveObject in cubeLightInfo.emissiveObjects)
                {
                    Renderer emissiveRenderer = emissiveObject.GetComponent<Renderer>();
                    if (emissiveRenderer != null)
                    {
                        emissiveRenderer.material.EnableKeyword("_EMISSION");
                        DynamicGI.SetEmissive(emissiveRenderer, Color.white);
                        Debug.Log("Enabled emission for: " + emissiveObject.name);
                    }
                }

                // 重置當前索引並禁用輸入
                currentInputIndex[cube] = 0;
                isInputActive[cube] = false;
            }
        }
        else
        {
            // 播放錯誤的音效
            if (incorrectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(incorrectSound);
            }

            Debug.Log("Incorrect color for Cube: " + cube.name);
        }
    }


    CubeLightInfo GetCubeLightInfo(GameObject cube)
    {
        foreach (var cubeLightInfo in cubeLightInfos)
        {
            if (cubeLightInfo.cube == cube)
            {
                return cubeLightInfo;
            }
        }
        return null;
    }
}