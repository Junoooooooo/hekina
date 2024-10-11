using UnityEngine;
using System.IO.Ports;

public class PressureSensor : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM8", 9600); // 設置串口號和波特率 (根據你的Arduino設定調整)
    public float pressureValue;

    void Start()
    {
        sp.Open(); // 打開串口
        sp.ReadTimeout = 1000; // 設置讀取超時
    }

    void Update()
    {
        if (sp.IsOpen)
        {
            try
            {
                string value = sp.ReadLine(); // 讀取串口數據
                pressureValue = float.Parse(value); // 將數據轉換為浮點數
            }
            catch (System.Exception)
            {
                // 忽略串口讀取異常
            }
        }
    }

    public float GetPressureValue()
    {
        return pressureValue; // 返回壓力值
    }
}
