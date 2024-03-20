using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoilDatas : MonoBehaviour
{
    public GameObject csv;
    CSVDataLoader csvloader;
    public GameObject truck;
    truckTrigger trucktrigger;
    public float InCoilID, InCoilWeight, InCoilWidth, InCoilOD, InCoilReceiveOrder, InCoilSendOrder;   
    public int Number;
    // Start is called before the first frame update
    void Start()
    {
        csv =GameObject.Find("CSVLoader");
        csvloader = csv.GetComponent<CSVDataLoader>();
        trucktrigger = truck.GetComponent<truckTrigger>();
        Number = trucktrigger.truckNum;
        InCoilID = csvloader.coilDatas[Number].CoilID;
        InCoilWeight = csvloader.coilDatas[Number].CoilWeight;
        InCoilWidth = csvloader.coilDatas[Number].CoilWidth;
        InCoilOD = csvloader.coilDatas[Number].CoilID;
        InCoilReceiveOrder = csvloader.coilDatas[Number].CoilReceiveOrder;
        InCoilSendOrder = csvloader.coilDatas[Number].CoilSendOrder;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
