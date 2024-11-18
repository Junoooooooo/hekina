using UnityEngine;
using System.Collections.Generic;

public class FloatingTargetsManager : MonoBehaviour
{
    [System.Serializable]
    public class FloatingTargetSettings
    {
        public GameObject target;
        public float moveRange = 2.0f;
        public float moveSpeed = 1.0f;
        public Vector3 moveDirection = Vector3.up;
        public bool stopMovement = false;  // 每個目標是否停止移動的控制變數
    }

    public List<FloatingTargetSettings> targetsSettings = new List<FloatingTargetSettings>();

    private List<Vector3> startPositions = new List<Vector3>();
    private List<bool> movingForwards = new List<bool>();

    void Start()
    {
        foreach (var targetSetting in targetsSettings)
        {
            if (targetSetting.target != null)
            {
                startPositions.Add(targetSetting.target.transform.position);
                movingForwards.Add(true);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < targetsSettings.Count; i++)
        {
            if (targetsSettings[i].target != null && !targetsSettings[i].stopMovement)
            {
                UpdateFloatingTarget(targetsSettings[i], i);
            }
        }
    }

    void UpdateFloatingTarget(FloatingTargetSettings settings, int index)
    {
        GameObject target = settings.target;
        Vector3 startPos = startPositions[index];
        Vector3 currentPosition = target.transform.position;

        Vector3 targetPos = startPos + settings.moveDirection.normalized * settings.moveRange;

        if (movingForwards[index])
        {
            target.transform.position = Vector3.MoveTowards(currentPosition, targetPos, settings.moveSpeed * Time.deltaTime);

            if (Vector3.Distance(currentPosition, targetPos) < 0.1f)
            {
                movingForwards[index] = false;
            }
        }
        else
        {
            target.transform.position = Vector3.MoveTowards(currentPosition, startPos, settings.moveSpeed * Time.deltaTime);

            if (Vector3.Distance(currentPosition, startPos) < 0.1f)
            {
                movingForwards[index] = true;
            }
        }
    }

    // 停止特定目標的浮動
    public void StopTargetMovement(GameObject targetToStop)
    {
        foreach (var targetSetting in targetsSettings)
        {
            if (targetSetting.target == targetToStop)
            {
                targetSetting.stopMovement = true;  // 停止此目標的移動
                break;
            }
        }
    }


}