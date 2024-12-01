using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // 移動速度
    public float rotationSpeed = 2f; // 旋轉的平滑速度
    public float rotationStep = 10f; // 每次旋轉的角度
    private Quaternion targetRotation; // 目標旋轉

    void Start()
    {
        // 初始化目標旋轉為當前攝影機的旋轉
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // 只用 W 和 S 控制前後移動
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 forwardMovement = transform.forward;
            transform.position += forwardMovement * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 backwardMovement = -transform.forward;
            transform.position += backwardMovement * moveSpeed * Time.deltaTime;
        }

        // A 和 D 控制旋轉
        if (Input.GetKey(KeyCode.A))
        {
            // 每次旋轉基於當前目標旋轉，向左轉
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - 15f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // 每次旋轉基於當前目標旋轉，向右轉
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 15f, 0);
        }

        // 平滑插值到目標旋轉
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
