using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CoilManager : MonoBehaviour
{
    public List<CoilList> coilLists = new List<CoilList>(); // CoilData 객체를 저장할 리스트입니다.

    // CoilData를 CoilManager 내부에 중첩 클래스로 정의합니다.
    // MonoBehaviour를 상속받지 않습니다.
    public class CoilList
    {
        public string coilID;
        public float coilWeight;
        public float coilWidth;
        public float coilOD; // 외경(Outer Diameter)
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

            // CSV 데이터의 각 항목을 변수에 할당합니다.
            string coilID = row[0];
            if (!float.TryParse(row[1], out float coilWeight)) continue;
            if (!float.TryParse(row[2], out float coilWidth)) continue;
            if (!float.TryParse(row[3], out float coilOD)) continue;

            // CoilData 객체를 생성하고 정보를 할당한 뒤 리스트에 추가합니다.
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
