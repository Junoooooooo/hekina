using UnityEngine;
using System.Collections;

public class StarColorTutorial : MonoBehaviour
{
    public Renderer starRenderer; // 3D �P�P�� Renderer
    public Light tutorialLight; // ���������O��
    public AudioSource correctSound, wrongSound; // ���\/���ѭ���

    private Color[] starColors = { Color.red, new Color(0.2f, 0.4f, 0.5f), Color.yellow }; // �T�w����
    private KeyCode[] keyBindings = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow }; // ��L����
    private int currentStep = 0; // ��e�B�J
    private Material starMaterial; // �P�P����
    private Color originalEmission; // ��l Emission �C��

    public bool isTutorialComplete = false; // �s�W�ˬd�C���оǬO�_�������ܼ�

    void Start()
    {
        tutorialLight.enabled = false; // �T�O�O������
        starMaterial = starRenderer.material;
        originalEmission = starMaterial.GetColor("_EmissionColor");
        SetStarColor(); // �]�w�Ĥ@���P�P�C��
    }

    void Update()
    {
        if (currentStep < starColors.Length)
        {
            if (Input.GetKeyDown(keyBindings[currentStep])) // �˴���������
            {
                correctSound.Play();
                StartCoroutine(LightUp());
            }
            else if (Input.anyKeyDown) // �p�G������
            {
                wrongSound.Play();
                StartCoroutine(FlashStar());
            }
        }
    }

    void SetStarColor()
    {
        if (currentStep < starColors.Length)
        {
            Color newEmission = starColors[currentStep] * 1.0f; // �W�[�G��
            starMaterial.SetColor("_EmissionColor", newEmission);
            starMaterial.EnableKeyword("_EMISSION"); // �T�O Emission �ҥ�
        }
        else
        {
            Debug.Log("�оǧ����I");
            isTutorialComplete = true; // ��оǵ�����]�m�� true
        }
    }

    IEnumerator LightUp()
    {
        tutorialLight.enabled = true;
        yield return new WaitForSeconds(0.5f);
        tutorialLight.enabled = false;

        currentStep++;
        SetStarColor();
    }

    IEnumerator FlashStar()
    {
        starMaterial.SetColor("_EmissionColor", Color.black);
        yield return new WaitForSeconds(0.2f);
        SetStarColor();
    }
}
