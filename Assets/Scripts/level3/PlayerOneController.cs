using UnityEngine;
using System.Collections.Generic;

public class PlayerOneController : MonoBehaviour
{
    private int currentIndex = 0; // 當前密碼索引
    private string[][] sequences = new string[][] // 所有密碼序列
    {
        new string[] { "up", "down", "up", "down", "up" }, // 第一組密碼
        new string[] { "down", "up", "down", "up", "down" }, // 第二組密碼
        new string[] { "up", "down", "up", "up", "up" }   // 第三組密碼
    };

    private Queue<string> inputQueue = new Queue<string>(); // 用於存儲玩家輸入的按鍵序列

    // 每組密碼對應的燈光和物件
    public Light[] sequenceLights1;
    public Light[] sequenceLights2;
    public Light[] sequenceLights3;

    // 每組密碼對應的 GameObject，這些 GameObject 的材質會改變 Emissive 效果
    public GameObject[] sequenceObjects1;
    public GameObject[] sequenceObjects2;
    public GameObject[] sequenceObjects3;

    // 用於儲存原始的材質和原始的 Emissive 顏色
    private Material[][] originalMaterials;
    private Color[][] originalEmissionColors;

    void Start()
    {
        // 初始化原始材質和 Emissive 顏色
        originalMaterials = new Material[3][];
        originalMaterials[0] = GetMaterialsFromObjects(sequenceObjects1);
        originalMaterials[1] = GetMaterialsFromObjects(sequenceObjects2);
        originalMaterials[2] = GetMaterialsFromObjects(sequenceObjects3);

        originalEmissionColors = new Color[3][];
        originalEmissionColors[0] = GetEmissionColorsFromMaterials(originalMaterials[0]);
        originalEmissionColors[1] = GetEmissionColorsFromMaterials(originalMaterials[1]);
        originalEmissionColors[2] = GetEmissionColorsFromMaterials(originalMaterials[2]);

        // 關閉所有材質的 Emissive 效果
        DisableAllEmissiveEffect();

        // 一開始將所有燈光關閉
        foreach (var light in sequenceLights1)
        {
            light.enabled = false;
        }
        foreach (var light in sequenceLights2)
        {
            light.enabled = false;
        }
        foreach (var light in sequenceLights3)
        {
            light.enabled = false;
        }
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            HandleInput("up");
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            HandleInput("down");
        }
    }

    private void HandleInput(string input)
    {
        inputQueue.Enqueue(input); // 將玩家輸入添加到隊列中

        // 檢查當前輸入是否正確
        if (inputQueue.Count == sequences[currentIndex].Length)
        {
            if (CheckSequence())
            {
                TriggerLightsAndMaterials(); // 如果正確，觸發燈光和材質變更
            }
            else
            {
                inputQueue.Clear(); // 如果錯誤，清空隊列
            }
        }
    }

    private bool CheckSequence()
    {
        // 檢查輸入的順序是否正確
        string[] correctSequence = sequences[currentIndex];
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (inputQueue.Dequeue() != correctSequence[i])
            {
                return false; // 如果任何一步不正確，返回 false
            }
        }
        return true; // 全部正確
    }

    private void TriggerLightsAndMaterials()
    {
        Debug.Log("Correct sequence entered!");

        // 根據密碼開啟燈光和改變物件的 Emissive 效果
        switch (currentIndex)
        {
            case 0:
                EnableLights(sequenceLights1);
                RestoreEmissiveEffect(sequenceObjects1, originalEmissionColors[0]);
                break;

            case 1:
                EnableLights(sequenceLights2);
                RestoreEmissiveEffect(sequenceObjects2, originalEmissionColors[1]);
                break;

            case 2:
                EnableLights(sequenceLights3);
                RestoreEmissiveEffect(sequenceObjects3, originalEmissionColors[2]);
                break;

            default:
                break;
        }

        // 進入下一組密碼
        currentIndex++;
        inputQueue.Clear(); // 清空隊列以便下一組密碼使用

        if (currentIndex >= sequences.Length)
        {
            Debug.Log("All sequences completed!");
        }
    }

    private void EnableLights(Light[] lights)
    {
        foreach (var light in lights)
        {
            light.enabled = true;
        }
    }

    private void DisableAllEmissiveEffect()
    {
        // 關閉所有物件的 Emissive 效果
        DisableEmissiveEffect(sequenceObjects1);
        DisableEmissiveEffect(sequenceObjects2);
        DisableEmissiveEffect(sequenceObjects3);
    }

    private void DisableEmissiveEffect(GameObject[] objects)
    {
        foreach (var obj in objects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                foreach (Material mat in renderer.materials)
                {
                    mat.DisableKeyword("_EMISSION"); // 關閉 Emissive 效果
                }
            }
        }
    }

    private void RestoreEmissiveEffect(GameObject[] objects, Color[] originalColors)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Renderer renderer = objects[i].GetComponent<Renderer>();
            if (renderer != null)
            {
                for (int j = 0; j < renderer.materials.Length; j++)
                {
                    renderer.materials[j].EnableKeyword("_EMISSION"); // 啟用 Emissive 效果
                    renderer.materials[j].SetColor("_EmissionColor", originalColors[j]); // 恢復原始的 Emissive 顏色
                }
            }
        }
    }

    private Material[] GetMaterialsFromObjects(GameObject[] objects)
    {
        List<Material> materials = new List<Material>();
        foreach (var obj in objects)
        {
            Renderer renderer = obj.GetComponent<Renderer>();
            if (renderer != null)
            {
                materials.AddRange(renderer.materials);
            }
        }
        return materials.ToArray();
    }

    private Color[] GetEmissionColorsFromMaterials(Material[] materials)
    {
        List<Color> emissionColors = new List<Color>();
        foreach (var mat in materials)
        {
            Color emissionColor = mat.GetColor("_EmissionColor");
            emissionColors.Add(emissionColor); // 儲存原始 Emissive 顏色
        }
        return emissionColors.ToArray();
    }
}
