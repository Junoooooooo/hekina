using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float rotationSpeed = 100f;

    private Rigidbody rb;
    private Quaternion targetRotation;

    void Start()
    {
        rb = GetComponent<Rigidbody>(); // 取得 Rigidbody
        targetRotation = transform.rotation;
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = Vector3.zero;

        // W S 控制前後移動（用 Rigidbody 計算碰撞）
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.W))
        {
            moveDirection -= transform.forward;
        }

        // 移動方式：Rigidbody.MovePosition
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

        // A D 控制旋轉
        float rotationAmount = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            rotationAmount -= rotationSpeed * Time.fixedDeltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            rotationAmount += rotationSpeed * Time.fixedDeltaTime;
        }

        if (rotationAmount != 0f)
        {
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAmount, 0);
            rb.MoveRotation(targetRotation);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("goal"))
        {
            Debug.Log("Goal reached! Loading Level 3...");
            LoadLevel3();
        }
    }

    void LoadLevel3()
    {
        SceneManager.LoadScene("level3");
    }
}
