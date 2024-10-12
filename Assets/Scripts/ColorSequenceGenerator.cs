using UnityEngine;
using System.Collections;

public class ColorSequenceGenerator : MonoBehaviour
{
    private Color[] colors = { Color.red, Color.yellow, Color.blue }; // �i���C��
    private int sequenceLength; // �C��ǦC������
    private Color[] colorSequence; // �x�s�ͦ����C��ǦC
    public GameObject colorObjectPrefab; // �C�⪫�󪺹w�s��
    private GameObject currentColorObject; // ��e�C�⪫��
    private int currentInputIndex = 0; // ��e��J����
    private bool isInputActive = false; // �O�_�i�H�i���J
    public Light pointLight; // �Ω���ܬO�_�������O��

    void Start()
    {
        GenerateColorSequence();
        StartCoroutine(DisplayColorSequence());
    }

    void GenerateColorSequence()
    {
        sequenceLength = Random.Range(3, 6); // �H���ͦ� 3 �� 5 ���C��ǦC����
        colorSequence = new Color[sequenceLength]; // ��l���C��ǦC

        // �H���ͦ��C��ǦC
        for (int i = 0; i < sequenceLength; i++)
        {
            colorSequence[i] = colors[Random.Range(0, colors.Length)]; // �H������C��
        }
    }

    IEnumerator DisplayColorSequence()
    {
        for (int i = 0; i < colorSequence.Length; i++)
        {
            // �Ы��C�⪫��
            if (currentColorObject != null)
            {
                Destroy(currentColorObject); // �p�G�w���C�⪫��A�R��
            }

            currentColorObject = Instantiate(colorObjectPrefab); // �Ыطs���C�⪫��
            currentColorObject.GetComponent<Renderer>().material.color = colorSequence[i]; // �]�m�C��

            yield return new WaitForSeconds(1.0f); // ����C�� 1 ��

            Destroy(currentColorObject); // �R���C�⪫��
            yield return new WaitForSeconds(0.5f); // ���� 0.5 ��A��ܤU�@���C��
        }

        // �Ҧ��C��ͦ������A�{�b�i�H���� Makey Makey ��J
        isInputActive = true;
        currentInputIndex = 0; // ���m��J����
    }

    void Update()
    {
        if (isInputActive) // ���J���ʮɡA��ť Makey Makey �����J
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) // �W���������
            {
                CheckInput(Color.red);
            }
            if (Input.GetKeyDown(KeyCode.Space)) // �ť�������Ŧ�
            {
                CheckInput(Color.blue);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)) // �U���������
            {
                CheckInput(Color.yellow);
            }
        }
    }

    void CheckInput(Color inputColor)
    {
        // �ˬd��J���C��O�_���T
        if (inputColor == colorSequence[currentInputIndex])
        {
            Debug.Log("Correct color: " + inputColor);
            currentInputIndex++;

            if (currentInputIndex >= colorSequence.Length)
            {
                Debug.Log("Congratulations! You've completed the sequence.");
                // �b�o�̥i�H�[�J�����᪺�欰�A�Ҧp���m�C������ܰT��
                pointLight.enabled = true; // �I�G�O��
                isInputActive = false; // �T�ο�J
            }
        }
        else
        {
            Debug.Log("Incorrect color! Try again.");
            currentInputIndex = 0; // �p�G���~�A���m��J����
        }
    }
}
