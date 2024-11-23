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
            // �p��q�۾��������V�V�q�]���� Y �b�t���^
            Vector3 directionToDoor = currentDoor.transform.position - transform.position;
            directionToDoor.y = 0; // ���� Y �b�A�ȭp���������

            // �p��ؼЪ� Y �b���ਤ��
            Quaternion targetRotation = Quaternion.LookRotation(directionToDoor);

            // �T�w X �b�����ਤ��
            Vector3 targetEulerAngles = targetRotation.eulerAngles;
            targetEulerAngles.x = fixedRotationX; // �T�w X �b����
            targetEulerAngles.z = 0; // �T�w Z �b����

            // �p���e�۾��� Y �b���שM�ؼЪ� Y �b���פ������t��
            float currentYAngle = transform.eulerAngles.y;
            float targetYAngle = targetEulerAngles.y;
            float angleDifference = Mathf.Abs(Mathf.DeltaAngle(currentYAngle, targetYAngle));

            // �p�G���׮t���j�� 5 �סA�i�����
            if (angleDifference > 5f)
            {
                // �]�m�s�� Y �b�ؼб��ਤ�ס]�� 90 �ס^
                float newYAngle = Mathf.MoveTowardsAngle(currentYAngle, targetYAngle, 90f);
                targetEulerAngles.y = newYAngle;

                // �ϥΥؼЪ� Euler ���רӳ]�m���Ʊ���
                targetRotation = Quaternion.Euler(targetEulerAngles);

                // �ϥ� Slerp ���Ʊ����ؼ� Y �b��V
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
            else
            {
                // �w�g��ǡA���A����
                Debug.Log("Camera is already facing the door or within acceptable angle.");
            }
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
