using UnityEngine;
using UnityEngine.SceneManagement; // �ޤJ�����޲z

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
                Debug.Log("Shadow reached the end point!");
            }
        }
    }

    // ��v�l�I�쪱�a��
    private void OnTriggerEnter(Collider other)
    {
        // �ˬd�I�쪺����O�_�� "Player" ����
        if (other.CompareTag("Player"))
        {
            Debug.Log("Shadow touched the Player! Restarting the game...");
            RestartGame();
        }
    }

    // �C�������޿�
    private void RestartGame()
    {
        // ���s�[����e����
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
