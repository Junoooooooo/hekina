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
            // ���ʨ��e�`�I
            Transform targetNode = nodes[currentNodeIndex];
            transform.position = Vector3.MoveTowards(transform.position, targetNode.position, moveSpeed * Time.deltaTime);
            transform.LookAt(targetNode); // �Ϩ��⭱�V�`�I

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
