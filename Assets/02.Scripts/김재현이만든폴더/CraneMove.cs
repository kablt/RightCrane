using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CraneMove : MonoBehaviour
{
    public Transform PointA; //코일이 있는 위치
    public Transform LiftRollBack; // 리프트가 대기할떄의 y축 위치
    public float moveSpeed = 4f; // 크레인이 움직이는 속도
    public float downSpeed = 4f; // 크레인 내려가는 속도
    public GameObject CraneBody; // 움직일 크레인 body
    public GameObject CraneHoist; // 움직일 크레인 hosit
    public GameObject CraneLift; // 움직일 크레인 lift
    public GameObject LayShooter; // ray를 쏘는 객체
    public CoilCollision coilcollision;
    public LayerMask CoilLayer; // 충돌할 레이어 변수;
    public bool LiftStatus = true;
    public bool moveStatus = true;
    public Transform PointB;
    public Transform[] SkidPositions;

    enum CraneStatus
    {
        Idle, // 리프트가 위로 올라와져있는 상태
        MovePickUpPoint,// 코일을 집기위한 위치로 이동
        APoint, // 코일을 줍기위해 트럭이 들어오는 위치로 이동
        Detected, // 리프트를 내려서 코일을 집는 함수
        CoilMove, // Coil을 적절한 위치에 옮기는 함수
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
                Debug.Log("오류가 발생했습니다");
                break;
        }

    }

    //-------------------------------------------------------------IDLE------------------------------------------------------------------------
    //대기상태일떄 리프트 올려두기
    public void IdleMove()
    {
        StartCoroutine(IdleStatusLift());
    }
    public void StopIdleStatus()
    {
        StopCoroutine(IdleStatusLift());
        IdleMove();
    }
    //LayShoot스크립트에서 쓰는 함수.
    public void StatusChangeMovePickUpPopint()
    {
        StopCoroutine(IdleStatusLift());
        cranstatus = CraneStatus.MovePickUpPoint;
    }

    //잡을 코일이 없을떄 대기 상태에 들어가기 위한 코루틴
    IEnumerator IdleStatusLift()
    {
        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
        CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionY, moveSpeed * Time.deltaTime);
        yield return new WaitForSeconds(1f);
        LayShoot objectBShooter = LayShooter.GetComponent<LayShoot>();
        //해당 객체가 null이 아니라면.(해당 객제가 있다면)
        objectBShooter.ShootAndCheckForCoil();
    }
    //--------------------------------------------MovePointA----------------------------------------------------------------

    //대기상태일떄 막대기에서 ray쏴서 코일이 있는지 상시 체크. 있으면 PointA로 이동
    public void MovePickUpPoint()
    {
        StartCoroutine(MovementRoutine());
    }
    //코일을 집기위해 APoint로 옮기는 함수
    IEnumerator MovementRoutine()
    {
        downSpeed = 4f;
        // 리프트 y축으로 올리는거
        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
        CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionY, moveSpeed * Time.deltaTime);
        //Debug.Log("Move1");
        yield return new WaitForSeconds(1f);
        //Hoist z축으로 움직이는거
        Vector3 targetpositionZ = new Vector3(CraneHoist.transform.position.x, CraneHoist.transform.position.y, PointA.position.z);
        CraneHoist.transform.position = Vector3.Lerp(CraneHoist.transform.position, targetpositionZ, moveSpeed * Time.deltaTime);
        //Debug.Log("Move2");
        yield return new WaitForSeconds(1f);
        // Body를 x축으로 움직이기
        Vector3 targetpositionX = new Vector3(PointA.position.x, CraneBody.transform.position.y, CraneBody.transform.position.z);
        CraneBody.transform.position = Vector3.Lerp(CraneBody.transform.position, targetpositionX, moveSpeed * Time.deltaTime);
        //Debug.Log("Move3");
        yield return new WaitForSeconds(1f);
        LiftStatus = true;
        cranstatus = CraneStatus.Detected;
    }
    //-----------------------------------------LiftDown-------------------------------------------------------------------------
    //ray에 Coil태그가 있을때 해당 코일을 집는 코드
    public void CraneDetectedCoil()
    {
        StopCoroutine(MovementRoutine());
       // Debug.Log("두번쨰 case 함수로 잘넘어왔다.");
        StartCoroutine(DetectCoil());
        //리프트의 특정 지점과 코일이 충돌시 코일의 위치를 리프트의 특정위치로 업데이트하는 함수만들기.
    }
    //충돌스크립트에서 쓰는 함수

    IEnumerator DetectCoil()
    {
       // Debug.Log("리프트 내려가는중");
        Vector3 targetPositionY = new Vector3(CraneLift.transform.position.x, PointA.position.y, CraneLift.transform.position.z);
        float distance = Vector3.Distance(CraneLift.transform.position, targetPositionY);
        if (distance > 0.01f && LiftStatus == true)
        {
            Debug.Log("여기도 반복되고있는가");
            CraneLift.transform.position = Vector3.MoveTowards(CraneLift.transform.position, targetPositionY, downSpeed * Time.deltaTime);
            yield return null;
            distance = Vector3.Distance(CraneLift.transform.position, targetPositionY);
        }

        yield return new WaitForSeconds(1f);

        if (downSpeed == 0f && cranstatus != CraneStatus.CoilMove)
        {
            //다시 올라가는거
            Vector3 targetPositionYT = new Vector3(CraneLift.transform.position.x, LiftRollBack.position.y, CraneLift.transform.position.z);
            CraneLift.transform.position = Vector3.Lerp(CraneLift.transform.position, targetPositionYT, moveSpeed * Time.deltaTime);
            // Once the lift has reached the target position, change crane status
           // Debug.Log("코일상탭전환체크");
            yield return new WaitForSeconds(3f);
            cranstatus = CraneStatus.CoilMove;
        }

    }
    //-----------------------------------------(MoveTOSkid)-------------------------------------------------------------------------

    //스키드로 옮기는 코루틴 실핼하는 함수
    public void MovementPointB()
    {
        LiftStatus = true;
        StopCoroutine(DetectCoil());
        StartCoroutine(MovePoint());
    }
    public void StopMovePoint()
    {
        //Debug.Log("스키드 충돌후 idle 상태로 가는 코드");
        StopCoroutine(MovePoint());
        cranstatus = CraneStatus.Idle;
    }

    //코일 충돌후 위치가 리프트로 업데이트 디고 있을떄 목표지점으로 이동하는 함수
    IEnumerator MovePoint()
    {
        InitializePointB();
        downSpeed = 4f;
       // Debug.Log("MovePoint로 넘어왔다.");
        yield return new WaitForSeconds(1f);
       // Debug.Log("포인트지점으로 옮기는 함수가 시작되는 부분이다.");
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
        //해당위치 도착한 후 , 코일의 위치가 리프트의 특정위치로 업데이트되고있는 상황에서 리프트가 내려가는 매서드 실행. 내려가는동안 스키드의 특정부분과 충돌시 코일의 위치가 리프트의 특정 위치로 업데이트 되는 함수 종료. 
        //코일의 위치를 스키드의 특정위치로 옮기는 함수 만들어서 코일이 해당 위치에 놓여지는 것처럼 보이게 만들기
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
            //Debug.Log("등록되어있지않은 코일입니다");
        }
    }
}
