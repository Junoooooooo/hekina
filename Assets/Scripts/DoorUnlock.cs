using UnityEngine;
using System.Collections.Generic;

public class DoorUnlock : MonoBehaviour
{
    public GameObject[] doors; // �x�s�h�D�j���� GameObject �}�C
    public GameObject[] sephers; // �x�s�ݭn���ê� SEPHER GameObject �}�C
    private int currentDoorIndex = 0; // ��e�j������
    private NodeController nodeController; // �ޥ� NodeController
    private string[][] sequences = new string[][] // �Ҧ������ǦC
    {
        new string[] {  "right", "right", "left", "left", "left"  }, // �Ĥ@�D��
        new string[] {  "right", "right", "left", "left", "left"  }, // �ĤG�D��
        new string[] { "right", "right", "left", "left", "left" },  // �ĤT�D��
        new string[] { "right", "right", "left", "left", "left" },// �ĥ|�D��
        new string[] { "right", "right", "left", "left", "left" },// �Ĥ��D��
        new string[] { "right", "right", "left", "left", "left" },// �Ĥ��D��
        new string[] { "right", "right", "left", "left", "left" },// �ĤC�D��
        new string[] { "right", "right", "left", "left", "left" },// �ĤK�D��
        new string[] { "right", "right", "left", "left", "left" },// �ĤE�D��
        new string[] { "right", "right", "left", "left", "left" },// �ĤQ�D��
        new string[] { "right", "right", "left", "left", "left" },// �ĤQ�@�D��
    };

    private Queue<string> inputQueue = new Queue<string>(); // �Ω�s�x���a��J������ǦC

    void Start()
    {
        nodeController = FindObjectOfType<NodeController>();
        StartUnlocking();
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

        // �ˬd��e��J�O�_���T
        if (inputQueue.Count == sequences[currentDoorIndex].Length)
        {
            if (CheckSequence())
            {
                UnlockDoor(); // �p�G���T�A����
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

        // �q�� NodeController �~�򲾰�
        if (nodeController != null)
        {
            nodeController.UnlockCurrentDoor();
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
