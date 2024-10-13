using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorSequenceGenerator : MonoBehaviour
{
    public GameObject player; // ���l���Ѧ�
    public float triggerDistance = 15f; // �Z��CUBE�h�֮ɶ}�l�ͦ��C��
    public GameObject colorObjectPrefab; // �C�⪫�󪺹w�s��
    public GameObject[] cubes; // �s��������Ҧ�CUBE
    public Light[] cubeLights; // �C��CUBE�������O��

    private Dictionary<GameObject, Color[]> cubeColorSequences = new Dictionary<GameObject, Color[]>(); // �C��CUBE���C��ǦC
    private Dictionary<GameObject, int> currentInputIndex = new Dictionary<GameObject, int>(); // �C��CUBE��e����J����
    private Dictionary<GameObject, bool> isInputActive = new Dictionary<GameObject, bool>(); // �C��CUBE�O�_�i������J
    private Dictionary<GameObject, bool> hasTriggered = new Dictionary<GameObject, bool>(); // �C��CUBE�O�_�wĲ�o�C��ͦ�

    private Color[] colors = { Color.red, Color.yellow, Color.blue }; // �i���C��

    void Start()
    {
        cubes = GameObject.FindGameObjectsWithTag("CUBE"); // �����������Ҧ�CUBE

        for (int i = 0; i < cubes.Length; i++)
        {
            GenerateColorSequenceForCube(cubes[i]); // ���C��CUBE�ͦ��C��ǦC
            currentInputIndex[cubes[i]] = 0; // ��l�ƿ�J����
            isInputActive[cubes[i]] = false; // ��l�ƿ�J���A
            hasTriggered[cubes[i]] = false; // ��l��Ĳ�o���A
        }
    }


    void Update()
    {
        foreach (GameObject cube in cubes)
        {
            float distance = Vector3.Distance(cube.transform.position, player.transform.position); // �p�⨮�l�PCUBE���Z��

            if (distance <= triggerDistance && !hasTriggered[cube])
            {
                hasTriggered[cube] = true; // �T�O�uĲ�o�@��
                StartCoroutine(DisplayColorSequence(cube)); // ��ܸ�CUBE���C��ǦC
            }

            if (isInputActive[cube]) // ���CUBE�i�H�i���J�ɡA��ť�����J
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
        int sequenceLength = Random.Range(3, 6); // �H���ͦ� 3 �� 5 ���C��ǦC����
        Color[] colorSequence = new Color[sequenceLength]; // ��l���C��ǦC

        for (int i = 0; i < sequenceLength; i++)
        {
            colorSequence[i] = colors[Random.Range(0, colors.Length)]; // �H������C��
        }

        cubeColorSequences[cube] = colorSequence; // ����CUBE�x�s�C��ǦC
    }

    IEnumerator DisplayColorSequence(GameObject cube)
    {
        for (int i = 0; i < cubeColorSequences[cube].Length; i++)
        {
            Color currentColor = cubeColorSequences[cube][i];

            // �]�m�o���C��
            Renderer cubeRenderer = cube.GetComponent<Renderer>();
            cubeRenderer.material.SetColor("_EmissionColor", currentColor); // �]�m�o���C��
            DynamicGI.SetEmissive(cubeRenderer, currentColor); // ��s�������Өt��

            // �����C�⺥��
            yield return StartCoroutine(ColorFade(cube, currentColor, 0.3f));

            yield return new WaitForSeconds(0.1f); // ����C��h�֬�

            // ���m�C�⬰�զ�Ψ�L�w�]�C��
            yield return StartCoroutine(ColorFade(cube, Color.white, 0.3f)); // ���ܦ^�զ�

            // ���m�o���C��
            cubeRenderer.material.SetColor("_EmissionColor", Color.black); // ���m�o���C�⬰�¦�
            DynamicGI.SetEmissive(cubeRenderer, Color.black); // ��s�������Өt��

            yield return new WaitForSeconds(0.1f); // ���ݦh�֬�A��ܤU�@���C��
        }

        // ��ܧ�����A�}�l������J
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
            yield return null; // ���ݤU�@�V
        }

        cubeRenderer.material.color = targetColor; // �T�O�̲��C��]�w���ؼ��C��
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

                // ����������O���ó]�m���ե��G��50
                Light cubeLight = cubeLights[System.Array.IndexOf(cubes, cube)];
                cubeLight.color = Color.white; // �]�m�O���C�⬰�զ�
                cubeLight.intensity = 30f; // �]�m�O���G�׬�50
                cubeLight.enabled = true; // �I�G��CUBE���O��

                isInputActive[cube] = false; // �����J
            }
        }
        else
        {
            Debug.Log("Incorrect color for Cube: " + cube.name + ". Try again.");
            currentInputIndex[cube] = 0; // ���m��J����
        }
    }


}
