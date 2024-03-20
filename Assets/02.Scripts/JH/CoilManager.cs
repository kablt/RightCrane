using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoilManager : MonoBehaviour
{
    public List<CoilList> coilLists = new List<CoilList>(); // CoilData ��ü�� ������ ����Ʈ�Դϴ�.

    // CoilData�� CoilManager ���ο� ��ø Ŭ������ �����մϴ�.
    // MonoBehaviour�� ��ӹ��� �ʽ��ϴ�.
    public class CoilList
    {
        public string coilID;
        public float coilWeight;
        public float coilWidth;
        public float coilOD; // �ܰ�(Outer Diameter)
    }

    void Start()
    {
        AssignIDsFromCSV("CoilInformation");
    }

    void AssignIDsFromCSV(string fileName)
    {
        TextAsset coilList = Resources.Load<TextAsset>(fileName);
        string[] dataLines = coilList.text.Split('\n');

        for (int i = 1; i < dataLines.Length; i++)
        {

            string[] row = dataLines[i].Split(',');
            if (row.Length < 4 || string.IsNullOrWhiteSpace(row[0])) continue;

            // CSV �������� �� �׸��� ������ �Ҵ��մϴ�.
            string coilID = row[0];
            if (!float.TryParse(row[1], out float coilWeight)) continue;
            if (!float.TryParse(row[2], out float coilWidth)) continue;
            if (!float.TryParse(row[3], out float coilOD)) continue;

            // CoilData ��ü�� �����ϰ� ������ �Ҵ��� �� ����Ʈ�� �߰��մϴ�.
            CoilList newCoilList = new CoilList
            {
                coilID = coilID,
                coilWeight = coilWeight,
                coilWidth = coilWidth,
                coilOD = coilOD
            };
            coilLists.Add(newCoilList);
        }
    }
}
