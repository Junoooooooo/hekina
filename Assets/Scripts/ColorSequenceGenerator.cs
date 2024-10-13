using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSequenceGenerator : MonoBehaviour
{
    public GameObject player; // 車子的參考
    public float triggerDistance = 15f; // 距離CUBE多少時開始生成顏色
    public GameObject colorObjectPrefab; // 顏色物件的預製件
    public GameObject[] cubes; // 存放場景中所有CUBE
    public Light[] cubeLights; // 每個CUBE對應的燈光

    private Dictionary<GameObject, Color[]> cubeColorSequences = new Dictionary<GameObject, Color[]>(); // 每個CUBE的顏色序列
    private Dictionary<GameObject, int> currentInputIndex = new Dictionary<GameObject, int>(); // 每個CUBE當前的輸入索引
    private Dictionary<GameObject, bool> isInputActive = new Dictionary<GameObject, bool>(); // 每個CUBE是否可接受輸入
    private Dictionary<GameObject, bool> hasTriggered = new Dictionary<GameObject, bool>(); // 每個CUBE是否已觸發顏色生成

    private Color[] colors = { Color.red, Color.yellow, Color.blue }; // 可用顏色

    void Start()
    {
        cubes = GameObject.FindGameObjectsWithTag("CUBE"); // 找到場景中的所有CUBE

        for (int i = 0; i < cubes.Length; i++)
        {
            GenerateColorSequenceForCube(cubes[i]); // 為每個CUBE生成顏色序列
            currentInputIndex[cubes[i]] = 0; // 初始化輸入索引
            isInputActive[cubes[i]] = false; // 初始化輸入狀態
            hasTriggered[cubes[i]] = false; // 初始化觸發狀態
        }
    }


    void Update()
    {
        foreach (GameObject cube in cubes)
        {
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
                    CheckInput(cube, Color.red);
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    CheckInput(cube, Color.blue);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    CheckInput(cube, Color.yellow);
                }
            }
        }
    }

    void GenerateColorSequenceForCube(GameObject cube)
    {
        int sequenceLength = Random.Range(3, 6); // 隨機生成 3 到 5 的顏色序列長度
        Color[] colorSequence = new Color[sequenceLength]; // 初始化顏色序列

        for (int i = 0; i < sequenceLength; i++)
        {
            colorSequence[i] = colors[Random.Range(0, colors.Length)]; // 隨機選擇顏色
        }

        cubeColorSequences[cube] = colorSequence; // 為該CUBE儲存顏色序列
    }

    IEnumerator DisplayColorSequence(GameObject cube)
    {
        for (int i = 0; i < cubeColorSequences[cube].Length; i++)
        {
            Color currentColor = cubeColorSequences[cube][i];

            // 設置發光顏色
            Renderer cubeRenderer = cube.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_EmissionColor", currentColor); // 設置發光顏色
            DynamicGI.SetEmissive(cubeRenderer, currentColor); // 更新全局光照系統

            // 執行顏色漸變
            yield return StartCoroutine(ColorFade(cube, currentColor, 0.3f));

            yield return new WaitForSeconds(0.1f); // 顯示顏色多少秒

            // 重置顏色為白色或其他預設顏色
            yield return StartCoroutine(ColorFade(cube, Color.white, 0.3f)); // 漸變回白色

            // 重置發光顏色
            cubeRenderer.material.SetColor("_EmissionColor", Color.black); // 重置發光顏色為黑色
            DynamicGI.SetEmissive(cubeRenderer, Color.black); // 更新全局光照系統

            yield return new WaitForSeconds(0.1f); // 等待多少秒再顯示下一個顏色
        }

        // 顯示完畢後，開始接受輸入
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

        if (inputColor == colorSequence[currentInputIndex[cube]])
        {
            Debug.Log("Correct color for Cube: " + cube.name);
            currentInputIndex[cube]++;

            if (currentInputIndex[cube] >= colorSequence.Length)
            {
                Debug.Log("Cube " + cube.name + " sequence completed!");

                // 獲取對應的燈光並設置為白光亮度50
                Light cubeLight = cubeLights[System.Array.IndexOf(cubes, cube)];
                cubeLight.color = Color.white; // 設置燈光顏色為白色
                cubeLight.intensity = 30f; // 設置燈光亮度為50
                cubeLight.enabled = true; // 點亮該CUBE的燈光

                isInputActive[cube] = false; // 停止輸入
            }
        }
        else
        {
            Debug.Log("Incorrect color for Cube: " + cube.name + ". Try again.");
            currentInputIndex[cube] = 0; // 重置輸入索引
        }
    }


}
