using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float moveSpeed = 5f;     // �e�i��h���t��
    public float rotationSpeed = 100f; // ����t��
    public float jumpHeight = 2f;    // ���D����
    private bool isJumping = false;  // �O�_���b���D
    private float originalY;         // �O����l Y �b��m

    void Start()
    {
        originalY = transform.position.y; // �O����l������
    }

    void Update()
    {
        MoveCamera();
        RotateCamera();
        HandleJump();
    }

    void MoveCamera()
    {
        float moveZ = Input.GetAxis("Vertical"); // W / S ����e�i�P��h
        Vector3 move = transform.forward * moveZ * moveSpeed * Time.deltaTime;
        transform.position += move;
    }

    void RotateCamera()
    {
        float rotateY = Input.GetAxis("Horizontal"); // A / D ����k����
        transform.Rotate(Vector3.up * rotateY * rotationSpeed * Time.deltaTime);
    }

    void HandleJump()
    {
        if (Input.GetMouseButtonDown(1) && !isJumping) // ���k����D
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
