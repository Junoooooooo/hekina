using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovement : MonoBehaviour
{
    public float speed = 5f; // 移动速度
    public float turnAngle = 90f; // 转动角度
    private Vector3 startPosition; // 车子的起始位置
    private Vector3 currentDirection; // 当前前进方向

    void Start()
    {
        currentDirection = transform.forward; // 初始前进方向
    }

    void Update()
    {
        HandleTurnInput(); // 处理转弯输入
        MoveForward(); // 持续向前移动
    }

    // 向当前方向移动
    void MoveForward()
    {
        // 根据当前方向移动
        transform.Translate(currentDirection * speed * Time.deltaTime, Space.World);
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
        // 旋转 90 度并更新当前方向
        transform.Rotate(0, -turnAngle, 0); // 左转
        currentDirection = transform.forward; // 更新当前方向
        Debug.Log("Turned Left!");
    }

    // 右转处理
    void TurnRight()
    {
        // 旋转 90 度并更新当前方向
        transform.Rotate(0, turnAngle, 0); // 右转
        currentDirection = transform.forward; // 更新当前方向
        Debug.Log("Turned Right!");
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")) // 检测到与障碍物的碰撞
        {
            Debug.Log("Crashed into an obstacle! Returning to start position...");
            // 返回起始位置
            ResetPosition();
        }
    }

    // 重置位置和方向
    void ResetPosition()
    {
        transform.position = startPosition; // 返回起始位置
        transform.rotation = Quaternion.identity; // 重置方向（可选）
    }
}
