using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class truckTrigger : MonoBehaviour
{
    private GameObject ExitPoint;
    private GameObject TruckPointA;
    public int truckNum;

    // OnTriggerExit �Լ��� Ʈ���� ������ �������� �� ȣ��˴ϴ�.
    void Start()
    {
        // Scene���� ExitPoint GameObject�� ã�Ƽ� �Ҵ��մϴ�.
        ExitPoint = GameObject.Find("CarExit");
        TruckPointA = GameObject.Find("CarPoint");

        StartCoroutine(GoPointA());
    }

    private void OnTriggerExit(Collider other)
    {
        // Ʈ���� ������ �������� ������Ʈ�� Coil �±׸� ���� �ڽ� ������Ʈ���� Ȯ���մϴ�.
        if (other.CompareTag("Coil"))
        {
            // Ʈ���� ������ �������� �ڽ� ������Ʈ�� ���� ó���� �����մϴ�.
            Debug.Log("Coil �±׸� ���� ������Ʈ�� Ʈ���� ������ �����������ϴ�.");
            GohomeNow();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ʈ���� ������ �������� ������Ʈ�� ExitPoint���� Ȯ���մϴ�.
        if (other.CompareTag("Exit"))
        {
            // Ʈ���� ������ �������� �ڽ� ������Ʈ�� ���� ó���� �����մϴ�.
            Debug.Log("������°� Ȯ�ο�");
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
        Debug.Log("��Ȩ�ڷ�ƾȮ�ο�");
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
