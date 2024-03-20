using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayShoot : MonoBehaviour
{
    public GameObject CraneManager;
    CraneMove cranemove;
    void Awake()
    {
        cranemove = CraneManager.GetComponent<CraneMove>();
    }
    public float rayDis = 20f;
    public void ShootAndCheckForCoil()
    {
        // Shoot a ray from the current GameObject in the forward direction
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.right, out hit, rayDis))
        {
            // Check if the hit object has the specified tag "coil"
            if (hit.collider.CompareTag("Coil"))
            {
                Debug.Log("����Ȯ�� �Ϸ�");
                //MovePickUpPoint ���·� ��ȯ
                cranemove.StatusChangeMovePickUpPopint();
            }
            else
            {
                cranemove.StopIdleStatus();
                Debug.Log("������ ���� �������־���մϴ�");
                return;
            }
        }

    }
}
