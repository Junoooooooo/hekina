using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public float turnSpeed = 5f; // 旋转的速度，用于平滑过渡
    private Quaternion targetRotation; // 目标旋转
    private Vector3 currentDirection; // 当前前进方向

    void Start()
    {
        currentDirection = transform.forward; // 初始前进方向
        targetRotation = transform.rotation; // 初始化目标旋转为当前旋转
    }

    void Update()
    {
        HandleTurnInput(); // 处理转弯输入
        MoveForward(); // 持续向前移动
        SmoothRotate(); // 平滑地旋转到目标方向
    }

    // 向当前方向移动
    void MoveForward()
    {
        // 根据当前方向移动
        transform.Translate(currentDirection * speed * Time.deltaTime, Space.World);
    }

    // 平滑旋转到目标方向
    void SmoothRotate()
    {
        // 使用 RotateTowards 实现平滑旋转，逐渐接近目标旋转
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // 更新当前前进方向为旋转后的方向
        currentDirection = transform.forward;
    }

    // 处理转弯输入
    void HandleTurnInput()
    {
        // 检查左转输入
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // MakeyMakey 左箭头按键
        {
            TurnLeft();
        }
        // 检查右转输入
        else if (Input.GetKeyDown(KeyCode.RightArrow)) // MakeyMakey 右箭头按键
        {
            TurnRight();
        }
    }

    // 左转处理
    void TurnLeft()
    {
        // 更新目标旋转为当前旋转的左转 90 度
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - 15f, 0);
        Debug.Log("Turning Left (smooth)!");
    }

    // 右转处理
    void TurnRight()
    {
        // 更新目标旋转为当前旋转的右转 90 度
        targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 15f, 0);
        Debug.Log("Turning Right (smooth)!");
    }

    // 碰撞检测
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // 碰撞到障碍物
        {
            Debug.Log("Crashed into an obstacle! Restarting level...");
            RestartCurrentLevel(); // 重启场景
        }
    }

    // 重新加载当前场景
    public void RestartCurrentLevel()
    {
        // 获取当前场景名称
        string currentSceneName = SceneManager.GetActiveScene().name;

        // 重新加载当前场景
        SceneManager.LoadScene(currentSceneName);
    }
}
