using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoilMouseOver : MonoBehaviour
{
    public TMP_Text idDisplayText; // ID�� ǥ���� Text ������Ʈ�� ����
    private CoilManager coilManager; // CoilManager ��ũ��Ʈ�� ����

    void Start()
    {
        // CoilManager ������Ʈ�� ������ ã���ϴ�.
        coilManager = FindObjectOfType<CoilManager>();
    }

    void OnMouseEnter()
    {
        if (idDisplayText != null && coilManager != null)
        {
            // gameObject.name���� "Coil_"�� �����Ͽ� ���� coilID�� ����ϴ�.
            string coilID = gameObject.name.Replace("Coil_", "");
            // �ش� ID�� �´� CoilData�� ã���ϴ�.
            CoilManager.CoilList coilData = coilManager.coilLists.Find(data => data.coilID == coilID);

            if (coilData != null)
            {
                // ã�� �����͸� �ؽ�Ʈ�� ǥ���մϴ�.
                idDisplayText.text = $"ID: {coilData.coilID}\nWeight: {coilData.coilWeight}\nWidth: {coilData.coilWidth}\nOD: {coilData.coilOD}";
            }
        }
    }

    void OnMouseExit()
    {
        // ���콺�� ������Ʈ�� ����� �� �ؽ�Ʈ �ʱ�ȭ
        if (idDisplayText != null)
        {
            idDisplayText.text = "";
        }
    }
}



//public class CoilMouseOver : MonoBehaviour
//{
//    public TMP_Text idDisplayText; // ID�� ǥ���� Text ������Ʈ�� ����
//    private CSVDataLoader dataLoader; // CSVDataLoader ��ũ��Ʈ�� ����


//    void OnMouseEnter()
//    {
//        // ���콺�� ������Ʈ ���� �ö���� �� ID ǥ��
//        if (idDisplayText != null)
//        {
//            idDisplayText.text = gameObject.name; // ���⼭ gameObject.name�� Coil ������Ʈ�� �̸��� �����ɴϴ�.
//        }
//    }

//    void OnMouseExit()
//    {
//        // ���콺�� ������Ʈ�� ����� �� �ؽ�Ʈ �ʱ�ȭ
//        if (idDisplayText != null)
//        {
//            idDisplayText.text = "";
//        }
//    }
//}
