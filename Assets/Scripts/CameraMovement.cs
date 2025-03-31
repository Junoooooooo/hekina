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
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W)) moveZ = 1f; // 只允許 W 鍵前進
        if (Input.GetKey(KeyCode.S)) moveZ = -1f; // 只允許 S 鍵後退

        Vector3 move = transform.forward * moveZ * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void RotateCamera()
    {
        float rotateY = 0f;

        if (Input.GetKey(KeyCode.A)) rotateY = -1f; // 只允許 A 鍵左轉
        if (Input.GetKey(KeyCode.D)) rotateY = 1f; // 只允許 D 鍵右轉

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
