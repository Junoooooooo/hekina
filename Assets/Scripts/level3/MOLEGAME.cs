using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MOLEGAME : MonoBehaviour
{
    public GameObject[] moles; // �s��Ҧ��a������
    public Color[] moleColors = { Color.red, Color.blue, Color.yellow }; // �a���C��
    public KeyCode[] colorKeys = { KeyCode.UpArrow, KeyCode.RightArrow, KeyCode.DownArrow }; // �����C�⪺����
    public Text scoreText; // ��ܤ��ƪ�UI�奻
    public float moleAppearanceTime = 1f; // �a����ܮɶ�
    public float moleCooldownTime = 0.5f; // �a���A����ܪ��N�o�ɶ�
    public float gameDuration = 30f; // �C���ɶ�

    private int score = 0; // ����
    private bool gameActive = false; // �C���O�_�}�l
    private float gameTimer; // �C���p�ɾ�
    private int currentMoleIndex = -1; // ��e��ܪ��a��

    void Start()
    {
        gameTimer = gameDuration;
        StartGame();
    }

    void Update()
    {
        if (gameActive)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer <= 0)
            {
                EndGame();
            }
        }

        // ���a��J�˴�
        if (currentMoleIndex >= 0 && moles[currentMoleIndex].activeSelf)
        {
            // �ˬd���a���U����O�_���T
            if (Input.GetKeyDown(colorKeys[0]) && moles[currentMoleIndex].GetComponent<Renderer>().material.GetColor("_EmissionColor") == moleColors[0])
            {
                CorrectHit();
            }
            else if (Input.GetKeyDown(colorKeys[1]) && moles[currentMoleIndex].GetComponent<Renderer>().material.GetColor("_EmissionColor") == moleColors[1])
            {
                CorrectHit();
            }
            else if (Input.GetKeyDown(colorKeys[2]) && moles[currentMoleIndex].GetComponent<Renderer>().material.GetColor("_EmissionColor") == moleColors[2])
            {
                CorrectHit();
            }
            else if (Input.anyKeyDown)
            {
                IncorrectHit();
            }
        }
    }

    void StartGame()
    {
        score = 0;
        scoreText.text = "Score: " + score;
        gameActive = true;
        StartCoroutine(SpawnMoles());
    }

    void EndGame()
    {
        gameActive = false;
        scoreText.text = "Game Over! Final Score: " + score;
    }

    void CorrectHit()
    {
        score++;
        scoreText.text = "Score: " + score;
        moles[currentMoleIndex].SetActive(false);
        StartCoroutine(SpawnMoles()); // �H�����ͦa��
    }

    void IncorrectHit()
    {
        score--;
        scoreText.text = "Score: " + score;
        moles[currentMoleIndex].SetActive(false);
        StartCoroutine(SpawnMoles()); // �H�����ͦa��
    }

    // �H����ܦa������ܥ�
    IEnumerator SpawnMoles()
    {
        yield return new WaitForSeconds(moleCooldownTime); // ���ݧN�o�ɶ�

        // �H����ܤ@�Ӧa��
        currentMoleIndex = Random.Range(0, moles.Length);

        // �H����ܤ@���C��ó]�m���a��
        Color moleColor = moleColors[Random.Range(0, moleColors.Length)];
        Renderer moleRenderer = moles[currentMoleIndex].GetComponent<Renderer>();

        // �]�m�o���C��
        moleRenderer.material.SetColor("_EmissionColor", moleColor); // �]�m�o���C��
        DynamicGI.SetEmissive(moleRenderer, moleColor); // ��s�������Өt��

        // ���a�����
        moles[currentMoleIndex].SetActive(true);

        // ���ݦa����ܮɶ�
        yield return new WaitForSeconds(moleAppearanceTime);

        // ���æa��
        moles[currentMoleIndex].SetActive(false);

        // ���m��e�a������
        currentMoleIndex = -1;
    }
}
