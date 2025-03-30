using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // 前進後退的速度
    public float rotationSpeed = 100f; // 旋轉速度
    public float jumpHeight = 2f;    // 跳躍高度
    private bool isJumping = false;  // 是否正在跳躍
    private float originalY;         // 記錄初始 Y 軸位置

    void Start()
    {
        originalY = transform.position.y; // 記錄原始的高度
    }

    void Update()
    {
        MoveCamera();
        RotateCamera();
        HandleJump();
    }

    void MoveCamera()
    {
        float moveZ = Input.GetAxis("Vertical"); // W / S 控制前進與後退
        Vector3 move = transform.forward * moveZ * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void RotateCamera()
    {
        float rotateY = Input.GetAxis("Horizontal"); // A / D 控制左右旋轉
        transform.Rotate(Vector3.up * rotateY * rotationSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetMouseButtonDown(1) && !isJumping) // 按右鍵跳躍
        {
            isJumping = true;
            StartCoroutine(Jump());
        }
    }

    System.Collections.IEnumerator Jump()
    {
        float targetY = originalY + jumpHeight;
        while (transform.position.y < targetY)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        while (transform.position.y > originalY)
        {
            transform.position -= Vector3.up * moveSpeed * Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, originalY, transform.position.z);
        isJumping = false;
    }
}
