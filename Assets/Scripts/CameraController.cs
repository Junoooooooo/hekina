using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f; // ���ʳt��
    public float rotationSpeed = 2f; // ���઺���Ƴt��
    public float rotationStep = 10f; // �C�����઺����
    private Quaternion targetRotation; // �ؼб���

    void Start()
    {
        // ��l�ƥؼб��ର��e��v��������
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // �u�� W �M S ����e�Ჾ��
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 forwardMovement = transform.forward;
            transform.position += forwardMovement * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            Vector3 backwardMovement = -transform.forward;
            transform.position += backwardMovement * moveSpeed * Time.deltaTime;
        }

        // A �M D �������
        if (Input.GetKey(KeyCode.A))
        {
            // �C���������e�ؼб���A�V����
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y - 15f, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            // �C���������e�ؼб���A�V�k��
            targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 15f, 0);
        }

        // ���ƴ��Ȩ�ؼб���
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}
