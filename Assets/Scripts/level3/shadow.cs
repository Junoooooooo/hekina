using UnityEngine;

public class shadow : MonoBehaviour
{
    // �i�վ㪺�v�l�l���t��
    public float chaseSpeed = 2f;

    // �v�l���ؼЦ�m�]�q�`�O�q�D�������^
    public Transform endPoint;

    // �O�_���b�l��
    private bool isChasing = true;

    void Update()
    {
        if (isChasing)
        {
            // �ϥδ������v�l�v���V�ؼЦ�m����
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, chaseSpeed * Time.deltaTime);

            // �ˬd�v�l�O�_�w�g�F��ؼЦ�m
            if (Vector3.Distance(transform.position, endPoint.position) < 0.1f)
            {
                isChasing = false;
                // �v�l�F����I��i�H���檺�ާ@�A�Ҧp�����C��
                // GameOver();
            }
        }
    }

    // �i��G����έ��Ұl��
    public void StopChase()
    {
        isChasing = false;
    }

    public void StartChase()
    {
        isChasing = true;
    }
}
