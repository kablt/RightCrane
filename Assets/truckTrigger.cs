using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truckTrigger : MonoBehaviour
{
    private GameObject ExitPoint;
    private GameObject TruckPointA;
    public int truckNum;

    // OnTriggerExit 함수는 트리거 영역을 빠져나갈 때 호출됩니다.
    void Start()
    {
        // Scene에서 ExitPoint GameObject를 찾아서 할당합니다.
        ExitPoint = GameObject.Find("CarExit");
        TruckPointA = GameObject.Find("CarPoint");

        StartCoroutine(GoPointA());
    }

    private void OnTriggerExit(Collider other)
    {
        // 트리거 영역을 빠져나간 오브젝트가 Coil 태그를 가진 자식 오브젝트인지 확인합니다.
        if (other.CompareTag("Coil"))
        {
            // 트리거 영역을 빠져나간 자식 오브젝트에 대한 처리를 수행합니다.
            Debug.Log("Coil 태그를 가진 오브젝트가 트리거 영역을 빠져나갔습니다.");
            GohomeNow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 트리거 영역을 빠져나간 오브젝트가 ExitPoint인지 확인합니다.
        if (other.CompareTag("Exit"))
        {
            // 트리거 영역을 빠져나간 자식 오브젝트에 대한 처리를 수행합니다.
            Debug.Log("사라지는거 확인용");
            Destroy(gameObject);
        }
        if(other.CompareTag("PointA"))
        {
            StopTruck();
        }
    }

    public void GohomeNow()
    {
        StartCoroutine(GoHome());
    }

    IEnumerator GoHome()
    {
        Debug.Log("고홈코루틴확인용");
        // Move the object towards the ExitPoint direction.
        if (ExitPoint != null)
        {
            // Define the speed at which the object will move.
            float speed = 3f; // Adjust as needed

            // Loop until the distance between the current position and ExitPoint is 2 or less.
            while (Vector3.Distance(transform.position, ExitPoint.transform.position) >= 0f)
            {
                // Calculate the direction towards the ExitPoint.
                Vector3 direction = ExitPoint.transform.position - transform.position;

                // Normalize the direction vector to maintain constant speed.
                direction.Normalize();

                // Move the object towards the ExitPoint with constant speed.
                transform.position += direction * speed * Time.deltaTime;

                yield return null; // Wait for the next frame.
            }
        }
        else
        {
            Debug.LogWarning("ExitPoint is not assigned. Please assign the ExitPoint transform in the Inspector.");
        }
    }

   IEnumerator GoPointA()
    {
        float truckspeed = 3f;

        while (Vector3.Distance(transform.position, TruckPointA.transform.position) >= 0f)
        {
            // Calculate the direction towards the ExitPoint.
            Vector3 direction = TruckPointA.transform.position - transform.position;

            // Normalize the direction vector to maintain constant speed.
            direction.Normalize();

            // Move the object towards the ExitPoint with constant speed.
            transform.position += direction * truckspeed * Time.deltaTime;

            yield return null; // Wait for the next frame.
        }
        yield return null;
    }
    public void StopTruck()
    {
        StopCoroutine(GoPointA());
    }
}
