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
            // �p�G�w�g���\���U5�����T������A����ͦ����I
            if (correctKeyPresses >= totalCorrectPressesRequired)
            {
                Debug.Log("5 correct key presses reached. Stopping light spawn.");
                break; // ����ͦ����I
            }

            // �H����ܤ@�ӥ��I�C��]�q�۩w�q�C�⤤��ܡ^
            int randomColorIndex = Random.Range(0, lightColors.Length);
            Color randomColor = lightColors[randomColorIndex];

            // �H���ͦ���m�A��� BoxCollider ���d��
            Vector3 randomPosition = new Vector3(
                Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x),
                Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y),
                Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z)
            );

            // �ͦ����I�ó]�m�C��M�۵o��
            GameObject spawnedLight = Instantiate(lightPrefabs[randomColorIndex], randomPosition, Quaternion.identity);
            Renderer lightRenderer = spawnedLight.GetComponent<Renderer>();
            lightRenderer.material.color = randomColor;

            // �}�ҵo���ĪG�]�ϥ� Emission �ݩʡ^
            lightRenderer.material.SetColor("_EmissionColor", randomColor);

            // �]�m����������
            KeyCode keyToPress = keyMappings[randomColorIndex];

            // ��ܸӥ��I�����䴣�ܡ]�i��A�����a���D�L�����ӫ�������^
            Debug.Log("Press the key: " + keyToPress);

            // ���ݪ��a���U��������Ϊ���ɶ�����
            float timer = 0f;
            bool keyPressed = false;  // �Ψ��ˬd���a�O�_�������

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

            // �L�ת��a����P�_�A���I���|�b�ɶ����������
            Destroy(spawnedLight);

            // �H�����ݤ@�q�ɶ��A�@���ͦ��U�@�ӥ��I�����j
            float randomInterval = Random.Range(spawnIntervalRange.x, spawnIntervalRange.y);
            yield return new WaitForSeconds(randomInterval);
        }

        // �ˬd�O�_�F�즨�\���U�һݪ����I�ƶq
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
        // �b���B�}�ҸӼh���Ҧ��O��
        if (layerLights != null)
        {
            foreach (Light light in layerLights)
            {
                light.enabled = true; // �}�ҿO��
            }
        }
        Debug.Log("Lights are unlocked!");
    }

    private void OnDrawGizmos()
    {
        // �e�X BoxCollider ���d��]���ء^
        if (spawnArea != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.size);
        }
    }
}