using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkidBool : MonoBehaviour
{
    public bool SkidUse = true;
    GameObject coil;
    CoilrightData coildata;
    public float PirorNum;
    public GameObject craneright;
    CraneSkidNumManager cm;

    // Start is called before the first frame update
    void Awake()
    {
        PirorNum = 0f;
    }
    void Start()
    {
        
    }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coil"))
            {
                Debug.Log("���ϻ���üũ");
                coil = other.gameObject;
                coildata = coil.GetComponent<CoilrightData>();
                PirorNum = coildata.coilrightcode;
                cm = craneright.GetComponent<CraneSkidNumManager>();
                cm.SortSkidsByCode();

            }
        }


    void Update()
    {
        
    }

}
