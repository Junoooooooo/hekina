using UnityEngine;
using System.Collections.Generic;

public class PlayerOneController : MonoBehaviour
{
    private int currentIndex = 0; // ��e�K�X����
    private int currentCircle = 0; // ��e���
    private string[] sequence = new string[] { "right", "left", "right", "left" }; // �C�@�骺��J�ǦC

    private Queue<string> inputQueue = new Queue<string>(); // �Ω�s�x���a��J������ǦC

    // �C��������O���M����
    public Light[] sequenceLights1;
    public Light[] sequenceLights2;
    public Light[] sequenceLights3;

    // �C������� GameObject�A�o�� GameObject ������|���� Emissive �ĪG
    public GameObject[] sequenceObjects1;
    public GameObject[] sequenceObjects2;
    public GameObject[] sequenceObjects3;

    // �Ω��x�s��l������M��l�� Emissive �C��
    private Material[][] originalMaterials;
    private Color[][] originalEmissionColors;

    void Start()
    {
        // ��l�ƭ�l����M Emissive �C��
        originalMaterials = new Material[3][];
        originalMaterials[0] = GetMaterialsFromObjects(sequenceObjects1);
        originalMaterials[1] = GetMaterialsFromObjects(sequenceObjects2);
        originalMaterials[2] = GetMaterialsFromObjects(sequenceObjects3);

        originalEmissionColors = new Color[3][];
        originalEmissionColors[0] = GetEmissionColorsFromMaterials(originalMaterials[0]);
        originalEmissionColors[1] = GetEmissionColorsFromMaterials(originalMaterials[1]);
        originalEmissionColors[2] = GetEmissionColorsFromMaterials(originalMaterials[2]);

        // �����Ҧ����誺 Emissive �ĪG
        DisableAllEmissiveEffect();

        // �@�}�l�N�Ҧ��O������
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleInput("left");
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleInput("right");
        }
    }

    private void HandleInput(string input)
    {
        inputQueue.Enqueue(input); // �N���a��J�K�[�춤�C��

        // �C�����@��]���k��@���^�N�ˬd�O�_���T
        if (inputQueue.Count == sequence.Length)
        {
            if (CheckSequence())
            {
                TriggerLightsAndMaterials(); // �p�G���T�AĲ�o�O���M�����ܧ�
            }
            else
            {
                inputQueue.Clear(); // �p�G���~�A�M�Ŷ��C
            }
        }
    }

    private bool CheckSequence()
    {
        // �ˬd��J�����ǬO�_���T
        string[] correctSequence = sequence;
        for (int i = 0; i < correctSequence.Length; i++)
        {
            if (inputQueue.Dequeue() != correctSequence[i])
            {
                return false; // �p�G����@�B�����T�A��^ false
            }
        }
        return true; // �������T
    }

    private void TriggerLightsAndMaterials()
    {
        Debug.Log("Correct sequence entered!");

        // �ھڰ�ƶ}�ҿO���M���ܪ��� Emissive �ĪG
        switch (currentCircle)
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

        // �i�J�U�@��
        currentCircle++;
        inputQueue.Clear(); // �M�Ŷ��C�H�K�U�@��ϥ�

        if (currentCircle >= 3) // ���]�̦h��3��
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
        // �����Ҧ����� Emissive �ĪG
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
                    mat.DisableKeyword("_EMISSION"); // ���� Emissive �ĪG
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
                    renderer.materials[j].EnableKeyword("_EMISSION"); // �ҥ� Emissive �ĪG
                    renderer.materials[j].SetColor("_EmissionColor", originalColors[j]); // ��_��l�� Emissive �C��
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
            emissionColors.Add(emissionColor); // �x�s��l Emissive �C��
        }
        return emissionColors.ToArray();
    }
}
