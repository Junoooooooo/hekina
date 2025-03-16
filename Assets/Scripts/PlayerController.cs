using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float energy = 100f;                // ���a��q
    public float energyConsumptionRate = 1f;   // ��q���ӳt�v
    public float energyRecoveryAmount = 10f;   // ��q��_�q
    public float minEnergyThreshold = 0f;      // �̤p��q�H��
    public Light[] lightSources;                // �O�����}�C
    public GameObject cubePrefab;               // �ߤ���w�s��
    public float existenceTime = 5f; // �ߤ���s�b�ɶ�

  public float minCubeSpawnInterval = 1f;    // �ͦ����j���̤p��
  public float maxCubeSpawnInterval = 3f;    // �ͦ����j���̤j��
    public Slider energyBar;                    // ��q�q���� UI ����
  

    private bool isHoldingSpace = false;        // �O�_����ť���
    private float holdTime = 0f;                // ����ť��䪺�ɶ�
    private float targetLightIntensity = 0f;    // �ؼпO���G��
    private float lightIntensityDecayRate = 5f; // �O���I��t�v




    public float timeRemaining = 300f;    // 3���� = 180��
    public TMP_Text timerText;                  // �s�� UI �� Text ����

    private void Start()
    {
        StartCoroutine(WaitForGameStart());

        UpdateTimer();  // ��l����ܮɶ� 
        UpdateEnergyBar(); // ��l�Ư�q��

        // �]�w�O������l�G�׬�0
        foreach (var lightSource in lightSources)
        {
            if (lightSource != null)
            {
                lightSource.intensity = 0f; // �N�O���G�׳]��0
            }
        }
    }
    IEnumerator WaitForGameStart()
    {
        Debug.Log("Waiting for game to start...");
        // ���ݹC���}�l�]�קK Time.timeScale = 0 �v�T�ͦ��^
        yield return new WaitUntil(() => Time.timeScale == 1);
        Debug.Log("Game Started, Spawning Cubes...");
        StartCoroutine(SpawnCubes());
    }


    private void Update()
    {
        Debug.Log("Time.timeScale: " + Time.timeScale); // �T�{�ɶ��Y�񪬺A
        UpdateTimer();
        HandleInput();
        UpdateEnergy();
        UpdateLightIntensity(); // ��s�O���G��
        if (Time.timeScale > 0 && Input.GetMouseButtonDown(0))
        {
            RecoverEnergy();
        }
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

    public void UpdateEnergy()
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
    private void UpdateTimer()
    {
        // ��ֳѾl�ɶ�
        timeRemaining -= Time.deltaTime;

        // �ˬd TimerText �O�_�� null
        if (timerText != null)
        {
            // �ˬd�ɶ��O�_�j�� 0
            if (timeRemaining > 0)
            {
                // �N�Ѿl�ɶ��ഫ�������M��
                int minutes = Mathf.FloorToInt(timeRemaining / 60F);
                int seconds = Mathf.FloorToInt(timeRemaining % 60F);

                // �]�w�榡�� "����:��" ���Φ�
                timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            }
     
        
        else
            {
                // �p�G�ɶ��w�g�����A��� "Game Over"
                timerText.text = "00:00";
            }
        }
    }


    public void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = energy; // ��s�q�����
        }
    }

    public IEnumerator SpawnCubes()
    {
        Camera mainCamera = Camera.main; // ����D��v��

        while (true) // �L���`���ͦ��ߤ���
        {
            // 1. �H���ͦ��ߤ����m
            float randomX = Random.Range(0.2f, 0.8f);
            float randomY = Random.Range(0.2f, 0.8f);
            Vector3 randomViewportPos = new Vector3(randomX, randomY, mainCamera.nearClipPlane + 45f);
            Vector3 spawnPosition = mainCamera.ViewportToWorldPoint(randomViewportPos);

            // 2. �ͦ��ߤ���
            GameObject cube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            Debug.Log($"[�ͦ�] �ߤ���ͦ��� {spawnPosition}");

            cube.isStatic = false; // �T�O�ߤ��餣�O�R�A����

            // 3. �]�w�ߤ��骺�s�b�ɶ�
            float existenceTime = Random.Range(1f, 3f);
            float elapsedTime = 0f;
            bool isCollected = false;

            while (elapsedTime < existenceTime)
            {
                Debug.Log($"[�p��] elapsedTime: {elapsedTime} / {existenceTime}");

                // �ˬd�ƹ��I��
                if (Input.GetMouseButtonDown(0) && cube != null)
                {
                    Debug.Log("[�I��] �ߤ���Q�I���A����P��");
                    RecoverEnergy();

                    Destroy(cube); // �����P���ߤ���
                    isCollected = true;
                    yield return null;
                    break;
                }

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // 4. �ɶ���F�٨S�I���A�h�۰ʾP��
            if (!isCollected && cube != null)
            {
                Debug.Log("[�W��] �ߤ���ɶ���A����P��");
                Destroy(cube);
            }

            // 5. ���ݤU�@�ӥߤ���ͦ�
            float randomSpawnInterval = Random.Range(minCubeSpawnInterval, maxCubeSpawnInterval);
            Debug.Log($"[����] ���� {randomSpawnInterval} ���ͦ��U�@�ӥߤ���");
            yield return new WaitForSeconds(randomSpawnInterval);
        }
    }

    public void RecoverEnergy()
    {
        energy += energyRecoveryAmount;
        energy = Mathf.Min(energy, 100f);
        UpdateEnergyBar(); // ��s��q��
        Debug.Log("��q�^�_�I");
    }

}