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
    public GameObject[] layerObjects; // 需要開啟的物件（新增）

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
            Debug.Log("Triggered Object: " + gameObject.name);
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
            if (correctKeyPresses >= totalCorrectPressesRequired)
            {
                Debug.Log("5 correct key presses reached. Stopping light spawn.");
                break; // 停止生成光點
            }

            int randomColorIndex = Random.Range(0, lightColors.Length);
            Color randomColor = lightColors[randomColorIndex];

            Vector3 randomPosition = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );

            GameObject spawnedLight = Instantiate(lightPrefabs[randomColorIndex], randomPosition, Quaternion.identity);
            Renderer lightRenderer = spawnedLight.GetComponent<Renderer>();
            lightRenderer.material.color = randomColor;
            lightRenderer.material.SetColor("_EmissionColor", randomColor);

            KeyCode keyToPress = keyMappings[randomColorIndex];
            Debug.Log("Press the key: " + keyToPress);

            float timer = 0f;
            bool keyPressed = false;

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

            Destroy(spawnedLight);

            float randomInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(randomInterval);
        }

        if (correctKeyPresses >= totalCorrectPressesRequired)
        {
            Debug.Log("Successfully pressed 5 keys! Unlocking lights and objects...");
            UnlockLightsAndObjects();
        }
        else
        {
            Debug.Log("Not enough correct key presses.");
        }
    }

    private void UnlockLightsAndObjects()
    {
        // 開啟燈光
        if (layerLights != null)
        {
            foreach (Light light in layerLights)
            {
                light.enabled = true;
            }
        }

        // 開啟物件
        if (layerObjects != null)
        {
            foreach (GameObject obj in layerObjects)
            {
                obj.SetActive(true); // 啟用物件
            }
        }

        Debug.Log("Lights and objects are unlocked!");
    }

    private void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.size);
        }
    }
}
