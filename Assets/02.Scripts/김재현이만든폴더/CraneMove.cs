using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CraneMove : MonoBehaviour
{
    public Transform PointA; //������ �ִ� ��ġ
    public Transform LiftRollBack; // ����Ʈ�� ����ҋ��� y�� ��ġ
    public float moveSpeed = 4f; // ũ������ �����̴� �ӵ�
    public float downSpeed = 4f; // ũ���� �������� �ӵ�
    public GameObject CraneBody; // ������ ũ���� body
    public GameObject CraneHoist; // ������ ũ���� hosit
    public GameObject CraneLift; // ������ ũ���� lift
    public GameObject LayShooter; // ray�� ��� ��ü
    public CoilCollision coilcollision;
    public LayerMask CoilLayer; // �浹�� ���̾� ����;
    public bool LiftStatus = true;
    public bool moveStatus = true;
    public Transform PointB;
    public Transform[] SkidPositions;

    enum CraneStatus
    {
        Idle, // ����Ʈ�� ���� �ö�����ִ� ����
        MovePickUpPoint,// ������ �������� ��ġ�� �̵�
        APoint, // ������ �ݱ����� Ʈ���� ������ ��ġ�� �̵�
        Detected, // ����Ʈ�� ������ ������ ���� �Լ�
        CoilMove, // Coil�� ������ ��ġ�� �ű�� �Լ�
    }
    CraneStatus cranstatus;
    void Start()
    {
        cranstatus = CraneStatus.Idle;
    }
    void Update()
    {

        switch (cranstatus)
        {
            case CraneStatus.Idle:
                IdleMove();
                break;
            case CraneStatus.MovePickUpPoint:
                MovePickUpPoint();
                break;
            case CraneStatus.Detected:
                CraneDetectedCoil();
                break;
            case CraneStatus.CoilMove:
                StopCoroutine(DetectCoil());
                MovementPointB();
                break;
            default:
                Debug.Log("������ �߻��߽��ϴ�");
                break;
        }

    }

    //-------------------------------------------------------------IDLE------------------------------------------------------------------------
    //�������ϋ� ����Ʈ �÷��α�
    public void IdleMove()
    {
        StartCoroutine(IdleStatusLift());
    }
    public void StopIdleStatus()
    {
        StopCoroutine(IdleStatusLift());
        IdleMove();
    }
    //LayShoot��ũ��Ʈ���� ���� �Լ�.
    public void StatusChangeMovePickUpPopint()
    {
        StopCoroutine(IdleStatusLift());
        cranstatus = CraneStatus.MovePickUpPoint;
    }

    //���� ������ ������ ��� ���¿� ���� ���� �ڷ�ƾ
    IEnumerator IdleStatusLift()
    {
        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
        CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionY, moveSpeed * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        LayShoot objectBShooter = LayShooter.GetComponent<LayShoot>();
        //�ش� ��ü�� null�� �ƴ϶��.(�ش� ������ �ִٸ�)
        objectBShooter.ShootAndCheckForCoil();
    }
    //--------------------------------------------MovePointA----------------------------------------------------------------

    //�������ϋ� ����⿡�� ray���� ������ �ִ��� ��� üũ. ������ PointA�� �̵�
    public void MovePickUpPoint()
    {
        StartCoroutine(MovementRoutine());
    }
    //������ �������� APoint�� �ű�� �Լ�
    IEnumerator MovementRoutine()
    {
        downSpeed = 4f;
        // ����Ʈ y������ �ø��°�
        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
        CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionY, moveSpeed * Time.deltaTime);
        //Debug.Log("Move1");
        yield return new WaitForSeconds(1f);
        //Hoist z������ �����̴°�
        Vector3 targetpositionZ = new Vector3(CraneHoist.transform.position.x, CraneHoist.transform.position.y, PointA.position.z);
        CraneHoist.transform.position = Vector3.Lerp(CraneHoist.transform.position, targetpositionZ, moveSpeed * Time.deltaTime);
        //Debug.Log("Move2");
        yield return new WaitForSeconds(1f);
        // Body�� x������ �����̱�
        Vector3 targetpositionX = new Vector3(PointA.position.x, CraneBody.transform.position.y, CraneBody.transform.position.z);
        CraneBody.transform.position = Vector3.Lerp(CraneBody.transform.position, targetpositionX, moveSpeed * Time.deltaTime);
        //Debug.Log("Move3");
        yield return new WaitForSeconds(1f);
        LiftStatus = true;
        cranstatus = CraneStatus.Detected;
    }
    //-----------------------------------------LiftDown-------------------------------------------------------------------------
    //ray�� Coil�±װ� ������ �ش� ������ ���� �ڵ�
    public void CraneDetectedCoil()
    {
        StopCoroutine(MovementRoutine());
       // Debug.Log("�ι��� case �Լ��� �߳Ѿ�Դ�.");
        StartCoroutine(DetectCoil());
        //����Ʈ�� Ư�� ������ ������ �浹�� ������ ��ġ�� ����Ʈ�� Ư����ġ�� ������Ʈ�ϴ� �Լ������.
    }
    //�浹��ũ��Ʈ���� ���� �Լ�

    IEnumerator DetectCoil()
    {
       // Debug.Log("����Ʈ ����������");
        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, PointA.position.y, CraneLift.transform.position.z);
        float distance = Vector3.Distance(CraneLift.transform.position, targetPositionY);
        if (distance > 0.01f && LiftStatus == true)
        {
            Debug.Log("���⵵ �ݺ��ǰ��ִ°�");
            CraneLift.transform.position = Vector3.MoveTowards(CraneLift.transform.position, targetPositionY, downSpeed * Time.deltaTime);
            yield return null;
            distance = Vector3.Distance(CraneLift.transform.position, targetPositionY);
        }

        yield return new WaitForSeconds(1f);

        if (downSpeed == 0f && cranstatus != CraneStatus.CoilMove)
        {
            //�ٽ� �ö󰡴°�
            Vector3 targetPositionYT = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
            CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionYT, moveSpeed * Time.deltaTime);
            // Once the lift has reached the target position, change crane status
           // Debug.Log("���ϻ�����ȯüũ");
            yield return new WaitForSeconds(3f);
            cranstatus = CraneStatus.CoilMove;
        }

    }
    //-----------------------------------------(MoveTOSkid)-------------------------------------------------------------------------

    //��Ű��� �ű�� �ڷ�ƾ �����ϴ� �Լ�
    public void MovementPointB()
    {
        LiftStatus = true;
        StopCoroutine(DetectCoil());
        StartCoroutine(MovePoint());
    }
    public void StopMovePoint()
    {
        //Debug.Log("��Ű�� �浹�� idle ���·� ���� �ڵ�");
        StopCoroutine(MovePoint());
        cranstatus = CraneStatus.Idle;
    }

    //���� �浹�� ��ġ�� ����Ʈ�� ������Ʈ ��� ������ ��ǥ�������� �̵��ϴ� �Լ�
    IEnumerator MovePoint()
    {
        InitializePointB();
        downSpeed = 4f;
       // Debug.Log("MovePoint�� �Ѿ�Դ�.");
        yield return new WaitForSeconds(1f);
       // Debug.Log("����Ʈ�������� �ű�� �Լ��� ���۵Ǵ� �κ��̴�.");
        Vector3 targetpositionX = new Vector3(PointB.position.x, CraneBody.transform.position.y, CraneBody.transform.position.z);
        CraneBody.transform.position = Vector3.Lerp(CraneBody.transform.position, targetpositionX, downSpeed * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        Vector3 targetpositionZ = new Vector3(CraneHoist.transform.position.x, CraneHoist.transform.position.y, PointB.position.z);
        CraneHoist.transform.position = Vector3.Lerp(CraneHoist.transform.position, targetpositionZ, downSpeed * Time.deltaTime);
        yield return new WaitForSeconds(1f);



        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, PointB.position.y, CraneLift.transform.position.z);
        float distance = Vector3.Distance(CraneLift.transform.position, targetPositionY);
        if (distance > 0.01f && moveStatus == true)
        {
            CraneLift.transform.position = Vector3.MoveTowards(CraneLift.transform.position, targetPositionY, downSpeed * Time.deltaTime);
            yield return null;
            distance = Vector3.Distance(CraneLift.transform.position, targetPositionY);
        }
        yield return new WaitForSeconds(2f);
        //�ش���ġ ������ �� , ������ ��ġ�� ����Ʈ�� Ư����ġ�� ������Ʈ�ǰ��ִ� ��Ȳ���� ����Ʈ�� �������� �ż��� ����. �������µ��� ��Ű���� Ư���κа� �浹�� ������ ��ġ�� ����Ʈ�� Ư�� ��ġ�� ������Ʈ �Ǵ� �Լ� ����. 
        //������ ��ġ�� ��Ű���� Ư����ġ�� �ű�� �Լ� ���� ������ �ش� ��ġ�� �������� ��ó�� ���̰� �����
    }

    void InitializePointB()
    {
        float priorNum = coilcollision.CoilNumber;
        if (priorNum >= 0 && priorNum < 5)
        {
            int startIndex = 0; 
            int endIndex = 4;   

            for (int i = startIndex; i <= endIndex && i < SkidPositions.Length; i++)
            {
                Transform skidPosition = SkidPositions[i];
                SkidBool skidBoolScript = skidPosition.GetComponent<SkidBool>();

                // Check if the SkidBool script is attached and SkidUse is true
                if (skidBoolScript != null && skidBoolScript.SkidUse)
                {
                    // Set PointB to the current skidPosition
                    PointB = skidPosition;
                    break; // Exit the loop since we found a valid PointB
                }
            }
        }
        else if(priorNum >4 && priorNum <10)
        {
            int startIndex = 5;
            int endIndex = 9;

            for (int i = startIndex; i <= endIndex && i < SkidPositions.Length; i++)
            {
                Transform skidPosition = SkidPositions[i];
                SkidBool skidBoolScript = skidPosition.GetComponent<SkidBool>();

                // Check if the SkidBool script is attached and SkidUse is true
                if (skidBoolScript != null && skidBoolScript.SkidUse)
                {
                    // Set PointB to the current skidPosition
                    PointB = skidPosition;
                    break; // Exit the loop since we found a valid PointB
                }
            }
        }
        else if (priorNum > 9 && priorNum < 15)
        {
            int startIndex = 10;
            int endIndex = 14;

            for (int i = startIndex; i <= endIndex && i < SkidPositions.Length; i++)
            {
                Transform skidPosition = SkidPositions[i];
                SkidBool skidBoolScript = skidPosition.GetComponent<SkidBool>();

                // Check if the SkidBool script is attached and SkidUse is true
                if (skidBoolScript != null && skidBoolScript.SkidUse)
                {
                    // Set PointB to the current skidPosition
                    PointB = skidPosition;
                    break; // Exit the loop since we found a valid PointB
                }
            }
        }
        else if (priorNum > 14 && priorNum < 20)
        {
            int startIndex = 15;
            int endIndex = 19;

            for (int i = startIndex; i <= endIndex && i < SkidPositions.Length; i++)
            {
                Transform skidPosition = SkidPositions[i];
                SkidBool skidBoolScript = skidPosition.GetComponent<SkidBool>();

                // Check if the SkidBool script is attached and SkidUse is true
                if (skidBoolScript != null && skidBoolScript.SkidUse)
                {
                    // Set PointB to the current skidPosition
                    PointB = skidPosition;
                    break; // Exit the loop since we found a valid PointB
                }
            }
        }
        else
        {
            //Debug.Log("��ϵǾ��������� �����Դϴ�");
        }
    }
}
