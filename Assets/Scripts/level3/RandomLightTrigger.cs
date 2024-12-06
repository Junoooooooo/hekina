using UnityEngine;
using System.Collections;

public class RandomLightTrigger : MonoBehaviour
{
    public GameObject[] lightPrefabs; // 可用光點的預置物件
    public BoxCollider spawnArea;    // 用於隨機生成的區域（需要 BoxCollider）
    public int spawnCount = 10;      // 每層生成光點的數量
    public float lightDuration = 1f; // 每個光點持續的時間（秒）
    public Vector2 spawnIntervalRange = new Vector2(0.5f, 2f); // 光點生成間隔的隨機範圍（最小和最大時間）

    public KeyCode[] keyMappings;   // 自定義顏色對應的按鍵
    public Color[] lightColors;     // 自定義顏色的列表
    public Light[] layerLights;     // 需要開啟的燈光

    private bool hasTriggered = false; // 確保只觸發一次
    private int correctKeyPresses = 0; // 記錄成功按下的光點數量
    private int totalCorrectPressesRequired = 5; // 需要成功按下的光點數量

    private void OnTriggerEnter(Collider other)
    {
        // 檢查是否玩家進入觸發區
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // 防止重複觸發
            Debug.Log("Player entered the trigger zone. Spawning lights...");
            StartCoroutine(SpawnRandomLightsCoroutine());
        }
    }

    private IEnumerator SpawnRandomLightsCoroutine()
    {
        if (spawnArea == null)
        {
            Debug.LogError("SpawnArea is not assigned! Please assign a BoxCollider.");
            yield break;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            // 如果已經成功按下5次正確的按鍵，停止生成光點
            if (correctKeyPresses >= totalCorrectPressesRequired)
            {
                Debug.Log("5 correct key presses reached. Stopping light spawn.");
                break; // 停止生成光點
            }

            // 隨機選擇一個光點顏色（從自定義顏色中選擇）
            int randomColorIndex = Random.Range(0, lightColors.Length);
            Color randomColor = lightColors[randomColorIndex];

            // 隨機生成位置，基於 BoxCollider 的範圍
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );

            // 生成光點並設置顏色和自發光
            GameObject spawnedLight = Instantiate(lightPrefabs[randomColorIndex], randomPosition, Quaternion.identity);
            Renderer lightRenderer = spawnedLight.GetComponent<Renderer>();
            lightRenderer.material.color = randomColor;

            // 開啟發光效果（使用 Emission 屬性）
            lightRenderer.material.SetColor("_EmissionColor", randomColor);

            // 設置對應的按鍵
            KeyCode keyToPress = keyMappings[randomColorIndex];

            // 顯示該光點的按鍵提示（可選，讓玩家知道他們應該按的按鍵）
            Debug.Log("Press the key: " + keyToPress);

            // 等待玩家按下對應的鍵或直到時間結束
            float timer = 0f;
            bool keyPressed = false;  // 用來檢查玩家是否按對按鍵

            while (timer < lightDuration)
            {
                if (Input.GetKeyDown(keyToPress))
                {
                    correctKeyPresses++;
                    keyPressed = true;
                    Debug.Log("Correct key pressed! Total correct: " + correctKeyPresses);
                    break;
                }

                timer += Time.deltaTime;
                yield return null;
            }

            // 無論玩家按對與否，光點都會在時間結束後消失
            Destroy(spawnedLight);

            // 隨機等待一段時間，作為生成下一個光點的間隔
            float randomInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(randomInterval);
        }

        // 檢查是否達到成功按下所需的光點數量
        if (correctKeyPresses >= totalCorrectPressesRequired)
        {
            Debug.Log("Successfully pressed 5 keys! Unlocking lights...");
            UnlockLights();
        }
        else
        {
            Debug.Log("Not enough correct key presses.");
        }
    }


    private void UnlockLights()
    {
        // 在此處開啟該層的所有燈光
        if (layerLights != null)
        {
            foreach (Light light in layerLights)
            {
                light.enabled = true; // 開啟燈光
            }
        }
        Debug.Log("Lights are unlocked!");
    }

    private void OnDrawGizmos()
    {
        // 畫出 BoxCollider 的範圍（綠色框）
        if (spawnArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.size);
        }
    }
}