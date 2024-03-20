using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CubeMouseOver : MonoBehaviour
{
    public TMP_Text cubeText; // 인스펙터 창에서 설정할 UI Text

    void Start()
    {
        cubeText.gameObject.SetActive(false); // 시작 시 텍스트를 숨깁니다.
    }

    void OnMouseOver()
    {
        cubeText.gameObject.SetActive(true); // 마우스가 오브젝트 위에 있을 때 텍스트를 보여줍니다.
    }

    void OnMouseExit()
    {
        cubeText.gameObject.SetActive(false); // 마우스가 오브젝트를 벗어날 때 텍스트를 숨깁니다.
    }
}
