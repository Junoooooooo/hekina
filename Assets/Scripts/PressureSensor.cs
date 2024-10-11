using UnityEngine;
using System.IO.Ports;

public class PressureSensor : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM8", 9600); // �]�m��f���M�i�S�v (�ھڧA��Arduino�]�w�վ�)
    public float pressureValue;

    void Start()
    {
        sp.Open(); // ���}��f
        sp.ReadTimeout = 1000; // �]�mŪ���W��
    }

    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                string value = sp.ReadLine(); // Ū����f�ƾ�
                pressureValue = float.Parse(value); // �N�ƾ��ഫ���B�I��
            }
            catch (System.Exception)
            {
                // ������fŪ�����`
            }
        }
    }

    public float GetPressureValue()
    {
        return pressureValue; // ��^���O��
    }
}
