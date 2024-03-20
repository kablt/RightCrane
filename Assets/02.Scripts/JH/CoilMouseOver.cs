using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoilMouseOver : MonoBehaviour
{
    public TMP_Text idDisplayText; // ID를 표시할 Text 컴포넌트의 참조
    private CoilManager coilManager; // CoilManager 스크립트의 참조

    void Start()
    {
        // CoilManager 컴포넌트의 참조를 찾습니다.
        coilManager = FindObjectOfType<CoilManager>();
    }

    void OnMouseEnter()
    {
        if (idDisplayText != null && coilManager != null)
        {
            // gameObject.name에서 "Coil_"을 제거하여 실제 coilID를 얻습니다.
            string coilID = gameObject.name.Replace("Coil_", "");
            // 해당 ID에 맞는 CoilData를 찾습니다.
            CoilManager.CoilList coilData = coilManager.coilLists.Find(data => data.coilID == coilID);

            if (coilData != null)
            {
                // 찾은 데이터를 텍스트로 표시합니다.
                idDisplayText.text = $"ID: {coilData.coilID}\nWeight: {coilData.coilWeight}\nWidth: {coilData.coilWidth}\nOD: {coilData.coilOD}";
            }
        }
    }

    void OnMouseExit()
    {
        // 마우스가 오브젝트를 벗어났을 때 텍스트 초기화
        if (idDisplayText != null)
        {
            idDisplayText.text = "";
        }
    }
}



//public class CoilMouseOver : MonoBehaviour
//{
//    public TMP_Text idDisplayText; // ID를 표시할 Text 컴포넌트의 참조
//    private CSVDataLoader dataLoader; // CSVDataLoader 스크립트의 참조


//    void OnMouseEnter()
//    {
//        // 마우스가 오브젝트 위에 올라왔을 때 ID 표시
//        if (idDisplayText != null)
//        {
//            idDisplayText.text = gameObject.name; // 여기서 gameObject.name은 Coil 오브젝트의 이름을 가져옵니다.
//        }
//    }

//    void OnMouseExit()
//    {
//        // 마우스가 오브젝트를 벗어났을 때 텍스트 초기화
//        if (idDisplayText != null)
//        {
//            idDisplayText.text = "";
//        }
//    }
//}
