using UnityEngine;

public class RotateObject : MonoBehaviour
{
    // ����t�ס]���G��/��^
    public Vector3 rotationSpeed = new Vector3(0, 100, 0);

    // Update �O�C�V�եΤ@�������
    void Update()
    {
        // ��a����A�T�O�u�v�T���󪺱���Ӥ��v�T��m
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);
    }
}
