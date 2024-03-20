using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class CoilList // Ŭ���� �� ����
{
  // CoilID�� ������ ���ڿ��� �޽��ϴ�.  (ex,C001)
    public float CoilID,CoilWeight, CoilWidth, CoilOD; // ���� �� �ʵ���� float Ÿ���Դϴ�.
    public float CoilReceiveOrder, CoilSendOrder; // ���ڸ� ���ԵǾ� �ִٸ�, int�� �޴� ���� �� ������ �� �ֽ��ϴ�.
}

public class CSVDataLoader : MonoBehaviour
{
    public TextAsset csvFile; // �ν����� â���� ������ CSV ����
    public List<CoilList> coilDatas = new List<CoilList>(); // CSV���� ���� �����͸� ������ ����Ʈ, Ÿ�� ����

    void Awake()
    {
        ReadCSV();
        DebugLogFourthLine();
    }

    void ReadCSV()
    {
        string[] dataLines = csvFile.text.Split('\n');
        for (int i = 0; i < dataLines.Length; i++) // ù ��° ���� ������ �����ϰ� �ǳʶݴϴ�.
        {
            string[] data = dataLines[i].Split(',');
            if (data.Length == 6)
            {
                coilDatas.Add(new CoilList() // Ÿ�� ����
                {
                    CoilID = float.Parse(data[0]), // ���ڿ� �״�� ����
                    CoilWeight = float.Parse(data[1]), // ���ڿ��� float�� ��ȯ
                    CoilWidth = float.Parse(data[2]), // ���ڿ��� float�� ��ȯ
                    CoilOD = float.Parse(data[3]), // ���ڿ��� float�� ��ȯ
                    CoilReceiveOrder = float.Parse(data[4]), // ���ڿ��� int�� ��ȯ
                    CoilSendOrder = float.Parse(data[5]) // ���ڿ��� int�� ��ȯ
                });
            }
        }
    }

    void DebugLogFourthLine()
    {
        if (coilDatas.Count >= 4)
        {
            CoilList fourthLineData = coilDatas[3]; // �����ʹ� �ε��� 3�� �ֽ��ϴ�, Ÿ�� ����
            Debug.Log($"4��° �� ������: CoilID: {fourthLineData.CoilID}, CoilWeight: {fourthLineData.CoilWeight}, CoilWidth: {fourthLineData.CoilWidth}, CoilOD: {fourthLineData.CoilOD}, CoilReceiveOrder: {fourthLineData.CoilReceiveOrder}, CoilSendOrder: {fourthLineData.CoilSendOrder}");
        }
        else
        {
            Debug.LogError("CSV ���Ͽ� ����� �����Ͱ� �����ϴ�.");
        }
    }
}
