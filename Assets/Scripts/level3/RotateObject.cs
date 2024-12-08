using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // 旋轉速度（單位：度/秒）
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    // Update 是每幀調用一次的函數
    void Update()
    {
        // 原地旋轉，確保只影響物件的旋轉而不影響位置
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
