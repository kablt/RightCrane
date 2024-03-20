using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CoilCollision : MonoBehaviour

{
    public GameObject CraneManager;
    CraneMove cranemove;
    private GameObject coil;
    public GameObject SkidGB;
    CoilDatas coildata;
    public float CoilNumber;
    SkidBool skidbool;

    void Awake()
    {
        cranemove = CraneManager.GetComponent<CraneMove>();
    }

    void OnTriggerEnter(Collider collision)
    {

        if (collision.gameObject.tag == "Coil")
        {
            coil = collision.gameObject;
            coildata = coil.GetComponent<CoilDatas>();
            CoilNumber = coildata.Number;
            cranemove.LiftStatus = false;
            cranemove.moveStatus = true;
            cranemove.downSpeed = 0f;
            Debug.Log($"downspeed : {cranemove.downSpeed}");
            collision.transform.SetParent(transform);
            Debug.Log("부모바꾸는 디버그");
           
        }

        if (collision.gameObject.CompareTag("Skid"))
        {
            Debug.Log("Crash with Skid");
            SkidGB = collision.gameObject;
            skidbool = SkidGB.GetComponent<SkidBool>();
            skidbool.SkidUse = false;
            cranemove.moveStatus = false;
            cranemove.StopMovePoint();
            coil.transform.SetParent(collision.transform);
            coil.transform.position = collision.transform.position;
            coil.GetComponent<Rigidbody>().velocity = Vector3.zero;
            // Function called when a collision with an object with the Skid tag has ended
        }
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {

    }
}
