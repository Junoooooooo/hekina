using UnityEngine;
using UnityEngine.UI;

public class PressureBarController : MonoBehaviour
{
    public Slider pressureBar;                 // �q�ȱ��� Slider
    public PressureSensor pressureSensor;      // ���O�P�����}���ޥ�
    private int targetMinValue;                // �ؼнd�򪺳̤p��
    private int targetMaxValue;                // �ؼнd�򪺳̤j��
    private bool isInRange = false;            // �P�_���a�O�_�F��ؼнd��
    private float timer = 0f;                  // �p�ɾ��A�Ω󵥫ݭ��s�ͦ��d��

    void Start()
    {
        // �H����ܽd��
        SetRandomPressureRange();

        // �]�m�q�ȱ����d�����
        pressureBar.minValue = 70;   // �T�w�q�ȱ����̤p��
        pressureBar.maxValue = 1023; // �T�w�q�ȱ����̤j��
        pressureBar.value = targetMinValue;  // �N�q�ȱ���l�]�m���ؼнd�򪺳̤p��
    }

    void Update()
    {
        // �q�P����Ū�����O�ƾ�
        float currentPressure = pressureSensor.GetPressureValue();

        // �P�_�O�_�F��ؼнd��
        if (currentPressure >= targetMinValue && currentPressure <= targetMaxValue)
        {
            if (!isInRange)
            {
                Debug.Log("�L���I���O�F��F�d�򤺡I");
                isInRange = true;
                // �b�o�̥i�H�K�[�L���޿�A��p���������B����S�ĵ�
            }
        }
        else
        {
            // �p�G���O�Ȥ��b�d�򤺡A�P�w������
            if (isInRange)
            {
                Debug.Log("���ѡI���O���F��d��ζW�L�d��A���s�}�l�I");
                isInRange = false;

                // ����1���s�ͦ��s�d��
                Invoke("SetRandomPressureRange", 1f);
            }
        }
    }

    // �H����ܤ@�����O�d��
    void SetRandomPressureRange()
    {
        int randomRange = Random.Range(1, 6); // �H���ͦ�1~5���������

        switch (randomRange)
        {
            case 1:
                targetMinValue = 30;
                targetMaxValue = 200;
                break;
            case 2:
                targetMinValue = 210;
                targetMaxValue = 500;
                break;
            case 3:
                targetMinValue = 510;
                targetMaxValue = 800;
                break;
            case 4:
                targetMinValue = 810;
                targetMaxValue = 1000;
                break;
            case 5:
                targetMinValue = 1010;
                targetMaxValue = 1023;
                break;
        }

        // ��s�q�ȱ����d�����
        pressureBar.value = targetMinValue;

        Debug.Log("�H����ܪ��d��O: " + targetMinValue + " ~ " + targetMaxValue);
    }
}
