using UnityEngine;

public class NodeController : MonoBehaviour
{
    public Transform[] nodes; // �`�I�C��
    public float moveSpeed = 2f; // ���ʳt��
    public float arrivalThreshold = 0.1f; // ��F�`�I���H��
    public GameObject[] doors; // ��e���� GameObject �C��
    private int currentNodeIndex = 0; // ��e�`�I����
    private bool isMoving = true; // �O�_�b����

    // �Ψ��x�s�ثe�I�쪺��
    private GameObject currentDoor;

    // �T�w�� X �b���ਤ��
    private const float fixedRotationX = -3.8f;

    // ����t��
    public float rotationSpeed = 2f;

    void Update()
    {
        if (isMoving)
        {
            MoveTowardsCurrentNode();
        }
        else
        {
            // �p�G����ʨåB�I����A���۾����V��
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
            // ���ʨ��e�`�I
            Transform targetNode = nodes[currentNodeIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, moveSpeed * Time.deltaTime);

            // ������V�ؼи`�I
            SmoothLookAt(targetNode.position);

            // �ˬd�O�_��F��e�`�I
            if (Vector3.Distance(transform.position, targetNode.position) < arrivalThreshold)
            {
                currentNodeIndex++; // ���ʨ�U�@�Ӹ`�I
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�I�쪺����O�_�� "DOOR" ����
        if (other.CompareTag("DOOR"))
        {
            Debug.Log("Encountered a door: " + other.gameObject.name + ", stopping movement."); // ��ܰT��
            isMoving = false; // �����
            currentDoor = other.gameObject; // �x�s�ثe�I�쪺��
        }
    }

    private void FaceDoor()
    {
        if (currentDoor != null)
        {
            // ������V������m
            SmoothLookAt(currentDoor.transform.position);
        }
    }

    private void SmoothLookAt(Vector3 targetPosition)
    {
        // �p��ؼЪ���V
        Vector3 directionToTarget = targetPosition - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // �T�w X �b�����ਤ��
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        targetEulerAngles.x = fixedRotationX;
        targetRotation = Quaternion.Euler(targetEulerAngles);

        // �ϥ� Slerp ���Ʊ����ؼФ�V
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void UnlockCurrentDoor()
    {
        if (currentDoor != null)
        {
            Debug.Log("Unlocking door: " + currentDoor.name);
            currentDoor.SetActive(false); // ���ê��A�N��w����
            isMoving = true; // �~�򲾰�
            currentDoor = null; // �M���ثe����
        }
    }
}
