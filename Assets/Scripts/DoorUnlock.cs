using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class DoorUnlock : MonoBehaviour
{
    public GameObject[] doors; // �x�s�h�D�j���� GameObject �}�C
    public GameObject[] sephers; // �x�s�ݭn���ê� SEPHER GameObject �}�C
    private int currentDoorIndex = 0; // ��e�j������
    private NodeController nodeController; // �ޥ� NodeController
    private string[][] sequences = new string[][] // �Ҧ������ǦC
    {
        new string[] { "right", "right", "left", "left", "left" }, // �Ĥ@�D��
        new string[] { "right", "left", "right", "left", "right" }, // �ĤG�D��
        new string[] { "left", "right", "right", "left", "right" }, // �ĤT�D��
        new string[] { "left", "right", "left", "right", "left" }, // �ĥ|�D��
        new string[] { "right", "left", "left", "right", "right" }, // �Ĥ��D��
        new string[] { "left", "right", "left", "right", "right" }, // �Ĥ��D��
        new string[] { "right", "right", "left", "left", "left" }, // �ĤC�D��
        new string[] { "left", "right", "right", "left", "left" }, // �ĤK�D��
        new string[] { "right", "right", "right", "left", "right"}, // �ĤE�D��
        new string[] { "left", "right", "left", "right", "left" }, // �ĤQ�D��
        // ��l���ǦC...
    };

    private Queue<string> inputQueue = new Queue<string>(); // �Ω�s�x���a��J������ǦC
    private bool canAcceptInput = true; // �Ω󱱨�O�_������J

    void Start()
    {
      //  nodeController = FindObjectOfType<NodeController>();
        StartUnlocking();
    }

    void Update()
    {
        CheckInput();
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
        HandleInput(input);
        yield return new WaitForSeconds(0.2f); // ���� 0.1 ��
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

        // �q�� NodeController �~�򲾰�
        /*if (nodeController != null)
        {
            nodeController.UnlockCurrentDoor();
        }*/
    }

    public void StartUnlocking()
    {
        if (currentDoorIndex < doors.Length)
        {
            // ���m�B�J
        }
    }
}
