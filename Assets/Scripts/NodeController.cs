using UnityEngine;

public class NodeController : MonoBehaviour
{
    public Transform[] nodes; // 節點列表
    public float moveSpeed = 2f; // 移動速度
    public float arrivalThreshold = 0.1f; // 到達節點的閾值
    public GameObject[] doors; // 當前門的 GameObject 列表
    private int currentNodeIndex = 0; // 當前節點索引
    private bool isMoving = true; // 是否在移動

    // 用來儲存目前碰到的門
    private GameObject currentDoor;

    // 固定的 X 軸旋轉角度
    private const float fixedRotationX = -3.8f;

    // 旋轉速度
    public float rotationSpeed = 2f;

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsCurrentNode();
        }
        else
        {
            // 如果停止移動並且碰到門，讓相機面向門
            if (currentDoor != null)
            {
                FaceDoor();
            }
        }
    }

    private void MoveTowardsCurrentNode()
    {
        if (currentNodeIndex < nodes.Length)
        {
            // 移動到當前節點
            Transform targetNode = nodes[currentNodeIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, moveSpeed * Time.deltaTime);

            // 平滑轉向目標節點
            SmoothLookAt(targetNode.position);

            // 檢查是否到達當前節點
            if (Vector3.Distance(transform.position, targetNode.position) < arrivalThreshold)
            {
                currentNodeIndex++; // 移動到下一個節點
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 檢查碰到的物件是否有 "DOOR" 標籤
        if (other.CompareTag("DOOR"))
        {
            Debug.Log("Encountered a door: " + other.gameObject.name + ", stopping movement."); // 顯示訊息
            isMoving = false; // 停止移動
            currentDoor = other.gameObject; // 儲存目前碰到的門
        }
    }

    private void FaceDoor()
    {
        if (currentDoor != null)
        {
            // 計算從相機到門的方向向量（忽略 Y 軸差異）
            Vector3 directionToDoor = currentDoor.transform.position - transform.position;
            directionToDoor.y = 0; // 忽略 Y 軸，僅計算水平旋轉

            // 計算目標的 Y 軸旋轉角度
            Quaternion targetRotation = Quaternion.LookRotation(directionToDoor);

            // 固定 X 軸的旋轉角度
            Vector3 targetEulerAngles = targetRotation.eulerAngles;
            targetEulerAngles.x = fixedRotationX; // 固定 X 軸角度
            targetEulerAngles.z = 0; // 固定 Z 軸角度

            // 計算當前相機的 Y 軸角度和目標的 Y 軸角度之間的差異
            float currentYAngle = transform.eulerAngles.y;
            float targetYAngle = targetEulerAngles.y;
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentYAngle, targetYAngle));

            // 如果角度差異大於 5 度，進行旋轉
            if (angleDifference > 5f)
            {
                // 設置新的 Y 軸目標旋轉角度（轉 90 度）
                float newYAngle = Mathf.MoveTowardsAngle(currentYAngle, targetYAngle, 90f);
                targetEulerAngles.y = newYAngle;

                // 使用目標的 Euler 角度來設置平滑旋轉
                targetRotation = Quaternion.Euler(targetEulerAngles);

                // 使用 Slerp 平滑旋轉到目標 Y 軸方向
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // 已經對準，不再旋轉
                Debug.Log("Camera is already facing the door or within acceptable angle.");
            }
        }
    }





    private void SmoothLookAt(Vector3 targetPosition)
    {
        // 計算目標的方向
        Vector3 directionToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // 固定 X 軸的旋轉角度
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        targetEulerAngles.x = fixedRotationX;
        targetRotation = Quaternion.Euler(targetEulerAngles);

        // 使用 Slerp 平滑旋轉到目標方向
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void UnlockCurrentDoor()
    {
        if (currentDoor != null)
        {
            Debug.Log("Unlocking door: " + currentDoor.name);
            currentDoor.SetActive(false); // 隱藏門，代表已解鎖
            isMoving = true; // 繼續移動
            currentDoor = null; // 清除目前的門
        }
    }
}
