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

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsCurrentNode();
        }
    }

    private void MoveTowardsCurrentNode()
    {
        if (currentNodeIndex < nodes.Length)
        {
            // 移動到當前節點
            Transform targetNode = nodes[currentNodeIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, moveSpeed * Time.deltaTime);
            transform.LookAt(targetNode); // 使角色面向節點

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
