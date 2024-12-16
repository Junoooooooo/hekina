using UnityEngine;
using System.Collections;

public class RandomLightTrigger : MonoBehaviour
{
    public GameObject[] lightPrefabs; // �i�Υ��I���w�m����
    public BoxCollider spawnArea;    // �Ω��H���ͦ����ϰ�]�ݭn BoxCollider�^
    public int spawnCount = 10;      // �C�h�ͦ����I���ƶq
    public float lightDuration = 1f; // �C�ӥ��I���򪺮ɶ��]��^
    public Vector2 spawnIntervalRange = new Vector2(0.5f, 2f); // ���I�ͦ����j���H���d��]�̤p�M�̤j�ɶ��^

    public KeyCode[] keyMappings;   // �۩w�q�C�����������
    public Color[] lightColors;     // �۩w�q�C�⪺�C��
    public Light[] layerLights;     // �ݭn�}�Ҫ��O��
    public GameObject[] layerObjects; // �ݭn�}�Ҫ�����]�s�W�^

    private bool hasTriggered = false; // �T�O�uĲ�o�@��
    private int correctKeyPresses = 0; // �O�����\���U�����I�ƶq
    private int totalCorrectPressesRequired = 5; // �ݭn���\���U�����I�ƶq

    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�O�_���a�i�JĲ�o��
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // �����Ĳ�o
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
                break; // ����ͦ����I
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
        // �}�ҿO��
        if (layerLights != null)
        {
            foreach (Light light in layerLights)
            {
                light.enabled = true;
            }
        }

        // �}�Ҫ���
        if (layerObjects != null)
        {
            foreach (GameObject obj in layerObjects)
            {
                obj.SetActive(true); // �ҥΪ���
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
