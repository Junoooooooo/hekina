using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float energy = 100f;                // ���a��q
    public float energyConsumptionRate = 1f;   // ��q���ӳt�v
    public float energyRecoveryAmount = 10f;   // ��q��_�q
    public float minEnergyThreshold = 0f;      // �̤p��q�H��
    public Light[] lightSources;                // �O�����}�C
    public GameObject cubePrefab;               // �ߤ���w�s��
    public float minCubeSpawnInterval = 1f;    // �ͦ����j���̤p��
    public float maxCubeSpawnInterval = 3f;    // �ͦ����j���̤j��
    public Vector3[] centerPositions;           // �ߤ���ͦ������ߦ�m�}�C
    public float rangeX = 5.0f;                 // X��V���H���d��
    public float rangeY = 5.0f;                 // Y��V���H���d��
    public float rangeZ = 5.0f;                 // Z��V���H���d��
    public Slider energyBar;                    // ��q�q���� UI ����

    private bool isHoldingSpace = false;        // �O�_����ť���
    private float holdTime = 0f;                // ����ť��䪺�ɶ�
    private float targetLightIntensity = 0f;    // �ؼпO���G��
    private float lightIntensityDecayRate = 1f; // �O���I��t�v

    private void Start()
    {
        UpdateEnergyBar(); // ��l�Ư�q��

        // �]�w�O������l�G�׬�0
        foreach (var lightSource in lightSources)
        {
            if (lightSource != null)
            {
                lightSource.intensity = 0f; // �N�O���G�׳]��0
            }
        }

        StartCoroutine(SpawnCubes()); // �}�l�ͦ��ߤ���
    }

    private void Update()
    {
        HandleInput();
        UpdateEnergy();
        UpdateLightIntensity(); // ��s�O���G��
    }

    private void HandleInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            isHoldingSpace = true;
            holdTime += Time.deltaTime; // �p�����ť��䪺�ɶ�

            // �ˬd��q�O�_�����A�Y�����h�ھګ�������ɶ��ӼW�[�O�����G��
            if (energy > minEnergyThreshold)
            {
                targetLightIntensity = Mathf.Clamp(holdTime * 2f, 0f, 8f); // �ؼЫG�׽d��
            }
        }
        else
        {
            isHoldingSpace = false;

            // ��ť��䥼�Q����ɡA�N�ؼЫG�׳]�w��0
            targetLightIntensity = 0f;
            holdTime = 0f; // ���m��������ɶ�
        }
    }

    private void UpdateEnergy()
    {
        if (isHoldingSpace && energy > minEnergyThreshold)
        {
            energy -= energyConsumptionRate * Time.deltaTime;
            energy = Mathf.Max(energy, minEnergyThreshold);
            UpdateEnergyBar(); // ��s��q��
        }
    }

    private void UpdateLightIntensity()
    {
        // �M���Ҧ��O�����A��s���̪��G��
        foreach (var lightSource in lightSources)
        {
            if (lightSource != null)
            {
                // �ϥ� Lerp �����ܤƿO���G��
                float currentIntensity = lightSource.intensity;

                // ���q�Ȥj��0�ɡA�~��W�[�O���G��
                if (energy > minEnergyThreshold)
                {
                    lightSource.intensity = Mathf.Lerp(currentIntensity, targetLightIntensity, Time.deltaTime * lightIntensityDecayRate);
                }
                else
                {
                    // ��q�Ӻɫ�A�O���G�׳v����֦�0
                    lightSource.intensity = Mathf.Lerp(currentIntensity, 0f, Time.deltaTime * lightIntensityDecayRate);
                }
            }
        }
    }

    private void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = energy; // ��s�q�����
        }
    }

    private IEnumerator SpawnCubes()
    {
        while (true) // ���_�ͦ��ߤ���
        {
            // �H����ܥͦ���m
            Vector3 selectedCenter = centerPositions[Random.Range(0, centerPositions.Length)];

            // �]�w�ͦ���m
            float randomX = Random.Range(selectedCenter.x - rangeX, selectedCenter.x + rangeX);
            float randomY = Random.Range(selectedCenter.y - rangeY, selectedCenter.y + rangeY);
            float randomZ = Random.Range(selectedCenter.z - rangeZ, selectedCenter.z + rangeZ);

            // �b�H����m�ͦ��ߤ���
            GameObject cube = Instantiate(cubePrefab, new Vector3(randomX, randomY, randomZ), Quaternion.identity);

            // �H���ͦ��s�b�ɶ�
            float existenceTime = Random.Range(0.5f, 1f);
            float elapsedTime = 0f; // �O���g�L���ɶ�

            // �b�s�b�ɶ����ˬd���a�O�_���U Enter ��
            while (elapsedTime < existenceTime)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    RecoverEnergy(); // ���a���\�����q
                    Destroy(cube); // �R���ߤ���
                    break;
                }

                elapsedTime += Time.deltaTime; // �W�[�g�L���ɶ�
                yield return null; // ���ݤU�@�V
            }

            // �p�G���a�S�����U Enter ��h�R���ߤ���
            Destroy(cube);

            // �H���ͦ��U�@�ӥߤ��骺�ͦ����j
            float randomSpawnInterval = Random.Range(minCubeSpawnInterval, maxCubeSpawnInterval);
            yield return new WaitForSeconds(randomSpawnInterval); // ���ݤU�@���ͦ�
        }
    }

    private void RecoverEnergy()
    {
        energy += energyRecoveryAmount;
        energy = Mathf.Min(energy, 100f);
        UpdateEnergyBar(); // ��s��q��
    }
}
