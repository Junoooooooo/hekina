using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class DoorUnlock : MonoBehaviour
{
    public float energy = 100f;                // ���a��q
    public float energyConsumptionRate = 1f;   // ��q���ӳt�v
    public float energyRecoveryAmount = 10f;   // ��q��_�q
    public Light[] lightSources;                // �O�����}�C
    public float minEnergyThreshold = 0f;      // �̤p��q�H��
    public Slider energyBar;                    // ��q�q���� UI ����
    public GameObject[] doors; // �x�s�h�D�j���� GameObject �}�C
    public GameObject[] sephers; // �x�s�ݭn���ê� SEPHER GameObject �}�C
    public AudioClip leftKeySound;  // ���䭵��
    public AudioClip rightKeySound; // �k�䭵��
    public Image nextImage;
    public float timeRemaining = 300f;    // 3���� = 180��
    public TMP_Text timerText;                  // �s�� UI �� Text ����
    private AudioSource audioSource; // ����
    private int currentDoorIndex = 0; // ��e�j������
    private string[][] sequences = new string[][] // �Ҧ������ǦC
    {
        new string[] { "right", "right", "left" }, // �Ĥ@�D��
        new string[] { "right", "left", "right" }, // �ĤG�D��
        new string[] { "left", "right", "left"}, // �ĤT�D��
        new string[] { "right", "right", "left"}, // �ĥ|�D��
        new string[] { "right", "left", "left"}, // �Ĥ��D��
        new string[] { "left", "right", "left"}, // �Ĥ��D��
        new string[] { "right", "right", "left" }, // �ĤC�D��
        new string[] { "left", "right", "right"}, // �ĤK�D��
        new string[] { "right", "right", "right"} ,// �ĤE�D��
        new string[] { "left", "right", "left"}, // �ĤQ�D��
        // ��l���ǦC...
    };

    private Queue<string> inputQueue = new Queue<string>(); // �Ω�s�x���a��J������ǦC
    private bool canAcceptInput = true; // �Ω󱱨�O�_������J

    private bool isHoldingSpace = false;        // �O�_����ť���
    private float holdTime = 0f;                // ����ť��䪺�ɶ�
    private float targetLightIntensity = 0f;    // �ؼпO���G��
    private float lightIntensityDecayRate = 5f; // �O���I��t�v

    void Start()
    {
        StartCoroutine(WaitForGameStart());

        UpdateTimer();  // ��l����ܮɶ� 
        UpdateEnergyBar(); // ��l�Ư�q��
        audioSource = GetComponent<AudioSource>(); // ��l�� AudioSource
        StartUnlocking(); // �}�l����y�{
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
    }

    void Update()
    {
        CheckInput(); // �O���쥻����J�ˬd
        Debug.Log("Time.timeScale: " + Time.timeScale); // �T�{�ɶ��Y�񪬺A
        UpdateTimer();
        HandleInput();
        //UpdateEnergy();
        UpdateLightIntensity(); // ��s�O���G��
        UpdateEnergy();
    }

    private void HandleInput()
    {

        if (Input.GetKey(KeyCode.Space) ||
    Input.GetKey(KeyCode.DownArrow) ||
    Input.GetMouseButton(0)) // �ƹ�����)
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
    private void CheckInput()
    {
        if (canAcceptInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                StartCoroutine(HandleInputWithDelay("left"));
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                StartCoroutine(HandleInputWithDelay("right"));
            }
        }
    }

    private IEnumerator HandleInputWithDelay(string input)
    {
        canAcceptInput = false; // �ȮɸT�ο�J

        // �������������
        if (input == "left")
        {
            PlaySound(leftKeySound); // �����䭵��
        }
        else if (input == "right")
        {
            PlaySound(rightKeySound); // ����k�䭵��
        }

        HandleInput(input); // �B�z��J�޿�
        yield return new WaitForSeconds(0.2f); // ���� 0.4 ��
        canAcceptInput = true; // ��_��J
    }

    private void HandleInput(string input)
    {
        // �����e���T�ǦC
        string[] correctSequence = sequences[currentDoorIndex];

        // �K�[���a��J���C��
        inputQueue.Enqueue(input);

        // ���J��C�F��ǦC���׮ɶi���ˬd
        if (inputQueue.Count == correctSequence.Length)
        {
            if (CheckSequence())
            {
                UnlockDoor(); // �p�G���T�A�����

                RecoverEnergy();

            }
            else
            {
                Debug.Log("Incorrect sequence! Please try again.");
                inputQueue.Clear(); // �M�Ŧ�C�A���\���s��J
            }
        }
    }

    private bool CheckSequence()
    {
        // �ˬd��J�����ǬO�_���T
        string[] correctSequence = sequences[currentDoorIndex];
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (inputQueue.Dequeue() != correctSequence[i])
            {
                return false; // �p�G����@�B�����T�A��^ false
            }
        }
        return true; // �������T
    }

    private void UnlockDoor()
    {
        Debug.Log("Door " + (currentDoorIndex + 1) + " Unlocked!");
        doors[currentDoorIndex].SetActive(false); // ���äj��

        // ���ì����� SEPHER
        if (currentDoorIndex < sephers.Length)
        {
            sephers[currentDoorIndex].SetActive(false); // ���ù����� SEPHER
        }

        currentDoorIndex++; // ���ʨ�U�@�D��
        inputQueue.Clear(); // �M�Ŷ��C�H�K�U�@�����ϥ�
        if (currentDoorIndex < doors.Length)
        {
            StartUnlocking(); // �}�l�U�@�D��������y�{

        }
        else
        {
            Debug.Log("All doors unlocked!");
        }
    }

    public void UpdateEnergyBar()
    {
        if (energyBar != null)
        {
            energyBar.value = energy; // ��s�q�����
        }
    }

    public void RecoverEnergy()
    {
        energy += energyRecoveryAmount;
        energy = Mathf.Min(energy, 100f);
        UpdateEnergyBar(); // ��s��q��
        Debug.Log("��q�^�_�I");
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip, 1.0f); // �̤j���q���񭵮�
        }
    }

    public void StartUnlocking()
    {
        if (currentDoorIndex < doors.Length)
        {
            // ���m�B�J
        }
    }
}