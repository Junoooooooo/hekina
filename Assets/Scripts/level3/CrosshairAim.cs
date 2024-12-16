using UnityEngine;
using UnityEngine.SceneManagement; // 用於重新載入場景或退出遊戲
using UnityEngine.UI;

public class CrosshairAim : MonoBehaviour
{
    public Camera playerCamera;
    public Image crosshairUI;
    public Color targetAvailableColor = Color.green;
    public Color targetUnavailableColor = Color.white;
    public float maxAimDistance = 30f;
    public LayerMask grappleableLayer;
    public LayerMask goalLayer; // 新增的目標層
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private float yaw = 0f;
    private float pitch = 0f;

    public FloatingTargetsManager targetsManager; // 引用 FloatingTargetsManager
    private GameObject currentTarget = null;

    public float jumpHeight = 5f;
    public float jumpDuration = 1f;
    private bool isJumping = false;
    private float jumpStartTime = 0f;
    private Vector3 jumpTargetPosition;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Quaternion targetRotation;

    void Start()
    {
        if (crosshairUI != null)
        {
            crosshairUI.color = targetUnavailableColor;
        }

        originalPosition = playerCamera.transform.position;
        originalRotation = playerCamera.transform.rotation;
    }

    void Update()
    {
        CheckAim();
        MoveCamera();
        RotateCamera();
        UpdateCrosshairPosition();

        if (Input.GetKeyDown(KeyCode.F) && currentTarget != null)
        {
            Debug.Log("successful");
            if (targetsManager != null)
            {
                targetsManager.StopTargetMovement(currentTarget);
            }

            StartJumpToTarget();
        }

        if (isJumping)
        {
            PerformJump();
        }
    }

    void MoveCamera()
    {
        float horizontal = 0f;
        float vertical = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            vertical = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            vertical = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontal = 1f;
        }

        Vector3 moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float moveAmount = moveSpeed * Time.deltaTime;
            yaw += moveDirection.x * rotationSpeed * Time.deltaTime;
            pitch -= moveDirection.z * rotationSpeed * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, -90f, 90f);
        }
    }

    void RotateCamera()
    {
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void CheckAim()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxAimDistance, grappleableLayer))
        {
            currentTarget = hit.collider.gameObject;
            if (crosshairUI != null)
            {
                crosshairUI.color = targetAvailableColor;
            }
        }
        else
        {
            currentTarget = null;
            if (crosshairUI != null)
            {
                crosshairUI.color = targetUnavailableColor;
            }
        }
    }

    void UpdateCrosshairPosition()
    {
        if (crosshairUI != null)
        {
            crosshairUI.rectTransform.position = new Vector2(Screen.width / 2, Screen.height / 2);
        }
    }

    void StartJumpToTarget()
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpStartTime = Time.time;
            targetRotation = playerCamera.transform.rotation;

            if (currentTarget != null)
            {
                jumpTargetPosition = currentTarget.transform.position + Vector3.up * 5f;
            }
        }
    }

    void PerformJump()
    {
        float t = (Time.time - jumpStartTime) / jumpDuration;

        if (t < 1f)
        {
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight;
            Vector3 targetPosition = Vector3.Lerp(originalPosition, jumpTargetPosition, t);
            targetPosition.y += height;
            playerCamera.transform.position = targetPosition;
            playerCamera.transform.rotation = targetRotation;
        }
        else
        {
            playerCamera.transform.position = jumpTargetPosition;
            playerCamera.transform.rotation = targetRotation;
            originalPosition = jumpTargetPosition;
            isJumping = false;
        }
    }

    // 檢測碰撞
    private void OnTriggerEnter(Collider other)
    {
        if (goalLayer == (goalLayer | (1 << other.gameObject.layer))) // 檢查目標是否屬於目標層
        {
            Debug.Log("Goal reached!");
            EndGame(); // 遊戲結束處理
        }
    }

    void EndGame()
    {
        // 可執行的遊戲結束邏輯
        Debug.Log("Game Over!");

        // 例如重新載入場景
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        // 或退出遊戲
        // Application.Quit();
        SceneManager.LoadScene("Level1");
    }
}
