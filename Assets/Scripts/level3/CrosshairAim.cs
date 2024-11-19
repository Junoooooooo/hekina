using UnityEngine;
using UnityEngine.UI;

public class CrosshairAim : MonoBehaviour
{
    public Camera playerCamera;
    public Image crosshairUI;
    public Color targetAvailableColor = Color.green;
    public Color targetUnavailableColor = Color.white;
    public float maxAimDistance = 30f;
    public LayerMask grappleableLayer;
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private float yaw = 0f;
    private float pitch = 0f;

    // 新增的变量
    public FloatingTargetsManager targetsManager;  // 引用 FloatingTargetsManager

    private GameObject currentTarget = null;  // 记录当前准星对准的目标

    // 跳跃相关变量
    public float jumpHeight = 5f;   // 跳跃的最大高度
    public float jumpDuration = 1f; // 跳跃的持续时间
    private bool isJumping = false; // 判断是否正在跳跃
    private float jumpStartTime = 0f; // 跳跃开始的时间
    private Vector3 jumpTargetPosition; // 跳跃的目标位置
    private Vector3 originalPosition; // 记录相机的初始位置
    private Quaternion originalRotation; // 记录相机的初始旋转
    private Quaternion targetRotation; // 記錄跳躍時相機應保持的旋轉

    void Start()
    {
        // 初始化准星颜色
        if (crosshairUI != null)
        {
            crosshairUI.color = targetUnavailableColor;
        }

        // 记录初始相机位置
        originalPosition = playerCamera.transform.position;
        originalRotation = playerCamera.transform.rotation;
    }

    void Update()
    {
        // 呼叫检测准星是否对准目标的功能
        CheckAim();

        // 移动和旋转相机
        MoveCamera();
        RotateCamera();

        // 更新准星位置
        UpdateCrosshairPosition();

        // 按下 "F" 键，并且准星对准有效目标时，触发跳跃效果
        if (Input.GetKeyDown(KeyCode.F) && currentTarget != null)
        {
            Debug.Log("successful");  // 在控制台显示 "successful"
            if (targetsManager != null)
            {
                targetsManager.StopTargetMovement(currentTarget);  // 停止对应目标的浮动
            }

            // 触发跳跃到目标位置
            StartJumpToTarget();
        }

        // 如果正在跳跃，执行跳跃逻辑
        if (isJumping)
        {
            PerformJump();
        }
    }

    void MoveCamera()
    {
        // 移动相机逻辑
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

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
        // 旋转相机逻辑
        playerCamera.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
    }

    void CheckAim()
    {
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxAimDistance, grappleableLayer))
        {
            currentTarget = hit.collider.gameObject;  // 记录当前对准的目标
            if (crosshairUI != null)
            {
                crosshairUI.color = targetAvailableColor;  // 改为绿色
            }
        }
        else
        {
            currentTarget = null;  // 没有对准目标时，清除
            if (crosshairUI != null)
            {
                crosshairUI.color = targetUnavailableColor;  // 恢复为白色
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

    // 开始跳跃到目标位置
    void StartJumpToTarget()
    {
        if (!isJumping)
        {
            isJumping = true;
            jumpStartTime = Time.time; // 記錄跳躍開始的時間

            // 保存當前相機的旋轉作為跳躍期間的目標旋轉
            targetRotation = playerCamera.transform.rotation;

            // 設置跳躍目標位置為當前對準目標的位置 + 高度偏移
            if (currentTarget != null)
            {
                jumpTargetPosition = currentTarget.transform.position + Vector3.up * 30f;  // 增加 5 的高度
            }
        }
    }


    // 执行跳跃逻辑
    void PerformJump()
    {
        float t = (Time.time - jumpStartTime) / jumpDuration; // 計算跳躍的時間進度

        if (t < 1f)
        {
            // 使用Lerp平滑過渡相機的位置，並增加高度使其模擬跳躍的感覺
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight; // 使用正弦函數模擬跳躍
            Vector3 targetPosition = Vector3.Lerp(originalPosition, jumpTargetPosition, t);
            targetPosition.y += height; // 給相機一個上升的高度

            // 更新相機位置
            playerCamera.transform.position = targetPosition;

            // 保持相機跳躍開始時的旋轉
            playerCamera.transform.rotation = targetRotation;
        }
        else
        {
            // 跳躍完成，相機停在目標位置
            playerCamera.transform.position = jumpTargetPosition;
            playerCamera.transform.rotation = targetRotation; // 確保旋轉不變

            // 記錄跳躍後的位置並準備跳躍到下一個目標
            originalPosition = jumpTargetPosition;
            isJumping = false; // 跳躍結束
        }
    }
}
