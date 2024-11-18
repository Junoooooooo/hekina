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
            jumpStartTime = Time.time; // 记录跳跃开始的时间

            // 设置跳跃目标位置为当前对准目标的位置 + 高度偏移
            if (currentTarget != null)
            {
                jumpTargetPosition = currentTarget.transform.position + Vector3.up * 5f;  // 增加 5 的高度
            }
        }
    }

    // 执行跳跃逻辑
    void PerformJump()
    {
        float t = (Time.time - jumpStartTime) / jumpDuration; // 计算跳跃的时间进度

        if (t < 1f)
        {
            // 使用Lerp平滑过渡相机的位置，并增加高度使其模拟跳跃的感觉
            float height = Mathf.Sin(t * Mathf.PI) * jumpHeight; // 使用正弦函数模拟跳跃
            Vector3 targetPosition = Vector3.Lerp(originalPosition, jumpTargetPosition, t);
            targetPosition.y += height; // 给相机一个上升的高度

            // 更新相机位置
            playerCamera.transform.position = targetPosition;

            // 保持相机的初始旋转
            playerCamera.transform.rotation = originalRotation;
        }
        else
        {
            // 跳跃完成，相机停在目标位置
            playerCamera.transform.position = jumpTargetPosition;
            playerCamera.transform.rotation = originalRotation; // 确保旋转不变

            // 记录跳跃后的位置并准备跳跃到下一个目标
            originalPosition = jumpTargetPosition;
            isJumping = false; // 跳跃结束
        }
    }
}
