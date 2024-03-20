using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CubeMouseOver : MonoBehaviour
{
    public TMP_Text cubeText; // �ν����� â���� ������ UI Text

    void Start()
    {
        cubeText.gameObject.SetActive(false); // ���� �� �ؽ�Ʈ�� ����ϴ�.
    }

    void OnMouseOver()
    {
        cubeText.gameObject.SetActive(true); // ���콺�� ������Ʈ ���� ���� �� �ؽ�Ʈ�� �����ݴϴ�.
    }

    void OnMouseExit()
    {
        cubeText.gameObject.SetActive(false); // ���콺�� ������Ʈ�� ��� �� �ؽ�Ʈ�� ����ϴ�.
    }
}
